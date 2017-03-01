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

            UpdateInstructionQueueBox();
            UpdateClockCountBox();
        }

        // Fill box with info in instructions.
        private void UpdateInstructionQueueBox()
        {
            instructionQueue.Clear();
            instructionQueue.View = View.Details;
            instructionQueue.Columns.Add("Inst", 50);
            instructionQueue.Columns.Add("Op1", 50);
            instructionQueue.Columns.Add("Op2", 50);
            instructionQueue.Columns.Add("Op3", 50);

            foreach (Instruction inst in instructionUnit.GetCurrentInstructions())
            {
                ListViewItem row = new ListViewItem(inst.opcode.ToString());
                row.SubItems.Add(inst.op1);
                row.SubItems.Add(inst.op2);
                row.SubItems.Add(inst.op3);
                instructionQueue.Items.Add(row);
            }
        }

        // This is the main method that will run a cycle using Tomasulo's Algorithm.
        private void RunOneCycle()
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
        public enum Opcodes { LD, MULD, SUBD, DIVD, ADDD };
        public Opcodes opcode;
        public string op1;
        public string op2;
        public string op3;

        public string result;
        public int cyclesToComplete;
        public bool issued, executed, completed;

        public Instruction(Opcodes opCode, string operand1, string operand2, string operand3)
        {
            opcode = opCode;
            op1 = operand1;
            op2 = operand2;
            op3 = operand3;
            issued = executed = completed = false;
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

        public Instruction[] GetCurrentInstructions()
        {
            return instructions.ToArray<Instruction>();
        }
    }

    public class StoreBuffer
    {

    }

    public class LoadBuffer
    {

    }

    public class ReservationStation
    {

    }

    public class FloatingPointRegister
    {

    }
}