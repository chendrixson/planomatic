using System;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.Serialization.Json;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

using System.ComponentModel;
using System.Windows.Data;

using Microsoft.Win32;

namespace Planomatic
{
    public delegate void IterationListChangedEventHandler(string iterationList);

    public class IterationDayConfig : INotifyPropertyChanged
    {
        public IterationDayConfig()
        {

        }

        public IterationDayConfig(float iteration1, float iteration2, float iteration3, float iteration4, float iteration5, float iteration6, Config config)
        {
            _iteration1 = iteration1;
            _iteration2 = iteration2;
            _iteration3 = iteration3;
            _iteration4 = iteration4;
            _iteration5 = iteration5;
            _iteration6 = iteration6;

            _config = config;
        }

        private Config _config;

        private float _iteration1;
        private float _iteration2;
        private float _iteration3;
        private float _iteration4;
        private float _iteration5;
        private float _iteration6;

        public void SetConfig(Config c)
        {
            _config = c;
        }

        public float Iteration1
        {
            get { return _iteration1; }
            set
            {
                if (value != _iteration1)
                {
                    _iteration1 = value;
                    NotifyPropertyChanged("Iteration1");
                }
            }
        }

        public float Iteration2
        {
            get { return _iteration2; }
            set
            {
                if (value != _iteration2)
                {
                    _iteration2 = value;
                    NotifyPropertyChanged("Iteration2");
                }
            }
        }

        public float Iteration3
        {
            get { return _iteration3; }
            set
            {
                if (value != _iteration3)
                {
                    _iteration3 = value;
                    NotifyPropertyChanged("Iteration3");
                }
            }
        }

        public float Iteration4
        {
            get { return _iteration4; }
            set
            {
                if (value != _iteration4)
                {
                    _iteration4 = value;
                    NotifyPropertyChanged("Iteration4");
                }
            }
        }

        public float Iteration5
        {
            get { return _iteration5; }
            set
            {
                if (value != _iteration5)
                {
                    _iteration5 = value;
                    NotifyPropertyChanged("Iteration5");
                }
            }
        }

        public float Iteration6
        {
            get { return _iteration6; }
            set
            {
                if (value != _iteration6)
                {
                    _iteration6 = value;
                    NotifyPropertyChanged("Iteration6");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

                _config.UpdateCapacity();
            }
        }
    }

    public class TeamConfig : INotifyPropertyChanged
    {
        public TeamConfig()
        {

        }

        public TeamConfig(Config config)
        {
            _config = config;
        }

        public TeamConfig(string groupName, float iteration1, float iteration2, float iteration3, float iteration4, float iteration5, float iteration6, Config config)
        {
            _groupName = groupName;
            _iteration1 = iteration1;
            _iteration2 = iteration2;
            _iteration3 = iteration3;
            _iteration4 = iteration4;
            _iteration5 = iteration5;
            _iteration6 = iteration6;

            _config = config;
        }

        private Config _config;

        private string _groupName;
        private float _iteration1;
        private float _iteration2;
        private float _iteration3;
        private float _iteration4;
        private float _iteration5;
        private float _iteration6;

        private float _capacity;
        private float _assigned;
        private float _overUnder;

        private const string koverUnderGreen = "#FF72CA72";
        private const string koverUnderRed = "#FFE75B5B";
        private string _overUnderColor = koverUnderGreen;

        public void UpdateOverUnder()
        {
            OverUnder = Assigned - Capacity;

            if (OverUnder > 0)
            {
                OverUnderColor = koverUnderRed;
            }
            else
            {
                OverUnderColor = koverUnderGreen;
            }
        }

        public string GroupName
        {
            get { return _groupName;  }
            set
            {
                if (value != _groupName)
                {
                    _groupName = value;
                    NotifyPropertyChanged("GroupName");
                }
            }
        }

        public float Iteration1
        {
            get { return _iteration1; }
            set
            {
                if (value != _iteration1)
                {
                    _iteration1 = value;
                    NotifyPropertyChanged("Iteration1");
                }
            }
        }
        public float Iteration2
        {
            get { return _iteration2; }
            set
            {
                if (value != _iteration2)
                {
                    _iteration2 = value;
                    NotifyPropertyChanged("Iteration3");
                }
            }
        }

