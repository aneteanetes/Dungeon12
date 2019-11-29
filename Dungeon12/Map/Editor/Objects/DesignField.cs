namespace Dungeon12.Map.Editor.Objects
{
    using Newtonsoft.Json;
    using Dungeon.Data.Region;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class DesignField
    {
        private readonly List<DesignCell[][]> Map = new List<DesignCell[][]>();

        public DesignCell[][] this[int level]
        {
            get
            {
                if (Map.Count < level)
                {
                    for (int i = 0; i < level; i++)
                    {
                        if (Map.ElementAtOrDefault(i) == null)
                        {
                            DesignCell[][] hundred = new DesignCell[100][];
                            for (int j = 0; j < 100; j++)
                            {
                                hundred[j] = new DesignCell[100];
                            }
                            Map.Add(hundred);
                        }
                    }
                }

                return Map[level-1];
            }
        }

        public List<RegionPart> Save()
        {
            List<RegionPart> rp = new List<RegionPart>();

            for (int lvl = 0; lvl < this.Map.Count; lvl++)
            {
                for (int x = 0; x < 100; x++)
                {
                    for (int y = 0; y < 100; y++)
                    {
                        var obj = this[lvl+1][x][y];
                        if (obj != null)
                        {
                            rp.Add(new RegionPart
                            {
                                Image = obj.Image,
                                Region = obj.ImageRegion,
                                Position = new Dungeon.Types.Point(x, y),
                                Obstruct = obj.Obstruction,
                                Layer = lvl + 1
                            });
                        }
                    }
                }
            }

            File.WriteAllText("map.json", JsonConvert.SerializeObject(rp));

            var clear = rp.Where(x => x.Obstruct);
            File.WriteAllText("map_publish.json", JsonConvert.SerializeObject(clear));

            return rp;
        }
    }
}