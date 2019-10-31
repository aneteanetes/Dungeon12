namespace Dungeon.Items.Types
{
    using Dungeon.Items.Enums;

    public class Poison : Item
    {

        //public override void TakePoison()
        //{
        //    switch (this.Icon())
        //    {
        //        case '*': { Dungeon.RAM.Player.CMP += 1; DrawEngine.InfoWindow.Custom("Вы изготовили 1 яд!"); break; }
        //        case '+': { Dungeon.RAM.Player.CMP += 1; DrawEngine.InfoWindow.Custom("Вы изготовили 1 яд!"); break; }
        //        case '#': { Dungeon.RAM.Player.CMP += 2; DrawEngine.InfoWindow.Custom("Вы изготовили 2 яда!"); break; }
        //        case '^': { Dungeon.RAM.Player.CMP += 2; DrawEngine.InfoWindow.Custom("Вы изготовили 2 яда!"); break; }
        //        default: { break; }
        //    }
        //    DrawEngine.GUIDraw.ReDrawCharStat();
        //    if (Dungeon.RAM.Player.CMP > Dungeon.RAM.Player.MMP)
        //    { Thread.Sleep(500); DrawEngine.InfoWindow.Custom("Вы несёте с собой слишком много бутылочек с ядом, ваши движения замедленны!"); }
        //}
        public override Stats AvailableStats => Stats.None;
    }
}