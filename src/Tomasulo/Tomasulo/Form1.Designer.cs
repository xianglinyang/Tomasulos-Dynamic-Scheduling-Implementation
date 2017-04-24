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
            this.SuspendLayout();
            // 
            // instructionQueue
            // 
            this.instructionQueue.Location = new System.Drawing.Point(3, 33);
            this.instructionQueue.Name = "instructionQueue";
            this.instructionQueue.Size = new System.Drawing.Size(230, 138);
            this.instructionQueue.TabIndex = 0;
            this.instructionQueue.UseCompatibleStateImageBehavior = false;
            this.instructionQueue.SelectedIndexChanged += new System.EventHandler(this.instructionQueue_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(236, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "Instruction";
            // 
            // loadBuffers
            // 
            this.loadBuffers.Location = new System.Drawing.Point(798, 16);
            this.loadBuffers.Name = "loadBuffers";
            this.loadBuffers.Size = new System.Drawing.Size(155, 114);
            this.loadBuffers.TabIndex = 0;
            this.loadBuffers.UseCompatibleStateImageBehavior = false;
            this.loadBuffers.SelectedIndexChanged += new System.EventHandler(this.loadBuffers_SelectedIndexChanged);
            // 
            // storeBuffers
            // 
            this.storeBuffers.Location = new System.Drawing.Point(557, 16);
            this.storeBuffers.Name = "storeBuffers";
            this.storeBuffers.Size = new System.Drawing.Size(235, 114);
            this.storeBuffers.TabIndex = 0;
            this.storeBuffers.UseCompatibleStateImageBehavior = false;
            this.storeBuffers.SelectedIndexChanged += new System.EventHandler(this.loadBuffers_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(795, -2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Load";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(554, -2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Store";
            // 
            // fpRegisters
            // 
            this.fpRegisters.Location = new System.Drawing.Point(93, 352);
            this.fpRegisters.Name = "fpRegisters";
            this.fpRegisters.Size = new System.Drawing.Size(860, 59);
            this.fpRegisters.TabIndex = 10;
            this.fpRegisters.UseCompatibleStateImageBehavior = false;
            // 
            // reservationStation1
            // 
            this.reservationStation1.Location = new System.Drawing.Point(557, 241);
            this.reservationStation1.Name = "reservationStation1";
            this.reservationStation1.Size = new System.Drawing.Size(396, 105);
            this.reservationStation1.TabIndex = 11;
            this.reservationStation1.UseCompatibleStateImageBehavior = false;
            // 
            // reservationStation2
            // 
            this.reservationStation2.Location = new System.Drawing.Point(557, 133);
            this.reservationStation2.Name = "reservationStation2";
            this.reservationStation2.Size = new System.Drawing.Size(396, 105);
            this.reservationStation2.TabIndex = 12;
            this.reservationStation2.UseCompatibleStateImageBehavior = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(423, 1);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(127, 25);
            this.label8.TabIndex = 15;
            this.label8.Text = "Reservation";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(-1, 373);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 18);
            this.label9.TabIndex = 16;
            this.label9.Text = "FP Registers";
            // 
            // Clock
            // 
            this.Clock.Location = new System.Drawing.Point(3, 4);
            this.Clock.Name = "Clock";
            this.Clock.Size = new System.Drawing.Size(75, 23);
            this.Clock.TabIndex = 18;
            this.Clock.Text = "Clock";
            this.Clock.UseVisualStyleBackColor = true;
            this.Clock.Click += new System.EventHandler(this.Clock_Click);
            // 
            // ClockCount
            // 
            this.ClockCount.AutoSize = true;
            this.ClockCount.Location = new System.Drawing.Point(79, 9);
            this.ClockCount.Name = "ClockCount";
            this.ClockCount.Size = new System.Drawing.Size(13, 13);
            this.ClockCount.TabIndex = 19;
            this.ClockCount.Text = "0";
            // 
            // intRegisters
            // 
            this.intRegisters.Location = new System.Drawing.Point(93, 413);
            this.intRegisters.Name = "intRegisters";
            this.intRegisters.Size = new System.Drawing.Size(860, 59);
            this.intRegisters.TabIndex = 20;
            this.intRegisters.UseCompatibleStateImageBehavior = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 432);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 18);
            this.label4.TabIndex = 21;
            this.label4.Text = "Int Registers";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(459, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 25);
            this.label5.TabIndex = 22;
            this.label5.Text = "Stations";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(500, 133);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 18);
            this.label6.TabIndex = 23;
            this.label6.Text = "Multiply";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(524, 241);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 18);
            this.label7.TabIndex = 24;
            this.label7.Text = "Add";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(236, 49);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 18);
            this.label10.TabIndex = 25;
            this.label10.Text = "Queue";
            // 
            // editInstructions
            // 
            this.editInstructions.Location = new System.Drawing.Point(239, 70);
            this.editInstructions.Name = "editInstructions";
            this.editInstructions.Size = new System.Drawing.Size(75, 23);
            this.editInstructions.TabIndex = 26;
            this.editInstructions.Text = "Edit";
            this.editInstructions.UseVisualStyleBackColor = true;
            this.editInstructions.Click += new System.EventHandler(this.editInstructions_Click);
            // 
            // issuedInstructionBox
            // 
            this.issuedInstructionBox.Location = new System.Drawing.Point(3, 177);
            this.issuedInstructionBox.Name = "issuedInstructionBox";
            this.issuedInstructionBox.Size = new System.Drawing.Size(364, 138);
            this.issuedInstructionBox.TabIndex = 27;
            this.issuedInstructionBox.UseCompatibleStateImageBehavior = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(367, 177);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(76, 18);
            this.label11.TabIndex = 28;
            this.label11.Text = "Instruction";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(369, 192);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(50, 18);
            this.label12.TabIndex = 29;
            this.label12.Text = "Status";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(125, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 30;
            this.button1.Text = "Reset";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 474);
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
    }
}

