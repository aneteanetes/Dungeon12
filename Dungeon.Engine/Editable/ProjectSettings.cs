using Dungeon.Data;
using Dungeon.Monogame;
using Dungeon.View;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Dungeon.Engine.Projects
{
    public class ProjectSettings : MonogameClientSettings
    {        
        [Display(Name ="Проброс исключений", Description ="Вместо обработок ошибок приложение будет выбрасывать исключение")]
        public bool ExceptionRethrow { get; set; } = false;

        [Display(Name = "Глобальный перехват Ex", Description = "// означает что глобальный перехват не используется, очистить поле если требуется")]
        public string GlobalExceptionHandling { get; set; } = "//";

        [Display(Name = "Не выгружать ресурсы", Description = "При смене сцены ресурсы не будут выгружаться, это поможет переиспользовать ресурсы, но будет влиять на память")]
        public bool NotDisposingResources { get; set; } = false;

        [Display(Name = "Кэш масок", Description = "Кэширование масок изображений, влияет на производительность")]
        public bool CacheImagesAndMasks { get; set; } = true;

        [Display(Name = "Размер клетки", Description = "Размер клетки для позиционирования элементов, по умолчанию 1=32")]
        public int CellSize { get; set; } = 32;

        [Title("Доступные разрешения экрана")]
        public ObservableCollection<PossibleResolution> Resolutions { get; set; } = new ObservableCollection<PossibleResolution>();
    }
}