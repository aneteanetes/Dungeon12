namespace Dungeon12.Drawing.SceneObjects.UI
{
    using Dungeon;
    using Dungeon.Drawing;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.SceneObjects;
    using Dungeon.Types;
    using Dungeon12;
    using Dungeon12.Classes;
    using Dungeon12.Events;
    using Dungeon12.SceneObjects;
    using System;

    public class PlayerUI : Dungeon12.SceneObjects.SceneObject<Character>
    {
        public override bool Events => true;

        public override bool AbsolutePosition => true;

        public PlayerUI(Character player) : base(player, false)
        {
            Width = 10;
            Height = 22.5;

            OnEvent(new ClassChangeEvent()
            {
                Character = player
            });
        }

        public void OnEvent(ClassChangeEvent @event)
        {
            var player = @event.Character.As<Character>();

            this.ClearChildrens();

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

            var resbarType = player.GetTypeFromAssembly<ResourceBar>();
            var resbar = resbarType.NewAs<ResourceBar>(player);
            resbar.Left = 2.35;
            resbar.Top = 1.1;

            this.AddChild(resbar);
        }

        protected override void CallOnEvent(dynamic obj) => OnEvent(obj);

        private class AvatarSceneObject : Dungeon.Drawing.SceneObjects.ImageControl
        {
            private Character player;

            public AvatarSceneObject(Character player) : base(player.Avatar)
            {
                this.player = player;
            }

            public AvatarSceneObject WithFrame()
            {
                this.AddChild(new ImageControl("Dungeon12.Resources.Images.ui.player.avatar.png")
                {
                    Width = this.Width,
                    Height = this.Height,
                    Left=+.06
                });

                this.AddChild(new LevelUpMask(player));

                this.AddChild(new LevelSceneObject(this.player)
                {
                    Top=1.35,
                    Left=1.6
                });

                return this;
            }

            private class LevelUpMask : ImageControl
            {
                private Character _character;
                public LevelUpMask(Character character) : base("Dungeon12.Resources.Images.GUI.levelup.png")
                {
                    _character = character;
                }

                public override bool Visible => _character.FreeStatPoints > 0;
            }
        }

        private class LevelSceneObject : EmptySceneObject
        {
            public override bool CacheAvailable => false;

            private readonly Character player;

            public LevelSceneObject(Character player)
            {
                this.player = player;
                this.Text = new DrawText(player.Level.ToString(), ConsoleColor.White)
                {
                    Size = 11
                }.Montserrat();
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