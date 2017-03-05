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
                opVal = float.Parse(rawOperand.Substring(0, rawOperand.Length - 2));
            }
            else
            {   // Plain Number
                opType = OperandType.Num;
                opVal = float.Parse(rawOperand);
            }
        }
    }
}