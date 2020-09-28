using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.SceneObjects
{
    public class Toast : EmptyControlSceneObject
    {
        public override bool Filtered => false;

        public override bool AbsolutePosition => true;

        public override bool CacheAvailable => false;

        public int Frames { get; set; } = 400;

        public Toast(DrawText component)
        {
            this.Width = 11;
            this.Height = 1;

            this.Left = 40 / 2 - this.Width / 2;
            this.Top = .5 + (ShowedBoxes * 1.5);

            order = CurrentBoxes.Count;

            var key = component.StringData;
            if (!CurrentBoxes.ContainsKey(key))
            {
                CurrentBoxes.Add(key, this);
                ShowedBoxes++;
            }

            this.Image = "Dungeon12.Resources.Images.GUI.msg.png";

            this.AddTextCenter(component);

            this.Destroy += () =>
            {
                if (CurrentBoxes.ContainsKey(key))
                {
                    CurrentBoxes.Remove(key);
                    ShowedBoxes--;

                    CurrentBoxes.Where(b => b.Value.order > this.order).ForEach(c =>
                    {
                        c.Value.order -= 1;
                        c.Value.Top -= 1.5;
                    });
                }
            };
        }

        private int order = 0;

        private double _opacity = 1;

        public override double Opacity
        {
            get => _opacity;
            set
            {
                _opacity = value;
                this.Children.ForEach(c => c.Opacity = value);
            }
        }

        private static int ShowedBoxes { get; set; }
        private static Dictionary<string, Toast> CurrentBoxes = new Dictionary<string, Toast>();

        public static void Show(string text) => Show(text, Global.SceneManager.CurrentScene.ShowEffectsBinding);

        public static void Show(DrawText text) => Show(text, Global.SceneManager.CurrentScene.ShowEffectsBinding);

        public static void Show(string text, Action<List<ISceneObject>> publisher) => Show(text.AsDrawText().InSize(10).Montserrat(), publisher);

        public static void Show(DrawText text, Action<List<ISceneObject>> publisher, int frames =400)
        {
            if (CurrentBoxes.TryGetValue(text.StringData, out var msgBox))
            {
                msgBox.Opacity = 1;
            }
            else
            {
                publisher?.Invoke(new Toast(text)
                {
                    Frames = frames
                }.InList<ISceneObject>());
            }
        }

        public override void Click(PointerArgs args)
        {
            Global.Interacting = true;
            this.Destroy?.Invoke();
        }

        public override Rectangle BoundPosition
        {
            get
            {
                FrameCounter++;

                DrawLoop();

                return base.BoundPosition;
            }

        }

        private int FrameCounter = 0;        
        private int _frames = 0;

        protected void DrawLoop()
        {
            if (_frames >= Frames)
            {
                this.Destroy?.Invoke();
                return;
            }

            if (FrameCounter % (60 / 14) == 0)
            {
                _frames++;
                if(_frames>100)
                this.Opacity -= 0.0008;
            }
        }
    }
}