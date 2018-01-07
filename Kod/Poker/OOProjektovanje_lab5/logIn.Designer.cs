namespace OOProjektovanje_lab5
{
    partial class logIn
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
            this.tbusername = new System.Windows.Forms.TextBox();
            this.tbpass = new System.Windows.Forms.TextBox();
            this.lbusername = new System.Windows.Forms.Label();
            this.lbpass = new System.Windows.Forms.Label();
            this.btnlogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbusername
            // 
            this.tbusername.Location = new System.Drawing.Point(74, 40);
            this.tbusername.Name = "tbusername";
            this.tbusername.Size = new System.Drawing.Size(135, 20);
            this.tbusername.TabIndex = 0;
            // 
            // tbpass
            // 
            this.tbpass.Location = new System.Drawing.Point(74, 101);
            this.tbpass.Name = "tbpass";
            this.tbpass.Size = new System.Drawing.Size(135, 20);
            this.tbpass.TabIndex = 1;
            // 
            // lbusername
            // 
            this.lbusername.AutoSize = true;
            this.lbusername.Location = new System.Drawing.Point(74, 13);
            this.lbusername.Name = "lbusername";
            this.lbusername.Size = new System.Drawing.Size(55, 13);
            this.lbusername.TabIndex = 2;
            this.lbusername.Text = "Username";
            // 
            // lbpass
            // 
            this.lbpass.AutoSize = true;
            this.lbpass.Location = new System.Drawing.Point(74, 85);
            this.lbpass.Name = "lbpass";
            this.lbpass.Size = new System.Drawing.Size(45, 13);
            this.lbpass.TabIndex = 3;
            this.lbpass.Text = "Passord";
            // 
            // btnlogin
            // 
            this.btnlogin.Location = new System.Drawing.Point(170, 165);
            this.btnlogin.Name = "btnlogin";
            this.btnlogin.Size = new System.Drawing.Size(75, 23);
            this.btnlogin.TabIndex = 4;
            this.btnlogin.Text = "Prijavi se";
            this.btnlogin.UseVisualStyleBackColor = true;
            this.btnlogin.Click += new System.EventHandler(this.btnlogin_Click);
            // 
            // logIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnlogin);
            this.Controls.Add(this.lbpass);
            this.Controls.Add(this.lbusername);
            this.Controls.Add(this.tbpass);
            this.Controls.Add(this.tbusername);
            this.Name = "logIn";
            this.Text = "logIn";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbusername;
        private System.Windows.Forms.TextBox tbpass;
        private System.Windows.Forms.Label lbusername;
        private System.Windows.Forms.Label lbpass;
        private System.Windows.Forms.Button btnlogin;
    }
}