        public float Iteration3
        {
            get { return _iteration3; }
            set
            {
                if (value != _iteration3)
                {
                    _iteration3 = value;
                    NotifyPropertyChanged("Iteration3");
                }
            }
        }

        public float Iteration4
        {
            get { return _iteration4; }
            set
            {
                if (value != _iteration4)
                {
                    _iteration4 = value;
                    NotifyPropertyChanged("Iteration4");
                }
            }
        }

        public float Iteration5
        {
            get { return _iteration5; }
            set
            {
                if (value != _iteration5)
                {
                    _iteration5 = value;
                    NotifyPropertyChanged("Iteration5");
                }
            }
        }

        public float Iteration6
        {
            get { return _iteration6; }
            set
            {
                if (value != _iteration6)
                {
                    _iteration6 = value;
                    NotifyPropertyChanged("Iteration6");
                }
            }
        }

        [XmlIgnore]
        public float Capacity
        {
            get { return _capacity; }
            set
            {
                if (value != _capacity)
                {
                    _capacity = value;
                    NotifyPropertyChanged("Capacity");

                    UpdateOverUnder();
                }
            }
        }

        [XmlIgnore]
        public float Assigned
        {
            get { return _assigned; }
            set
            {
                if (value != _assigned)
                {
                    _assigned = value;
                    NotifyPropertyChanged("Assigned");

                    UpdateOverUnder();
                }
            }
        }

        [XmlIgnore]
        public float OverUnder
        {
            get { return _overUnder; }
            set
            {
                if (value != _overUnder)
                {
                    _overUnder = value;
                    NotifyPropertyChanged("OverUnder");
                }
            }
        }

        [XmlIgnore]
        public string OverUnderColor
        {
            get { return _overUnderColor; }
            set
            {
                if (String.Compare(value, _overUnderColor) != 0)
                {
                    _overUnderColor = value;
                    NotifyPropertyChanged("OverUnderColor");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

                _config.UpdateCapacity();
            }
        }
    }

    [XmlRootAttribute("PlanOMatic", IsNullable=false)]
    public class Config : INotifyPropertyChanged
    {
        private const string ProfileLastConfig = "lastConfig";

        public Config()
        {

        }

        public Config(string command)
        {
            // Reload an existing config if there is one
            LastConfigFilename = "no config file";

            ReloadStoredLastConfig();
            if(_lastConfigSet)
            {
                LoadConfig(LastConfigFilename);
            }

            // If we don't have an iteration day config item, create one
            if(IterationDays.Count == 0)
            {
                IterationDays.Add(new IterationDayConfig(10, 10, 10, 10, 10, 10, this));
            }

            // And if we've got no teams, create some default ones
            if(Teams.Count == 0)
            { 
                this.Teams.Add(new TeamConfig("App Deployment", 8, 8, 8, 8, 8, 8, this));
                this.Teams.Add(new TeamConfig("BTAD-Base Tools and Developer experiences", 8, 8, 8, 8, 8, 8, this));
                this.Teams.Add(new TeamConfig("EMR-Enterprise MSIX Runtime", 8, 8, 8, 8, 8, 8, this));
                this.Teams.Add(new TeamConfig("GaP-Game Platform", 8, 8, 8, 8, 8, 8, this));
                this.Teams.Add(new TeamConfig("PEET-Packaging, Enterprise, Education, and Tools", 8, 8, 8, 8, 8, 8, this));
                this.Teams.Add(new TeamConfig("MAUS-Management of App and User State", 8, 8, 8, 8, 8, 8, this));
            }

            UpdateCapacity();

            // Setup binding
            TeamView = CollectionViewSource.GetDefaultView(Teams);
            IterationDayView = CollectionViewSource.GetDefaultView(IterationDays);
            DevsPerIterationView = CollectionViewSource.GetDefaultView(DevsPerIteration);

            IterationDayView.CollectionChanged += IterationDayView_CollectionChanged;
        }

        private void IterationDayView_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateCapacity();
        }

        private App myApp()
        {
            return (App)App.Current;
        }

        [XmlIgnore]
        private string _lastConfigFilename = "default";

        [XmlIgnore]
        private bool _lastConfigSet = false;

        [XmlIgnore]
        public bool RefreshingItems = false;

