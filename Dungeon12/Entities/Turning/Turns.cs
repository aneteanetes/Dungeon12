using Dungeon.GameObjects;
using Dungeon.SceneObjects.Grouping;
using Dungeon12.Entities.Objects;
using MoreLinq;
using System.Collections;

namespace Dungeon12.Entities.Turning
{
    internal class Turns : GameComponent, IEnumerable<Turn>
    {
        private readonly Game game;

        public Turns(Game game)
        {
            this.game=game;
            this.game.Turns=this;
        }


        private LinkedList<Turn> turns = new();

        public LinkedListNode<Turn> Current { get; private set; }

        public int Count => turns.Count;

        public Action<IEnumerable<GameObject>> BeforeNewDraw { get; set; } = x => { };

        public Action<LinkedList<Turn>> BeforeNewRound { get; set; }= x => { };

        public Action<Turns> BeforeNewRoundTurn { get; set; } = x => { };

        public Action<Turns> AfterRound { get; set; } = x => { };

        public void NewRound()
        {
            var gameObjects = GetObjects();
            BeforeNewDraw(gameObjects);
            var roundTurns = Draw(gameObjects);
            BeforeNewRound(roundTurns);
            turns.Clear();
            turns=roundTurns;

            ObjectGroupBuilder<Turn>.Build(turns, x => x.IsActive);

            Current = turns.First;
            Current.Value.IsActive.True();

            BeforeNewRoundTurn(this);
            Turn();
        }

        public IEnumerable<GameObject> GetObjects()
        {
            var game = Global.Game;

            if (game.State.IsBattle)
            {
                var heroes = Global.Game.Party.Where(h => h.CanFight);
                return game.Location.Enemies.Concat(heroes);
            }
            else return Global.Game.Party.ToList();
        }

        public LinkedList<Turn> Draw(IEnumerable<GameObject> objects)
        {
            var ordered = objects
                .OrderBy(x => x.Initiative.FlatValue)
                .ThenBy(x => x.GlobalId);

            return new LinkedList<Turn>(ordered.Select(x=>new Turn(x)));
        }

        public Action<Turns> BeforeTurn { get; set; } = x => { };
        public Action<Turns> AfterTurn { get; set; } = x => { };

        /// <summary>
        /// Игровое время состоит из ходов. 
        /// <para>
        /// Каждый ход состоит из:
        /// До хода
        /// Ход
        /// После хода
        /// </para>
        /// <para>
        /// Ход:
        /// Эффекты до(персистентные подписки)
        /// Ход - привязан к объекту.Ход(), или объект.Пропуск()
        /// Эффекты после(персистентные подписки)
        /// </para>
        /// </summary>
        public TurnResult Turn()
        {
            if (QueuedTurn)
                return TurnInternal();

            BeforeTurn(this);
            var result = Current.Value.Object.DoTurn();
            if (result is TurnType.Next)
                return TurnInternal();

            if (result is TurnType.Await)
            {
                QueuedTurn=true;
                return TurnResult.AwaitUserInput;
            }

            return TurnResult.UnknownState;
        }

        private bool QueuedTurn = false;

        private TurnResult TurnInternal()
        {
            Console.WriteLine($"turn: {Current.Value}");

            // 1 шаг - 2 минуты
            game.Calendar.Add(0, 2);

            QueuedTurn=false;
            AfterTurn(this);

            Current = Current.Next;
            if (Current==null)
            {
                AfterRound(this);
                NewRound();
                return TurnResult.NewRound;
            }
            else
            {
                Current.Value.IsActive.True();
                return TurnResult.Success;
            }
        }

        public IEnumerator<Turn> GetEnumerator() => turns.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <summary>
        /// Выбрать Х героя:
        /// <para>1. Если в бою - "вы уверены что хотите закончить ход?"</para> 
        /// <para>2. Если нет: делаем "шагов пропуска" пока не придем куда нужно</para> 
        /// <para>3. Если по пути встретили скрытый объект - он вскрывается и делает ход, дальше передается ход ближайшему</para> 
        /// <para>4. Если не встретили и надо это сделать через раунд</para> 
        /// <para>5. Делаем "шагов пропуска" до конца раунда, разыгрываем новый раунд, разыгрываем "шагов пропуска"</para> 
        /// </summary>
        public TurnResult TurnHero(Hero hero)
        {
            if (game.State.IsBattle)
                return TurnResult.Awareness;

            while (Current.Value.Object!=hero)
            {
                game.Log.Push($"{Current.Value.Object.Name} пропускает ход");
                var result = Turn();
                if (result is TurnResult.AwaitUserInput && Current.Value.Object!=hero)
                {
                    Turn();
                }
                
                if (result.IsFailed())
                    return result;
            }

            return TurnResult.Success;
        }
    }
}