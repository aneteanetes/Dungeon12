using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities;
using Dungeon12.Entities.Enums;
using System;

namespace Dungeon12.SceneObjects.Create
{
    public class AvatarSelector : SceneControl<Hero>
    {
        private ImageObject AvatarViewer;

        private int index = 1;

        public AvatarSelector(Hero component) : base(component)
        {
            AvatarViewer = this.AddChild(new ImageObject($"Avatars/{component.Class.Short()}1.png")
            {
                Width = 100,
                Height = 158,
                Left = 30
            });

            this.Width = 160;
            this.Top = 158;

            this.AddChild(new IndexButton(false)
            {
                OnClick = Leaf,
                Top = 57
            });

            this.AddChild(new IndexButton(true)
            {
                OnClick = Leaf,
                Top = 57,
                Left = 133
            });

            Component.ClassChange += (was, now) =>
            {
                AvatarViewer.Image = $"Avatars/{now.Short()}{index}.png".AsmImg();
            };
        }

        private void Leaf(bool next)
        {
            if(next)
            {
                if (index == 6)
                    index = 1;
                else
                    index++;
            }
            else
            {
                if (index == 1)
                    index = 6;
                else
                    index--;
            }

            AvatarViewer.Image = $"Avatars/{Component.Class.Short()}{index}.png".AsmImg();
            Component.Avatar = AvatarViewer.Image;
            Component.Sex = Component.Class.Sex(index);
        }

        private class IndexButton : EmptySceneControl, ITooltiped
        {
            private bool _next;

            public IndexButton(bool next)
            {
                _next = next;
                this.Width = 30;
                this.Height = 30;
                this.Image = $"UI/start/{(next ? "right" : "left")}arrow.png".AsmImg();
            }

            public Action<bool> OnClick { get; set; }

            public IDrawText TooltipText => (_next ? Global.Strings.Next : Global.Strings.Prev).AsDrawText().Gabriela();

            public bool ShowTooltip => true;

            public override void Click(PointerArgs args)
            {
                OnClick(_next);
                base.Click(args);
            }

            public void RefreshTooltip() { }
        }
    }
}