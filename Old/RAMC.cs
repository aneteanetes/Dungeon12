using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue
{
    public class RAMC
    {
        public MechEngine.Character Player;

        public MechEngine.Character Shadow;

        public MechEngine.StepManager Step;

        public MechEngine.Character Temp;

        public MechEngine.Labirinth Map=new MechEngine.Labirinth();

        public MechEngine.Labirinth InPortal;

        public MechEngine.Monster Enemy;

        public MechEngine.ActiveObject Merch;

        public MechEngine.Questgiver qGiver;

        public List<SystemEngine.SaveLoadFile> LoadFiles;

        public List<MechEngine.Summoned> SummonedList;

        public List<MechEngine.Item> SelfChest = new List<MechEngine.Item>();

        public List<SystemEngine.Timers> Timers;

        public List<string> RealLog = new List<string>();

        public ConsoleColor CUIColor;

        public SystemEngine.AbilityShadow SecondAbility = new SystemEngine.AbilityShadow();

        public int EnemyX;

        public int EnemyY;

        public bool DungeonSound = false;

        public bool WasHelp = false;
        public bool WasHelpAgain = false;

        public bool EducationFight = true;

        public string AddonName;

        public SystemEngine.QuestFlag Flags = new SystemEngine.QuestFlag();

        public bool YQuestmain=false;

        public bool EnemyBool = false;

        public bool History = false;

        public bool Bone = true;

        public List<string> Log;

        public List<SystemEngine.ArmorSet> ArmSet = new List<SystemEngine.ArmorSet>();

        public MechEngine.InventoryTab PopUpTab;

        public MechEngine.InventoryTab iTab;

        public MechEngine.InventoryTab MerchTab;

        public MechEngine.InventoryTab Qtab;

        public SystemEngine.ColorSettings ColorSet;

        public SystemEngine.SoundSettings SoundSet;

        public SystemEngine.GraphicHeroSettings GraphHeroSet;        

        public SystemEngine.Judge Judge;        
    }
}
