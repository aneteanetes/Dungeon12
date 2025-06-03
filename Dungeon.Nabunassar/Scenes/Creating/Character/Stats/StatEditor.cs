using Dungeon.Control;
using Nabunassar.Entities;
using Nabunassar.SceneObjects.Base;
using Nabunassar.SceneObjects.UserInterface.Common;
using Nabunassar.Scenes.Creating.Character.Stats.RankValue;

namespace Nabunassar.Scenes.Creating.Character.Stats
{
    internal class StatEditor : SceneControl<Hero>
    {
        protected override ControlEventType[] Handles => [ControlEventType.Focus];

        private string _statName;

        public StatEditor(Hero component, int primaryIdx) : base(component)
        {
            _statName = component.PrimaryStats.GetName(primaryIdx);

            this.Width = 350; // 37
            this.Height = 100;

            this.AddBorderMapBack(new BorderConfiguration()
            {
                ImagesPath = "UI/bordermin/panel-border-022.png",
                Size = 16,
                Padding = 2
            });

            var title = AddTextCenter(Global.Strings[_statName].ToString().DefaultTxt(17));
            title.Top = 5;

            var uiTop = 50;
            var leftOffset = 10;
            var btnSize = 25;
            var signsSize = 16;

            var minus = this.AddChild(new ClassicButton("-", btnSize, btnSize, signsSize, "bord15.png")
            {
                Left = leftOffset,
                Top = uiTop,
                OnClick = () => component.PrimaryStats.Decrease(primaryIdx)
            });

            var plus = this.AddChild(new ClassicButton("+", btnSize, btnSize, signsSize, "bord15.png")
            {
                Top = uiTop,
                Left = this.Width- btnSize - leftOffset,
                OnClick = () => component.PrimaryStats.Increase(primaryIdx)
            });

            var bar = this.AddChild(new RankValueScale(() => component.PrimaryStats[primaryIdx],40,40));
            bar.Top = uiTop-10;
            bar.Left = leftOffset + btnSize + 15;
        }

        public override void Focus()
        {
            Global.Game.Creation.Hint = Global.Strings["guide"][_statName];
            base.Focus();
        }

        public override void Unfocus()
        {
            Global.Game.Creation.Hint = Global.Strings["guide"]["stats"];
            base.Unfocus();
        }
    }
}
