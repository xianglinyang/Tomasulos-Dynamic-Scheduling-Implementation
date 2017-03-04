using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tomasulo
{
    public partial class Form1 : Form
    {
        InstructionUnit instructionUnit = new InstructionUnit();
        ReservationStation loadStation, storeStation, addStation, multiplyStation;
        FloatingPointRegisters floatRegs;
        private List<Instruction> instructions = new List<Instruction>();
        private int clocks = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void loadBuffers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Temporary. Will eventually get from user.
            instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.LD, "F6", "32(R2)", null));
            instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.LD, "F2", "44(R3)", null));
            instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.MULD, "F0", "F2", "F4"));
            instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.SUBD, "F8", "F2", "F6"));
            instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.DIVD, "F10", "F0", "F6"));
            instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.ADDD, "F6", "F8", "F2"));

            loadStation = new ReservationStation(3, ReservationStation.RSType.Load);
            storeStation = new ReservationStation(3, ReservationStation.RSType.Store);
            addStation = new ReservationStation(3, ReservationStation.RSType.Add);
            multiplyStation = new ReservationStation(2, ReservationStation.RSType.Multiply);

            floatRegs = new FloatingPointRegisters(30);

            UpdateInstructionQueueBox();
            UpdateClockCountBox();
        }

        // Fill box with info in instructions.
        private void UpdateInstructionQueueBox()
        {
            instructionQueue.Clear();
            instructionQueue.View = View.Details;
            instructionQueue.Columns.Add("Inst", 50);
            instructionQueue.Columns.Add("Dest", 50);
            instructionQueue.Columns.Add("J", 50);
            instructionQueue.Columns.Add("K", 50);

            foreach (Instruction inst in instructionUnit.GetCurrentInstructions())
            {
                ListViewItem row = new ListViewItem(inst.opcode.ToString());
                row.SubItems.Add(inst.dest);
                row.SubItems.Add(inst.j);
                row.SubItems.Add(inst.k);
                instructionQueue.Items.Add(row);
            }
        }

        // This is the main method that will run a cycle using Tomasulo's Algorithm.
        private void RunOneCycle()
        {
            Issue();
            Execute();
            Write();
        }

        private void Issue()
        {
            int bufNum;
            Instruction instruction = instructionUnit.PeekAtInstruction();

            switch (instruction.instType)
            {
                case Instruction.InstructionType.Add:
                    if ((bufNum = addStation.GetFreeBuffer()) != -1)
                    {
                        // Issue.
                        addStation.PutInBuffer(instructionUnit.GetInstruction(), bufNum);
                        //floatRegs.[instruction.dest.Substring(1)] = 
                    }
                    else
                    {
                        Console.WriteLine("Stalling due to a structural hazard.");
                    }
                    break;

                case Instruction.InstructionType.Multiply:
                    if ((bufNum = multiplyStation.GetFreeBuffer()) != -1)
                    {
                        // Issue.
                        multiplyStation.PutInBuffer(instructionUnit.GetInstruction(), bufNum);
                    }
                    else
                    {
                        Console.WriteLine("Stalling due to a structural hazard.");
                    }
                    break;

                case Instruction.InstructionType.Load:
                    if ((bufNum = loadStation.GetFreeBuffer()) != -1)
                    {
                        // Issue.
                        loadStation.PutInBuffer(instructionUnit.GetInstruction(), bufNum);
                    }
                    else
                    {
                        Console.WriteLine("Stalling due to a structural hazard.");
                    }
                    break;

                case Instruction.InstructionType.Store:
                    if ((bufNum = storeStation.GetFreeBuffer()) != -1)
                    {
                        // Issue.
                        storeStation.PutInBuffer(instructionUnit.GetInstruction(), bufNum);
                    }
                    else
                    {
                        Console.WriteLine("Stalling due to a structural hazard.");
                    }
                    break;
            }
        }

        private void Execute()
        {

        }

        private void Write()
        {

        }

        private void UpdateClockCountBox()
        {
            ClockCount.Text = clocks.ToString();
        }

        private void instructionQueue_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Clock_Click(object sender, EventArgs e)
        {
            clocks++;
            RunOneCycle();
            UpdateClockCountBox();
        }
    }

    // A simple class with our instruction, and results.
    public class Instruction
    {   // This isn't a complete list of MIPS instructions yet.
        public enum Opcodes { LD, SD, MULD, SUBD, DIVD, ADDD };
        public enum InstructionType { Add, Multiply, Load, Store };
        public Opcodes opcode;
        public InstructionType instType;
        public string dest;
        public string j;
        public string k;

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
                    instType = InstructionType.Add;
                    break;
                case Opcodes.MULD:
                    instType = InstructionType.Multiply;
                    break;
                case Opcodes.LD:
                case Opcodes.SD:
                    instType = InstructionType.Store;
                    break;
            }
        }
    }

    public class InstructionUnit
    {
        Queue<Instruction> instructions = new Queue<Instruction>();

        public void AddInstruction(Instruction instruct)
        {
            instructions.Enqueue(instruct);
            instructions.ToArray<Instruction>();
        }

        public Instruction GetInstruction()
        {
            return instructions.Dequeue();
        }

        public Instruction PeekAtInstruction()
        {
            return instructions.Peek();
        }

        public Instruction[] GetCurrentInstructions()
        {
            return instructions.ToArray<Instruction>();
        }
    }

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

    public class ReservationStation
    {
        public enum RSType { Load, Store, Add, Multiply };
        public Operand Qj, Qk, Vj, Vk, dest;

        private int numBuffers = 2;
        private List<bool> busy = new List<bool>();
        private RSType rsType;

        public ReservationStation(int numToMake, RSType resStaType)
        {
            numBuffers = numToMake;
            rsType = resStaType;

            for (int i = 0; i < numToMake; i++)
            {
                busy[i] = false;
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

        public void PutInBuffer(Instruction instr, int index)
        {
            Qj = new Operand(instr.j);
            Qk = new Operand(instr.k);
            dest = new Operand(instr.dest);

            switch (instr.instType)
            {
                case Instruction.InstructionType.Add:
                    
                    // Get registers/numbers.

                    break;

                case Instruction.InstructionType.Multiply:
                    
                    break;

                case Instruction.InstructionType.Load:
                    
                    break;

                case Instruction.InstructionType.Store:
                    
                    break;
            }
        }
    }

    public class FloatingPointRegisters
    {
        public enum WaitState { LoadStation, StoreStation, MultStation, AddStation, Compute, Avail };

        public class FPRegStruct
        {
            public float value;
            public WaitState waitState = new WaitState();
        }

        private List<FPRegStruct> fpRegs = new List<FPRegStruct>();

        public FloatingPointRegisters(int numToCreate)
        {
            for (int i = 0; i < numToCreate; i++)
            {
                fpRegs[i].value = 0.0f;
                fpRegs[i].waitState = WaitState.Avail;
            }
        }

        public void Set(WaitState state, float val, int index)
        {
            fpRegs[index].waitState = state;
            fpRegs[index].value = val;
        }

        public FPRegStruct Get(int index)
        {
            return fpRegs[index];
        }
    }
}