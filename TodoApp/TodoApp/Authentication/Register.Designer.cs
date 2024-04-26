namespace TodoApp.Authentication
{
    partial class Register
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gotoLoginLink = new System.Windows.Forms.LinkLabel();
            this.registerBtn = new System.Windows.Forms.Button();
            this.newPassword = new System.Windows.Forms.TextBox();
            this.newUsername = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.confirmPassword = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.AliceBlue;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(660, 598);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.confirmPassword);
            this.panel2.Controls.Add(this.gotoLoginLink);
            this.panel2.Controls.Add(this.registerBtn);
            this.panel2.Controls.Add(this.newPassword);
            this.panel2.Controls.Add(this.newUsername);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(75, 54);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(505, 489);
            this.panel2.TabIndex = 1;
            // 
            // gotoLoginLink
            // 
            this.gotoLoginLink.AutoSize = true;
            this.gotoLoginLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gotoLoginLink.LinkColor = System.Drawing.Color.MidnightBlue;
            this.gotoLoginLink.LinkVisited = true;
            this.gotoLoginLink.Location = new System.Drawing.Point(118, 397);
            this.gotoLoginLink.Name = "gotoLoginLink";
            this.gotoLoginLink.Padding = new System.Windows.Forms.Padding(10);
            this.gotoLoginLink.Size = new System.Drawing.Size(248, 45);
            this.gotoLoginLink.TabIndex = 4;
            this.gotoLoginLink.TabStop = true;
            this.gotoLoginLink.Text = "Already have an account";
            // 
            // registerBtn
            // 
            this.registerBtn.BackColor = System.Drawing.Color.MidnightBlue;
            this.registerBtn.Font = new System.Drawing.Font("Candara", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.registerBtn.ForeColor = System.Drawing.SystemColors.Control;
            this.registerBtn.Location = new System.Drawing.Point(173, 330);
            this.registerBtn.Name = "registerBtn";
            this.registerBtn.Size = new System.Drawing.Size(139, 49);
            this.registerBtn.TabIndex = 3;
            this.registerBtn.Text = "Register";
            this.registerBtn.UseVisualStyleBackColor = false;
            // 
            // newPassword
            // 
            this.newPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newPassword.Location = new System.Drawing.Point(95, 201);
            this.newPassword.Multiline = true;
            this.newPassword.Name = "newPassword";
            this.newPassword.Size = new System.Drawing.Size(304, 38);
            this.newPassword.TabIndex = 2;
            // 
            // newUsername
            // 
            this.newUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newUsername.Location = new System.Drawing.Point(94, 141);
            this.newUsername.Multiline = true;
            this.newUsername.Name = "newUsername";
            this.newUsername.Size = new System.Drawing.Size(304, 38);
            this.newUsername.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 31.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label1.Location = new System.Drawing.Point(127, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(238, 62);
            this.label1.TabIndex = 0;
            this.label1.Text = "Register";
            // 
            // confirmPassword
            // 
            this.confirmPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.confirmPassword.Location = new System.Drawing.Point(95, 263);
            this.confirmPassword.Multiline = true;
            this.confirmPassword.Name = "confirmPassword";
            this.confirmPassword.Size = new System.Drawing.Size(304, 38);
            this.confirmPassword.TabIndex = 5;
            // 
            // Register
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 598);
            this.Controls.Add(this.panel1);
            this.Name = "Register";
            this.Text = "Register Form";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.LinkLabel gotoLoginLink;
        private System.Windows.Forms.Button registerBtn;
        private System.Windows.Forms.TextBox newPassword;
        private System.Windows.Forms.TextBox newUsername;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox confirmPassword;
    }
}