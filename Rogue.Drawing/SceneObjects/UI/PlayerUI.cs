namespace Rogue.Drawing.SceneObjects.UI
{
    using Rogue.Drawing.Impl;
    using Rogue.Entites.Alive.Character;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;

    public class PlayerUI : SceneObject
    {
        public override bool AbsolutePosition => true;

        public PlayerUI(Player player)
        {
            this.AddChild(new AvatarSceneObject(player)
            {
                Width = 2,
                Height = 2,
                Left = 0.25,
                Top = 0.25
            }.WithFrame());

            this.AddChild(new ResourceBarHP(player)
            {
                Left = 2.35,
                Top = 0.4
            });


            var resbarType= player.GetTypeFromAssembly<ResourceBar>();
            var resbar = resbarType.NewAs<ResourceBar>(player);
            resbar.Left = 2.35;
            resbar.Top = 1.1;

            this.AddChild(resbar);
        }

        private class AvatarSceneObject : ImageControl
        {
            private Player player;

            public AvatarSceneObject(Player player) : base(player.Avatar)
            {
                this.player = player;
            }

            public AvatarSceneObject WithFrame()
            {
                this.AddChild(new ImageControl("Rogue.Resources.Images.ui.player.avatar.png")
                {
                    Width = this.Width,
                    Height = this.Height,
                    Left=+.06
                });

                this.AddChild(new LevelSceneObject(this.player)
                {
                    Top=1.35,
                    Left=1.25
                });

                return this;
            }
        }

        private class LevelSceneObject : SceneObject
        {
            public override bool CacheAvailable => false;

            private readonly Player player;

            public LevelSceneObject(Player player)
            {
                this.player = player;
                this.Text = new DrawText(player.Level.ToString(), ConsoleColor.White)
                {
                    Size = 11
                }.Monserrat();
            }
            
            public override Rectangle Position
            {
                get
                {
                    this.Text.SetText(player.Level.ToString());
                    var basePos = base.Position;

                    if (player.Level >= 10)
                    {
                        basePos.X -= 0.12;
                    }

                    return basePos;
                }

            }
        }
    }
}