using Nabunassar.Entities;
using Nabunassar.SceneObjects;
using Nabunassar.SceneObjects.Base;
using Nabunassar.Scenes.Creating.Character.Names;

namespace Nabunassar.Scenes.Creating.Character
{
    internal class NameSelector : CreatePart
    {
        TextInputControl _input;

        public NameSelector(Hero component) : base(component, Global.Strings["guide"]["name"])
        {
            Width = 325;
            Height = 700;
            Top = 300;
            Left = 50;

            this.AddBorderMapBack(new BorderConfiguration()
            {
                ImagesPath = "UI/bordermin/panel-border-022.png",
                Size = 16,
                Padding = 2
            });

            var title = AddTextCenter(Global.Strings["EnterName"].ToString().DefaultTxt(20));
            title.Top = 20;

            var name = Global.Strings["Unknown"].ToString().DefaultTxt(18);

            _input = new TextInputControl(name, 12, true, absolute: false, placeholder:name.Clone());
            _input.Top = 65;
            _input.Left = this.CalculateHorizontalCenterPoint(_input.Width);

            var titlemeaning = AddTextCenter(Global.Strings["NameMeaning"].ToString().DefaultTxt(20));
            titlemeaning.Top = _input.TopMax + 20;

            var descBlock = new NameDescriptionBlock();

            var attack = new NameAbilSelector(component, "Icons/Common/attackname.tga", Global.Strings["guide"]["AttackName"],1, descBlock);
            attack.Left = 45;
            attack.Top = titlemeaning.TopMax + 15;

            var defence = new NameAbilSelector(component, "Icons/Common/defencename.tga", Global.Strings["guide"]["DefenceName"],2, descBlock);
            defence.Left = attack.LeftMax + 45;
            defence.Top = attack.Top;

            var magic = new NameAbilSelector(component, "Icons/Common/magicName.tga", Global.Strings["guide"]["Magicname"],3, descBlock);
            magic.Left = defence.LeftMax + 45;
            magic.Top = attack.Top;

            descBlock.Top = magic.TopMax + 20;
            descBlock.Left = this.CalculateHorizontalCenterPoint(descBlock.Width);

            this.AddChild(_input);
            this.AddChild(titlemeaning);
            this.AddChild(descBlock);
            this.AddChild(attack);
            this.AddChild(defence);
            this.AddChild(magic);
        }

        public override bool Updatable => true;

        public override void Update(GameTimeLoop gameTime)
        {
            var inputValue = _input.GetValue();

            var textEntered = !inputValue.IsEmpty();
            if (textEntered)
            {
                Component.Name = inputValue;
            }

            var abilitySelected = Global.Game.Creation.SelectedAbilityName != 0;
            if (textEntered && abilitySelected)
            {
                this.Cube.Next.Visible = !_input.GetValue().IsEmpty();
            }

            base.Update(gameTime);
        }
    }
}
