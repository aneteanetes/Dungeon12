using Dungeon;
using Dungeon.View.Interfaces;
using Dungeon12.Abilities.Talants.NotAPI;
using Dungeon12.Classes;
using Dungeon12.Entities.Quests;
using Dungeon12.Map;
using Dungeon12.SceneObjects;
using System;
using System.Linq;

namespace Dungeon12.Entities.Rewards.Triggers
{
    public class TalantRewardTrigger : IRewardTrigger
    {
        public IDrawText Trigger(Reward arg1, Character arg2, GameMap arg3)
        {
            var talantMap = arg1.TalantMap;

            var updateTalantArg2 = UpdateTalant(arg2);

            Toast.Show($"Вы получили талант!");

            //if (talantMap.TryGetValue(arg2.GetType().Name, out var talantsExpr))
            //{
            //    if (talantsExpr.Contains("&&"))
            //    {
            //        foreach (var item in new string[] { })
            //        {
            //            updateTalantArg2(item);
            //        }

            //        talantsExpr.Split("&&", StringSplitOptions.RemoveEmptyEntries)
            //            .ForEach(updateTalantArg2);
            //    }
            //    else if (talantsExpr.Contains("||"))
            //    {
            //        var talants = talantsExpr.Split("||", StringSplitOptions.RemoveEmptyEntries);
            //        var talant1 = ParseTalant(arg2, talants.First());
            //        var talant2 = ParseTalant(arg2, talants.Last());

            //        QuestionBox.Show(new QuestionBoxModel()
            //        {
            //            Text = "Выберите какой талант хотите получить:",
            //            YesText = talant1.Name,
            //            NoText = talant2.Name,
            //            Yes = () => UpdateTalant(talant1),
            //            No = () => UpdateTalant(talant2),
            //        });
            //    }
            //    else updateTalantArg2(talantsExpr);
            //}

            return default;
        }

        private TalantBase ParseTalant(Character arg2, string talantName)
        {
            var talantInfo = talantName.Split(".", StringSplitOptions.RemoveEmptyEntries);
            var tree = arg2.GetProperty<Abilities.Talants.TalantTrees.TalantTree>(talantInfo[0]);
            return tree.GetProperty<TalantBase>(talantInfo[1]);
        }


        private void UpdateTalant(TalantBase talant)
        {
            var isNew = talant.Level == 0;
            talant.Level++;

            Toast.Show($"Вы {(isNew ? "получили" : "улучшили")} талант: {talant.Name}");
        }

        private Action<string> UpdateTalant(Character arg2) => talantName => UpdateTalant(ParseTalant(arg2, talantName));
    }
}