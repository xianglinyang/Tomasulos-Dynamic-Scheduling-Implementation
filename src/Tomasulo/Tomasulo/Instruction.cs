namespace Tomasulo
{
    // A simple class with our instruction, and results.
    public class Instruction
    {   // This isn't a complete list of MIPS instructions yet.
        public enum Opcodes { UNINIT, LD, SD, MULD, SUBD, DIVD, ADDD, BEQ, BGEZ, BLEZ, BNE};
        public enum InstructionType { Add, Multiply, Load, Store, Branch};
        public enum State { Issue, Exec_Inprog, Exec_Fin, WB };
        public Opcodes opcode;
        public InstructionType instType;
        public string dest;
        public string j;
        public string k;
        public State state;

        public string result;
        public int cyclesToComplete;
        public bool issued, executed, completed;

        public Instruction(Opcodes opCode, string operand1, string operand2, string operand3)
        {
            opcode = opCode;
            dest = operand1;
            j = operand2;
            k = operand3;
            issued = executed = completed = false;

            switch (opCode)
            {
                case Opcodes.ADDD:
                case Opcodes.SUBD:
                    instType = InstructionType.Add;
                    break;
                case Opcodes.MULD:
                case Opcodes.DIVD:
                    instType = InstructionType.Multiply;
                    break;
                case Opcodes.LD:
                    instType = InstructionType.Load;
                    break;
                case Opcodes.SD:
                    instType = InstructionType.Store;
                    break;
                case Opcodes.BEQ:
                case Opcodes.BGEZ:
                case Opcodes.BLEZ:
                case Opcodes.BNE:
                    instType = InstructionType.Branch;
                    break;
            }
        }
    }
}