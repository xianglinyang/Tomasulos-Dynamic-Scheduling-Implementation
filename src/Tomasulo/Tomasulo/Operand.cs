namespace Tomasulo
{
    public class Operand
    {
        public enum OperandType { FloatReg, IntReg, RegOffset, Num };
        public OperandType opType;
        public float opVal;

        public Operand(string rawOperand)
        {
            if (rawOperand[0] == 'F')
            {   // Floating-point Register
                opType = OperandType.FloatReg;
                opVal = float.Parse(rawOperand.Substring(1));
            }
            else if (rawOperand[0] == 'R')
            {   // Integer Register
                opType = OperandType.IntReg;
                opVal = float.Parse(rawOperand.Substring(1));
            }
            else if (rawOperand[rawOperand.Length - 1] == '+')
            {   // Register Offset
                opType = OperandType.RegOffset;
                opVal = float.Parse(rawOperand.Substring(0, rawOperand.Length - 1));
            }
            else if (rawOperand[rawOperand.Length - 1] == '-')
            {   // Register Offset
                opType = OperandType.RegOffset;
                opVal = -1 * float.Parse(rawOperand.Substring(0, rawOperand.Length - 1));
            }
            else
            {   // Plain Number
                opType = OperandType.Num;
                opVal = float.Parse(rawOperand);
            }
        }

        public Operand(OperandType type, float val)
        {
            opType = type;
            opVal = val;
        }

        public string PrintString()
        {

            switch (opType)
            {
                case OperandType.FloatReg:
                    return "F" + opVal.ToString();
                case OperandType.IntReg:
                    return "R" + opVal.ToString();
                case OperandType.Num:
                    return opVal.ToString();
                case OperandType.RegOffset:
                    return opVal.ToString();
                default:
                    return opVal.ToString();
            }
        }
    }
}