using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Text;
using System.IO;

namespace Tomasulo
{
    public partial class Form1 : Form
    {
        Form2 form2;
        Form3 form3;//添加浮点、通用寄存器值

        //暂停、继续标志
        bool pause_or_continue = true;
        bool run = true;
        public List<int> delay;
        public int click = 0;

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
            //设置原始代码，重点在于instructionUnit，
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

            floatRegs = new FloatingPointRegisters(30);
            for (int i = 0; i < 30; i++)
            {
                floatRegs.Set(WaitInfo.WaitState.Avail, i + 1, i);
            }
            //初始化整型寄存器
            intRegs = new IntegerRegisters(30);
            //初始化浮点寄存器
            memLocs = new FloatingPointMemoryArrary(64);
            for (int i = 0; i < 64; i++)
            {
                memLocs.Set(i + 1, i);
            }

            UpdateInstructionQueueBox();
            UpdateIssuedInstructionsBox();
            UpdateFPRegisterBox();
            UpdateIntRegisterBox();
            UpdateClockCountBox();

            //初始化bool值，可以自动开始或者暂停
            pause_or_continue = true;
            run = true;
        }

        private void Form1_Load(object sender, EventArgs e)

        {
            form2 = new Form2();
            form3 = new Form3();
            delay = new List<int>();
            //默认延迟都是一个周期
            delay.Add(1);//load默认值
            delay.Add(1);//add
            delay.Add(1);//multi
            delay.Add(1);//div
            delay.Add(1);//store
            delay.Add(1);//branch


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
            //保留站初始化
            loadStation = new ReservationStation(3, ReservationStation.RSType.Load);
            storeStation = new ReservationStation(3, ReservationStation.RSType.Store);
            addStation = new ReservationStation(3, ReservationStation.RSType.Add);
            multiplyStation = new ReservationStation(2, ReservationStation.RSType.Multiply);
            branchStation = new ReservationStation(5, ReservationStation.RSType.Multiply);
            UpdateReservationStationBoxes();




            Init();
        }

        //批量设置浮点寄存器值
        public void SetFloatingPointMemory(float value)
        {
            for (int i = 0; i < 30; i++)
            {
                floatRegs.Set(WaitInfo.WaitState.Avail, value, i);
            }
        }

        //设置单个浮点寄存器值
        public void SetSingleFloatPointMemory(float value,int index)
        {
            floatRegs.Set(WaitInfo.WaitState.Avail, value, index);
        }

        //批量设置通用寄存器值
        public void SetIntRegister(int value)
        {
            for (int i = 0; i < 30; i++)
            {
                intRegs.Set(WaitInfo.WaitState.Avail, value, i);
            }

        }
        
        //设置单个通用寄存器值
        public void SetSingleIntRegister(int value,int index)
        {
            intRegs.Set(WaitInfo.WaitState.Avail, value, index);
        }

        //批量设置存储器
        public void SetFloatingPointMemoryArray(float value_float)
        {
            for(int i = 0; i < 64; i++)
            {
                memLocs.Set(value_float, i);
            }

        }

