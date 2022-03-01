using Dungeon;
using Dungeon.Drawing;
using Dungeon.SceneObjects;
using Dungeon12.Entities.Talks;
using System.Collections.Generic;

namespace Dungeon12.SceneObjects.Talk
{
    public class MainTextSceneObject : EmptySceneObject
    {
        public static MainTextSceneObject Instance;

        private TextControl Title;

        // 14 lines

        List<ReplicaText> Replics = new List<ReplicaText>();
        int Index = 0;

        ReplySceneObject replySceneObject;

        public MainTextSceneObject(ReplySceneObject replySceneObject)
        {
            this.replySceneObject = replySceneObject;

            this.Width = 962;
            this.Height = 637;
            this.Image = @"Talk/text.png".AsmImg();
            Instance = this;

            Title = this.AddTextCenter("Title".AsDrawText().Gabriela().InSize(25));
            Title.Top = 10;
            Title.Visible = false;
        }

        private double ReplTop;

        public void Start(Subject subject)
        {
            Title.Visible = true;
            Title.SetText(subject.Name.AsDrawText().Gabriela().InSize(25));
            this.CenterText(Title, vertical: false);
            Start(subject.Replica);
        }

        public void Start(Replica replica, bool answer = false)
        {
            ReplTop = Title.Top + this.MeasureText(Title.Text).Y + 15;

            ReplicaText.ProduceReplics(replica.Text,
                answer ? Global.Game.Party.Hero1.Name : replica.Subject.Dialogue.Personaly,
                answer ? DrawColor.Cyan : DrawColor.Yellow)
                .ForEach(r =>
                {
                    var robj = this.AddChild(r);
                    robj.Top = ReplTop;
                    robj.Top += Replics.Count * 15;
                    robj.Left += 15;
                    Replics.Add(robj);
                });

            replySceneObject.Bind(replica);
        }

        private class ReplicaText : EmptySceneObject
        {
            public ReplicaText(string text, string name = null, DrawColor namecolor = null)
            {
                this.Width = 940;

                if (namecolor == null)
                    namecolor = DrawColor.Yellow;

                var leftOffset = 0d;

                if (name.IsNotEmpty())
                {
                    var nametxt = this.AddChild(new TextControl(DrawText(name + ":", namecolor)));
                    leftOffset = this.MeasureText(nametxt.Text).X + 5;
                }

                var texttxt = this.AddChild(new TextControl(DrawText(text)));
                texttxt.Left = leftOffset;
            }

            private static DrawText DrawText(string text, DrawColor color = null)
            {
                if (color == null)
                    color = DrawColor.White;

                return text.AsDrawText().Gabriela().InSize(13).InColor(color);
            }

            public static List<ReplicaText> ProduceReplics(string text, string name, DrawColor nameColor = null)
            {
                List<ReplicaText> repls = new List<ReplicaText>();

                var n = name + " : ";

                var txt = DrawText(n+text);
                //var measure = DungeonGlobal.DrawClient.MeasureText(txt, new EmptySceneControl() { Width = 940 });

                var chunks = (n + text).Split(165);
                foreach (var chunk in chunks)
                {
                    var str = chunk;
                    var nme = n;

                    if (chunk.Contains(n))
                    {
                        str = str.Replace(n, "");
                        nme = nme.Replace(" : ", "");
                    }
                    else nme = null;

                    repls.Add(new ReplicaText(str, nme, nameColor));
                }

                return repls;
            }
        }
    }
}