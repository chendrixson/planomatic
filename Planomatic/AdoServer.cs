using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Planomatic
{
    class AdoServer
    {
        public VssConnection _connection;

        public void Connect(string serverName)
        {
            // Interactively ask the user for credentials, caching them so the user isn't constantly prompted
            VssCredentials creds = new VssClientCredentials();
            creds.Storage = new VssClientCredentialStorage();

            // Connect to Azure DevOps Services
            _connection = new VssConnection(new Uri(serverName), creds);
            _connection.ConnectAsync().SyncResult();

            Debug.WriteLine("Has authenticaed: " + _connection.HasAuthenticated);
        }


        // Field IDs used, hsould match the below order
        public string[] KnownFields = { "System.Id", "System.Title", "OSG.Rank", "System.AreaLevel5", "Microsoft.VSTS.Scheduling.OriginalEstimate", "OSG.RemainingDevDays", "Microsoft.VSTS.Common.CustomString01", "OSG.PMOwner", "System.AreaLevel6", "Microsoft.VSTS.Common.CustomString02", "Microsoft.VSTS.Common.CustomString03", "System.AreaPath" };
        //private int _id;
        //private string _title;
        //private int _rank;
        //private string _team;
        //private float _originalEstimate;
        //private float _remainingDevDays;
        //private string _customString1;
        //private string _pmOwner;
        //private string _subTeam;
        //private string _customString2;
        //private string _customString3;
        // End known field IDs

        public string FailedIdList;

        public List<Dictionary<string,object>> GetWorkItems(string rootNode, string project, string releases, string extraQuery)
        {
            WorkItemTrackingHttpClient witClient = _connection.GetClient<WorkItemTrackingHttpClient>();
            List<int> idList = new List<int>();

            // Run a query for each release to get the IDs, then get all the fields
            string[] releaseSplit = releases.Split(';'); 
            if(releaseSplit.Length == 0)
            {
                Debug.WriteLine("No releases in list");
                return null;
            }

            // check if the rootnode is semi-colon separated or not 
            string rootNodeQuery;

            string[] rootSplit = rootNode.Split(';');            
            if(rootSplit.Length > 1)
            {
                Debug.WriteLine("Got multiple root nodes");
                // build up something like this
                // AND ( [System.AreaPath] UNDER 'OS\Core\DEP\DnI  - Input and Win32'  OR  [System.AreaPath] UNDER 'OS\Core\DEP\DnI - AI and Ink'  OR  [System.AreaPath] UNDER 'OS\Core\DEP\DnI - Accessibility' ) 
                rootNodeQuery = $"AND ( [System.AreaPath] UNDER '{rootSplit[0]}' ";
                for(int rootNodeCounter = 1; rootNodeCounter < rootSplit.Length; rootNodeCounter++)
                {
                    rootNodeQuery += $" OR [System.AreaPath] UNDER '{rootSplit[rootNodeCounter]}' ";
                }
                rootNodeQuery += " ) ";
            }
            else
            {
                Debug.WriteLine("Got single root node");
                rootNodeQuery = $"AND [System.AreaPath] UNDER '{rootNode}'";
            }

            // Build up the query string
            string queryString = $"SELECT [ID] FROM WorkItems WHERE [Work Item Type] = 'Deliverable' AND [System.TeamProject] = '{project}'" +
                $" {rootNodeQuery} AND [System.State] <> 'Cut'  AND  [System.State] <> 'Completed'";

            if(string.Compare(extraQuery, "<empty>") != 0)
            {
                queryString += $" AND {extraQuery}";
            }
                
            if (releases.Length == 1)
            {
                queryString += $" AND [Microsoft.VSTS.Common.Release] = '{releases}'";
            }
            else
            {
                // AND ( [Microsoft.VSTS.Common.Release] = '19H1'  OR  [Microsoft.VSTS.Common.Release] = '2019.01' )
                queryString += " AND ( ";
                int counter = 0;
                foreach(string s in releaseSplit)
                {
                    queryString += $" [Microsoft.VSTS.Common.Release] = '{s}' ";
                    counter++;

                    if(counter != releaseSplit.Length)
                    {
                        queryString += " OR ";
                    }
                }
                queryString += " )";
            }

            Wiql query = new Wiql() { Query = queryString };
                WorkItemQueryResult queryResults = witClient.QueryByWiqlAsync(query).Result;

                if (queryResults == null) 
                {
                    Debug.WriteLine("Got no query result");
                }
                else if (queryResults.WorkItems.Count() == 0)
                {
                    Debug.WriteLine("Got empty query result");
                }
                else
                {
                    Debug.WriteLine($"Got {queryResults.WorkItems.Count()} results");
                    foreach (var queryItem in queryResults.WorkItems)
                    {
                        idList.Add(queryItem.Id);
                    }
                }

            var output = new List<Dictionary<string, object>>();

            if (idList.Count > 0)
            {
                var batches = idList.Batch(100); // ADO caps requests

                foreach(var batch in batches)
                {
                    var itemsWithFields = witClient.GetWorkItemsAsync(batch, KnownFields, queryResults.AsOf).Result;
                    foreach (var item in itemsWithFields)
                    {
                        var newItem = new Dictionary<string, object>(item.Fields);
                        // hide the URL in a special field outside known fields, since those are all ADO field names
                        // convert this to the real url from the API one
                        // eg this: https://microsoft.visualstudio.com/_apis/wit/workItems/10673502
                        // goes to this: https://microsoft.visualstudio.com/OS/_workitems/edit/10673502

                        string realUrl = item.Url.Replace("_apis/wit/workItems", $"{project}/_workitems/edit");

                        newItem["AdoUrl"] = realUrl;

                        output.Add(newItem);
                    }
                }

            }

            Debug.WriteLine("Got all the data!");
            return output;
        }

        public int UpdateWorkItems(List<Dictionary<string, object>> items)
        {
            FailedIdList = "";

            // Verify that they all work
            int validCount = UpdateWorkItemsInternal(items, false);

            if (validCount == items.Count)
            {
                // Commmit them!
                return UpdateWorkItemsInternal(items, true);
            }
            else
            {
                // Peel the last comma
                FailedIdList = FailedIdList.TrimEnd(',');
                return 0;
            }

        }

        private int UpdateWorkItemsInternal(List<Dictionary<string, object>> items, bool commitChanges)
        {
            int updateCount = 0;

            WorkItemTrackingHttpClient witClient = _connection.GetClient<WorkItemTrackingHttpClient>();
            foreach (var i in items)
            {
                var document = new JsonPatchDocument();
                string idKey = KnownFields[0];
                int id = int.Parse(i[idKey].ToString());

                foreach (var d in i)
                {
                    if (d.Key != idKey)
                    {
                        document.Add(new JsonPatchOperation()
                        {
                            Operation = Operation.Add,
                            Path = $"/fields/{d.Key}",
                            Value = $"{d.Value.ToString()}"
                        });
                    }
                }

                try
                {
                    var workItem = witClient.UpdateWorkItemAsync(document, id, commitChanges);
                    var result = workItem.Result;
                    updateCount++;
                }
                catch(Exception)
                {
                    Debug.WriteLine($"Failed to update ID {id}");
                    // Failed to update
                    FailedIdList += $"{id},";
                }
            }

            return updateCount;
        }
    }
}
