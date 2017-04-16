using System.Collections.Generic;

namespace Tomasulo
{
    public class IntegerRegisters
    {
        private List<WaitInfo> intRegs = new List<WaitInfo>();
        private List<int> lastNumVal = new List<int>();

        public IntegerRegisters(int numToCreate)
        {
            for (int i = 0; i < numToCreate; i++)
            {
                intRegs.Add(new WaitInfo(0.0f, WaitInfo.WaitState.Avail));
                lastNumVal.Add(0);
            }
        }

        public void Set(WaitInfo.WaitState state, int val, int index)
        {
            if (state == WaitInfo.WaitState.Avail)
            {
                lastNumVal[index] = val;
            }
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

        public void ClearROBRefs()
        {
            int i = 0;
            foreach (WaitInfo reg in intRegs)
            {
                if (reg.waitState == WaitInfo.WaitState.ReorderBuffer)
                {
                    reg.waitState = WaitInfo.WaitState.Avail;
                    reg.value = lastNumVal[i];
                }
                i++;
            }
        }
    }
}
