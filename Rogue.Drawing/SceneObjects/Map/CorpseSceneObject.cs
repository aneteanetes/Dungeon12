namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Control.Events;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.GUI;
    using Rogue.Drawing.SceneObjects.Base;
    using Rogue.Drawing.SceneObjects.Main.CharacterInfo;
    using Rogue.Loot;
    using Rogue.Map.Objects;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;

    public class CorpseSceneObject : TooltipedSceneObject
    {
        protected override ControlEventType[] Handles => new ControlEventType[]
        {
             ControlEventType.Focus,
             ControlEventType.Click
        };

        private Avatar avatar;
        private Loot loot;
        private Corpse corpse;

        public CorpseSceneObject(Corpse corpse, Avatar avatar) : base(corpse.Enemy.Name, null)
        {
            this.corpse = corpse;
            this.avatar = avatar;
            corpse.OnInteract += this.OnInteraction;

            this.Image = corpse.Enemy.DieImage;
            this.ImageRegion = corpse.Enemy.DieImagePosition;
            this.Height = 0.5;
            this.Width = 1;

            this.Left = corpse.Location.X;
            this.Top = corpse.Location.Y;
        }

        private void OnInteraction()
        {
            if (loot == null)
            {
                loot = LootGenerator.Generate();
            }

            List<ISceneObject> effects = new List<ISceneObject>();

            if (loot.Gold > 0)
            {
                avatar.Character.Gold += loot.Gold;

                effects.Add(new PopupString($"Вы нашли {loot.Gold} золота!", ConsoleColor.Yellow, new Types.Point(this.Left, this.Top), 25, 19, 0.06));

                loot.Gold = 0;
            }

            if (loot.Items.Count != 0)
            {
                var drawClient = Global.DrawClient;

                var inventory = new Inventory(this.ZIndex, new Types.Point(6, 6), loot.Items, true, avatar.Character)
                {
                    Left = this.Left - (drawClient.CameraOffsetX / 32 * -1),
                    Top = this.Top - (drawClient.CameraOffsetY / 32 * -1)
                };

                inventory.Destroy += () =>
                {
                    CheckDestroy();
                    avatar.SafeMode = false;
                };

                effects.Add(inventory);
            }

            this.ShowEffects(effects);

            CheckDestroy();
        }

        private void CheckDestroy()
        {
            if (loot.Gold == 0 && loot.Items.Count == 0)
            {
                corpse.Destroy?.Invoke();
                this.Destroy?.Invoke();
            }
        }

        public override void Click(PointerArgs args)
        {
            var range = corpse.Grow(3);

            if (avatar.IntersectsWith(range))
            {
                avatar.SafeMode = true;
                OnInteraction();
            }
        }
    }
}