namespace Dungeon12.SceneObjects.NPC
{
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.Entities.Animations;
    using Dungeon.SceneObjects;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using Dungeon12.Drawing.SceneObjects;
    using Dungeon12.Drawing.SceneObjects.Dialogs.NPC;
    using Dungeon12.Drawing.SceneObjects.Map;
    using Dungeon12.Map;
    using System;
    using System.Linq;

    public class NPCDialogue : EmptyControlSceneObject
    {
        public override int LayerLevel => 50;

        public override bool AbsolutePosition => true;

        private SubjectPanel subjectPanel;
        private AnswerPanel answerPanel;
        private PlayerSceneObject _playerSceneObject;
        
        public NPCDialogue(PlayerSceneObject playerSceneObject, Dungeon12.Map.Objects.Сonversational conversational, Action<ISceneObject> destroyBinding, Action<ISceneObjectControl> controlBinding, GameMap gameMap, MetallButtonControl customizeExit)
        {
            current = this;
            Global.Freezer.World = this;

            _playerSceneObject = playerSceneObject;

            answerPanel = new AnswerPanel(gameMap, playerSceneObject) { DestroyBinding = destroyBinding, ControlBinding = controlBinding };
            subjectPanel = new SubjectPanel(conversational, answerPanel.Select, ExitDialogue, customizeExit, answerPanel.ClearReplics);
            answerPanel.BackAction = subjectPanel.ButtonClick;

            if (conversational.ScreenImage != null)
            {
                AddChild(new DarkRectangle()
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
                    Size = new Point
                    {
                        X = 31,
                        Y = 15
                    }
                };

                var screen = new StandaloneSceneObject(playerSceneObject, conversational.ScreenImage, animMap, conversational.Name, null, new Rectangle(0, 0, 31 * 32, 15 * 32))
                {
                    Height = 15,
                    Width = 31,
                    FreezeForceAnimation = true
                };
                AddChild(screen);
            }

            AddChild(subjectPanel);
            AddChild(answerPanel);

            Height = 22.5;
            Width = 40;
        }

        private static NPCDialogue current;
        public static void ForceExit()
        {
            current?.ExitDialogue();
        }

        private void ExitDialogue()
        {
            Destroy?.Invoke();
            Global.Freezer.World = null;
            Global.Interacting = false;
        }

        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
        }
    }
}
