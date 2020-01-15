using Dungeon;
using Dungeon.Drawing;
using Dungeon.GameObjects;
using Dungeon.SceneObjects;
using Dungeon.Update;
using Dungeon12.Drawing.SceneObjects;
using Dungeon12.Drawing.SceneObjects.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.SceneObjects.Splash
{
    public class UpdateWindow : HandleSceneControl<UpdateModel>
    {
        public override bool AbsolutePosition => true;
        public override bool CacheAvailable => false;

        public UpdateWindow(UpdateModel model) : base(model)
        {
            this.AddChild(new HorizontalWindow("Dungeon12.Resources.Images.ui.horizontal(26x17).png"));

            this.Width = 26;
            this.Height = 17;

            var notes = this.AddTextCenter(model.Notes.AsDrawText().Montserrat().WithWordWrap(),false,false);
            notes.Top = 3;
            notes.Left = 1;

            var cancelBtn = new MetallButtonControl("Назад");
            cancelBtn.Top = this.Top + this.Height - 1.5;
            cancelBtn.Left -= 0;
            cancelBtn.OnClick = () =>
            {
                model.Cancel?.Invoke();
                this.Destroy?.Invoke();
            };


            var installBtn = new MetallButtonControl("Установить");
            installBtn.Top = this.Top + this.Height - 1.5;
            installBtn.Left -= this.Width/2-cancelBtn.Width/2;
            installBtn.OnClick = () =>
            {
                installBtn.Visible = false;
                UpdateManager.Update(model.ToVer);
            };
            installBtn.Visible = false;

            var updBtn = new MetallButtonControl("Обновить");
            updBtn.Top = this.Top + this.Height - 1.5;
            updBtn.Left = this.Width-updBtn.Width;
            updBtn.OnClick = () =>
            {
                cancelBtn.Visible = false;
                updBtn.Visible = false;
                var percent = this.AddTextCenter("0%".AsDrawText().Montserrat().InSize(18), true, false);
                percent.Top += 1;
                UpdateManager.Download(Global.Platform,Global.Version, model.ToVer, p =>
                  {
                      if (p == 100)
                      {
                          installBtn.Visible = true;
                      }
                      percent.Text.SetText($"{p}%");
                  });
            };

            this.AddChild(installBtn);
            this.AddChild(cancelBtn);
            this.AddChild(updBtn);
        }
    }

    public class UpdateModel : GameComponent
    {
        public string Notes { get; set; }

        public string FromVer { get; set; }

        public string ToVer { get; set; }

        public Action Cancel { get; set; }
    }
}
