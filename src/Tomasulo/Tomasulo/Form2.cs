using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Tomasulo
{
    public partial class Form2 : Form
    {
        public Form1 parent;
        public Instruction[] insts;
        List<TextBox> tBoxes = new List<TextBox>();
        List<ComboBox> cBoxes = new List<ComboBox>();
        int tBoxIndex = 0, cBoxIndex = 0;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            GetInstructions();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            parent.Show();
            SetInstructions();
            parent.Init();
            Hide();
        }

        private void SetInstructions()
        {
            int tBIdx = 0;
            Instruction.Opcodes opC = Instruction.Opcodes.ADDD;
            parent.instructionUnit.ClearInstructions();
            foreach (ComboBox cbx in cBoxes)
            {
                switch (cbx.SelectedIndex)
                {
                    case 0:
                        opC = Instruction.Opcodes.ADDD;
                        break;

                    case 1:
                        opC = Instruction.Opcodes.SUBD;
                        break;

                    case 2:
                        opC = Instruction.Opcodes.MULD;
                        break;

                    case 3:
                        opC = Instruction.Opcodes.DIVD;
                        break;

                    case 4:
                        opC = Instruction.Opcodes.LD;
                        break;

                    case 5:
                        opC = Instruction.Opcodes.SD;
                        break;

                    case 6:
                        opC = Instruction.Opcodes.BEQ;
                        break;

                    case 7:
                        opC = Instruction.Opcodes.BNE;
                        break;
                }


                parent.instructionUnit.AddInstruction(new Instruction(opC, tBoxes[tBIdx].Text.ToString(), tBoxes[tBIdx + 1].Text.ToString(), tBoxes[tBIdx + 2].Text.ToString()));
                tBIdx += 3;
            }
        }

        private void GetInstructions()
        {
            int verticalOffset = 60;
            foreach (Instruction inst in insts)
            {
                // Set up ComboBox.
                cBoxes.Add(new ComboBox());
                cBoxes[cBoxIndex].Items.Add("ADDD");
                cBoxes[cBoxIndex].Items.Add("SUBD");
                cBoxes[cBoxIndex].Items.Add("MULD");
                cBoxes[cBoxIndex].Items.Add("DIVD");
                cBoxes[cBoxIndex].Items.Add("LD");
                cBoxes[cBoxIndex].Items.Add("SD");
                cBoxes[cBoxIndex].Items.Add("BEQ");
                cBoxes[cBoxIndex].Items.Add("BNE");

                // Set ComboBox Item.
                switch (inst.opcode)
                {
                    case Instruction.Opcodes.ADDD:
                        cBoxes[cBoxIndex].SelectedIndex = 0;
                        break;

                    case Instruction.Opcodes.SUBD:
                        cBoxes[cBoxIndex].SelectedIndex = 1;
                        break;

                    case Instruction.Opcodes.MULD:
                        cBoxes[cBoxIndex].SelectedIndex = 2;
                        break;

                    case Instruction.Opcodes.DIVD:
                        cBoxes[cBoxIndex].SelectedIndex = 3;
                        break;

                    case Instruction.Opcodes.LD:
                        cBoxes[cBoxIndex].SelectedIndex = 4;
                        break;

                    case Instruction.Opcodes.SD:
                        cBoxes[cBoxIndex].SelectedIndex = 5;
                        break;

                    case Instruction.Opcodes.BEQ:
                        cBoxes[cBoxIndex].SelectedIndex = 6;
                        break;

                    case Instruction.Opcodes.BNE:
                        cBoxes[cBoxIndex].SelectedIndex = 7;
                        break;

                }
                
                cBoxes[cBoxIndex].Top = verticalOffset;
                cBoxes[cBoxIndex].Left = 5;
                cBoxes[cBoxIndex].Width = 100;
                Controls.Add(cBoxes[cBoxIndex++]);

                tBoxes.Add(new TextBox());
                tBoxes[tBoxIndex].Text = inst.dest.ToString();
                tBoxes[tBoxIndex].Top = verticalOffset;
                tBoxes[tBoxIndex].Left = 105;
                tBoxes[tBoxIndex].Width = 50;
                Controls.Add(tBoxes[tBoxIndex++]);

                tBoxes.Add(new TextBox());
                tBoxes[tBoxIndex].Text = inst.j.ToString();
                tBoxes[tBoxIndex].Top = verticalOffset;
                tBoxes[tBoxIndex].Left = 155;
                tBoxes[tBoxIndex].Width = 50;
                Controls.Add(tBoxes[tBoxIndex++]);

                tBoxes.Add(new TextBox());
                tBoxes[tBoxIndex].Text = inst.k.ToString();
                tBoxes[tBoxIndex].Top = verticalOffset;
                tBoxes[tBoxIndex].Left = 205;
                tBoxes[tBoxIndex].Width = 50;
                Controls.Add(tBoxes[tBoxIndex++]);

                verticalOffset += 20;
            }
        }
    }
}