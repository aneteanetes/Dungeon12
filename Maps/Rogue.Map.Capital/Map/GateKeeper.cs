namespace Rogue.Map.Capital.Map
{
    public class GateKeeper : CapitalDoor
    {
        //public int Location;
        //public override bool Use()
        //{
        //    PlayEngine.EnemyMoves.Move(false);
        //    this.Quarter = 2;
        //    DrawEngine.ActiveObjectDraw.Draw(new List<string>()
        //            {
        //                "{1} - Верхний зал",
        //                "{2} - Зал огня",
        //                "{3} - Зал воды",
        //                "{4} - Зал земли",
        //                "{5} - Зал воздуха",
        //                "{6} - Нижний зал",
        //                "{7} - Приёмная",
        //            }, this);
        //    if (PlayEngine.GateKeeperGamePlay.Main(this))
        //    {
        //        LabirinthEngine.Create(1, true, this.Quarter);
        //        if (this.Quarter == 2)
        //        {
        //            Rogue.RAM.Map.Map[68][11].Player = null; Rogue.RAM.Map.Map[34][11].Vision = ' ';
        //        }
        //        int x = 0, y = 0;
        //        switch (this.Location)
        //        {
        //            case 0: { x = 33; y = 3; break; }
        //            case 1: { x = 5; y = 10; break; }
        //            case 2: { x = 17; y = 8; break; }
        //            case 3: { x = 33; y = 8; break; }
        //            case 4: { x = 26; y = 13; break; }
        //            case 5: { x = 38; y = 17; break; }
        //            case 6: { x = 52; y = 11; break; }
        //        }
        //        Rogue.RAM.Map.Map[x][y].Player = Rogue.RAM.Player; Rogue.RAM.Map.Map[34][11].Vision = Rogue.RAM.Player.Icon;
        //        PlayEngine.EnemyMoves.Move(true);
        //        PlayEngine.GamePlay.Play();
        //        return true;
        //    }
        //    else
        //    {
        //        PlayEngine.EnemyMoves.Move(true);
        //        PlayEngine.GamePlay.Play();
        //        return false;
        //    }
        //}
    }
}