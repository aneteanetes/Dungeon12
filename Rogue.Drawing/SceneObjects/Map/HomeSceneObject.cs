namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.SceneObjects.Dialogs.NPC;
    using Rogue.Drawing.SceneObjects.Dialogs.Shop;
    using Rogue.Map;
    using Rogue.Map.Objects;
    using Rogue.View.Interfaces;

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