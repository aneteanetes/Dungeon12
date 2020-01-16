using Dungeon.Control.Pointer;
using Dungeon12.Drawing.SceneObjects;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.Map.Objects;

namespace Dungeon12.SceneObjects.Map
{
    public class ChestSceneObject : AnimatedSceneObject<Chest>
    {
        public override bool CacheAvailable => false;

        protected override bool SilentTooltip => true;

        public override string Cursor => "take";

        protected override bool Loop => false;

        private bool inited = false;
        public ChestSceneObject(PlayerSceneObject playerSceneObject, Chest @object) : base(playerSceneObject, @object, @object.Name, @object.Animation.DefaultFramePosition)
        {
            this.RequestStop();
            this.SetAnimation(@object.Animation);
            inited = true;

            this.ImageForceSet(@object.Animation.TileSet);
            this.ImageRegion = @object.Animation.DefaultFramePosition;

            Left = @object.Location.X;
            Top = @object.Location.Y;
            Width = 1;
            Height = 1.5;

            if (@object.Used)
            {
                this.FramePosition = @object.Animation.LastFramePosition; //last frame
                SetEmptyTooltip(@object);
            }
        }

        private void SetEmptyTooltip(Chest @object)
        {
            this.TooltipText = "Пустой " + @object.Name;
        }

        protected override void Action(MouseButton mouseButton)
        {
            if (!@object.Used)
            {
                Global.AudioPlayer.Effect("opendoorchest.wav".AsmSoundRes());
                this.RequestResume();
            }
        }

        public override bool Updatable => true;

        public override void Update()
        {
            if (@object.Used)
            {
                this.@object.Destroy?.Invoke();
                this.Destroy?.Invoke();
            }
        }

        protected override void OnAnimationStop()
        {
            if (inited)
            {
                @object.Use(playerSceneObject.Avatar.Character);
                @object.Used = true;
                SetEmptyTooltip(@object);
            }
        }

        protected override void DrawLoop() { }
    }
}