using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VegaPQ.SceneObjects;

namespace VegaPQ.Entities
{
    public class FieldInvalidationResult
    {
        public IEnumerable<GameShard> Horizontal { get; set; }

        public IEnumerable<GameShard> Vertical { get; set; }

        public bool HorizontalCanMatch => Horizontal.Distinct().Count() > 2;

        public bool VerticalCanMatch => Vertical.Distinct().Count() > 2;

        public bool Empty { get; set; }

        public bool CanMatch => HorizontalCanMatch || VerticalCanMatch;

        public static FieldInvalidationResult EmptyResult => new FieldInvalidationResult()
        {
            Empty = true,
            Horizontal = Enumerable.Empty<GameShard>(),
            Vertical = Enumerable.Empty<GameShard>()
        };
    }
}
