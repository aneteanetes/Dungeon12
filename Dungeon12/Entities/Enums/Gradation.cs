using System.ComponentModel.DataAnnotations;

namespace Dungeon12.Entities.Enums
{
    public enum Gradation
    {
        [Display(Name = "Отсутствует")]
        None,

        [Display(Name = "Минимально")]
        Min,

        [Display(Name = "Небольшое")]
        Low,

        [Display(Name = "Среднее")]
        Mid,

        [Display(Name = "Большое")]
        Long,

        [Display(Name = "Максимально")]
        Max,
    }
}
