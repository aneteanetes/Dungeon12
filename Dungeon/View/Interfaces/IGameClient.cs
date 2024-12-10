namespace Dungeon.View.Interfaces
{
    using Dungeon.Resources;
    using Dungeon.Types;
    using System.Threading.Tasks;

    public interface IGameClient : ICamera
    {
        /// <summary>
        /// Активирует сцену на следующем цикле отрисовки
        /// </summary>
        /// <param name="scene"></param>
        /// <returns>After 1 draw circle</returns>
        Task ChangeScene(IScene scene);

        Dot MeasureText(ResourceTable resources, IDrawText drawText,ISceneObject parent=default);

        Dot MeasureImage(ResourceTable resources, string image);

        void SaveObject(ISceneObject sceneObject, string path = default, Dot offset = default, string runtimeCacheName = null);

        void Drag(ISceneObject @object, ISceneObject area = null);

        void Drop();

        void Clear(IDrawColor drawColor=default);

        void SetCursor(ResourceTable resources, string texture);

        string GetCursor();

        /// <summary>
        /// Кэширование изображения
        /// </summary>
        /// <param name="image">Путь к изображению</param>
        void CacheImage(ResourceTable resources, string image);

        /// <summary>
        /// Создать эффект который зависит от платформы
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IEffect GetEffect(string name);
    }
}