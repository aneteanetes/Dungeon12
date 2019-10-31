namespace Dungeon12.Scenes.Menus
{
    using Dungeon;
    using Dungeon.Control.Keys;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.Events.Network;
    using Dungeon.Scenes;
    using Dungeon.Scenes.Manager;
    using Dungeon12.Drawing.SceneObjects;
    using Dungeon12.Drawing.SceneObjects.NetworkTest;

    public class NetworkScene : GameScene<SoloDuoScene>
    {
        public NetworkScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        MetallButtonControl server, client;

        public override void Init()
        {
            this.AddObject(new Background(false));
            this.AddObject(new ImageControl("Dungeon.Resources.Images.d12textM.png")
            {
                Top = 2f,
                Left = 10f
            });

            server = new MetallButtonControl("Сервер")
            {
                Left = 15.5f,
                Top = 8,
                OnClick = () => CreateServer()
            };

            client = new MetallButtonControl("Клиент")
            {
                Left = 15.5f,
                Top = 11,
                OnClick = () => ConnectClient()
            };

            this.AddObject(server);
            this.AddObject(client);
        }

        private void CreateServer()
        {
            Global.Events.Raise(new CreateNetworkServerEvent());
            server.Destroy?.Invoke();
            client.Destroy?.Invoke();
            AddAlives(true);
        }

        private void ConnectClient()
        {
            Global.Events.Raise(new ConnectNetworkServerEvent());
            server.Destroy?.Invoke();
            client.Destroy?.Invoke();
            AddAlives();
        }

        private void AddAlives(bool server=false)
        {
            this.AddObject(new NetworkSceneObject(new NetworkObject()
            {
                Name = "ServerAlive",
                HitPoints = 100,
                MaxHitPoints=100
            }, server)
            {
                Left = 5,
                Top = 8
            });
            this.AddObject(new NetworkSceneObject(new NetworkObject()
            {
                Name = "ClientAlive",
                HitPoints = 100,
                MaxHitPoints = 100
            }, !server)
            {
                Left = 5,
                Top = 10
            });
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape && !hold)
            {
                this.Switch<SoloDuoScene>();
            }
        }
    }
}
