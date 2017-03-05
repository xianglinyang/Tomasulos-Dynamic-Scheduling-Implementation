using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Tomasulo
{
    public partial class Form1 : Form
    {
        InstructionUnit instructionUnit = new InstructionUnit();
        ReservationStation loadStation, storeStation, addStation, multiplyStation;
        FloatingPointRegisters floatRegs;
        IntegerRegisters intRegs;
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
            intRegs = new IntegerRegisters(30);

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
        {   // Do backwards so that only 1 stage is run on eac instruction per cycle.
            Write();
            Execute();
            Issue();
        }

        private WaitInfo FindRegister(string name)
        {
            if (name.Substring(0) == "F")
            {   // Floating Point.
                return floatRegs.Get(Int32.Parse(name.Substring(1)));
            }
            else if (name.Substring(0) == "R")
            {   // Integer.
                //jReg = intRegs.Get(Int32.Parse(instruction.j.Substring(1)));
                return null;
            }
            else if (name[name.Length - 1] == '+')
            {   // Number offset.
                return new WaitInfo(float.Parse(name.Substring(0, name.Length - 2)),
                    WaitInfo.WaitState.Avail);
            }
            else
            {   // Number.
                return new WaitInfo(float.Parse(name), WaitInfo.WaitState.Avail);
            }
        }

        private void Issue()
        {
            // If empty.
            if (instructionUnit.GetCurrentInstructions().Length == 0) return;

            int bufNum;
            Instruction instruction = instructionUnit.PeekAtInstruction();
            Operand jReg, kReg;
            WaitInfo wsJ, wsK;

            // Get Source Regs.
            jReg = new Operand(instruction.j);
            kReg = new Operand(instruction.k);
            wsJ = FindRegister(instruction.j);
            wsK = FindRegister(instruction.k);

            switch (instruction.instType)
            {
                case Instruction.InstructionType.Add:
                    if ((bufNum = addStation.GetFreeBuffer()) != -1)
                    {
                        new Operand(instruction.j);
                        // Put in Reservation Station.
                        addStation.PutInBuffer(instructionUnit.GetInstruction(),
                            bufNum, jReg, kReg, wsJ, wsK);

                        // Set Dest Reg.
                        floatRegs.Set(WaitInfo.WaitState.AddStation, bufNum,
                            Int32.Parse(instruction.dest.Substring(1)));
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
                        multiplyStation.PutInBuffer(instructionUnit.GetInstruction(),
                            bufNum, jReg, kReg, wsJ, wsK);

                        // Set Dest Reg.
                        floatRegs.Set(WaitInfo.WaitState.MultStation, bufNum,
                            Int32.Parse(instruction.dest.Substring(1)));
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
                        loadStation.PutInBuffer(instructionUnit.GetInstruction(),
                            bufNum, jReg, kReg, wsJ, wsK);
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
                        storeStation.PutInBuffer(instructionUnit.GetInstruction(),
                            bufNum, jReg, kReg, wsJ, wsK);
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
            // Add Station.
            for (int i = 0; i < addStation.numBuffers; i++)
            {
                addStation.RunExecution(i);
            }

            // Multiply Station.
            for (int i = 0; i < multiplyStation.numBuffers; i++)
            {
                multiplyStation.RunExecution(i);
            }

            // Load Station.
            for (int i = 0; i < loadStation.numBuffers; i++)
            {
                loadStation.RunExecution(i);
            }

            // Store Station.
            for (int i = 0; i < storeStation.numBuffers; i++)
            {
                storeStation.RunExecution(i);
            }
        }

        private void Write()
        {
            // Add Results.
            for (int i = 0; i < addStation.numBuffers; i++)
            {
                if (addStation.results[i] != -1)
                {   // Ready.
                    if (addStation.dest[i].opType == Operand.OperandType.FloatReg)
                    {
                        floatRegs.Set(WaitInfo.WaitState.Avail, addStation.results[i],
                            (int) addStation.dest[i].opVal);
                    }
                    else if (addStation.dest[i].opType == Operand.OperandType.IntReg)
                    {
                        intRegs.Set(WaitInfo.WaitState.Avail, addStation.results[i],
                            (int) addStation.dest[i].opVal);
                    }
                    else
                    {
                        Console.WriteLine("Don't know what to do with result.");
                    }
                }
            }

            // Multiply Results.
            for (int i = 0; i < multiplyStation.numBuffers; i++)
            {
                if (multiplyStation.results[i] != -1)
                {   // Ready.
                    if (multiplyStation.dest[i].opType == Operand.OperandType.FloatReg)
                    {
                        floatRegs.Set(WaitInfo.WaitState.Avail, multiplyStation.results[i],
                            (int) multiplyStation.dest[i].opVal);
                    }
                    else if (multiplyStation.dest[i].opType == Operand.OperandType.IntReg)
                    {
                        intRegs.Set(WaitInfo.WaitState.Avail, multiplyStation.results[i],
                            (int) multiplyStation.dest[i].opVal);
                    }
                    else
                    {
                        Console.WriteLine("Don't know what to do with result.");
                    }
                }
            }

            // Load Results.
            for (int i = 0; i < loadStation.numBuffers; i++)
            {
                if (loadStation.results[i] != -1)
                {   // Ready.
                    if (loadStation.dest[i].opType == Operand.OperandType.FloatReg)
                    {
                        floatRegs.Set(WaitInfo.WaitState.Avail, loadStation.results[i],
                            (int) loadStation.dest[i].opVal);
                    }
                    else if (loadStation.dest[i].opType == Operand.OperandType.IntReg)
                    {
                        intRegs.Set(WaitInfo.WaitState.Avail, loadStation.results[i],
                            (int) loadStation.dest[i].opVal);
                    }
                    else
                    {
                        Console.WriteLine("Don't know what to do with result.");
                    }
                }
            }

            // Store Results.
            for (int i = 0; i < loadStation.numBuffers; i++)
            {
                if (storeStation.results[i] != -1)
                {   // Ready.
                    //memLocs.Set(WaitInfo.WaitState.Avail, storeStation.results[i],
                    //   (int) storeStation.dest[i].opVal);
                }
            }
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
}