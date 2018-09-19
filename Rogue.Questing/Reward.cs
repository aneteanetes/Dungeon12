namespace Rogue.Questing
{
    using System.Collections.Generic;

    /// <summary>
    /// Can use for quests, main quests, chests of treasure, etc. Any way when u need much objects to give character
    /// NO MORE 4 REWARDS!!! (item==item can be more 1; ex: Health potion x20)
    /// </summary>
    public class Reward
    {
        public Reward()
        { Items = new List<Item>(); Abilityes = new List<Ability>(); Perks = new List<Perk>(); Exp = new List<int>(); Gold = new List<int>(); }
        public List<Item> Items; public List<Ability> Abilityes; public List<Perk> Perks; public List<int> Exp; public List<int> Gold;
    }
}