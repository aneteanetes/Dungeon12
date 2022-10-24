namespace Dungeon.View.Interfaces
{
    using Dungeon.Types;

    public interface IGameClient : ICamera
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scene"></param>
        /// <returns>After 1 draw circle</returns>
        Callback SetScene(IScene scene);

        Dot MeasureText(IDrawText drawText,ISceneObject parent=default);

        Dot MeasureImage(string image);

        void SaveObject(ISceneObject sceneObject, string path = default, Dot offset = default, string runtimeCacheName = null);

        void Drag(ISceneObject @object, ISceneObject area = null);

        void Drop();

        void Clear(IDrawColor drawColor=default);

        void SetCursor(string texture);

        /// <summary>
        /// Кэширование изображения
        /// </summary>
        /// <param name="image">Путь к изображению</param>
        void CacheImage(string image);

        /// <summary>
        /// Создать эффект который зависит от платформы
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IEffect GetEffect(string name);
    }
}