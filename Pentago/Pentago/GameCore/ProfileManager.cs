using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
                        int campaignProgress = Convert.ToInt32(line.Substring(name.Length + 3, 1));
                        AddProfileToList(new Profile(name, campaignProgress));
                    }
                    catch { }
                }
            }
        }

        private void AddProfileToList(Profile profile)
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

        public bool IsProfileValid(string newProfileName)
        {
            foreach(Profile profile in _Profiles)
            {
                if (newProfileName == profile.ProfileName)
                    return false;
            }
            return true;
        }

        public void CreateNewProfile(string newProfileName)
        {
            Profile newProfile = new Profile(newProfileName, 0);
            string[] profileParser = new string[_Profiles.Count + 1];

            string profileLineConvention = newProfile.ProfileName + "[&]";
            profileLineConvention += 0 + "[&]";
            profileParser[0] = profileLineConvention;
            for (int i = 0; i < _Profiles.Count; i++)
            {
                profileLineConvention = _Profiles[i].ProfileName + "[&]";
                profileLineConvention += _Profiles[i].CampaignProgress + "[&]";
                profileParser[i + 1] = profileLineConvention;
            }

            if (!System.IO.Directory.Exists(pathToDirectory))
                System.IO.Directory.CreateDirectory(pathToDirectory);
            try
            {
                System.IO.File.WriteAllLines(pathToProfiles, profileParser);
                //Clear list
                _Profiles.Clear();
                //Load new list with new profiles
                ParseProfile();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public Profile SearchProfile(string profileName)
        {
            foreach (Profile profile in _Profiles)
            {
                if (profile.ProfileName == profileName)
                    return profile;
            }
            return null;
        }

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
