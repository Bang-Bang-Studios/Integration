using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pentago.GUI;

namespace Pentago.GameCore
{
    class Quotes
    {
        public Quotes()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {

            LoadingQuotes = new List<string>();
            VikingQuotes = new List<string>();
            VikingNames = new List<string>();
            ElderQuotes = new List<string>();

            Create();
        }

        private void Create()
        {
            CreateVikingQuotes();
            CreateLoadingQuotes();
            CreateVikingNames();
            CreateElderQuotes();
        }

        public string Viking
        {
            get
            {
                //speechCounter++;
                //if (speechCounter > VikingQuotes.Count() - 1)
                //{
                //    speechCounter = 0;
                //}
                Random rand = new Random();
                return VikingQuotes[rand.Next(VikingQuotes.Count - 1)];
            }
        }

        public string LoadingScreen
        {
            get
            {
                Random rand = new Random();
                return LoadingQuotes[rand.Next(LoadingQuotes.Count() - 1)];
            }
        }

        public string Names
        {
            get
            {
                Random rand = new Random();
                return VikingNames[rand.Next(VikingNames.Count() - 1)];
            }
        }

        public string Elder
        {
            get
            {
                speechCounter++;
                if (speechCounter > ElderQuotes.Count() - 1)
                {
                    speechCounter = 0;
                }
                return ElderQuotes[speechCounter];
            }
        }

        private void CreateVikingQuotes()
        {
            //VikingQuotes.Add("horgis borgis njord");
            VikingQuotes.Add("Ice giants are nothing to dragon fire");
            VikingQuotes.Add("What are your orders Warlord?");
            VikingQuotes.Add("Your dragons are restless");
            VikingQuotes.Add("CHARRRGGGEE!!!!");
            VikingQuotes.Add("The ice giants will stop at nothing to freeze this world");
            VikingQuotes.Add("I hope my wife isn't cooking freijord gopher again. YUCK!");
            VikingQuotes.Add("That's an...interesting strategy my Lord");
            VikingQuotes.Add("Stop poking me ya over sized chipmunk");
            VikingQuotes.Add("The ice giants can't handle your attack keep it up!");
            VikingQuotes.Add("Sometime's I wear my wife's clothing when she isn't home");
            VikingQuotes.Add("It is a great honor to defend the world from a icy death");
            VikingQuotes.Add("Ice giant heart's are as cold as they are dark");
            VikingQuotes.Add("I once defeated 3 ice giants using only my dagger and an ill tempered bunny");
            VikingQuotes.Add("You truly are our greatest strategist!");
            VikingQuotes.Add("I'm going to poke you back with my sword if you don't stop that");
            VikingQuotes.Add("YOLO. Yodeling on Large Oxen");
            VikingQuotes.Add("SWAG...Sword, War Axe, Grunting");
        }

        private void CreateLoadingQuotes()
        {
            LoadingQuotes.Add("Vikings are ");
        }

        private void CreateVikingNames()
        {
            VikingNames.Add("War Lord");
        }

        private void CreateElderQuotes()
        {
            ElderQuotes.Add("");
        }

        private int speechCounter = -1;
        private List<string> LoadingQuotes;
        private List<string> VikingQuotes;
        private List<string> VikingNames;
        private List<string> ElderQuotes;
    }
}
