using Dungeon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Items
{
    public class ItemNamePart : ValueAttribute
    {
        public VariableString Name { get; set; }

        public bool IsPrefix { get; set; }

        public ItemNamePart(VariableString part, bool prefix = true) : base(default)
        {
            Name = part;
            IsPrefix = prefix;

            this.Value = this;
        }

        public ItemNamePart(string part, bool prefix = true) : base(default)
        {
            Name = new VariableString(part.Split(",", StringSplitOptions.RemoveEmptyEntries));
            IsPrefix = prefix;

            this.Value = this;
        }
    }

    public class VariableString
    {
        private string[] _variations;
        public VariableString(params string[] variations)
        {
            _variations = variations;
        }

        public string Value => _variations[RandomDungeon.Range(0, _variations.Length - 1)];
    }
}