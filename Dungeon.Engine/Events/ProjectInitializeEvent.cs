using Dungeon.Engine.Projects;
using Dungeon.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Engine.Events
{
    public class ProjectInitializeEvent : IEvent
    {
        public EngineProject Project { get; set; }

        public ProjectInitializeEvent(EngineProject project) => Project = project;
    }
}
