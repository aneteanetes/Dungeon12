using Dungeon.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon.Update;
using Dungeon12.Scenes.Menus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Scenes
{
    public class SplashScene : StartScene<Start>
    {
        public SplashScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void FatalException()
        {
            MessageBox.Show("Произошла фатальная ошибка, требуется перезапустить игру.", () => { Global.Exit?.Invoke(); });
        }

        public override void Init()
        {
            if (UpdateManager.CheckUpdate(Global.Version, out var v))
            {
                // если надо обновиться показываем окно обновления с вопросом обновиться
                UpdateManager.Notes(Global.Platform, v);

                //показываем патчноты и кнопку "начать"
                //запускаем обнову и показываем прогресс
                UpdateManager.Download(Global.Platform, v, percent => Console.WriteLine(percent));

                //когда прогресс закончился, запускаем апдейтер
                UpdateManager.Update(v);

                //после того как апдейтер закончит он сам запустит
            }

            //если не надо обновиться просто нажимаем любую клавишу

            base.Init();
        }
    }
}
