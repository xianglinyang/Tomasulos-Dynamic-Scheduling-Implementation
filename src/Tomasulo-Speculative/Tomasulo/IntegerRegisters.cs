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
                intRegs.Add(new WaitInfo(0.0f, WaitInfo.WaitState.Avail));
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

        public int GetNumRegs()
        {
            return intRegs.Count;
        }
    }
}
