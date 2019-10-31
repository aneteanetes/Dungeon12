﻿using Dungeon.Transactions;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Entites.Alive
{
    public abstract class Perk : Applicable, IDrawable
    {
        public abstract string Description { get; }

        public virtual bool ClassDependent { get; set; }
    }

}
