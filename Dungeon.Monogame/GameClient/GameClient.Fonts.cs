using Dungeon.Resources;
using FontStashSharp;
using Microsoft.Xna.Framework;

namespace Dungeon.Monogame
{
    public partial class GameClient
    {
		private FontSystem alegreya;
		private FontSystem montserrat;

		public void LoadFontSystem()
        {
			alegreya = new FontSystem();
			var alegreyaRes = ResourceLoader.Load("Fonts/ttf/Alegreya.ttf".AsmRes());
			alegreya.AddFont(alegreyaRes.Data);
			alegreya.UseKernings = true;

			montserrat = new FontSystem();
			var montserratRes = ResourceLoader.Load("Fonts/ttf/Montserrat-Bold.ttf".AsmRes());
			montserrat.AddFont(montserratRes.Data);
			montserrat.UseKernings = true;

			//FontSystemDefaults.FontResolutionFactor = 2.0f;
			//FontSystemDefaults.KernelWidth = 2;
			//FontSystemDefaults.KernelHeight = 2;
		}

		public void DrawFPS()
		{
			DefaultSpriteBatch.Begin();

			alegreya.GetFont(30).DrawText(DefaultSpriteBatch, DungeonGlobal.FPS.ToString(), new Vector2(10, 0), Color.Yellow);

			montserrat.GetFont(30).DrawText(DefaultSpriteBatch, DungeonGlobal.FPS.ToString(), new Vector2(10, 30), Color.Yellow);

			DefaultSpriteBatch.End();
		}
	}
}