        //检查保留站是否还有剩余，用于自动执行停下
        //true表示所有的保留站都是空的
        public bool IsAllBufferFree()
        {
            if (!addStation.IsAllBufferFree()) return false;
            if (!storeStation.IsAllBufferFree()) return false;
            if (!multiplyStation.IsAllBufferFree()) return false;
            if (!branchStation.IsAllBufferFree()) return false;
            if (!loadStation.IsAllBufferFree()) return false;
            return true;
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
            reservationStation1.Columns.Add("Busy", 50);
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
            reservationStation2.Columns.Add("Busy", 50);
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
            storeBuffers.Columns.Add("Name", 50);
            storeBuffers.Columns.Add("Busy", 50);
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

        public void UpdateFPRegisterBox()
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

        public void UpdateIntRegisterBox()
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
            //所以也出现了错误，本来这三个是没有先后顺序的，现在变成有先后顺序，所以会有错
            Write();
            Execute();
            Issue();
            //这里调换了以下顺序，感觉这样才是对的
            //后来发现，如果先流入，然后执行，就变成一个时钟周期会做三件事，这样更错
            //QAQ
            //无解

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

        //流入
        private void Issue()//所有的延迟都在这边，参数传入
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
                            bufNum, jReg, kReg, wsJ, wsK, delay[1]);
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
                        if(instruction.opcode==Instruction.Opcodes.MULD)
                            multiplyStation.PutInBuffer(instructionUnit.GetInstruction(),
                            bufNum, jReg, kReg, wsJ, wsK,delay[2]);
                        else multiplyStation.PutInBuffer(instructionUnit.GetInstruction(),
                            bufNum, jReg, kReg, wsJ, wsK, delay[3]);
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
                            bufNum, jReg, kReg, wsJ, wsK, delay[0]);
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
                            bufNum, jReg, kReg, wsJ, wsK,delay[4]);
                        storeStation.instrNum[bufNum] = issuedInstructions.Count - 1;

                        //原来居然没写！！！
                        if (instruction.dest.Substring(0, 1) == "F")
                        {
                            floatRegs.Set(WaitInfo.WaitState.StoreStation, bufNum,
                            Int32.Parse(instruction.dest.Substring(1)));
                        }
                        else
                        {
                            intRegs.Set(WaitInfo.WaitState.StoreStation, bufNum,
                            Int32.Parse(instruction.dest.Substring(1)));
                        }
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
                            bufNum, jReg, kReg, wsJ, wsK,delay[5]);
                        branchStation.instrNum[bufNum] = issuedInstructions.Count - 1;

                    }
                    else
                    {
                        Console.WriteLine("Stalling due to a structural hazard.");
                    }
                    break;
            }
        }

        //修改代码
        private void editInstructions_Click(object sender, EventArgs e)
        {
            form2.parent = this;
            form2.insts = instructionUnit.GetCurrentInstructions();
            form2.Show();
            Hide();
        }
        
        //reset
        private void button1_Click(object sender, EventArgs e)
        {
            instructionUnit.ClearInstructions();
            foreach (Instruction inst in originalInstructions)
            {
                instructionUnit.AddInstruction(inst);
            }
            Init();
        }

        //设置浮点、通用寄存器的值
        private void setfloatvalue_Click(object sender, EventArgs e)
        {
            form3.parent = this;
            form3.Show();
            Hide();
        }

        //自动
        private void Auto_Click(object sender, EventArgs e)
        {
            run = true;
            while (run)
            {
                if (!pause_or_continue) break;
                //缺少一个执行完毕可以停下来
               // if (instructionUnit.GetCurrentInstructions().Length == 0) break;
               //如果每个保留站都是空的，则完成
                System.Threading.Thread.Sleep(1000);
                clocks++;
                RunOneCycle();
                UpdateClockCountBox();
                //TODO:每个保留站检查是否清空
                Application.DoEvents();
                if (IsAllBufferFree()) break;
            }

        }
        
        //暂停or继续，通过一个bool值
        private void button2_Click(object sender, EventArgs e)
        {
            pause_or_continue = !pause_or_continue;
            Auto_Click(sender, e);
        }

        //停止进程
        private void Stop_Click(object sender, EventArgs e)
        {
            run = false;
        }

        //疯了，怎么会有这么蠢的方法
        private void read_file_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("program.txt", FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(fs, Encoding.Default);
            string strReadline;
            //一个操作码和三个操作数
            string s2 = "", s3 = "", s4 = "";
            int i = 0;
            instructionUnit.ClearInstructions();
            while ((strReadline = read.ReadLine()) != null)
            {
                switch (strReadline[0])
                {
                    case 'L':
                        i = 3;
                        while (strReadline[i] != ',')
                        {
                            s2 = s2 + strReadline[i];
                            i++;
                        }
                        i++;
                        while (strReadline[i] != '(')
                        {
                            s3 = s3 + strReadline[i];
                            i++;
                        }
                        s3 = s3 + '+';
                        i++;
                        while (strReadline[i] != ')')
                        {
                            s4 = s4 + strReadline[i];
                            i++;
                        }
                        instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.LD, s2, s3, s4));
                        s2 = "";
                        s3 = "";
                        s4 = "";
                        break;
                    case 'S':
                        if (strReadline[1] == 'D')
                        {
                          i = 3;
                        while (strReadline[i] != ',')
                        {
                            s2 = s2 + strReadline[i];
                            i++;
                        }
                        i++;
                        while (strReadline[i] != ',')
                        {
                            s3 = s3 + strReadline[i];
                            i++;
                        }
                        i++;
                        while (true)
                        {
                            s4 = s4 + strReadline[i];
                            i++;
                            if (i == strReadline.Length) break;
                        }
                            instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.SD, s2, s3, s4));
                            s2 = "";
                            s3 = "";
                            s4 = "";
                            break;
                        }
                        else//SUBD
                        {
                            i = 5;
                            while (strReadline[i] != ',')
                            {
                                s2 = s2 + strReadline[i];
                                i++;
                            }
                            i++;
                            while (strReadline[i] != ',')
                            {
                                s3 = s3 + strReadline[i];
                                i++;
                            }
                            i++;
                            while (true)
                            {
                                s4 = s4 + strReadline[i];
                                i++;
                                if (i == strReadline.Length) break;
                            }
                            instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.SUBD, s2, s3, s4));
                            s2 = "";
                            s3 = "";
                            s4 = "";
                            break;
                        }
                    case 'A':
                        i = 5;
                        while (strReadline[i] != ',')
                        {
                            s2 = s2 + strReadline[i];
                            i++;
                        }
                        i++;
                        while (strReadline[i] != ',')
                        {
                            s3 = s3 + strReadline[i];
                            i++;
                        }
                        i++;
                        while (true)
                        {
                            s4 = s4 + strReadline[i];
                            i++;
                            if (i == strReadline.Length) break;
                        }
                        instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.ADDD, s2, s3, s4));
                        s2 = "";
                        s3 = "";
                        s4 = "";
                        break;
                    case 'M':
                        i = 5;
                        while (strReadline[i] != ',')
                        {
                            s2 = s2 + strReadline[i];
                            i++;
                        }
                        i++;
                        while (strReadline[i] != ',')
                        {
                            s3 = s3 + strReadline[i];
                            i++;
                        }
                        i++;
                        while (true)
                        {
                            s4 = s4 + strReadline[i];
                            i++;
                            if (i == strReadline.Length) break;
                        }
                        instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.MULD, s2, s3, s4));
                        s2 = "";
                        s3 = "";
                        s4 = "";
                        break;
                    case 'D':
                        i = 5;
                        while (strReadline[i] != ',')
                        {
                            s2 = s2 + strReadline[i];
                            i++;
                        }
                        i++;
                        while (strReadline[i] != ',')
                        {
                            s3 = s3 + strReadline[i];
                            i++;
                        }
                        i++;
                        while (true)
                        {
                            s4 = s4 + strReadline[i];
                            i++;
                            if (i == strReadline.Length) break;
                        }
                        instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.DIVD, s2, s3, s4));
                        s2 = "";
                        s3 = "";
                        s4 = "";
                        break;
                    case 'B':
                        i = 4;
                        while (strReadline[i] != ',')
                        {
                            s2 = s2 + strReadline[i];
                            i++;
                        }
                        i++;
                        while (strReadline[i] != ',')
                        {
                            s3 = s3 + strReadline[i];
                            i++;
                        }
                        i++;
                        while (true)
                        {
                            s4 = s4 + strReadline[i];
                            i++;
                            if (i == strReadline.Length) break;
                        }
                        instructionUnit.AddInstruction(new Instruction(Instruction.Opcodes.BNE, s2, s3, s4));
                        s2 = "";
                        s3 = "";
                        s4 = "";
                        break;
                }
            }
            fs.Close();
            read.Close();
            UpdateInstructionQueueBox();
        }

        //一如既往的蠢
        private void Reservation_read_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("parts.txt", FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(fs, Encoding.Default);
            string strReadline;
            int i = 0, read_delay, read_number, read_delay2;
            string s1 = "", s2 = "", s3 = "";
            //清空原来的
                addStation.ClearReservation();
                storeStation.ClearReservation();
                loadStation.ClearReservation();
                multiplyStation.ClearReservation();
                branchStation.ClearReservation();

            while ((strReadline = read.ReadLine())!= null)
            {
                switch (strReadline[0])
                {
                    case 'L':
                        i = 5;
                        while(strReadline[i]!=' ')
                        {
                            s1 = s1 + strReadline[i];
                            i++;
                        }
                        i++;
                        while (i < strReadline.Length)
                        {
                            s2 = s2 + strReadline[i];
                            i++;
                        }
                        read_delay = int.Parse(s1);
                        read_number = int.Parse(s2);
                        delay[0] = read_delay;
                        loadStation = new ReservationStation(read_number, ReservationStation.RSType.Load);
                        s1 = "";
                        s2 = "";
                        s3 = "";
                        break;
                    case 'A':
                        i = 4;
                        while (strReadline[i] != ' ')
                        {
                            s1 = s1 + strReadline[i];
                            i++;
                        }
                        i++;
                        while (i < strReadline.Length)
                        {
                            s2 = s2 + strReadline[i];
                            i++;
                        }
                        read_delay = int.Parse(s1);
                        delay[1] = read_delay;
                        read_number = int.Parse(s2);
                        addStation = new ReservationStation(read_number, ReservationStation.RSType.Add);
                        s1 = "";
                        s2 = "";
                        s3 = "";
                        break;
                    case 'M':
                        i = 6;
                        while (strReadline[i] != ',')
                        {
                            s1 = s1 + strReadline[i];
                            i++;
                        }
                        i++;
                        while (strReadline[i]!=' ')
                        {
                            s2 = s2 + strReadline[i];
                            i++;
                        }
                        i++;
                        while (i < strReadline.Length)
                        {
                            s3 = s3 + strReadline[i];
                            i++;
                        }
                        read_delay = int.Parse(s1);
                        read_delay2 = int.Parse(s2);
                        read_number = int.Parse(s3);
                        delay[2] = read_delay;
                        delay[3] = read_delay2;
                        multiplyStation = new ReservationStation(read_number, ReservationStation.RSType.Multiply);
                        s1 = "";
                        s2 = "";
                        s3 = "";
                        break;
                    case 'S':
                        i = 6;
                        while (strReadline[i] != ' ')
                        {
                            s1 = s1 + strReadline[i];
                            i++;
                        }
                        i++;
                        while (i < strReadline.Length)
                        {
                            s2 = s2 + strReadline[i];
                            i++;
                        }
                        read_delay = int.Parse(s1);
                        read_number = int.Parse(s2);
                        delay[4] = read_delay;
                        storeStation = new ReservationStation(read_number, ReservationStation.RSType.Store);
                        s1 = "";
                        s2 = "";
                        s3 = "";
                        break;
                    case '#':
                        break;
                }
            }
            
            fs.Close();
            read.Close();
            branchStation = new ReservationStation(5, ReservationStation.RSType.Multiply);
            UpdateReservationStationBoxes();
        }

        

        //load和store延2，add和multi延1，分支延1，原本
        //修改后都需要经过RunExecution延迟执行，都是延迟1个时钟周期，并且根据传入的延迟参数延迟执行
        private void Execute()
        {
            int result;

            // Add Station.
            for (int i = 0; i < addStation.numBuffers; i++)
            {
                if ((result = addStation.RunExecution(i)) == -1)
                {   // Check operand avalability.
                    //更新保留站，WB环节
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
                    //执行结束
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
                //addr好的时候不延迟，直接做，这里加一下,本来就有2个周期的延迟，最少了
                    //这边就是load延迟两个时钟周期的原因
                    if ((result=loadStation.RunExecution(i)) == -1)
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
                    else if(result==0)
                    {
                        loadStation.addresses[i] = loadStation.ComputeAddress(intRegs, i);
                        loadStation.cyclesToComplete[i] = 2;    // Arbitrary, make user-settable later.
                        loadStation.remainingCycles[i] = -1;
                        loadStation.isReady[i] = false;
                        loadStation.addrReady[i] = true;
                        // Address Computed.
                        loadStation.GetFromMemory(i, memLocs);//在里面直接设置执行结束
                        //这个地方会有延迟
                        if (executeClocks[loadStation.instrNum[i]] == -1)
                        {
                            executeClocks[loadStation.instrNum[i]] = clocks;
                        }
                    }
                
            }

            // Store Station.
            //这里做一个大改动，原来store和load都是没办法延迟周期，觉得超级奇怪
            //因为在RunExecution里面应该要有延迟执行的才对
            //后来发现居然是因为没有用reslut...直接如果RunExecution！=-1就到下一步了，就是固定2个周期的延迟
            //我还傻傻的改了好几天
            //愣是没发现错，以后再有这样的QAQ，先看是哪里不一样！！
            for (int i = 0; i < storeStation.numBuffers; i++)
            {
                // Compute Address.
                    if ((result=storeStation.RunExecution(i)) == -1)
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
                    else if(result==0)
                    {
                        storeStation.addresses[i] = storeStation.ComputeAddress(intRegs, i);
                    //这个延迟是wb的延迟
                    //一开始还以为是RunExecution的延迟
                    //而且其实这里一开始的RunExecution也没有延迟，233，傻了吧
                        storeStation.cyclesToComplete[i] = 2;   // Arbitrary, make user-settable later.
                        storeStation.remainingCycles[i] = -1;
                        storeStation.isReady[i] = false;
                        storeStation.addrReady[i] = true;

                        // Address Computed.
                        storeStation.BufferValue(i, floatRegs, intRegs);
                        if (executeClocks[storeStation.instrNum[i]] == -1)
                        {
                            executeClocks[storeStation.instrNum[i]] = clocks;
                        }
                    }
                
            }

            // Branch Station.
            for (int i = 0; i < branchStation.numBuffers; i++)
            {
                //感到深深的恶意，为什么分支加法乘法都是在执行里面可以延迟，load和store就不这么写呢
                //？？？
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
                    //这里有一个错就是，先做了wb，相当于本来是一起做的，现在有先后顺序，在检验冲突的时候，有一个时钟周期是错的
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