namespace SidusXII.SceneObjects.ItemSelector
{
    using Dungeon;
    using Dungeon.Drawing;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.SceneObjects;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ItemSelector<T> : EmptySceneControl
        where T : GameEnum
    {
        private Action _back;
        private IEnumerable<GameEnum> Items;
        private List<ItemBtn> ListItems = new List<ItemBtn>();

        public ItemSelector(Action back, IEnumerable<GameEnum> items=default)
        {
            Items = items ?? GameEnum.AllValues<T>().ToList();
            _back = back;
        }

        public override void Initialization()
        {
            AddChild(new ImageObject("GUI/Planes/SelectWindow.png".AsmImg()));

            Width = 811;
            Height = 850;

            var arrowBot = AddChild(new ImgBtn("GUI/Parts/arrow_bot.png")
            {
                Left = 745,
                Top = 686
            });

            var arrowTop = AddChild(new ImgBtn("GUI/Parts/arrow_top.png")
            {
                Left = 745,
                Top = 143
            });

            var arrowBack = AddChild(new ImgBtn("GUI/Parts/arrow_left.png")
            {
                Left = 588,
                Top = 745,
                OnClick = _back
            });

            var nextBtn = AddChild(new ImgBtn("GUI/Parts/minibtn.png")
            {
                Width = 98,
                Left = 645,
                Top = 745,
                OnClick = () => OnSelect?.Invoke(ListItems.FirstOrDefault(x => x.Active)?.Value)
            });
            nextBtn.AddTextCenter("Далее".AsDrawText().InSize(10));

            var qBtn = AddChild(new ImgBtn("GUI/Parts/qbtn.png")
            {
                Left = 745,
                Top = 745
            });

            var listitemtop = 126;
            Items.ForEach((item, i) =>
            {
                ListItems.Add(AddChild(new ItemBtn(false, item.Display, () => ShowDescription(item))
                {
                    Value = item,
                    Left = 46,
                    Top = listitemtop,
                    OnClick = () =>
                    {
                        ListItems.ForEach(lb =>
                        {
                            if (lb.Active)
                            {
                                lb.Active = false;
                                lb.Unfocus();
                            }
                        });
                    }
                }));

                listitemtop += 84;
            });


            var txt = AddTextCenter(typeof(T).Display().AsDrawText().InSize(24).InColor(new DrawColor(154, 119, 87)), true, false);
            txt.Top = 35;
        }

        private void ShowDescription(GameEnum gameEnum)
        {

        }

        public Action<GameEnum> OnSelect;
    }
}