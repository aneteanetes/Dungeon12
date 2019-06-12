namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Control.Events;
    using Rogue.Drawing.GUI;
    using Rogue.Loot;
    using Rogue.Map.Objects;
    using System;

    public class CorpseSceneObject : TooltipedSceneObject
    {
        protected override ControlEventType[] Handles => new ControlEventType[]
        {
             ControlEventType.Focus
        };

        public CorpseSceneObject(Corpse corpse) : base(corpse.Enemy.Name, null)
        {
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
            var loot = LootGenerator.Generate();

            this.ShowEffects(new System.Collections.Generic.List<View.Interfaces.ISceneObject>()
            {
                new PopupString($"Вы нашли {loot.Gold} золота!", ConsoleColor.Yellow,new Types.Point(this.Left,this.Top),25,19,0.06)
            });

            this.Destroy?.Invoke();
        }
    }
}