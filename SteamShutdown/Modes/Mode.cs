using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteamShutdown.Modes
{
    public abstract class Mode
    {
        public abstract string Name { get; protected set; }

        public abstract void Execute();

        public override string ToString()
        {
            return Name;
        }
    }
}
