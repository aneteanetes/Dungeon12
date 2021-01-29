using Dungeon.Control.Pointer;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.Map.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using Dungeon;

namespace Dungeon12.SceneObjects.Map
{
    public class BarrelSceneObject : ClickActionSceneObject<Barrel>
    {
        public override bool CacheAvailable => false;

        protected override bool SilentTooltip => true;

        public override string Cursor => "take";

        public BarrelSceneObject(PlayerSceneObject playerSceneObject, Barrel @object) : base(@object, @object.Name)
        {
            Left = @object.Location.X + .125;
            Top = @object.Location.Y;
            Width = .75;
            Height = .75;

            this.TooltipText = @object.Used ? "Пустая бочка" : @object.Name;
        }

        public override string Image => $"Objects/barrels/barell_{(@object.Used ? "empty" : @object.IdentifyName.ToLowerInvariant())}.png".AsmImg();

        protected override void Action(MouseButton mouseButton)
        {
            if (!@object.Used)
            {
                @object.Use(playerSceneObject.Avatar.Character);
                @object.Used = true;
                this.TooltipText = "Пустая бочка";
            }
        }
    }
}