using Dungeon.Data;
using Dungeon.Monogame;
using Dungeon.Monogame.Settings;
using Dungeon.View;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Dungeon.Engine.Projects
{
    public class ProjectSettings : MonogameSettings
    {        
        [Display(Name ="Проброс исключений", Description ="Вместо обработок ошибок приложение будет выбрасывать исключение")]
        public bool ExceptionRethrow { get; set; } = false;

        [Display(Name = "Глобальный перехват Ex", Description = "// означает что глобальный перехват не используется, очистить поле если требуется")]
        public string GlobalExceptionHandling { get; set; } = "//";

        [Display(Name = "Не выгружать ресурсы", Description = "При смене сцены ресурсы не будут выгружаться, это поможет переиспользовать ресурсы, но будет влиять на память")]
        public bool NotDisposingResources { get; set; } = false;

        [Display(Name = "Кэш масок", Description = "Кэширование масок изображений, влияет на производительность")]
        public bool CacheImagesAndMasks { get; set; } = true;

        [Title("Доступные разрешения экрана")]
        public ObservableCollection<PossibleResolution> Resolutions { get; set; } = new ObservableCollection<PossibleResolution>();
    }
}