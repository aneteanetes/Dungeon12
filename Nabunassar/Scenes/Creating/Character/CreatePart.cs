using Nabunassar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nabunassar.Scenes.Creating.Character
{
    internal class CreatePart : SceneControl<Hero>
    {
        public CreatePart(Hero component) : base(component)
        {
        }
        public bool IsActivated { get; set; }
    }
}
