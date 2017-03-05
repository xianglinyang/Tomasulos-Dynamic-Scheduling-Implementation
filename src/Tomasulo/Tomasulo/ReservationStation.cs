using System.Collections.Generic;

namespace Tomasulo
{
    public class ReservationStation
    {
        public enum RSType { Load, Store, Add, Multiply };
        public List<Instruction.Opcodes> opCodes = new List<Instruction.Opcodes>();
        public List<Operand> Qj = new List<Operand>();
        public List<Operand> Qk = new List<Operand>();
        public List<Operand> dest = new List<Operand>();
        public List<WaitInfo> Vj = new List<WaitInfo>();
        public List<WaitInfo> Vk = new List<WaitInfo>();
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
                opCodes[i] = Instruction.Opcodes.UNINIT;
                addresses[i] = 0;
                busy[i] = false;
                Qj[i] = null;
                Qk[i] = null;
                dest[i] = null;
                states[i] = Instruction.State.Issue;
                remainingCycles[i] = -1;
                cyclesToComplete[i] = -1;
                results[i] = -1;
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
                Qj[index] = jReg;
                Vj[index] = null;
            }
            else
            {
                Qj[index] = null;
                Vj[index] = wsJ;
            }

            if (wsK.waitState == WaitInfo.WaitState.Avail)
            {
                Qk[index] = kReg;
                Vk[index] = null;
            }
            else
            {
                Qk[index] = null;
                Vk[index] = wsK;
            }
        }

        public bool IsBusy(int index)
        {
            return busy[index];
        }

        public int RunExecution(int index)
        {   // -1 = not yet in exec, 0 = finished, > n means n cycles left.
            if ((Qj[index] != null) && (Qk[index] != null))
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

        private void GetResult(int index)
        {   // Will make this actually compute results.
            results[index] = 0;
        }
    }
}