        // Configuration properties
        private string _serverName = @"https://microsoft.visualstudio.com/DefaultCollection";
        private string _project = @"OS";
        private string _rootNode = @"OS\Core\DEP\APT-Application Platform and Tools";
        private string _releaseList = "Iron;2020.06;2020.07;2020.08;2020.09;2020.11";
        private string _iterationList = "2006;2007;2008;2009;2010;2011";
        private float _totalCapacity;
        private string _extraQuery = "<empty>";

        [XmlIgnore]
        public string LastConfigFilename
        {
            get { return _lastConfigFilename; }
            set
            {
                _lastConfigFilename = value;
                NotifyPropertyChanged("LastConfigFilename");
            }
        }

        public string ServerName
        {
            get { return _serverName; }
            set
            {
                _serverName = value;
                NotifyPropertyChanged("ServerName");
            }
        }

        public string Project
        {
            get { return _project; }
            set
            {
                _project = value;
                NotifyPropertyChanged("Project");
            }
        }

        /// <summary>
        /// Teams root node is used to capture the parent of all team nodes
        /// </summary>
        public string TeamsRootNode { get; set; }

        /// <summary>
        /// Root node is the user configured root node (can be also one of the team nodes)
        /// </summary>
        public string RootNode
        {
            get { return _rootNode; }
            set
            {
                _rootNode = value;
                NotifyPropertyChanged("RootNode");
            }
        }

        public string ExtraQuery
        {
            get { return _extraQuery; }
            set
            {
                _extraQuery = value;
                NotifyPropertyChanged("ExtraQuery");
            }
        }

        public string ReleaseList
        {
            get { return _releaseList; }
            set
            {
                _releaseList = value;
                NotifyPropertyChanged("ReleaseList");
            }
        }

        public string IterationList
        {
            get { return _iterationList; }
            set
            {
                _iterationList = value;
                NotifyPropertyChanged("IterationList");

                if (IterationListChanged != null)
                {
                    IterationListChanged.Invoke(value);
                }
            }
        }



        [XmlIgnore]
        public float TotalCapacity
        {
            get { return _totalCapacity; }
            set
            {
                if (_totalCapacity != value)
                {
                    _totalCapacity = value;
                    NotifyPropertyChanged("TotalCapacity");
                }
            }
        }

        public event IterationListChangedEventHandler IterationListChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void UpdateCapacity()
        {
            if(IterationDays.Count == 0)
            {
                return;
            }

            float newTotalCapacity = 0;

            foreach(TeamConfig team in Teams)
            {
                float teamCapacity = 0;

                teamCapacity += team.Iteration1 * IterationDays[0].Iteration1;
                teamCapacity += team.Iteration2 * IterationDays[0].Iteration2;
                teamCapacity += team.Iteration3 * IterationDays[0].Iteration3;
                teamCapacity += team.Iteration4 * IterationDays[0].Iteration4;
                teamCapacity += team.Iteration5 * IterationDays[0].Iteration5;
                teamCapacity += team.Iteration6 * IterationDays[0].Iteration6;

                newTotalCapacity += teamCapacity;

                team.Capacity = teamCapacity;
            }

            TotalCapacity = newTotalCapacity;

            if(DevsPerIteration.Count == 1)
            {
                float teamIt1 = 0;
                float teamIt2 = 0;
                float teamIt3 = 0;
                float teamIt4 = 0;
                float teamIt5 = 0;
                float teamIt6 = 0;

                foreach (TeamConfig team in Teams)
                {
                    teamIt1 += team.Iteration1;
                    teamIt2 += team.Iteration2;
                    teamIt3 += team.Iteration3;
                    teamIt4 += team.Iteration4;
                    teamIt5 += team.Iteration5;
                    teamIt6 += team.Iteration6;
                }

                DevsPerIteration[0].Iteration1 = teamIt1;
                DevsPerIteration[0].Iteration2 = teamIt2;
                DevsPerIteration[0].Iteration3 = teamIt3;
                DevsPerIteration[0].Iteration4 = teamIt4;
                DevsPerIteration[0].Iteration5 = teamIt5;
                DevsPerIteration[0].Iteration6 = teamIt6;

            }
            return;
        }

        public enum TeamSumMode
        {
            AllItems,
            AboveCutLine,
            AboveRank
        }

