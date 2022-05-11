using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon12.Entities.Talks;
using System.Collections.Generic;

namespace Dungeon12.SceneObjects.Talk
{
    public class ReplySceneObject : SceneControl<Replica>
    {
        List<ReplyClickSceneObject> replics = new List<ReplyClickSceneObject>();
        static ReplySceneObject Instance;

        public ReplySceneObject(Replica component) : base(component)
        {
            Instance = this;

            this.Width = 961;
            this.Height = 283;
            this.Image = @"Talk/replicsback.png".AsmImg();

            var alltop = 35;

            for (int i = 0; i < 4; i++)
            {
                var r = new ReplyClickSceneObject(null);
                replics.Add(r);
                r.Top = alltop;
                r.Left = 25;
                r.Visible = false;
                this.AddChild(r);

                alltop += 60;
            }
        }

        public void Bind(Replica replica)
        {
            var randomized = replica.Lines.Shuffle();

            for (int i = 0; i < randomized.Length; i++)
            {
                var replclick = replics[i];
                replclick.Bind(randomized[i]);
                replclick.Visible = true;
            }
        }

        private class ReplyClickSceneObject : SceneControl<ReplicaLine>
        {
            private ImageObject icon;
            private TextObject text;

            public ReplyClickSceneObject(ReplicaLine component) : base(component)
            {
                this.Width = 940;
                this.Height = 50;

                this.AddChild(icon = new ImageObject($"Talk/1.png".AsmImg()));
                this.AddChild(text = new TextObject("-".AsDrawText().Gabriela().InSize(16))
                {
                    Left = 55,
                    Top = 15
                });
            }

            public void Bind(ReplicaLine line)
            {
                this.BindComponent(line);
                BindText();
                var inttype = (int)line.Type;
                this.icon.Image = $"Talk/{inttype}.png".AsmImg();
            }

            private void BindText()
            {
                text.SetText(Component.Text.AsDrawText().Gabriela().InSize(14));
            }

            public override void Focus()
            {
                if (inanim)
                    return;
                text.Text.ForegroundColor = DrawColor.Yellow;
                base.Focus();
            }

            public override void Unfocus()
            {
                if (inanim)
                    return;
                text.Text.ForegroundColor = DrawColor.White;
                base.Unfocus();
            }

            private static bool inanim = false;

            public override void Click(PointerArgs args)
            {
                if (inanim)
                    return;

                inanim = true;
                this.Popup(args, $"+{Component.Weight}".AsDrawText().Gabriela().InSize(12).InColor(Component.Type.TypeColor()), .5,1.1)
                    .After(() =>
                    {
                        Global.Game.Party.Fame.Add(Component.Weight, Component.Type);
                        MainTextSceneObject.Instance.Start(Component.Replica);
                        Instance.replics.ForEach(r => r.Visible = false);
                        inanim = false;
                    });
                base.Click(args);
            }
        }
    }
}