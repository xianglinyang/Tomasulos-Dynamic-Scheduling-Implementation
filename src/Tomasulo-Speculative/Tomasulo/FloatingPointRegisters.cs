using System.Collections.Generic;

namespace Tomasulo
{
    public class FloatingPointRegisters
    {
        private List<WaitInfo> fpRegs = new List<WaitInfo>();
        private List<float> lastNumVal = new List<float>();

        public FloatingPointRegisters(int numToCreate)
        {
            for (int i = 0; i < numToCreate; i++)
            {
                fpRegs.Add(new WaitInfo(0.0f, WaitInfo.WaitState.Avail));
                lastNumVal.Add(0);
            }
        }

        public void Set(WaitInfo.WaitState state, float val, int index)
        {
            if (state == WaitInfo.WaitState.Avail)
            {
                lastNumVal[index] = val;
            }
            fpRegs[index].waitState = state;
            fpRegs[index].value = val;
        }

        public WaitInfo Get(int index)
        {
            return fpRegs[index];
        }

        public int GetNumRegs()
        {
            return fpRegs.Count;
        }

        public void ClearROBRefs()
        {
            int i = 0;
            foreach (WaitInfo reg in fpRegs)
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