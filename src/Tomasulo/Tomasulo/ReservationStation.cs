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
        public List<float> results = new List<float>();
        public List<bool> isReady = new List<bool>();
        public List<bool> addrReady = new List<bool>();
        public List<int> instrNum = new List<int>();
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
                addresses.Add(-1);
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
                isReady.Add(false);
                addrReady.Add(false);
                instrNum.Add(-1);
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
                Vj[index] = new Operand(Operand.OperandType.Num, wsJ.value);
                Qj[index] = null;
            }
            else
            {
                Vj[index] = null;
                Qj[index] = wsJ;
            }

            if (wsK.waitState == WaitInfo.WaitState.Avail)
            {
                Vk[index] = new Operand(Operand.OperandType.Num, wsK.value);
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
                        isReady[index] = true;
                        states[index] = Instruction.State.Exec_Fin;
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
            addresses[index] = -1;
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
            isReady[index] = false;
            addrReady[index] = false;
        }

        public int ComputeAddress(IntegerRegisters regs, int index)
        {
            int jVal = 0, kVal = 0;

            if (Vj[index].opType == Operand.OperandType.IntReg)
            {
                jVal = (int) regs.Get((int) Vj[index].opVal).value;
            }
            else if (Vj[index].opType == Operand.OperandType.Num)
            {
                jVal = (int) Vj[index].opVal;
            }

            if (Vk[index].opType == Operand.OperandType.IntReg)
            {
                kVal = (int) regs.Get((int) Vk[index].opVal).value;
            }
            else if (Vk[index].opType == Operand.OperandType.Num)
            {
                kVal = (int) Vk[index].opVal;
            }
            
            return jVal + kVal;
        }

        public int GetFromMemory(int index, FloatingPointMemoryArrary memory)
        {
            if (remainingCycles[index] == -1)
            {
                remainingCycles[index] = cyclesToComplete[index];
                states[index] = Instruction.State.Exec_Inprog;
                return remainingCycles[index]--;
            }
            else
            {
                if (remainingCycles[index] == 0)
                {
                    isReady[index] = true;
                    states[index] = Instruction.State.Exec_Fin;
                    return 0;
                }
                else
                {
                    return remainingCycles[index]--;
                }
            }
        }

        public float DetermineBranch(FloatingPointRegisters floatRegs, IntegerRegisters intRegs, int index)
        {
            float jVal = 0, kVal = 0;

            // Get Operands.
            if (Vj[index].opType == Operand.OperandType.FloatReg)
            {
                jVal = floatRegs.Get((int)Vj[index].opVal).value;
            }
            else if (Vj[index].opType == Operand.OperandType.IntReg)
            {
                jVal = intRegs.Get((int)Vj[index].opVal).value;
            }
            else if (Vj[index].opType == Operand.OperandType.Num)
            {
                jVal = Vj[index].opVal;
            }

            if (Vk[index].opType == Operand.OperandType.FloatReg)
            {
                kVal = floatRegs.Get((int)Vk[index].opVal).value;
            }
            else if (Vk[index].opType == Operand.OperandType.IntReg)
            {
                kVal = intRegs.Get((int)Vk[index].opVal).value;
            }
            else if (Vk[index].opType == Operand.OperandType.Num)
            {
                kVal = Vk[index].opVal;
            }

            switch (opCodes[index])
            {
                case Instruction.Opcodes.BEQ:
                    if (jVal == kVal)
                    {
                        return dest[index].opVal;
                    }
                    else
                    {
                        return 0;
                    }
                    
                case Instruction.Opcodes.BGEZ:
                    return 0;

                case Instruction.Opcodes.BLEZ:
                    return 0;

                case Instruction.Opcodes.BNE:
                    if (jVal != kVal)
                    {
                        return dest[index].opVal;
                    }
                    else
                    {
                        return 0;
                    }

                default:
                    return 0;
            }
        }

        public float Compute(FloatingPointRegisters floatRegs, IntegerRegisters intRegs, int index)
        {
            float jVal = 0, kVal = 0;

            // Get Operands.
            if (Vj[index].opType == Operand.OperandType.FloatReg)
            {
                jVal = floatRegs.Get((int) Vj[index].opVal).value;
            }
            else if (Vj[index].opType == Operand.OperandType.IntReg)
            {
                jVal = intRegs.Get((int) Vj[index].opVal).value;
            }
            else if (Vj[index].opType == Operand.OperandType.Num)
            {
                jVal = Vj[index].opVal;
            }

            if (Vk[index].opType == Operand.OperandType.FloatReg)
            {
                kVal = floatRegs.Get((int) Vk[index].opVal).value;
            }
            else if (Vk[index].opType == Operand.OperandType.IntReg)
            {
                kVal = intRegs.Get((int) Vk[index].opVal).value;
            }
            else if (Vk[index].opType == Operand.OperandType.Num)
            {
                kVal = Vk[index].opVal;
            }
            
            // Compute Result
            switch (opCodes[index])
            {
                case Instruction.Opcodes.ADDD:
                    return jVal + kVal;

                case Instruction.Opcodes.SUBD:
                    return jVal - kVal;

                case Instruction.Opcodes.MULD:
                    return jVal * kVal;

                case Instruction.Opcodes.DIVD:
                    return jVal / kVal;

                default:
                    return 0;
            }
        }

        public void BufferValue(int index, FloatingPointRegisters fRegs, IntegerRegisters iRegs)
        {
            switch (dest[index].opType)
            {
                case Operand.OperandType.FloatReg:
                    if (fRegs.Get((int) dest[index].opVal).waitState == WaitInfo.WaitState.Avail)
                    {
                        results[index] = fRegs.Get((int)dest[index].opVal).value;
                        isReady[index] = true;
                    }
                    break;

                case Operand.OperandType.IntReg:
                    if (iRegs.Get((int) dest[index].opVal).waitState == WaitInfo.WaitState.Avail)
                    {
                        results[index] = iRegs.Get((int) dest[index].opVal).value;
                        isReady[index] = true;
                    }
                    break;

                default:
                    break;
            }
        }
    }
}