using System.Collections.Generic;

namespace Tomasulo
{
    public class ReorderBuffer
    {
        public enum ROBDestination { FReg, IReg, FMem };
        public enum State { Issue, Execute, Write, Commit };
        public List<State> state;
        public List<Instruction.InstructionType> instrType;
        public List<ROBDestination> destType;
        public List<int> destLoc;
        public List<float> result;
        public List<bool> ready;
        public List<bool> resultWritten;
        public int robSize = 10;
        public int head = 0, tail = -1;
        public List<bool> inUse;
        public List<bool> prediction;
        public List<int> branchAmt;
        public InstructionUnit[] oldInstructions;
        public FloatingPointMemoryArrary[] oldMem;
        public FloatingPointRegisters[] oldFRegs;
        public IntegerRegisters[] oldIRegs;
        public int[] instrOffsets;
        public int[] instrNum;
        public List<Operand> j;
        public List<Operand> k;

        public ReorderBuffer()
        {
            state = new List<State>();
            instrType = new List<Instruction.InstructionType>();
            destType = new List<ROBDestination>();
            destLoc = new List<int>();
            result = new List<float>();
            ready = new List<bool>();
            resultWritten = new List<bool>();
            inUse = new List<bool>();
            prediction = new List<bool>();
            branchAmt = new List<int>();
            oldInstructions = new InstructionUnit[robSize];
            oldMem = new FloatingPointMemoryArrary[robSize];
            oldFRegs = new FloatingPointRegisters[robSize];
            oldIRegs = new IntegerRegisters[robSize];
            instrOffsets = new int[robSize];
            instrNum = new int[robSize];
            j = new List<Operand>();
            k = new List<Operand>();

            for (int i = 0; i < robSize; i++)
            {
                state.Add(State.Issue);
                instrType.Add(Instruction.InstructionType.Add);
                destType.Add(ROBDestination.FMem);
                destLoc.Add(0);
                result.Add(0);
                ready.Add(false);
                inUse.Add(false);
                resultWritten.Add(false);
                prediction.Add(false);
                branchAmt.Add(0);
                j.Add(null);
                k.Add(null);
            }
        }

        public void CleanBuffer(int index)
        {
            //state[index] = State.Issue;
            instrType[index] = Instruction.InstructionType.Add;
            destType[index] = ROBDestination.FMem;
            destLoc[index] = 0;
            result[index] = 0;
            ready[index] = false;
            inUse[index] = false;
            resultWritten[index] = false;
            prediction[index] = false;
            branchAmt[index] = 0;
            instrNum[index] = 0;
            j[index] = null;
            k[index] = null;
        }

        public void IncrementHead()
        {
            if (head == tail)
            {
                return;
            }

            head++;
            if (head == robSize)
            {
                head = 0;
            }
        }

        public void RemoveInstructionsAfterHead()
        {
            int i = head;
            while (i != tail)
            {
                CleanBuffer(i);

                i++;
                if (i == robSize)
                {
                    i = 0;
                }

                if (i == tail)
                {
                    CleanBuffer(i);
                }
            }
            IncrementHead();
            tail = head;
            DecrementTail();
        }

        public void IncrementTail()
        {
            tail++;
            if (tail == robSize)
            {
                tail = 0;
            }
        }

        public void DecrementTail()
        {
            tail--;
            if (tail < 0)
            {
                tail = robSize - 1;
            }
        }

        public int GetFreeBuffer()
        {
            IncrementTail();
            int value = tail;
            DecrementTail();
            if (inUse[value] == false)
            {
                return value;
            }
            
            return -1;
        }

        public void ReserveBuffer(int num, Instruction.InstructionType type)
        {
            IncrementTail();
            inUse[num] = true;
            instrType[num] = type;
            resultWritten[num] = false;
        }

        public void BufferResult(int index, Instruction.InstructionType iType, ROBDestination dType, int dLoc, float res, bool rdy)
        {
            instrType[index] = iType;
            destType[index] = dType;
            destLoc[index] = dLoc;
            result[index] = res;
            ready[index] = rdy;
        }
    }
}