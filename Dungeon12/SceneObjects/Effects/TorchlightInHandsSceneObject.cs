namespace Dungeon12.Drawing.SceneObjects.Effects
{
    using Dungeon.Drawing;
    using Dungeon.Drawing.Impl;
    using Dungeon.SceneObjects;
    using System.Collections.Generic;

    public class TorchlightInHandsSceneObject : EmptySceneObject
    {
        public override bool CacheAvailable => false;

        public TorchlightInHandsSceneObject()
        {
            this.Image = "Dungeon12.Resources.Images.Items.Doll.torchlighthands1.png";

            this.Height = 0.5;
            this.Width = 0.5;
            this.Top += 0.3;

            this.AddChild(new Flame());
        }


        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
        }

        private class Flame : EmptySceneObject
        {
            public override bool CacheAvailable => false;

            public Flame()
            {
                this.Left = 0.2;
                this.Top = 0.2;

                this.Light = new Light()
                {
                    Range = 1,
                    Color = new DrawColor(245, 132, 66)
                };

                this.Effects = new List<Dungeon.View.Interfaces.IEffect>()
                {
                    new ParticleEffect()
                    {
                        Name="Torchlight",
                        Scale = .1
                    }
                };

                Dungeon.Global.Time
                    .After(18).Do(() => Light.Range = 1)
                    .After(19).Do(() => Light.Range = 1.25f)
                    .After(20).Do(() => Light.Range = 1.5f)
                    .After(21).Do(() => Light.Range = 2f)
                    .After(22).Do(() => Light.Range = 2.5f)
                    .After(23).Do(() => Light.Range = 3f)
                    .After(3).Do(() => Light.Range = 2.5f)
                    .After(4).Do(() => Light.Range = 2f)
                    .After(5).Do(() => Light.Range = 1.5f)
                    .After(6).Do(() => Light.Range = 1.25f)
                    .Auto();
            }


            protected override void CallOnEvent(dynamic obj)
            {
                OnEvent(obj);
            }
        }
    }
}