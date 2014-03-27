using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pentago.GameCore
{
    public class ProfileManager
    {
        #region singleton design pattern
        private static List<Profile> _Profiles;
        private string  pathToProfiles = "C:\\Users\\Public\\Documents\\Dragon Horde\\profiles.txt";
        private string pathToDirectory = "C:\\Users\\Public\\Documents\\Dragon Horde";

        private ProfileManager()
        {
            _Profiles = new List<Profile>();
            //verify if directory exists
            if (!System.IO.Directory.Exists(pathToDirectory))
            {
                System.IO.Directory.CreateDirectory(pathToDirectory);
            }

            //verify if file exists
            if (File.Exists(pathToProfiles))
            {
                ParseProfile();
            }
            else
            {
                File.Create(pathToProfiles);
                File.SetAttributes(pathToProfiles, FileAttributes.Hidden);
            }
        }

        private void ParseProfile()
        {
            string[] profileParser = File.ReadAllLines(pathToProfiles);
            foreach (string line in profileParser)
            {
                if (line != "")
                {
                    try
                    {
                        string name = line.Substring(0, line.IndexOf("[&]"));
                        int campaignProgress = Convert.ToInt32(line.Substring(name.Length + 3));
                        AddProfile(new Profile(name, campaignProgress));
                    }
                    catch { }
                }
            }
        }

        private void AddProfile(Profile profile)
        {
            _Profiles.Add(profile);
        }

        private static ProfileManager instance;
        public static ProfileManager InstanceCreator()
        {
            if (instance == null)
                instance = new ProfileManager();
            return instance;
        }
        #endregion

        public List<Profile> GetAllProfiles()
        {
            return _Profiles;
        }


        /// <summary> Profile
        /// Private nested class to encapsulate 
        /// attributes of a profile
        /// </summary>
        public class Profile
        {
            private string _profileName;
            private int _profileCampaignProgress;

            public Profile(string pofileName, int profileCampaignProgress)
            {
                this._profileName = pofileName;
                this._profileCampaignProgress = profileCampaignProgress;
            }

            public string ProfileName
            {
                get { return this._profileName; }
                set { this._profileName = value; }
            }

            public int CampaignProgress
            {
                get { return this._profileCampaignProgress; }
            }

            public void incrementCampaignProgress()
            {
                this._profileCampaignProgress++;
            }
        }
    }
}
