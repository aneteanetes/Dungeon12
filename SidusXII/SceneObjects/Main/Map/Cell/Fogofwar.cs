using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using SidusXII.Models.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SidusXII.SceneObjects.Main.Map.Cell
{
    public class Fogofwar : EmptySceneObject
    {
        public bool IsDefault { get; set; } = true;

        public ImageObject L { get; set; } = new ImageObject("GUI/Parts/fog/parts/l.png".AsmImg()) { CacheAvailable = false };

        public ImageObject LT { get; set; } = new ImageObject("GUI/Parts/fog/parts/lt.png".AsmImg()) { CacheAvailable = false };

        public ImageObject LB { get; set; } = new ImageObject("GUI/Parts/fog/parts/lb.png".AsmImg()) { CacheAvailable = false };

        public ImageObject R { get; set; } = new ImageObject("GUI/Parts/fog/parts/r.png".AsmImg()) { CacheAvailable = false };

        public ImageObject RT { get; set; } = new ImageObject("GUI/Parts/fog/parts/rt.png".AsmImg()) { CacheAvailable = false };

        public ImageObject RB { get; set; } = new ImageObject("GUI/Parts/fog/parts/rb.png".AsmImg()) { CacheAvailable = false };

        public ImageObject C { get; set; } = new ImageObject("GUI/Parts/fog/parts/c.png".AsmImg()) { CacheAvailable = false };

        public Fogofwar(MapCellComponent mapCellComponent)
        {
            if (mapCellComponent.FogPartsForDelete.IsNotEmpty())
            {
                AddChild(L);
                AddChild(LT);
                AddChild(LB);
                AddChild(R);
                AddChild(RT);
                AddChild(RB);

                IsDefault = false;

                mapCellComponent.FogPartsForDelete.ForEach(Clear);

                if (mapCellComponent.FogPartsForDelete.Distinct().Count() == 6)
                {
                    AddChild(C);
                }
            }
            else
            {
                AddChild(new ImageObject("GUI/Parts/fogofwar.png".AsmImg()) { CacheAvailable = false });
            }
        }

        public void Clear(MapCellPart part)
        {
            var fogPart = this.GetPropertyExpr<ImageObject>(part.ToString());
            RemoveChild(fogPart);
        }
    }
}