        [XmlIgnore]
        public TeamSumMode CurrentSumMode = TeamSumMode.AllItems;

        [XmlIgnore]
        public bool CombineCustomStringGrouping = false;

        private int _teamSumRank = 99;

        public int TeamSumRank
        {
            get { return _teamSumRank; }
            set
            {
                if(value != _teamSumRank)
                {
                    _teamSumRank = value;
                    NotifyPropertyChanged("TeamSumRank");

                    if (myApp() != null)
                    {
                        myApp().DeliverableList.RecalcSums();
                    }
                }
            }
        }

        // Team data
        [XmlArray]
        public ObservableCollection<TeamConfig> Teams = new ObservableCollection<TeamConfig>();

        [XmlArray]
        public ObservableCollection<IterationDayConfig> IterationDays = new ObservableCollection<IterationDayConfig>();

        [XmlIgnore]
        public ObservableCollection<IterationDayConfig> DevsPerIteration = new ObservableCollection<IterationDayConfig>();

        [XmlIgnore]
        public ICollectionView TeamView { get; private set; }

        [XmlIgnore]
        public ICollectionView IterationDayView { get; private set; }

        [XmlIgnore]
        public ICollectionView DevsPerIterationView { get; private set; }

        public void AddTeam()
        {
            Teams.Add(new TeamConfig(this));
        }

        public void WriteConfig(string writeFilename)
        {
            var serializer = new XmlSerializer(typeof(Config));
            var writer = new StreamWriter(writeFilename);

            serializer.Serialize(writer, this);
            writer.Dispose();
        }

        private const string _registryKey = @"Software\Planomatic";
        private const string _registryValue = "LastConfigFileName";

        void UpdateStoredLastConfig()
        {
            RegistryKey myKey = Registry.CurrentUser.CreateSubKey(_registryKey);
            myKey.SetValue(_registryValue, LastConfigFilename);
        }

        void ReloadStoredLastConfig()
        {
            RegistryKey myKey = Registry.CurrentUser.OpenSubKey(_registryKey);
            if(myKey != null)
            {
                LastConfigFilename = (string)myKey.GetValue(_registryValue);
                _lastConfigSet = true;
            }            
        }

        public bool Save()
        {
            if(!_lastConfigSet)
            {
                return false;
            }
            else
            {
                SaveAs(LastConfigFilename);
            }

            return true;
        }

        public void SaveAs(string configFilename)
        {
            // Try and write to this
            try
            {
                WriteConfig(configFilename);
            }
            catch(Exception e)
            {
                Debug.WriteLine("Exception saving: " + e);
            }

            // Save off the filename for our next Save action
            LastConfigFilename = configFilename;
            _lastConfigSet = true;

            UpdateStoredLastConfig();

            myApp().UpdateStatus($"Saved file {configFilename}");
        }

        public bool LoadConfig(string configFilename)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(Config));
                var reader = new StreamReader(configFilename);

                Config newConfig = (Config)serializer.Deserialize(reader);

                // Load all the properties into THIS instance of the config
                this.ServerName = newConfig.ServerName;
                this.RootNode = newConfig.RootNode;
                this.ReleaseList = newConfig.ReleaseList;
                this.IterationList = newConfig.IterationList;
                this.TeamSumRank = newConfig.TeamSumRank;
                this.ExtraQuery = newConfig.ExtraQuery;

                // Grab the day config
                this.IterationDays.Clear();
                newConfig.IterationDays[0].SetConfig(this);
                this.IterationDays.Add(newConfig.IterationDays[0]);

                // Grab the team config
                this.Teams.Clear();
                foreach(TeamConfig team in newConfig.Teams)
                {
                    this.Teams.Add(new TeamConfig(team.GroupName, team.Iteration1, team.Iteration2, team.Iteration3, team.Iteration4, team.Iteration5, team.Iteration6, this));
                }

                UpdateCapacity();

                LastConfigFilename = configFilename;
                _lastConfigSet = true;

                UpdateStoredLastConfig();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to deserialize from file " + configFilename);
                Debug.WriteLine(e.ToString());
                return false;
            }

            Debug.WriteLine("Successfully deserailized from file " + configFilename);
            return true;
        }
    }
}
