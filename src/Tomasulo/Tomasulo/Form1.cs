using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Tomasulo
{
    public partial class Form1 : Form
    {
        Form2 form2;
        public InstructionUnit instructionUnit = new InstructionUnit();
        Instruction[] originalInstructions;
        ReservationStation loadStation, storeStation, addStation, multiplyStation, branchStation;
        FloatingPointRegisters floatRegs;
        IntegerRegisters intRegs;
        FloatingPointMemoryArrary memLocs;
        private List<Instruction> issuedInstructions = new List<Instruction>();
        private int[] issueClocks;
        private int [] executeClocks;
        private int [] writeClocks;
        private int clocks = 0, numIssued = 0;
        int instrOffset = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void loadBuffers_SelectedIndexChanged(object sender, EventArgs e)
        {}

        public void Init()
        {
            clocks = 0;
            originalInstructions = instructionUnit.GetCurrentInstructions();

            issueClocks = new int[100];
            executeClocks = new int[100];
            writeClocks = new int[100];

            for (int i = 0; i < 100; i++)
            {
                issueClocks[i] = -1;
                executeClocks[i] = -1;
                writeClocks[i] = -1;
            }

            loadStation = new ReservationStation(3, ReservationStation.RSType.Load);
            storeStation = new ReservationStation(3, ReservationStation.RSType.Store);
            addStation = new ReservationStation(3, ReservationStation.RSType.Add);
            multiplyStation = new ReservationStation(2, ReservationStation.RSType.Multiply);
            branchStation = new ReservationStation(5, ReservationStation.RSType.Multiply);

            floatRegs = new FloatingPointRegisters(30);
            for (int i = 0; i < 30; i++)
            {
                floatRegs.Set(WaitInfo.WaitState.Avail, i + 1, i);
            }
            intRegs = new IntegerRegisters(30);
            memLocs = new FloatingPointMemoryArrary(64);
            for (int i = 0; i < 64; i++)
            {
                memLocs.Set(i + 1, i);
            }

            UpdateInstructionQueueBox();
            UpdateIssuedInstructionsBox();
            UpdateReservationStationBoxes();
            UpdateFPRegisterBox();
            UpdateIntRegisterBox();
            UpdateClockCountBox();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            form2 = new Form2();

            // Defaults. Can also get from user.
            instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.LD, "F6", "34+", "R2"));
            instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.LD, "F2", "45+", "R3"));
            instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.SD, "F2", "3", "R3"));
            instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.LD, "F25", "3+", "0"));
            instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.MULD, "F0", "F2", "F4"));
            instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.SUBD, "F8", "F0", "F6"));
            instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.DIVD, "F10", "F0", "F6"));
            instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.ADDD, "F6", "F8", "F2"));
            instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.BNE, "-8", "F6", "F10"));

            Init();
        }
        
        private void UpdateIssuedInstructionsBox()
        {
            issuedInstructionBox.Clear();
            issuedInstructionBox.View = View.Details;
            issuedInstructionBox.Columns.Add("Inst", 50);
            issuedInstructionBox.Columns.Add("Dest", 50);
            issuedInstructionBox.Columns.Add("J", 50);
            issuedInstructionBox.Columns.Add("K", 50);
            issuedInstructionBox.Columns.Add("Issued", 50);
            issuedInstructionBox.Columns.Add("Exec'd", 50);
            issuedInstructionBox.Columns.Add("Written", 50);

            int i = 0;
            foreach (Instruction inst in issuedInstructions)
            {
                ListViewItem row = new ListViewItem(inst.opcode.ToString());
                row.SubItems.Add(inst.dest);
                row.SubItems.Add(inst.j);
                row.SubItems.Add(inst.k);
                
                row.SubItems.Add(issueClocks[i].ToString());
                row.SubItems.Add(executeClocks[i].ToString());
                row.SubItems.Add(writeClocks[i].ToString());

                issuedInstructionBox.Items.Add(row);
                i++;
            }
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
                    addStation.Qj[i].waitState.ToString().Substring(0, 1) + (addStation.Qj[i].value + 1));
                row.SubItems.Add((addStation.Qk[i] == null) ? "" :
                    addStation.Qk[i].waitState.ToString().Substring(0, 1) + (addStation.Qk[i].value + 1));
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
                    multiplyStation.Qj[i].waitState.ToString().Substring(0, 1) + (multiplyStation.Qj[i].value + 1));
                row.SubItems.Add((multiplyStation.Qk[i] == null) ? "" :
                    multiplyStation.Qk[i].waitState.ToString().Substring(0, 1) + (multiplyStation.Qj[i].value + 1));
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

            for (int i = 0; i < storeStation.numBuffers; i++)
            {
                ListViewItem row = new ListViewItem("Store" + (i + 1).ToString());
                row.SubItems.Add(storeStation.IsBusy(i).ToString().Substring(0, 1));
                row.SubItems.Add(storeStation.addresses[i].ToString());
                row.SubItems.Add((storeStation.Qj[i] == null) ? "" :
                    storeStation.Qj[i].waitState.ToString().Substring(0, 1) + (storeStation.Qj[i].value + 1));
                row.SubItems.Add((storeStation.Vj[i] == null) ? "" :
                    storeStation.Vj[i].PrintString());
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

        private void UpdateIntRegisterBox()
        {
            intRegisters.Clear();
            intRegisters.View = View.Details;
            intRegisters.Columns.Add("");
            for (int i = 0; i < intRegs.GetNumRegs(); i++)
            {
                intRegisters.Columns.Add("R" + i.ToString());
            }

            ListViewItem row = new ListViewItem("FU");
            for (int i = 0; i < intRegs.GetNumRegs(); i++)
            {
                switch (intRegs.Get(i).waitState)
                {
                    case WaitInfo.WaitState.AddStation:
                        row.SubItems.Add("Add" + (intRegs.Get(i).value + 1).ToString());
                        break;

                    case WaitInfo.WaitState.Avail:
                        row.SubItems.Add(intRegs.Get(i).value.ToString());
                        break;

                    case WaitInfo.WaitState.Compute:
                        row.SubItems.Add(intRegs.Get(i).value.ToString());
                        break;

                    case WaitInfo.WaitState.LoadStation:
                        row.SubItems.Add("Load" + (intRegs.Get(i).value + 1).ToString());
                        break;

                    case WaitInfo.WaitState.MultStation:
                        row.SubItems.Add("Mult" + (intRegs.Get(i).value + 1).ToString());
                        break;

                    case WaitInfo.WaitState.StoreStation:
                        row.SubItems.Add("Store" + (intRegs.Get(i).value + 1).ToString());
                        break;
                }
            }
            intRegisters.Items.Add(row);
        }

        // This is the main method that will run a cycle using Tomasulo's Algorithm.
        private void RunOneCycle()
        {   // Do backwards so that only 1 stage is run on each instruction per cycle.
            Write();
            Execute();
            Issue();

            UpdateInstructionQueueBox();
            UpdateIssuedInstructionsBox();
            UpdateReservationStationBoxes();
            UpdateFPRegisterBox();
            UpdateIntRegisterBox();
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
                return intRegs.Get(Int32.Parse(name.Substring(1)));
            }
            else if (name[name.Length - 1] == '+')
            {   // Number offset.
                return new WaitInfo(float.Parse(name.Substring(0, name.Length - 1)),
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
                        issuedInstructions.Add(instruction);
                        issueClocks[numIssued++] = clocks;
                        addStation.PutInBuffer(instructionUnit.GetInstruction(),
                            bufNum, jReg, kReg, wsJ, wsK);
                        addStation.instrNum[bufNum] = issuedInstructions.Count - 1;

                        // Set Dest Reg.
                        if (instruction.dest.Substring(0, 1) == "F")
                        {   // Float.
                            floatRegs.Set(WaitInfo.WaitState.AddStation, bufNum,
                            Int32.Parse(instruction.dest.Substring(1)));
                        }
                        else
                        {   // Int.
                            intRegs.Set(WaitInfo.WaitState.AddStation, bufNum,
                            Int32.Parse(instruction.dest.Substring(1)));
                        }
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
                        issuedInstructions.Add(instruction);
                        issueClocks[numIssued++] = clocks;
                        multiplyStation.PutInBuffer(instructionUnit.GetInstruction(),
                            bufNum, jReg, kReg, wsJ, wsK);
                        multiplyStation.instrNum[bufNum] = issuedInstructions.Count - 1;

                        // Set Dest Reg.
                        if (instruction.dest.Substring(0, 1) == "F")
                        {
                            floatRegs.Set(WaitInfo.WaitState.MultStation, bufNum,
                            Int32.Parse(instruction.dest.Substring(1)));
                        }
                        else
                        {
                            intRegs.Set(WaitInfo.WaitState.MultStation, bufNum,
                            Int32.Parse(instruction.dest.Substring(1)));
                        }
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
                        issuedInstructions.Add(instruction);
                        issueClocks[numIssued++] = clocks;
                        loadStation.PutInBuffer(instructionUnit.GetInstruction(),
                            bufNum, jReg, kReg, wsJ, wsK);
                        loadStation.instrNum[bufNum] = issuedInstructions.Count - 1;

                        // Set Dest Reg.
                        if (instruction.dest.Substring(0, 1) == "F")
                        {
                            floatRegs.Set(WaitInfo.WaitState.LoadStation, bufNum,
                            Int32.Parse(instruction.dest.Substring(1)));
                        }
                        else
                        {
                            intRegs.Set(WaitInfo.WaitState.LoadStation, bufNum,
                            Int32.Parse(instruction.dest.Substring(1)));
                        }
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
                        issuedInstructions.Add(instruction);
                        issueClocks[numIssued++] = clocks;
                        storeStation.PutInBuffer(instructionUnit.GetInstruction(),
                            bufNum, jReg, kReg, wsJ, wsK);
                        storeStation.instrNum[bufNum] = issuedInstructions.Count - 1;
                    }
                    else
                    {
                        Console.WriteLine("Stalling due to a structural hazard.");
                    }
                    break;

                case Instruction.InstructionType.Branch:
                    if ((bufNum = branchStation.GetFreeBuffer()) != -1)
                    {
                        // Issue.
                        issuedInstructions.Add(instruction);
                        issueClocks[numIssued++] = clocks;
                        branchStation.PutInBuffer(instructionUnit.GetInstruction(),
                            bufNum, jReg, kReg, wsJ, wsK);
                        branchStation.instrNum[bufNum] = issuedInstructions.Count - 1;
                    }
                    else
                    {
                        Console.WriteLine("Stalling due to a structural hazard.");
                    }
                    break;
            }
        }

        private void editInstructions_Click(object sender, EventArgs e)
        {
            form2.parent = this;
            form2.insts = instructionUnit.GetCurrentInstructions();
            form2.Show();
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            instructionUnit.ClearInstructions();
            foreach (Instruction inst in originalInstructions)
            {
                instructionUnit.AddInstruction(inst);
            }
            Init();
        }

        private void Execute()
        {
            int result;

            // Add Station.
            for (int i = 0; i < addStation.numBuffers; i++)
            {
                if ((result = addStation.RunExecution(i)) == -1)
                {   // Check operand avalability.
                    if (addStation.Qj[i] != null)
                    {
                        if (addStation.Qj[i].waitState == WaitInfo.WaitState.Avail)
                        {
                            addStation.Vj[i] = new Operand(Operand.OperandType.Num, addStation.Qj[i].value);
                            addStation.Qj[i] = null;
                        }
                    }
                    if (addStation.Qk[i] != null)
                    {
                        if (addStation.Qk[i].waitState == WaitInfo.WaitState.Avail)
                        {
                            addStation.Vk[i] = new Operand(Operand.OperandType.Num, addStation.Qk[i].value);
                            addStation.Qk[i] = null;
                        }
                    }
                }
                else if (result == 0)
                {
                    if (executeClocks[addStation.instrNum[i]] == -1)
                    {
                        executeClocks[addStation.instrNum[i]] = clocks;
                    }
                }
            }

            // Multiply Station.
            for (int i = 0; i < multiplyStation.numBuffers; i++)
            {
                if ((result = multiplyStation.RunExecution(i)) == -1)
                {   // Check operand availability.
                    if (multiplyStation.Qj[i] != null)
                    {
                        if (multiplyStation.Qj[i].waitState == WaitInfo.WaitState.Avail)
                        {
                            multiplyStation.Vj[i] = new Operand(Operand.OperandType.Num, multiplyStation.Qj[i].value);
                            multiplyStation.Qj[i] = null;
                        }
                    }
                    if (multiplyStation.Qk[i] != null)
                    {
                        if (multiplyStation.Qk[i].waitState == WaitInfo.WaitState.Avail)
                        {
                            multiplyStation.Vk[i] = new Operand(Operand.OperandType.Num, multiplyStation.Qk[i].value);
                            multiplyStation.Qk[i] = null;
                        }
                    }
                }
                else if (result == 0)
                {
                    if (executeClocks[multiplyStation.instrNum[i]] == -1)
                    {
                        executeClocks[multiplyStation.instrNum[i]] = clocks;
                    }
                }
            }

            // Load Station.
            for (int i = 0; i < loadStation.numBuffers; i++)
            {
                if (loadStation.addrReady[i])
                {   // Address Computed.
                    loadStation.GetFromMemory(i, memLocs);
                    if (executeClocks[loadStation.instrNum[i]] == -1)
                    {
                        executeClocks[loadStation.instrNum[i]] = clocks;
                    }
                }
                else
                {   // Compute Address.
                    if (loadStation.RunExecution(i) == -1)
                    {   // Check operand availability.
                        if (loadStation.Qj[i] != null)
                        {
                            if (loadStation.Qj[i].waitState == WaitInfo.WaitState.Avail)
                            {
                                loadStation.Vj[i] = new Operand(Operand.OperandType.Num, loadStation.Qj[i].value);
                                loadStation.Qj[i] = null;
                            }
                        }
                        if (loadStation.Qk[i] != null)
                        {
                            if (loadStation.Qk[i].waitState == WaitInfo.WaitState.Avail)
                            {
                                loadStation.Vk[i] = new Operand(Operand.OperandType.Num, loadStation.Qk[i].value);
                                loadStation.Qk[i] = null;
                            }
                        }
                    }
                    else
                    {
                        loadStation.addresses[i] = loadStation.ComputeAddress(intRegs, i);
                        loadStation.cyclesToComplete[i] = 2;    // Arbitrary, make user-settable later.
                        loadStation.remainingCycles[i] = -1;
                        loadStation.isReady[i] = false;
                        loadStation.addrReady[i] = true;
                    }
                }
            }

            // Store Station.
            for (int i = 0; i < storeStation.numBuffers; i++)
            {
                if (storeStation.addrReady[i])
                {   // Address Computed.
                    storeStation.BufferValue(i, floatRegs, intRegs);
                    if (executeClocks[storeStation.instrNum[i]] == -1)
                    {
                        executeClocks[storeStation.instrNum[i]] = clocks;
                    }
                }
                else
                {   // Compute Address.
                    if (storeStation.RunExecution(i) == -1)
                    {   // Check operand availability.
                        if (storeStation.Qj[i] != null)
                        {
                            if (storeStation.Qj[i].waitState == WaitInfo.WaitState.Avail)
                            {
                                storeStation.Vj[i] = new Operand(Operand.OperandType.Num, storeStation.Qj[i].value);
                                storeStation.Qj[i] = null;
                            }
                        }
                    }
                    else
                    {
                        storeStation.addresses[i] = storeStation.ComputeAddress(intRegs, i);
                        storeStation.cyclesToComplete[i] = 2;   // Arbitrary, make user-settable later.
                        storeStation.remainingCycles[i] = -1;
                        storeStation.isReady[i] = false;
                        storeStation.addrReady[i] = true;
                    }
                }
            }

            // Branch Station.
            for (int i = 0; i < branchStation.numBuffers; i++)
            {
                if ((result = branchStation.RunExecution(i)) == -1)
                {   // Check operand avalability.
                    if (branchStation.Qj[i] != null)
                    {
                        if (branchStation.Qj[i].waitState == WaitInfo.WaitState.Avail)
                        {
                            branchStation.Vj[i] = new Operand(Operand.OperandType.Num, branchStation.Qj[i].value);
                            branchStation.Qj[i] = null;
                        }
                    }
                    if (branchStation.Qk[i] != null)
                    {
                        if (branchStation.Qk[i].waitState == WaitInfo.WaitState.Avail)
                        {
                            branchStation.Vk[i] = new Operand(Operand.OperandType.Num, branchStation.Qk[i].value);
                            branchStation.Qk[i] = null;
                        }
                    }
                }
                else if (result == 0)
                {
                    if (executeClocks[branchStation.instrNum[i]] == -1)
                    {
                        executeClocks[branchStation.instrNum[i]] = clocks;
                    }
                }
            }
            }

        private void Write()
        {
            // Add Results.
            for (int i = 0; i < addStation.numBuffers; i++)
            {
                if (addStation.isReady[i])
                {
                    writeClocks[addStation.instrNum[i]] = clocks;
                    if (addStation.dest[i].opType == Operand.OperandType.FloatReg)
                    {
                        floatRegs.Set(WaitInfo.WaitState.Avail, addStation.Compute(floatRegs, intRegs, i),
                            (int) addStation.dest[i].opVal);
                    }
                    else if (addStation.dest[i].opType == Operand.OperandType.IntReg)
                    {
                        intRegs.Set(WaitInfo.WaitState.Avail, (int)addStation.Compute(floatRegs, intRegs, i),
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
                if (multiplyStation.isReady[i])
                {
                    writeClocks[multiplyStation.instrNum[i]] = clocks;
                    if (multiplyStation.dest[i].opType == Operand.OperandType.FloatReg)
                    {
                        floatRegs.Set(WaitInfo.WaitState.Avail, multiplyStation.Compute(floatRegs, intRegs, i),
                            (int) multiplyStation.dest[i].opVal);
                    }
                    else if (multiplyStation.dest[i].opType == Operand.OperandType.IntReg)
                    {
                        intRegs.Set(WaitInfo.WaitState.Avail, (int) multiplyStation.Compute(floatRegs, intRegs, i),
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
                if (loadStation.isReady[i])
                {
                    writeClocks[loadStation.instrNum[i]] = clocks;
                    if (loadStation.dest[i].opType == Operand.OperandType.FloatReg)
                    {
                        floatRegs.Set(WaitInfo.WaitState.Avail, memLocs.Get(loadStation.addresses[i]),
                            (int) loadStation.dest[i].opVal);
                    }
                    else if (loadStation.dest[i].opType == Operand.OperandType.IntReg)
                    {
                        intRegs.Set(WaitInfo.WaitState.Avail, (int) memLocs.Get(loadStation.addresses[i]),
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
                if (storeStation.isReady[i])
                {   // Ready.
                    writeClocks[storeStation.instrNum[i]] = clocks;
                    memLocs.Set(storeStation.results[i], (int) storeStation.addresses[i]);
                    storeStation.Free(i);
                }
            }

            // Branch Results.
            for (int i = 0; i < branchStation.numBuffers; i++)
            {
                if (branchStation.isReady[i])
                {
                    writeClocks[branchStation.instrNum[i]] = clocks;
                    int amtToBranch = (int) branchStation.DetermineBranch(floatRegs, intRegs, i);
                    if (amtToBranch == 0)
                    {

                    }
                    else
                    {
                        // Clear Instruction Queue.
                        instructionUnit = new InstructionUnit();

                        // Put new instructions in.
                        int j;
                        for (j = branchStation.instrNum[i] + amtToBranch - instrOffset; j < originalInstructions.Length; j++)
                        {
                            instructionUnit.AddInstruction(originalInstructions[j]);
                        }
                        instrOffset += j;
                    }
                    branchStation.Free(i);
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