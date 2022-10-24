namespace Dungeon.Monogame
{
    using Dungeon.Monogame.Effects.Fogofwar;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using Microsoft.Xna.Framework;

    public partial class GameClient : Game, IGameClient
    {
        public Dungeon.Types.Dot MeasureText(IDrawText drawText, ISceneObject parent = default)
            => DrawClient.MeasureText(drawText, parent);

        public Dungeon.Types.Dot MeasureImage(string image) => DrawClient.MeasureImage(image);

        public void SaveObject(ISceneObject sceneObject, string path, Dot offset, string runtimeCacheName = null)
        {
            throw new System.Exception("Не тестировалось после рефакторинга, а нужно ли вообще?");
            //DrawClient.SaveObject(sceneObject, path, offset, runtimeCacheName);
        }

        public void Clear(IDrawColor drawColor = null)
        {
            DrawClient.Clear(drawColor);
        }

        public void CacheImage(string image) => DrawClient.CacheImage(image);

        public IEffect GetEffect(string name)
        {
            if (name == "FogOfWar")
                return new FogOfWarEffect();

            return null;
        }
    }
}