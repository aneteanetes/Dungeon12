using Dungeon.GameObjects;
using System.Collections.Generic;

namespace InTheWood.Entities.MapScreen
{
    public class Map : GameComponent
    {
        public List<SectorConnection> Connections { get; set; } = new List<SectorConnection>();

        public List<Sector> Sectors { get; set; } = new List<Sector>();

        public void AddSector(Sector sector)
        {
            Sectors.Add(sector);
        }

        public void AddSector(Sector sector, SectorConnection connection)
        {
            this.AddSector(sector);
            this.Connections.Add(connection);
        }
    }
}