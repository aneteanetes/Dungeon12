namespace Dungeon12.Drawing.SceneObjects.Dialogs.NPC
{
    using Dungeon.Control;
    using Dungeon.Control.Keys;
    using Dungeon12.Conversations;
    using Dungeon.Drawing;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon12.Drawing.SceneObjects.Map;
    using Dungeon.GameObjects;
    using Dungeon12.Map;
    using Dungeon12.SceneObjects;
    using Dungeon.SceneObjects;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Dungeon;
    using Dungeon12;

    public class AnswerPanel : EmptyHandleSceneControl
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        private GameMap gameMap;
        private PlayerSceneObject playerSceneObject;
        public Action BackAction { get; set; }

        public AnswerPanel(GameMap gameMap, PlayerSceneObject playerSceneObject)
        {
            this.gameMap = gameMap;
            this.playerSceneObject = playerSceneObject;

            //this.Opacity = 0.8;

            Image = "ui/dialogs/answerpanel.png".AsmImgRes();

            this.Top = 15;
            this.Left = 0;

            this.Height = 7.5;
            this.Width = 31;

            dialogText = this.AddTextCenter("".AsDrawText().InSize(12).Montserrat().WithWordWrap(), false, false);
            dialogText.Left = 1;
            dialogText.Top = .5;
            dialogText.Width = 30;
            dialogText.Height = 4;
        }

        public void ClearReplics()
        {
            dialogText.Text.SetText("");
            currentAnswers.ForEach(a =>
            {
                this.RemoveChild(a);
                a.Destroy?.Invoke();
            });
        }

        private TextControl dialogText;

        private double space;

        private List<AnswerClickable> currentAnswers = new List<AnswerClickable>();

        private Subject lastSubject;

        public void Select(Subject subject) => SelectSubject(subject);

        private void SelectSubject(Subject subject, string prevText = null)
        {
            lastSubject = subject;
            currentAnswers.ForEach(a =>
            {
                this.RemoveChild(a);
                a.Destroy?.Invoke();
            });

            List<Replica> replics = new List<Replica>();

            var triggeredVariable = subject.Variables.FirstOrDefault(@var => var.Triggered);
            if (triggeredVariable != null)
            {
                dialogText.Text.SetText(triggeredVariable.Replica.Text);
                replics.AddRange(triggeredVariable.Replica.Replics);
            }
            else
            {
                dialogText.Text.SetText(prevText ?? subject.Text);
                replics.AddRange(subject.Replics.Where(x => x.Shown));
            }

            space = this.MeasureText(dialogText.Text,dialogText).Y / 32;

            var y = space + 1;

            for (int i = 0; i < replics.Count; i++)
            {
                var replica = replics[i];
                var answer = new AnswerClickable(i + 1, replica, this.Select)
                {
                    Left = 1,
                    Top = y
                };

                this.AddChild(answer);

                currentAnswers.Add(answer);
                y++;
            }
        }

        public void Select(Replica replica)
        {
            var triggeredVariable = replica.Conversation.Variables.FirstOrDefault(@var => var.Triggered && var.TriggeredFrom == replica.Tag);

            if (triggeredVariable != null)
            {
                if (triggeredVariable.Replica != default)
                    replica = triggeredVariable.Replica;
            }

            if (replica.Variables != null)
            {
                foreach (var variable in replica.Variables)
                {
                    variable.Trigger(replica.Tag);
                }
            }

            if (replica.Escape)
            {
                FireTrigger(replica);
                currentAnswers.ForEach(a =>
                {
                    dialogText.Text.SetText("");
                    this.RemoveChild(a);
                    a.Destroy?.Invoke();
                });
                BackAction?.Invoke();
                return;
            }

            if (replica.Replics.Count == 0)
            {
                var triggerPassed = FireTrigger(replica);
                SelectSubject(lastSubject, triggerPassed ? dialogText.Text.StringData : replica.Text);
                return;
            }

            currentAnswers.ForEach(a =>
            {
                this.RemoveChild(a);
                a.Destroy?.Invoke();
            });

            dialogText.Text.SetText(replica.Text ?? " ");
            space = this.MeasureText(dialogText.Text,dialogText).Y / 32;

            var y = space + 1;

            var shownReplics = replica.Replics.ToArray();

            for (int i = 0; i < shownReplics.Length; i++)
            {
                var nextReplica = shownReplics[i];
                var answer = new AnswerClickable(i + 1, nextReplica, this.Select)
                {
                    Left = 1,
                    Top = y
                };

                this.AddChild(answer);

                currentAnswers.Add(answer);
                y++;
            }

            FireTrigger(replica);
        }

        private bool FireTrigger(Replica replica)
        {
            if (replica.TriggerClass != null)
            {
                var trigger = replica.TriggerClass
                    .Trigger<IConversationTrigger>();

                bool fire = true;
                var storeName = replica.TriggerClass + string.Join("`%", replica.TriggerClassArguments ?? new string[0]);
                if (trigger.Storable)
                {
                    fire = playerSceneObject.Component.Entity[storeName] != default;
                }
                if (fire)
                {
                    var triggerText = trigger.Trigger(playerSceneObject, gameMap, replica.TriggerClassArguments,replica);
                    if (triggerText != default)
                    {
                        dialogText.Text.SetText(triggerText.StringData);
                        space = this.MeasureText(dialogText.Text, dialogText).Y / 32;

                        if (trigger.Storable)
                        {
                            playerSceneObject.Component.Entity[storeName] = true;
                        }

                        return triggerText.StringData != string.Empty;
                    }
                    return true;
                }
            }

            return false;
        }

        private class AnswerClickable : EmptyHandleSceneControl
        {
            public override bool AbsolutePosition => true;

            public override bool CacheAvailable => false;

            private Action<Replica> select;
            private Replica repl;

            private int count;

            public AnswerClickable(int count, Replica replica, Action<Replica> select)
            {
                this.count = count;
                this.select = select;
                var txt = new DrawText($"{count}. {replica.Answer}").Montserrat();

                this.Text = txt;

                this.repl = replica;

                this.Width = MeasureText(txt).X / 32;
                this.Height = 1;

                Key key = Key.None;

                switch (count)
                {
                    case 1: key = Key.D1; break;
                    case 2: key = Key.D2; break;
                    case 3: key = Key.D3; break;
                    case 4: key = Key.D4; break;
                    case 5: key = Key.D5; break;
                    case 6: key = Key.D6; break;
                    case 7: key = Key.D7; break;
                    case 8: key = Key.D8; break;
                    case 9: key = Key.D9; break;
                    default:
                        break;
                }

                this.KeyHandles = new Key[]
                {
                    key
                };
            }

            public override void Focus()
            {
                this.Text.ForegroundColor = new DrawColor(System.ConsoleColor.Yellow);
                base.Focus();
            }

            public override void Unfocus()
            {
                this.Text.ForegroundColor = new DrawColor(System.ConsoleColor.White);
                base.Unfocus();
            }

            public override void Click(PointerArgs args)
            {
                select?.Invoke(this.repl);
            }

            public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
            {
                if (!hold)
                {
                    select?.Invoke(this.repl);
                }
            }

            protected override void CallOnEvent(dynamic obj)
            {
                OnEvent(obj);
            }
        }
    }
}