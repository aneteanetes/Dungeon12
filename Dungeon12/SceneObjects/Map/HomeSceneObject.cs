namespace Dungeon.Drawing.SceneObjects.Map
{
    using Dungeon.Control.Keys;
    using Dungeon.Control.Pointer;
    using Dungeon.Drawing.SceneObjects.Dialogs.NPC;
    using Dungeon.Map;
    using Dungeon.Map.Objects;
    using Dungeon.View.Interfaces;
    using Dungeon12.Drawing.SceneObjects.Dialogs.Shop;

    public class HomeSceneObject : ClickActionSceneObject<Home>
    {
        public override string Cursor => @object.Merchant == null
            ? "home"
            : "shop";

        private readonly GameMap gameMap;

        public HomeSceneObject(PlayerSceneObject playerSceneObject, Home home, string tooltip,GameMap gameMap) 
            : base(playerSceneObject,home, tooltip)
        {
            this.gameMap = gameMap;
            Left = home.Location.X;
            Top = home.Location.Y;
            Width = 1;
            Height = 1;
        }
        
        protected override void Action(MouseButton mouseButton)
        {
            playerSceneObject.StopMovings();
            var actionObject = Act();
            ShowEffects?.Invoke(actionObject.InList());
        }

        private ISceneObject Act() => @object.Merchant == null
            ? (ISceneObject)new NPCDialogue(playerSceneObject, @object, this.DestroyBinding, this.ControlBinding,gameMap)
            : (ISceneObject)new ShopWindow(@object.Name, playerSceneObject, @object.Merchant, this.DestroyBinding, this.ControlBinding, gameMap);

        protected override void StopAction() { }

        protected override Key[] KeyHandles => new Key[] { Key.LeftShift };

        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
        }
    }
}