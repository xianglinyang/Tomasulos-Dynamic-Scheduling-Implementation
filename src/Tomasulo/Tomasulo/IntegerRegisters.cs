using System.Collections.Generic;

namespace Tomasulo
{
    public class IntegerRegisters
    {
        private List<WaitInfo> intRegs = new List<WaitInfo>();

        public IntegerRegisters(int numToCreate)
        {
            for (int i = 0; i < numToCreate; i++)
            {
                intRegs[i].value = 0.0f;
                intRegs[i].waitState = WaitInfo.WaitState.Avail;
            }
        }

        public void Set(WaitInfo.WaitState state, int val, int index)
        {
            intRegs[index].waitState = state;
            intRegs[index].value = val;
        }

        public WaitInfo Get(int index)
        {
            return intRegs[index];
        }
    }
}
