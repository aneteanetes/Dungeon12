using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Update;
using Dungeon12.Drawing.SceneObjects;
using Dungeon12.SceneObjects.Splash;
using Dungeon12.Scenes.Menus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.SceneObjects.UI
{
    public class AnimatedSplash : EmptyControlSceneObject
    {
        private ImageControl splashText;

        private string actualVersion;

        private bool needUpdate = false;

        public override bool Updatable => true;

        public AnimatedSplash()
        {
            this.Width = 1280 / 32;
            this.Height = 720 / 32;

            this.AddChild(new Background(true));

            splashText = this.AddChildImageCenter(new ImageControl("Dungeon12.Resources.Images.d12textM.png")
            {
                Width=36,
                Height=8
            });

            splashText.Top = 5f;
            splashText.Left = 2f;
            splashText.AbsolutePosition = true;
            splashText.Opacity = 0.2;

            try
            {
                UpdateManager.GetLastVersion(VersionCheck);
            }
            catch (Exception ex)
            {
                Global.Logger.Log(ex.ToString());
                Global.Logger.Save("CRUSH.txt");
            }
        }

        private void VersionCheck(string version)
        {
            if (version != default)
            {
                actualVersion = version;
                var next = Semver.SemVersion.Parse(version);
                var now = Semver.SemVersion.Parse(Global.Version);

                needUpdate = next > now;
            }
        }

        private bool endAnimation = false;

        private bool canSwitch = true;

        public bool CanSwitch => canSwitch && endAnimation;

        private bool firstUpdate = false;

        public override void Update()
        {
            if(!firstUpdate)
            {
                Global.AudioPlayer.Effect("enter.wav".AsmSoundRes(), new Dungeon.Audio.AudioOptions()
                {
                    Repeat = true,
                    Volume = 0.3
                });
                firstUpdate = true;
            }

            if (endAnimation)
                return;

            if (splashText.Opacity < 1)
            {
                splashText.Opacity += .005;
            }
            else if (needUpdate)
            {
                endAnimation = true;
                canSwitch = false;
                QuestionBox.Show(new QuestionBoxModel()
                {
                    Text = "Доступно новое обновление, установить?",
                    YesText = "Да",
                    NoText = "Нет",
                    Yes = InstallUpdate,
                    No = () =>
                    {
                        Global.SceneManager.Change<Start>();
                    }
                });
            }
            else
            {
                endAnimation = true;
                PressAnyKey();
            }
        }

        private void InstallUpdate()
        {
            var upd = new UpdateWindow(new UpdateModel()
            {
                Notes= UpdateManager.Notes(Global.Platform, actualVersion),
                FromVer= Global.Version,
                ToVer= actualVersion,
                Cancel=()=>
                {
                    PressAnyKey();
                    canSwitch = true;
                }
            });

            upd.Left += 7;
            upd.Top += 4;

            this.AddChild(upd);
        }

        private void PressAnyKey()
        {
            var then = this.AddChildCenter(new TextControl("Нажмите любую клавишу для продолжения...".AsDrawText().Montserrat().InSize(17)));
            then.Left -= 9;
            then.Top += 4;
        }
    }
}