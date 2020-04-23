using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace Planomatic
{
    public class DeliverableGroup : INotifyPropertyChanged
    {
        private string _customString1;
        private float _totalEstimate;
        private Nullable<int> _rank;
        private string _sumColor;


        private Config _config;
        private Deliverables _deliverableList;
        private List<Deliverable> _groupDeliverables = new List<Deliverable>();

        public DeliverableGroup(string customString1, Config config, Deliverables deliverableList)
        {
            _customString1 = customString1;
            _config = config;
            _deliverableList = deliverableList;
        }

        public List<Deliverable> GroupDeliverables
        {
            get { return _groupDeliverables;  }
        }

        public string CustomString1
        {
            get { return _customString1; }
            set
            {
                if (value != _customString1)
                {
                    _customString1 = value;
                    NotifyPropertyChanged("CustomString1");
                }
            }
        }

        public float TotalEstimate
        {
            get { return _totalEstimate; }
            set
            {
                if (value != _totalEstimate)
                {
                    _totalEstimate = value;
                    NotifyPropertyChanged("TotalEstimate");
                }
            }
        }

        public Nullable<int> Rank
        {
            get { return _rank; }
            set
            {
                if (value != _rank)
                {
                    _rank = value;
                    NotifyPropertyChanged("Rank");
                }
            }
        }

        public string SumColor
        {
            get { return _sumColor; }
            set
            {
                if (String.Compare(value, _sumColor) != 0)
                {
                    _sumColor = value;
                    NotifyPropertyChanged("SumColor");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

                _deliverableList.RecalcSums();
            }
        }
    }

    public class Deliverable : INotifyPropertyChanged, IComparable<Deliverable> ,IEquatable<Deliverable>
    {
        private int _id;
        private string _title;
        private Nullable<int> _rank;
        private string _team;
        private Nullable<float> _originalEstimate;
        private Nullable<float> _remainingDevDays;
        private string _customString1;
        private string _customString2;
        private string _customString3;
        private string _pmOwner;
        private string _adoUrl;
        private float _sumEstimate;
        private string _sumColor;

        private bool _mod;
        private Config _config;
        private Deliverables _deliverableList;

        public Deliverable()
        {
        }

        public Deliverable(int id, Config config, Deliverables deliverableList)
        {
            _id = id;
            _config = config;
            _deliverableList = deliverableList;
        }

        public Deliverable(object id, Config config, Deliverables deliverableList)
        {
            _id = int.Parse(id.ToString());
            _config = config;
            _deliverableList = deliverableList;
        }

        private void SetMod()
        {
            if (_config.RefreshingItems == false)
            {
                Mod = true;
                _deliverableList.RecalcSums();
            }
        }

        private void UpdateColor()
        {
            if (_sumEstimate <= (_config.TotalCapacity * .7))
            {
                // Green!
                SumColor = "#FF72CA72";
            }
            else if (_sumEstimate <= _config.TotalCapacity)
            {
                // Yellow!
                SumColor = "#FFE6E75B";
            }
            else
            {
                //Red!
                SumColor = "#FFE75B5B";
            }
        }

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    NotifyPropertyChanged("Title");
                    SetMod();
                }
            }
        }

        public Nullable<int> Rank
        {
            get { return _rank; }
            set
            {
                if (value != _rank)
                {
                    _rank = value;
                    NotifyPropertyChanged("Rank");
                    SetMod();
                }
            }
        }

        public string AreaPath { get; set; }

        public string Team
        {
            get { return _team; }
            set
            {
                if (value != _team)
                {
                    _team = value;
                    NotifyPropertyChanged("Team");
                }
            }
        }

        public Nullable<float> OriginalEstimate
        {
            get { return _originalEstimate; }
            set
            {
                if (value != _originalEstimate)
                {
                    _originalEstimate = value;
                    NotifyPropertyChanged("OriginalEstimate");
                    SetMod();
                }
            }
        }

        public Nullable<float> RemainingDevDays
        {
            get { return _remainingDevDays; }
            set
            {
                if (value != _remainingDevDays)
                {
                    _remainingDevDays = value;
                    NotifyPropertyChanged("RemainingDevDays");
                    SetMod();
                }
            }
        }

        public string AdoUrl
        {
            get { return _adoUrl; }
            set
            {
                if (value != _adoUrl)
                {
                    _adoUrl = value;
                    NotifyPropertyChanged("RemainingDevDays");
                }
            }
        }

        public string CustomString1
        {
            get { return _customString1; }
            set
            {
                if (value != _customString1)
                {
                    _customString1 = value;
                    NotifyPropertyChanged("CustomString1");
                }
            }
        }

        public string CustomString2
        {
            get { return _customString2; }
            set
            {
                if (value != _customString2)
                {
                    _customString2 = value;
                    NotifyPropertyChanged("CustomString2");
                }
            }
        }

        public string CustomString3
        {
            get { return _customString3; }
            set
            {
                if (value != _customString3)
                {
                    _customString3 = value;
                    NotifyPropertyChanged("CustomString3");
                }
            }
        }

        public string PMOwner
        {
            get { return _pmOwner; }
            set
            {
                if (value != _pmOwner)
                {
                    _pmOwner = value;
                    NotifyPropertyChanged("PMOwner");
                }
            }
        }

        public bool Mod
        {
            get { return _mod; }
            set
            {
                if (value != _mod)
                {
                    _mod = value;
                    NotifyPropertyChanged("Mod");
                }
            }
        }

        public float SumEstimate
        {
            get { return _sumEstimate; }
            set
            {
                if (value != _sumEstimate)
                {
                    _sumEstimate = value;
                    NotifyPropertyChanged("SumEstimate");
                    UpdateColor();
                }
            }
        }

        public string SumColor
        {
            get { return _sumColor; }
            set
            {
                if(String.Compare(value, _sumColor) != 0)
                {
                    _sumColor = value;
                    NotifyPropertyChanged("SumColor");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int CompareTo(Deliverable other)
        {
            if (!this.Rank.HasValue && !other.Rank.HasValue)
            {
                return 0;
            }
            else if (!this.Rank.HasValue && other.Rank.HasValue)
            {
                return 1;
            }
            else if (this.Rank.HasValue && !other.Rank.HasValue)
            {
                return -1;
            }
            else
            {
                return this.Rank.Value - other.Rank.Value;
            }
        }

        public bool Equals(Deliverable other)
        {
            if (this.Id == other.Id)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

                _deliverableList.RecalcSums();
            }
        }
    }

    public class Deliverables
    {
        private AdoServer _ado = new AdoServer();
        private Config _myConfig;
        public string FailedIdList = "";

        public Deliverables()
        {
            AllItemsView = CollectionViewSource.GetDefaultView(AllItems);

            AllGroupsView = CollectionViewSource.GetDefaultView(AllGroups);

            CurrentGroupView = CollectionViewSource.GetDefaultView(CurrentGroup);
        }


        public Config myConfig()
        {
            return _myConfig;
        }
        private App myApp()
        {
            return (App)App.Current;
        }

        public void SetConfig(Config c)
        {
            _myConfig = c;

            TeamsCollection.Clear();
            TeamsCollection.Add("<unmapped>"); // Need to add <unmapped> to make this value legal.
            foreach (TeamConfig t in myConfig().Teams)
            {
                TeamsCollection.Add(t.GroupName);
            }
        }

        // Flat list
        public ObservableCollection<Deliverable> AllItems = new ObservableCollection<Deliverable>();
        public ICollectionView AllItemsView { get; private set; }

        // Grouped list
        public ObservableCollection<DeliverableGroup> AllGroups = new ObservableCollection<DeliverableGroup>();
        public ICollectionView AllGroupsView { get; private set; }

        // Current grouped view
        public ObservableCollection<Deliverable> CurrentGroup = new ObservableCollection<Deliverable>();
        public ICollectionView CurrentGroupView { get; private set; }

        public static ObservableCollection<string> TeamsCollection = new ObservableCollection<string>();

        private List<Dictionary<string, object>> RefreshTask()
        {
            _ado.Connect(myConfig().ServerName);

            var allItemsData = _ado.GetWorkItems(myConfig().RootNode, myConfig().Project, myConfig().ReleaseList, myConfig().ExtraQuery);

            return allItemsData;
        }

        private int UpdateTask(List<Dictionary<string, object>> updateItems)
        {
            _ado.Connect(myConfig().ServerName);

            int updateCount = _ado.UpdateWorkItems(updateItems);

            return updateCount;
        }

        public void RecalcSums()
        {
            float runningTotal = 0;

            // Clear out the teams and create a map
            var teamMap = new Dictionary<string, TeamConfig>();

            foreach(TeamConfig t in myConfig().Teams)
            {
                t.Assigned = 0;
                teamMap.Add(t.GroupName, t);
            }

            var unmappedTeams = new Dictionary<string, int>();

            foreach (Deliverable d in AllItems)
            {
                if (d.OriginalEstimate.HasValue)
                {
                    // Add up a total, and a per team value
                    runningTotal += d.OriginalEstimate.Value;
                    runningTotal = (float)Math.Round(runningTotal, 1);

                    string itemTeam = d.Team;

                    // If there is an unmapped team configured, use that for anything that's not in the real team list
                    if(teamMap.ContainsKey(itemTeam) == false && teamMap.ContainsKey("<unmapped>"))
                    {
                        itemTeam = "<unmapped>";
                    }

                    if (teamMap.ContainsKey(itemTeam) )
                    {
                        // See if we should add this to the team item
                        bool addToTeam = false;

                        switch (myConfig().CurrentSumMode)
                        {
                            case Config.TeamSumMode.AllItems:
                                addToTeam = true;
                                break;
                            case Config.TeamSumMode.AboveCutLine:
                                if (runningTotal <= myConfig().TotalCapacity)
                                {
                                    addToTeam = true;
                                }
                                break;
                            case Config.TeamSumMode.AboveRank:
                                if (d.Rank <= myConfig().TeamSumRank)
                                {
                                    addToTeam = true;
                                }
                                break;
                        }

                        if (addToTeam)
                        {

                            teamMap[itemTeam].Assigned += d.OriginalEstimate.Value;
                            teamMap[itemTeam].Assigned = (float)Math.Round(teamMap[itemTeam].Assigned, 1);
                        }
                    }
                    else
                    {
                        Debug.WriteLine($"Worked not mapped to team {d.Team}");
                        // work not mapped to a configured team!
                        if (!unmappedTeams.ContainsKey(d.Team))
                        {
                            unmappedTeams.Add(d.Team, 0);
                        }

                        unmappedTeams[d.Team]++;
                    }
                }
                d.SumEstimate = runningTotal;
            }

            if(unmappedTeams.Count > 0)
            {
                string unmappedTeamString = "";
                foreach(var i in unmappedTeams)
                {
                    unmappedTeamString += $"{i.Key}({i.Value}),";
                }
                unmappedTeamString = unmappedTeamString.TrimEnd(',');

                myApp().UpdateStatus("Deliverables for missing team: " + unmappedTeamString);
            }

            RedoGroups();
        }

        public void Resort()
        {
            // Get a NEW sorted list of all these deliverables using our custom comparator
            var observable = AllItems;
            List<Deliverable> sorted = AllItems.OrderBy(x => x).ToList();

            // Then clear and re-add them to the observable list
            AllItems.Clear();

            foreach (Deliverable d in sorted)
            {
                AllItems.Add(d);
            }

            RecalcSums();

            AllItemsView.Refresh();
        }

        public async Task<int> Refresh()
        {
            // CNOTETODO - prompt if there are dirty records
            myApp().UpdateProgress(true);
            myApp().UpdateStatus("Refreshing...");
            int tickStart = Environment.TickCount;

            AllItems.Clear();

            var refreshTask = Task<List<Dictionary<string, object>>>.Factory.StartNew(() => RefreshTask());

            await refreshTask;

            var allItemsData = refreshTask.Result;

            foreach (var item in allItemsData)
            {
                try
                {
                    //private int _id;
                    //private string _title;
                    //private int _rank;
                    //private string _team;
                    //private float _originalEstimate;
                    //private float _remainingDevDays;
                    //private string _customString1;
                    //private string _pmOwner;
                    //private string _area6 (subTeam)
                    //private string _customString2;
                    //private string _customString3;
                    // End known field IDs

                    Deliverable d = new Deliverable(
                        item[_ado.KnownFields[0]], myConfig(), this);

                    if (item.ContainsKey(_ado.KnownFields[1]))
                    {
                        d.Title = item[_ado.KnownFields[1]].ToString();
                    }

                    if (item.ContainsKey(_ado.KnownFields[2]))
                    {
                        d.Rank = int.Parse(item[_ado.KnownFields[2]].ToString());
                    }

                    /*  OLD TEAM HANDLINE
                    if (item.ContainsKey(_ado.KnownFields[3]))
                    {
                        bool addTeamNormal = true;

                        // See if we are matching against level 5 or 6, depending on root path
                        string[] queryPathParts = myConfig().RootNode.Split('\\');

                        if (queryPathParts.Length == 4)
                        {
                            // Regular mode, "per-DM"

                            // See if we have a subteam as well, which is mapped
                            if (item.ContainsKey(_ado.KnownFields[8]))
                            {
                                string possibleFullTeam = item[_ado.KnownFields[3]].ToString() + "\\" + item[_ado.KnownFields[8]].ToString();

                                // See if this possible name is in the list of teams
                                foreach (TeamConfig t in myConfig().Teams)
                                {
                                    if (string.Compare(t.GroupName, possibleFullTeam, true) == 0)
                                    {
                                        d.Team = possibleFullTeam;
                                        addTeamNormal = false;
                                        break;
                                    }
                                }
                            }
                        }
                        else if (queryPathParts.Length == 5)
                        {
                            // "per-lead" - check area 6 and use that or "<root>"
                            if (item.ContainsKey(_ado.KnownFields[8]))
                            {
                                d.Team = item[_ado.KnownFields[8]].ToString();
                            }
                            else
                            {
                                d.Team = "<root>";
                            }

                            addTeamNormal = false;
                        }

                        if(addTeamNormal)
                        { 
                            d.Team = item[_ado.KnownFields[3]].ToString();
                        }
                    }
                    else
                    {
                        d.Team = "<root>";
                    }
                    */



                    if (item.ContainsKey(_ado.KnownFields[4]))
                    {
                        d.OriginalEstimate = float.Parse(item[_ado.KnownFields[4]].ToString());
                    }

                    if (item.ContainsKey(_ado.KnownFields[5]))
                    {
                        d.RemainingDevDays = float.Parse(item[_ado.KnownFields[5]].ToString());
                    }

                    if (item.ContainsKey("AdoUrl"))
                    {
                        d.AdoUrl = item["AdoUrl"].ToString();
                    }

                    if (item.ContainsKey(_ado.KnownFields[6]))
                    {
                        d.CustomString1 = item[_ado.KnownFields[6]].ToString();
                    }

                    if (item.ContainsKey(_ado.KnownFields[7]))
                    {
                        d.PMOwner = item[_ado.KnownFields[7]].ToString();
                    }

                    if (item.ContainsKey(_ado.KnownFields[9]))
                    {
                        d.CustomString2 = item[_ado.KnownFields[9]].ToString();
                    }

                    if (item.ContainsKey(_ado.KnownFields[10]))
                    {
                        d.CustomString3 = item[_ado.KnownFields[10]].ToString();
                    }

                    // Map the team to anything that matches the substring
                    if (item.ContainsKey(_ado.KnownFields[11]))
                    {
                        string itemAreaPath = item[_ado.KnownFields[11]].ToString();

                        // Find which team
                        d.Team = "<unmapped>";

                        foreach (TeamConfig t in myConfig().Teams)
                        {
                            if (itemAreaPath.Contains(t.GroupName))
                            {
                                // To allow changing area path, We only consider area path up to the group name, as sub nodes can't be moved.
                                d.AreaPath = itemAreaPath.Substring(0, itemAreaPath.IndexOf(t.GroupName) + t.GroupName.Length);
                                d.Team = t.GroupName;
                                break;
                            }
                        }

                    }


                    AllItems.Add(d);
                }
                catch (Exception)
                {
                    Debug.WriteLine("Failed to create deliverable");
                }
            }

            Resort();

            RedoGroups();

            int totalCount = AllItems.Count;
            int totalTime = Environment.TickCount - tickStart;

            myApp().UpdateStatus($"Loaded {totalCount} items in {totalTime} ms.");
            myApp().UpdateProgress(false);

            return totalCount;
        }

        void RedoGroups()
        {
            AllGroups.Clear();

            // Build up the groups
            var groupHash = new Dictionary<string, DeliverableGroup>();

            foreach (Deliverable d in AllItems)
            {
                string groupName = d.CustomString1;

                // If we've been asked to combine CS1 and 2, treat it as such
                if(myConfig().CombineCustomStringGrouping)
                {
                    groupName += "-" + d.CustomString2;
                }

                if (groupName == null || groupName.Length == 0)
                {
                    groupName = "<no cs1>";
                }

                // Add it to the group hash and group list           
                if (groupHash.ContainsKey(groupName))
                {
                    groupHash[groupName].GroupDeliverables.Add(d);
                }
                else
                {
                    groupHash.Add(groupName, new DeliverableGroup(groupName, myConfig(), this));
                    groupHash[groupName].GroupDeliverables.Add(d);
                }

                // Then keep track of estimate
                if (d.OriginalEstimate.HasValue)
                {
                    groupHash[groupName].TotalEstimate += d.OriginalEstimate.Value;
                }

                // keep track of highest rank
                if (d.Rank.HasValue)
                {
                    if (groupHash[groupName].Rank.HasValue)
                    {
                        if (d.Rank.Value < groupHash[groupName].Rank.Value)
                        {
                            groupHash[groupName].Rank = d.Rank;
                            groupHash[groupName].SumColor = d.SumColor;
                        }
                    }
                    else
                    {
                        groupHash[groupName].Rank = d.Rank;
                        groupHash[groupName].SumColor = d.SumColor;
                    }
                }
            }

            foreach (DeliverableGroup g in groupHash.Values)
            {
                AllGroups.Add(g);
            }
        }

        public async Task<int> Update()
        {
            var updateItems = new List<Dictionary<string, object>>();

            foreach (Deliverable d in AllItems)
            {
                if (d.Mod == true)
                {
                    var newItem = new Dictionary<string, object>();

                    //private int _id;
                    //private string _title;
                    //private int _rank;
                    //private string _team;
                    //private float _originalEstimate;
                    //private float _remainingDevDays;

                    newItem[_ado.KnownFields[0]] = d.Id;

                    newItem[_ado.KnownFields[1]] = d.Title;

                    if (d.Rank.HasValue)
                    {
                        newItem[_ado.KnownFields[2]] = d.Rank;
                    }

                    if (d.OriginalEstimate.HasValue)
                    {
                        newItem[_ado.KnownFields[4]] = d.OriginalEstimate;
                    }

                    if (d.RemainingDevDays.HasValue)
                    {
                        newItem[_ado.KnownFields[5]] = d.RemainingDevDays;
                    }

                    if (!string.IsNullOrWhiteSpace(d.AreaPath) &&
                        !string.Equals(d.Team, "<unmapped>", StringComparison.OrdinalIgnoreCase))
                    {
                        newItem[_ado.KnownFields[11]] = d.AreaPath;
                    }

                    updateItems.Add(newItem);
                }
            }

            if (updateItems.Count == 0)
            {
                myApp().UpdateStatus("No items to update");
                return 0;
            }
            else
            {
                myApp().UpdateProgress(true);
                myApp().UpdateStatus($"Updating {updateItems.Count} items...");
                int tickStart = Environment.TickCount;

                var updateTask = Task<int>.Factory.StartNew(() => UpdateTask(updateItems));
                await updateTask;

                int totalUpdated = updateTask.Result;

                int totalTime = Environment.TickCount - tickStart;
                myApp().UpdateProgress(false);

                // Update the status.  If it hasn't fully updated all items, put an error in the output
                if (totalUpdated == updateItems.Count)
                {
                    // They all worked!
                    myApp().UpdateStatus($"Updated {totalUpdated} items in {totalTime} ms.");

                    // Clear the dirty bits
                    foreach (Deliverable d in AllItems)
                    {
                        if (d.Mod == true)
                        {
                            d.Mod = false;
                        }
                    }
                }
                else
                {
                    myApp().UpdateStatus($"Failed to update items: " + _ado.FailedIdList);
                }

                return totalUpdated;
            }
        }
    }
}
