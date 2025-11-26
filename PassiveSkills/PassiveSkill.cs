using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.PassiveSkills
{
    public abstract class PassiveSkill
    {
        private readonly Character _owner;

        public abstract void Dispose();
    }
}
