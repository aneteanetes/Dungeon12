namespace Dungeon.Drawing.SceneObjects.Dialogs.NPC
{
    using Dungeon.Control.Keys;
    using Dungeon.Drawing.SceneObjects.Base;
    using Dungeon.Drawing.SceneObjects.Common;
    using Dungeon.Drawing.SceneObjects.Map;
    using Dungeon.Entites.Animations;
    using Dungeon.Map;
    using Dungeon.View.Interfaces;
    using System;
    using System.Linq;

    public class NPCDialogue : HandleSceneControl
    {
        public override int Layer => 50;

        public override bool AbsolutePosition => true;

        private SubjectPanel subjectPanel;
        private AnswerPanel answerPanel;
        private PlayerSceneObject _playerSceneObject;

        public NPCDialogue(PlayerSceneObject playerSceneObject, Dungeon.Map.Objects.Сonversational conversational, Action<ISceneObject> destroyBinding, Action<ISceneObjectControl> controlBinding, GameMap gameMap)
        {
            Global.FreezeWorld = this;

            this._playerSceneObject = playerSceneObject;

            answerPanel = new AnswerPanel(gameMap,playerSceneObject) { DestroyBinding = destroyBinding, ControlBinding= controlBinding };
            subjectPanel = new SubjectPanel(conversational, answerPanel.Select, this.ExitDialogue);
                       
            if (conversational.ScreenImage != null)
            {
                this.AddChild(new DarkRectangle()
                {
                    Opacity = 1,
                    Height = 22.5,
                    Width = 40
                });

                AnimationMap animMap = new AnimationMap()
                {
                    TileSet = conversational.ScreenImage,
                    TilesetAnimation = false,
                    FramesPerSecond = conversational.Frames,
                    FullFrames = Enumerable.Range(1, conversational.Frames + 1).Select(f => conversational.ScreenImage.Replace("(1)", $"({f})")).ToArray(),
                    Size = new Types.Point
                    {
                        X = 31,
                        Y = 15
                    }
                };

                var screen = new StandaloneSceneObject(playerSceneObject, conversational.ScreenImage, animMap, conversational.Name, null, new Types.Rectangle(0, 0, 31 * 32, 15 * 32))
                {
                    Height = 15,
                    Width = 31,
                    FreezeForceAnimation = true
                };
                this.AddChild(screen);
            }

            this.AddChild(subjectPanel);
            this.AddChild(answerPanel);

            this.Height = 22.5;
            this.Width = 40;
        }

        private void ExitDialogue()
        {
            this.Destroy?.Invoke();
            Global.FreezeWorld = null;
            SkillControl.RestoreClick();
        }

        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
        }
    }
}
