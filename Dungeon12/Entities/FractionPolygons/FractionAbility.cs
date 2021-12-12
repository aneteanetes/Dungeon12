using Dungeon;
using System.ComponentModel.DataAnnotations;

namespace Dungeon12.Entities.FractionPolygons
{
    public enum FractionAbility
    {
        [Display(Name = "Рекрутинг")]
        [Value("Раз в 10 ходов позволяет нанять рекрута для этого региона.")]
        Vanguard,

        [Display(Name = "Сбор реагентов")]
        [Value("Каждые 5 ходов добавляет два случайных реагента увеличивающие силу заклинания в два раза.")]
        MageGuild,

        [Display(Name = "Охотничьи угодья")]
        [Value("Каждые 3 хода создаёт 10 ед. пищи.")]
        Mercenary,

        [Display(Name = "Сбор пожертвований")]
        [Value("Каждые 10 ходов позволяет собрать случайные ресурсы текущего региона.")]
        Exarch,

        [Display(Name = "Ритуал проклятых")]
        [Value("Каждые 15 ходов позволяет проклясть выбранный предмет.")]
        Cult
    }
}
