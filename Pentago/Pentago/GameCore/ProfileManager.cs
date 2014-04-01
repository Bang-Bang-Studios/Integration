using System;
using System.Collections.Generic;
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
        private string pathToProfileAvatars = "pack://application:,,,/GUI/images/avatars/";

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
                //File.SetAttributes(pathToProfiles, FileAttributes.Hidden);
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
            //Initialize everything to zero
            Profile newProfile = new Profile(newProfileName, 0);
            string[] profileParser = new string[_Profiles.Count + 1];

            string profileLineConvention = newProfileName + "[&]";
            profileLineConvention += 0;
            profileParser[0] = profileLineConvention;
            for (int i = 0; i < _Profiles.Count; i++)
            {
                profileLineConvention = _Profiles[i].ProfileName + "[&]";
                profileLineConvention += _Profiles[i].CampaignProgress;
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

        public List<ImageBrush> GetPlayerAvatar(string profileName)
        {
            List<ImageBrush> customProfileAvatar = new List<ImageBrush>(); 
            Profile profile = SearchProfile(profileName);
            if (profile != null)
            {
                ImageBrush avatarBeard = new ImageBrush();
                avatarBeard.ImageSource = new BitmapImage(new Uri(pathToProfileAvatars + profile.AvatarBeard, UriKind.Absolute));
                customProfileAvatar.Add(avatarBeard);

                ImageBrush armorBeard = new ImageBrush();
                armorBeard.ImageSource = new BitmapImage(new Uri(pathToProfileAvatars + profile.AvatarArmor, UriKind.Absolute));
                customProfileAvatar.Add(armorBeard);

                ImageBrush vikingBeard = new ImageBrush();
                vikingBeard.ImageSource = new BitmapImage(new Uri(pathToProfileAvatars + profile.AvatarViking, UriKind.Absolute));
                customProfileAvatar.Add(vikingBeard);
            }
            return customProfileAvatar;
        }

        private Profile SearchProfile(string profileName)
        {
            foreach (Profile profile in _Profiles)
            {
                if (profile.ProfileName == profileName)
                    return profile;
            }
            return null;
        }

        /// <summary> Profile
        /// Private nested class to encapsulate 
        /// attributes of a profile
        /// </summary>
        public class Profile
        {
            private string _profileName;
            private int _profileCampaignProgress;
            private string _avatarBeard;
            private string _avatarArmor;
            private string _avatarViking;

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

            public string AvatarBeard
            {
                get { return this._avatarBeard; }
            }

            public string AvatarArmor
            {
                get { return this._avatarArmor; }
            }

            public string AvatarViking
            {
                get { return this._avatarViking; }
            }

            public void incrementCampaignProgress()
            {
                this._profileCampaignProgress++;
            }
        }
    }
}
