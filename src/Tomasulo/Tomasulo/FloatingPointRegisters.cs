using System.Collections.Generic;

namespace Tomasulo
{
    public class FloatingPointRegisters
    {
        private List<WaitInfo> fpRegs = new List<WaitInfo>();

        public FloatingPointRegisters(int numToCreate)
        {
            for (int i = 0; i < numToCreate; i++)
            {
                fpRegs[i].value = 0.0f;
                fpRegs[i].waitState = WaitInfo.WaitState.Avail;
            }
        }

        public void Set(WaitInfo.WaitState state, float val, int index)
        {
            fpRegs[index].waitState = state;
            fpRegs[index].value = val;
        }

        public WaitInfo Get(int index)
        {
            return fpRegs[index];
        }
    }
}