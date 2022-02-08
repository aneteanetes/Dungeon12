using Dungeon;
using System.ComponentModel.DataAnnotations;

namespace Dungeon12.Entities.FractionPolygons
{
    public enum FractionAbility
    {
        [Display(Name = "Инициатива")]
        [Value("Увеличивает инициативу вашему отряду")]
        Vanguard,

        [Display(Name = "Внимание")]
        [Value("Позволяет замечать спрятанные объекты, двери, ловушки")]
        MageGuild,

        [Display(Name = "Воровство")]
        [Value("Позволяет воровать предметы, взламывать замки, разоружать ловушки")]
        Mercenary,

        [Display(Name = "Торговля")]
        [Value("Снижает цены и увеличивает ассортимент магазинов")]
        Exarch,

        [Display(Name = "Проклятья")]
        [Value("Шанс проклясть противника перед сражением")]
        Cult
    }
}
