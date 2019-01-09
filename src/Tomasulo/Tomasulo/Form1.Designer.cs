namespace Tomasulo
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.instructionQueue = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.loadBuffers = new System.Windows.Forms.ListView();
            this.storeBuffers = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.fpRegisters = new System.Windows.Forms.ListView();
            this.reservationStation1 = new System.Windows.Forms.ListView();
            this.reservationStation2 = new System.Windows.Forms.ListView();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.Clock = new System.Windows.Forms.Button();
            this.ClockCount = new System.Windows.Forms.Label();
            this.intRegisters = new System.Windows.Forms.ListView();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.editInstructions = new System.Windows.Forms.Button();
            this.issuedInstructionBox = new System.Windows.Forms.ListView();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.setfloatvalue = new System.Windows.Forms.Button();
            this.Auto = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.Stop = new System.Windows.Forms.Button();
            this.Instruction_read = new System.Windows.Forms.Button();
            this.Reservation_read = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // instructionQueue
            // 
            this.instructionQueue.Location = new System.Drawing.Point(4, 46);
            this.instructionQueue.Margin = new System.Windows.Forms.Padding(4);
            this.instructionQueue.Name = "instructionQueue";
            this.instructionQueue.Size = new System.Drawing.Size(343, 190);
            this.instructionQueue.TabIndex = 0;
            this.instructionQueue.UseCompatibleStateImageBehavior = false;
            this.instructionQueue.SelectedIndexChanged += new System.EventHandler(this.instructionQueue_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(354, 46);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 29);
            this.label1.TabIndex = 2;
            this.label1.Text = "Instruction";
            // 
            // loadBuffers
            // 
            this.loadBuffers.Location = new System.Drawing.Point(1197, 22);
            this.loadBuffers.Margin = new System.Windows.Forms.Padding(4);
            this.loadBuffers.Name = "loadBuffers";
            this.loadBuffers.Size = new System.Drawing.Size(230, 156);
            this.loadBuffers.TabIndex = 0;
            this.loadBuffers.UseCompatibleStateImageBehavior = false;
            this.loadBuffers.SelectedIndexChanged += new System.EventHandler(this.loadBuffers_SelectedIndexChanged);
            // 
            // storeBuffers
            // 
            this.storeBuffers.Location = new System.Drawing.Point(836, 22);
            this.storeBuffers.Margin = new System.Windows.Forms.Padding(4);
            this.storeBuffers.Name = "storeBuffers";
            this.storeBuffers.Size = new System.Drawing.Size(350, 156);
            this.storeBuffers.TabIndex = 0;
            this.storeBuffers.UseCompatibleStateImageBehavior = false;
            this.storeBuffers.SelectedIndexChanged += new System.EventHandler(this.loadBuffers_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1192, -3);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 29);
            this.label2.TabIndex = 3;
            this.label2.Text = "Load";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(831, -3);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 29);
            this.label3.TabIndex = 4;
            this.label3.Text = "Store";
            // 
            // fpRegisters
            // 
            this.fpRegisters.Location = new System.Drawing.Point(143, 487);
            this.fpRegisters.Margin = new System.Windows.Forms.Padding(4);
            this.fpRegisters.Name = "fpRegisters";
            this.fpRegisters.Size = new System.Drawing.Size(1288, 91);
            this.fpRegisters.TabIndex = 10;
            this.fpRegisters.UseCompatibleStateImageBehavior = false;
            // 
            // reservationStation1
            // 
            this.reservationStation1.Location = new System.Drawing.Point(836, 334);
            this.reservationStation1.Margin = new System.Windows.Forms.Padding(4);
            this.reservationStation1.Name = "reservationStation1";
            this.reservationStation1.Size = new System.Drawing.Size(592, 144);
            this.reservationStation1.TabIndex = 11;
            this.reservationStation1.UseCompatibleStateImageBehavior = false;
            // 
            // reservationStation2
            // 
            this.reservationStation2.Location = new System.Drawing.Point(836, 184);
            this.reservationStation2.Margin = new System.Windows.Forms.Padding(4);
            this.reservationStation2.Name = "reservationStation2";
            this.reservationStation2.Size = new System.Drawing.Size(592, 144);
            this.reservationStation2.TabIndex = 12;
            this.reservationStation2.UseCompatibleStateImageBehavior = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(634, 1);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(185, 37);
            this.label8.TabIndex = 15;
            this.label8.Text = "Reservation";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(-6, 521);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(153, 29);
            this.label9.TabIndex = 16;
            this.label9.Text = "FP Registers";
            // 
            // Clock
            // 
            this.Clock.Location = new System.Drawing.Point(4, 10);
            this.Clock.Margin = new System.Windows.Forms.Padding(4);
            this.Clock.Name = "Clock";
            this.Clock.Size = new System.Drawing.Size(106, 28);
            this.Clock.TabIndex = 18;
            this.Clock.Text = "Step";
            this.Clock.UseVisualStyleBackColor = true;
            this.Clock.Click += new System.EventHandler(this.Clock_Click);
            // 
            // ClockCount
            // 
            this.ClockCount.AutoSize = true;
            this.ClockCount.Location = new System.Drawing.Point(118, 12);
            this.ClockCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ClockCount.Name = "ClockCount";
            this.ClockCount.Size = new System.Drawing.Size(17, 18);
            this.ClockCount.TabIndex = 19;
            this.ClockCount.Text = "0";
            // 
            // intRegisters
            // 
            this.intRegisters.Location = new System.Drawing.Point(143, 586);
            this.intRegisters.Margin = new System.Windows.Forms.Padding(4);
            this.intRegisters.Name = "intRegisters";
            this.intRegisters.Size = new System.Drawing.Size(1288, 91);
            this.intRegisters.TabIndex = 20;
            this.intRegisters.UseCompatibleStateImageBehavior = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 604);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(147, 29);
            this.label4.TabIndex = 21;
            this.label4.Text = "Int Registers";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(688, 29);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 37);
            this.label5.TabIndex = 22;
            this.label5.Text = "Stations";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(749, 184);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 29);
            this.label6.TabIndex = 23;
            this.label6.Text = "Multiply";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(785, 334);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 29);
            this.label7.TabIndex = 24;
            this.label7.Text = "Add";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(354, 68);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(86, 29);
            this.label10.TabIndex = 25;
            this.label10.Text = "Queue";
            // 
            // editInstructions
            // 
            this.editInstructions.Location = new System.Drawing.Point(358, 97);
            this.editInstructions.Margin = new System.Windows.Forms.Padding(4);
            this.editInstructions.Name = "editInstructions";
            this.editInstructions.Size = new System.Drawing.Size(112, 32);
            this.editInstructions.TabIndex = 26;
            this.editInstructions.Text = "Edit";
            this.editInstructions.UseVisualStyleBackColor = true;
            this.editInstructions.Click += new System.EventHandler(this.editInstructions_Click);
            // 
            // issuedInstructionBox
            // 
            this.issuedInstructionBox.Location = new System.Drawing.Point(4, 245);
            this.issuedInstructionBox.Margin = new System.Windows.Forms.Padding(4);
            this.issuedInstructionBox.Name = "issuedInstructionBox";
            this.issuedInstructionBox.Size = new System.Drawing.Size(544, 190);
            this.issuedInstructionBox.TabIndex = 27;
            this.issuedInstructionBox.UseCompatibleStateImageBehavior = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(550, 245);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(122, 29);
            this.label11.TabIndex = 28;
            this.label11.Text = "Instruction";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(554, 266);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(79, 29);
            this.label12.TabIndex = 29;
            this.label12.Text = "Status";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(143, 10);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 30);
            this.button1.TabIndex = 30;
            this.button1.Text = "Reset";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // setfloatvalue
            // 
            this.setfloatvalue.Location = new System.Drawing.Point(12, 455);
            this.setfloatvalue.Name = "setfloatvalue";
            this.setfloatvalue.Size = new System.Drawing.Size(112, 35);
            this.setfloatvalue.TabIndex = 31;
            this.setfloatvalue.Text = "set value";
            this.setfloatvalue.UseVisualStyleBackColor = true;
            this.setfloatvalue.Click += new System.EventHandler(this.setfloatvalue_Click);
            // 
            // Auto
            // 
            this.Auto.Location = new System.Drawing.Point(262, 10);
            this.Auto.Name = "Auto";
            this.Auto.Size = new System.Drawing.Size(110, 30);
            this.Auto.TabIndex = 32;
            this.Auto.Text = "Auto";
            this.Auto.UseVisualStyleBackColor = true;
            this.Auto.Click += new System.EventHandler(this.Auto_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(378, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(101, 30);
            this.button2.TabIndex = 33;
            this.button2.Text = "Pause";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Stop
            // 
            this.Stop.Location = new System.Drawing.Point(485, 12);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(101, 30);
            this.Stop.TabIndex = 34;
            this.Stop.Text = "Stop";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // Instruction_read
            // 
            this.Instruction_read.Location = new System.Drawing.Point(359, 136);
            this.Instruction_read.Name = "Instruction_read";
            this.Instruction_read.Size = new System.Drawing.Size(151, 33);
            this.Instruction_read.TabIndex = 35;
            this.Instruction_read.Text = "read from file";
            this.Instruction_read.UseVisualStyleBackColor = true;
            this.Instruction_read.Click += new System.EventHandler(this.read_file_Click);
            // 
            // Reservation_read
            // 
            this.Reservation_read.Location = new System.Drawing.Point(671, 73);
            this.Reservation_read.Name = "Reservation_read";
            this.Reservation_read.Size = new System.Drawing.Size(148, 37);
            this.Reservation_read.TabIndex = 36;
            this.Reservation_read.Text = "read from file";
            this.Reservation_read.UseVisualStyleBackColor = true;
            this.Reservation_read.Click += new System.EventHandler(this.Reservation_read_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1434, 656);
            this.Controls.Add(this.Reservation_read);
            this.Controls.Add(this.Instruction_read);
            this.Controls.Add(this.Stop);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Auto);
            this.Controls.Add(this.setfloatvalue);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.issuedInstructionBox);
            this.Controls.Add(this.editInstructions);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.intRegisters);
            this.Controls.Add(this.ClockCount);
            this.Controls.Add(this.Clock);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.reservationStation2);
            this.Controls.Add(this.reservationStation1);
            this.Controls.Add(this.fpRegisters);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.storeBuffers);
            this.Controls.Add(this.loadBuffers);
            this.Controls.Add(this.instructionQueue);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Tomasulo\'s Simulator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView instructionQueue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView loadBuffers;
        private System.Windows.Forms.ListView storeBuffers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView fpRegisters;
        private System.Windows.Forms.ListView reservationStation1;
        private System.Windows.Forms.ListView reservationStation2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button Clock;
        private System.Windows.Forms.Label ClockCount;
        private System.Windows.Forms.ListView intRegisters;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button editInstructions;
        private System.Windows.Forms.ListView issuedInstructionBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button setfloatvalue;
        private System.Windows.Forms.Button Auto;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.Button Instruction_read;
        private System.Windows.Forms.Button Reservation_read;
    }
}

