using System.Collections.Generic;

namespace Tomasulo
{
    public class ReservationStation
    {
        public enum RSType { Load, Store, Add, Multiply };
        public List<Instruction.Opcodes> opCodes = new List<Instruction.Opcodes>();
        public List<Operand> Vj = new List<Operand>();
        public List<Operand> Vk = new List<Operand>();
        public List<Operand> dest = new List<Operand>();
        public List<WaitInfo> Qj = new List<WaitInfo>();
        public List<WaitInfo> Qk = new List<WaitInfo>();
        public List<int> addresses = new List<int>();
        public List<Instruction.State> states = new List<Instruction.State>();
        public List<int> remainingCycles = new List<int>();
        public List<int> cyclesToComplete = new List<int>();
        public List<int> results = new List<int>();
        public int numBuffers = 2;

        private List<bool> busy = new List<bool>();
        private RSType rsType;

        public ReservationStation(int numToMake, RSType resStaType)
        {
            numBuffers = numToMake;
            rsType = resStaType;

            for (int i = 0; i < numToMake; i++)
            {
                opCodes.Add(Instruction.Opcodes.UNINIT);
                addresses.Add(0);
                busy.Add(false);
                Qj.Add(null);
                Qk.Add(null);
                Vj.Add(null);
                Vk.Add(null);
                dest.Add(null);
                states.Add(Instruction.State.Issue);
                remainingCycles.Add(-1);
                cyclesToComplete.Add(-1);
                results.Add(-1);
            }
        }

        public int GetFreeBuffer()
        {
            for (int i = 0; i < numBuffers; i++)
            {
                if (!busy[i])
                {
                    return i;
                }
            }
            return -1;
        }

        public void PutInBuffer(Instruction instr, int index, Operand jReg, Operand kReg,
            WaitInfo wsJ, WaitInfo wsK)
        {
            dest[index] = new Operand(instr.dest);
            busy[index] = true;
            opCodes[index] = instr.opcode;
            states[index] = Instruction.State.Issue;
            cyclesToComplete[index] = instr.cyclesToComplete;

            if (wsJ.waitState == WaitInfo.WaitState.Avail)
            {
                Vj[index] = jReg;
                Qj[index] = null;
            }
            else
            {
                Vj[index] = null;
                Qj[index] = wsJ;
            }

            if (wsK.waitState == WaitInfo.WaitState.Avail)
            {
                Vk[index] = kReg;
                Qk[index] = null;
            }
            else
            {
                Vk[index] = null;
                Qk[index] = wsK;
            }
        }

        public bool IsBusy(int index)
        {
            return busy[index];
        }

        public int RunExecution(int index)
        {   // -1 = not yet in exec, 0 = finished, > 0 means n cycles left.
            if ((Vj[index] != null) && (Vk[index] != null))
            {   // Operands Available.
                if (remainingCycles[index] == -1)
                {
                    remainingCycles[index] = cyclesToComplete[index];
                    states[index] = Instruction.State.Exec_Inprog;
                }
                else
                {
                    if (remainingCycles[index] == 0)
                    {
                        states[index] = Instruction.State.Exec_Fin;
                        GetResult(index);
                        return 0;
                    }
                    else
                    {
                        return remainingCycles[index]--;
                    }
                }
            }
            else
            {   // Waiting for operands.
                return -1;
            }
            return -1;
        }

        public void Free(int index)
        {
            opCodes[index] = Instruction.Opcodes.UNINIT;
            addresses[index] = 0;
            busy[index] = false;
            Qj[index] = null;
            Qk[index] = null;
            Vj[index] = null;
            Vk[index] = null;
            dest[index] = null;
            states[index] = Instruction.State.Issue;
            remainingCycles[index] = -1;
            cyclesToComplete[index] = -1;
            results[index] = -1;
        }

        private void GetResult(int index)
        {   // Will make this actually compute results.
            results[index] = 0;
        }
    }
}