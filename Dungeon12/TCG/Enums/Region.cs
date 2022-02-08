using System.ComponentModel.DataAnnotations;

namespace Dungeon12.TCG.Enums
{
    public enum Region
    {
        [Display(Name = "Империя Ушал")]
        Ushal = 110,

        [Display(Name = "Конгрегация Вестников")]        
        Snowland = 0,

        [Display(Name = "Седой бор")]
        Silverdale = 180,

        [Display(Name = "Край мира")]
        Overland = 320,

        [Display(Name = "Экзархат Сокола")]
        Falconwatch = 60,

        [Display(Name = "Остров Веры")]
        FaithIsland = 160,

        [Display(Name = "Вечная роща")]
        Eternalgrove = 90,

        [Display(Name = "Пески мёртвых")]
        Deadplains = 200, // 6 насыщенность

        [Display(Name = "Тёмный лес")]
        Darkwood = 260,

        [Display(Name = "Берег отчаянья")]
        Braveedge = 360,

        [Display(Name = "Бухта Пустоты")]
        Voidbay = 20
    }
}
