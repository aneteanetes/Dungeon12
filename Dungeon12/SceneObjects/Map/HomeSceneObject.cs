namespace Dungeon12.Drawing.SceneObjects.Map
{
    using Dungeon.Control.Keys;
    using Dungeon.Control.Pointer;
    using Dungeon12.Entities.Fractions;
    using Dungeon12.Map;
    using Dungeon12.Map.Objects;
    using Dungeon.View.Interfaces;
    using Dungeon12.Drawing.SceneObjects;
    using Dungeon12.Drawing.SceneObjects.Dialogs.Shop;
    using Dungeon12.Map.Objects;
    using Dungeon12.SceneObjects.NPC;
    using System.Linq;
    using Dungeon;
    using Dungeon.Drawing.SceneObjects;

    public class HomeSceneObject : ClickActionSceneObject<Home>
    {
        public override string Cursor => @object.Merchant == null
            ? "home"
            : "shop";

        private readonly GameMap gameMap;

        public HomeSceneObject(PlayerSceneObject playerSceneObject, Home home, string tooltip,GameMap gameMap,string tileset=default) 
            : base(playerSceneObject,home, tooltip)
        {
            this.gameMap = gameMap;
            Left = home.Location.X;
            Top = home.Location.Y;
            Width = 1;
            Height = 1;
        }
        protected override void BeforeClick()
        {
            if (@object.Fraction != default && @object.Fraction.Playable)
            {
                if (!Global.GameState.Character.Fractions.Any(x => x.IdentifyName == @object.Fraction.IdentifyName))
                {
                    Global.GameState.Character.Fractions.Add(FractionView.Load(@object.Fraction.IdentifyName).ToFraction());
                }
            }
        }

        protected override void Action(MouseButton mouseButton)
        {
            playerSceneObject.StopMovings();
            var actionObject = Act();
            if(@object.Merchant==null)
            {
                Global.AudioPlayer.Effect("door.wav".AsmSoundRes());
            }
            ShowInScene?.Invoke(actionObject.InList());
        }

        private ISceneObject Act() => @object.Merchant == null
            ? (ISceneObject)new NPCDialogue(playerSceneObject, @object, this.DestroyBinding, this.ControlBinding, gameMap, new MetallButtonControl("Выход"))
            : (ISceneObject)new ShopWindow(@object.Name, playerSceneObject, @object.Merchant, this.DestroyBinding, this.ControlBinding, gameMap);

        protected override void StopAction() { }

        protected override Key[] KeyHandles => new Key[] { Key.LeftShift };

        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
        }
    }
}