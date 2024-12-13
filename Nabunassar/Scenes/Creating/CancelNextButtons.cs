using Dungeon.Scenes;
using Nabunassar.SceneObjects.HUD;
using Nabunassar.SceneObjects.UserInterface.Common;

namespace Nabunassar.Scenes.Creating
{
    internal static class CancelNextButtons
    {
        public static (ClassicButton cancel, ClassicButton next) AddCancelNextBtns<TCancelScene>(this SceneLayer layer)
            where TCancelScene : GameScene
        {
            var scene = layer.SceneOwner;

            var cancelBtn = new ClassicButton(scene.Strings["Cancel"])
            {
                Left = DungeonGlobal.Resolution.CenterH(250) - 250 / 2 - 25,
                Top = DungeonGlobal.Resolution.Height - 100,
                Disabled = false,
                OnClick = () =>
                {
                    var box = new DialogueBox(scene.Strings["Yes"], scene.Strings["Cancel"], scene.Strings["AreYouSureAbort"]);
                    box.OnLeft = () => scene.Switch<TCancelScene>();
                    box.OnRight = box.Destroy;

                    DungeonGlobal.Freezer.Freeze(box);

                    scene.ActiveLayer.AddObjectCenter(box);
                }
            };

            var completeBtn = new ClassicButton(scene.Strings["Next"])
            {
                Left = DungeonGlobal.Resolution.CenterH(250) + 250 / 2 + 25,
                Top = DungeonGlobal.Resolution.Height - 100,
            };

            layer.AddObject(cancelBtn);
            layer.AddObject(completeBtn);

            return (cancelBtn, completeBtn);
        }
    }
}
