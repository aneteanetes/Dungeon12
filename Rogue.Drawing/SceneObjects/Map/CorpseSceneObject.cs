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

    public class CorpseSceneObject : ClickActionSceneObject<Corpse>
    {
        private Avatar avatar;
        private Loot loot;
        private Corpse corpse;

        public CorpseSceneObject(PlayerSceneObject playerSceneObject,Corpse corpse) : base(playerSceneObject,corpse,corpse.Enemy.Name)
        {
            this.corpse = corpse;
            this.avatar = playerSceneObject.Avatar;
            corpse.OnInteract += this.OnInteraction;

            this.Image = corpse.Enemy.DieImage;
            this.ImageRegion = corpse.Enemy.DieImagePosition;
            this.Height = 0.5;
            this.Width = 1;

            this.Left = corpse.Location.X;
            this.Top = corpse.Location.Y;
        }

        private Inventory lootBox = null;

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

                lootBox = new Inventory(this.ZIndex, new Types.Point(6, 6), loot.Items, true, avatar.Character)
                {
                    Left = this.Left - (drawClient.CameraOffsetX / 32 * -1),
                    Top = this.Top - (drawClient.CameraOffsetY / 32 * -1)
                };

                lootBox.Destroy += () =>
                {
                    CheckDestroy();
                    avatar.SafeMode = false;
                };

                effects.Add(lootBox);
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

        protected override void Action(MouseButton mouseButton)
        {
            avatar.SafeMode = true;
            OnInteraction();
        }

        protected override void StopAction()
        {
            lootBox.Destroy?.Invoke();
        }
    }
}