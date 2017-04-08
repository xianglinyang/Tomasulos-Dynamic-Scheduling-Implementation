using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomasulo
{
    public class FloatingPointMemoryArrary
    {
        private List<WaitInfo> fpMem = new List<WaitInfo>();
        public FloatingPointMemoryArrary(int numToCreate)
        {
            for(int i = 0; i < numToCreate; i++)
            {
                fpMem[i].value = 0.0f;
                fpMem[i].waitState = WaitInfo.WaitState.Avail;
            }
        }

        public void Set(WaitInfo.WaitState state, float val, int address)
        {
            fpMem[address].waitState = state;
            fpMem[address].value = val;
        }

        public WaitInfo Get(int address)
        {
            return fpMem[address];
        }

    }
}
