namespace Dungeon12.Scenes.Menus.Creation
{
    using Dungeon.Control.Keys;
    using Dungeon.Data;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.Scenes;
    using Dungeon.Scenes.Manager;
    using Dungeon12.Classes;
    using Dungeon12.Drawing.SceneObjects.Dialogs;

    public class PlayerOriginScene : GameScene<PlayerSummaryScene, PlayerNameScene>
    {
        public PlayerOriginScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Init()
        {
            this.AddObject(new ImageControl("Dungeon12.Resources.Images.d12back.png"));
            this.AddObject(new OriginDialogue(this.AddControl, this.RemoveControl)
            {
                Top = 3f,
                Left = 7f,
                OnSelect = o =>
                {
                    Character anotherClass = null;
                    switch (o)
                    {
                        case Dungeon12.Entities.Alive.Enums.Origins.Liver:
                            Global.GameState.Equipment.Reset();
                            anotherClass = new Instance("Bowman").Value<Character>();
                            break;
                        case Dungeon12.Entities.Alive.Enums.Origins.Servant:
                            Global.GameState.Equipment.Reset();
                            anotherClass = new Instance("Servant").Value<Character>();
                            break;
                        default:break;
                    }

                    Global.GameState.Character.Recalculate();
                    if (anotherClass != null)
                    {
                        var to = anotherClass;
                        var from = Global.GameState.Character;

                        to.Backpack = from.Backpack;
                        to.Clothes = from.Clothes;
                        to.EXP = from.EXP;
                        to.Gold = from.Gold;
                        to.HitPoints = from.HitPoints;
                        to.MaxHitPoints = from.MaxHitPoints;
                        to.AbilityPower = from.AbilityPower;
                        to.AttackDamage = from.AttackDamage;
                        to.Barrier = from.Barrier;
                        to.Defence = from.Defence;
                        to.Idle = from.Idle;
                        to.MinDMG = from.MinDMG;
                        to.MaxDMG = from.MaxDMG;

                        to.Race = from.Race;
                        to.Name = from.Name;
                        to.Level = from.Level;

                        from.Destroy();
                        to.Reload();

                        Global.GameState.PlayerAvatar.Character = to;
                        Global.GameState.PlayerAvatar.ReEntity(to);
                    }

                    Global.GameState.PlayerAvatar.Character.Origin = o;

                    this.Switch<PlayerSummaryScene>();
                }
            });
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape && !hold)
            {
                this.Switch<PlayerNameScene>();
            }
        }
    }
}
