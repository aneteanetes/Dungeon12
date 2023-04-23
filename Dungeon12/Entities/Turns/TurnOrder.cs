using Dungeon;
using Dungeon.GameObjects;
using Dungeon.SceneObjects.Grouping;
using System.Collections.Generic;

namespace Dungeon12.Entities.Turns
{
    internal class TurnOrder : GameComponent
    {
        private readonly Game game;
        public TurnOrder(Game game)
        {
            this.game=game;
        }

        public IEnumerable<Turn> Turns => turns;

        private Turn[] turns = Array.Empty<Turn>();

        public int Count => turns.Length;

        private ObjectGroup<Turn> group;

        public void Clear()
        {
            turns=Array.Empty<Turn>();
            Init();
        }

        public override void Initialization()
        {
            turns=new Turn[4];

            int i = 0;
            foreach (var hero in game.Party)
            {
                turns[i]=hero;
                i++;
            }

            group = new ObjectGroupBuilder<Turn>(turns)
                .Property(x => x.IsActive)
                .Build();

            group.Select();
        }

        public void Add(Turn turn)
        {
            // logic

            Array.Resize(ref turns, turns.Length+1);
            turns[^1]=turn;
            group.Add(turn);
            this.Refresh();
        }

        public void Next() => Step();

        public void Step()
        {
            var next = group.List[0];
            var current = group.List.FirstOrDefault(x => x.IsActive==true);

            var currentIdx = group.List.IndexOf(current);
            if (currentIdx!=group.List.Count-1)
            {
                next = group.List[currentIdx+1];
            }

            // change initialives
            next.Property.True();
            Array.Copy(turns, 0, turns, 1, turns.Length - 1);
        }
    }
}