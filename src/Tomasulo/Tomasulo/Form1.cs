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
        FloatingPointMemoryArrary memLocs;
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
            //instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.LD, "F6", "32(R2)", null));
            //instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.LD, "F2", "44(R3)", null));
            instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.MULD, "F0", "F2", "F4"));
            instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.SUBD, "F8", "F0", "F6"));
            instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.DIVD, "F10", "F0", "F6"));
            instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.ADDD, "F6", "F8", "F2"));

            loadStation = new ReservationStation(3, ReservationStation.RSType.Load);
            storeStation = new ReservationStation(3, ReservationStation.RSType.Store);
            addStation = new ReservationStation(3, ReservationStation.RSType.Add);
            multiplyStation = new ReservationStation(2, ReservationStation.RSType.Multiply);

            floatRegs = new FloatingPointRegisters(30);
            intRegs = new IntegerRegisters(30);
            memLocs = new FloatingPointMemoryArrary(64);

            UpdateInstructionQueueBox();
            UpdateReservationStationBoxes();
            UpdateFPRegisterBox();
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

        // Fill box with info in instructions.
        private void UpdateReservationStationBoxes()
        {
            // Add.
            reservationStation1.Clear();
            reservationStation1.View = View.Details;
            reservationStation1.Columns.Add("Name", 50);
            reservationStation1.Columns.Add("Busy", 35);
            reservationStation1.Columns.Add("Op", 50);
            reservationStation1.Columns.Add("Vj", 50);
            reservationStation1.Columns.Add("Vk", 50);
            reservationStation1.Columns.Add("Qj", 50);
            reservationStation1.Columns.Add("Qk", 50);
            reservationStation1.Columns.Add("A", 50);

            for (int i = 0; i < addStation.numBuffers; i++)
            {
                ListViewItem row = new ListViewItem("Add" + (i + 1).ToString());
                row.SubItems.Add(addStation.IsBusy(i).ToString().Substring(0, 1));
                row.SubItems.Add(addStation.opCodes[i].ToString());
                row.SubItems.Add((addStation.Vj[i] == null) ? "" :
                    addStation.Vj[i].PrintString());
                row.SubItems.Add((addStation.Vk[i] == null) ? "" :
                    addStation.Vk[i].PrintString());
                row.SubItems.Add((addStation.Qj[i] == null) ? "" :
                    addStation.Qj[i].waitState.ToString().Substring(0, 1) + addStation.Qj[i].value);
                row.SubItems.Add((addStation.Qk[i] == null) ? "" :
                    addStation.Qk[i].waitState.ToString().Substring(0, 1) + addStation.Qk[i].value);
                row.SubItems.Add(addStation.results[i].ToString());
                reservationStation1.Items.Add(row);
            }

            // Multiply.
            reservationStation2.Clear();
            reservationStation2.View = View.Details;
            reservationStation2.Columns.Add("Name", 50);
            reservationStation2.Columns.Add("Busy", 35);
            reservationStation2.Columns.Add("Op", 50);
            reservationStation2.Columns.Add("Vj", 50);
            reservationStation2.Columns.Add("Vk", 50);
            reservationStation2.Columns.Add("Qj", 50);
            reservationStation2.Columns.Add("Qk", 50);
            reservationStation2.Columns.Add("A", 50);

            for (int i = 0; i < multiplyStation.numBuffers; i++)
            {
                ListViewItem row = new ListViewItem("Mult" + (i + 1).ToString());
                row.SubItems.Add(multiplyStation.IsBusy(i).ToString().Substring(0, 1));
                row.SubItems.Add(multiplyStation.opCodes[i].ToString());
                row.SubItems.Add((multiplyStation.Vj[i] == null) ? "" :
                    multiplyStation.Vj[i].PrintString());
                row.SubItems.Add((multiplyStation.Vk[i] == null) ? "" :
                    multiplyStation.Vk[i].PrintString());
                row.SubItems.Add((multiplyStation.Qj[i] == null) ? "" :
                    multiplyStation.Qj[i].waitState.ToString().Substring(0, 1));
                row.SubItems.Add((multiplyStation.Qk[i] == null) ? "" :
                    multiplyStation.Qk[i].waitState.ToString().Substring(0, 1));
                row.SubItems.Add(multiplyStation.results[i].ToString());
                reservationStation2.Items.Add(row);
            }

            // Load.
            loadBuffers.Clear();
            loadBuffers.View = View.Details;
            loadBuffers.Columns.Add("Name", 50);
            loadBuffers.Columns.Add("Busy", 35);
            loadBuffers.Columns.Add("Addr", 50);
            
            for (int i = 0; i < loadStation.numBuffers; i++)
            {
                ListViewItem row = new ListViewItem("Load" + (i + 1).ToString());
                row.SubItems.Add(loadStation.IsBusy(i).ToString().Substring(0, 1));
                row.SubItems.Add(loadStation.addresses[i].ToString());
                loadBuffers.Items.Add(row);
            }

            // Store.
            storeBuffers.Clear();
            storeBuffers.View = View.Details;
            storeBuffers.Columns.Add("Name", 40);
            storeBuffers.Columns.Add("Busy", 35);
            storeBuffers.Columns.Add("Addr", 50);
            storeBuffers.Columns.Add("Q", 50);
            storeBuffers.Columns.Add("V", 50);

            for (int i = 0; i < loadStation.numBuffers; i++)
            {
                ListViewItem row = new ListViewItem("Store" + (i + 1).ToString());
                row.SubItems.Add(storeStation.IsBusy(i).ToString().Substring(0, 1));
                row.SubItems.Add(storeStation.addresses[i].ToString());
                row.SubItems.Add((storeStation.Qj[i] == null) ? "" :
                    storeStation.Qj[i].waitState.ToString().Substring(0, 1) + storeStation.Qj[i].value);
                row.SubItems.Add((storeStation.Vj[i] == null) ? "" :
                    storeStation.Qj[i].waitState.ToString().Substring(0, 1));
                storeBuffers.Items.Add(row);
            }
        }

        private void UpdateFPRegisterBox()
        {
            fpRegisters.Clear();
            fpRegisters.View = View.Details;
            fpRegisters.Columns.Add("");
            for (int i = 0; i < floatRegs.GetNumRegs(); i++)
            {
                fpRegisters.Columns.Add("F" + i.ToString());
            }

            ListViewItem row = new ListViewItem("FU");
            for (int i = 0; i < floatRegs.GetNumRegs(); i++)
            {
                switch (floatRegs.Get(i).waitState)
                {
                    case WaitInfo.WaitState.AddStation:
                        row.SubItems.Add("Add" + (floatRegs.Get(i).value + 1).ToString());
                        break;

                    case WaitInfo.WaitState.Avail:
                        row.SubItems.Add(floatRegs.Get(i).value.ToString());
                        break;

                    case WaitInfo.WaitState.Compute:
                        row.SubItems.Add(floatRegs.Get(i).value.ToString());
                        break;

                    case WaitInfo.WaitState.LoadStation:
                        row.SubItems.Add("Load" + (floatRegs.Get(i).value + 1).ToString());
                        break;

                    case WaitInfo.WaitState.MultStation:
                        row.SubItems.Add("Mult" + (floatRegs.Get(i).value + 1).ToString());
                        break;

                    case WaitInfo.WaitState.StoreStation:
                        row.SubItems.Add("Store" + (floatRegs.Get(i).value + 1).ToString());
                        break;
                }
            }
            fpRegisters.Items.Add(row);
        }

        // This is the main method that will run a cycle using Tomasulo's Algorithm.
        private void RunOneCycle()
        {   // Do backwards so that only 1 stage is run on each instruction per cycle.
            Write();
            Execute();
            Issue();

            UpdateInstructionQueueBox();
            UpdateReservationStationBoxes();
            UpdateFPRegisterBox();
        }

        private WaitInfo FindRegister(string name)
        {
            if (name.Substring(0, 1) == "F")
            {   // Floating Point.
                return floatRegs.Get(Int32.Parse(name.Substring(1)));
            }
            else if (name.Substring(0, 1) == "R")
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
                if (addStation.RunExecution(i) == -1)
                {   // Check operand avalability.
                    if (addStation.Qj[i] != null)
                    {
                        if (addStation.Qj[i].waitState == WaitInfo.WaitState.Avail)
                        {
                            addStation.Vj[i] = new Operand(Operand.OperandType.FloatReg, addStation.Qj[i].value);
                            addStation.Qj[i] = null;
                        }
                    }
                    if (addStation.Qk[i] != null)
                    {
                        if (addStation.Qk[i].waitState == WaitInfo.WaitState.Avail)
                        {
                            addStation.Vk[i] = new Operand(Operand.OperandType.FloatReg, addStation.Qk[i].value);
                            addStation.Qk[i] = null;
                        }
                    }
                }
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
                    addStation.Free(i);
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
                    multiplyStation.Free(i);
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
                    loadStation.Free(i);
                }
            }

            // Store Results.
            for (int i = 0; i < loadStation.numBuffers; i++)
            {
                if (storeStation.results[i] != -1)
                {   // Ready.
                    memLocs.Set(storeStation.results[i], (int)storeStation.dest[i].opVal);
                    storeStation.Free(i);
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