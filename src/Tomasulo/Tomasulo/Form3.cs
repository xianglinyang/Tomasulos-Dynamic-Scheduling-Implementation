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
    //设置浮点寄存器内容
    public partial class Form3 : Form
    {
        public Form1 parent;
        public float value_float;
        public int index;
        public int value_int;

        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            parent.Show();
            value_float = float.Parse(textBox1.Text);
            //parent.Init();
            parent.SetFloatingPointMemory(value_float);
            parent.UpdateFPRegisterBox();
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            parent.Show();
            value_float = float.Parse(textBox3.Text);
            index = int.Parse(textBox2.Text);
           // parent.Init();
            parent.SetSingleFloatPointMemory(value_float, index);
            parent.UpdateFPRegisterBox();
            Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            parent.Show();
            value_int = int.Parse(textBox4.Text);
            //parent.Init();
            parent.SetIntRegister(value_int);
            parent.UpdateIntRegisterBox();
            Hide();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            parent.Show();
            value_int = int.Parse(textBox6.Text);
            index = int.Parse(textBox5.Text);
            // parent.Init();
            parent.SetSingleIntRegister(value_int, index);
            parent.UpdateIntRegisterBox();
            Hide();
        }

        private void SetMemory_Click(object sender, EventArgs e)
        {
            parent.Show();
            value_float = float.Parse(textBox7.Text);
            parent.SetFloatingPointMemoryArray(value_float);
            Hide();
        }
    }
}
