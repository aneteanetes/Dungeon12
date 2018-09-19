namespace Rogue.Items.Types
{
    using Rogue.Items.Enums;

    public class Poison : Item
    {

        //public override void TakePoison()
        //{
        //    switch (this.Icon())
        //    {
        //        case '*': { Rogue.RAM.Player.CMP += 1; DrawEngine.InfoWindow.Custom("Вы изготовили 1 яд!"); break; }
        //        case '+': { Rogue.RAM.Player.CMP += 1; DrawEngine.InfoWindow.Custom("Вы изготовили 1 яд!"); break; }
        //        case '#': { Rogue.RAM.Player.CMP += 2; DrawEngine.InfoWindow.Custom("Вы изготовили 2 яда!"); break; }
        //        case '^': { Rogue.RAM.Player.CMP += 2; DrawEngine.InfoWindow.Custom("Вы изготовили 2 яда!"); break; }
        //        default: { break; }
        //    }
        //    DrawEngine.GUIDraw.ReDrawCharStat();
        //    if (Rogue.RAM.Player.CMP > Rogue.RAM.Player.MMP)
        //    { Thread.Sleep(500); DrawEngine.InfoWindow.Custom("Вы несёте с собой слишком много бутылочек с ядом, ваши движения замедленны!"); }
        //}
        public override Stats AvailableStats => Stats.None;
    }
}