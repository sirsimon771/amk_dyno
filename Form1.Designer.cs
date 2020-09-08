namespace amk_dyno
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
            this.startstop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // startstop
            // 
            this.startstop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.startstop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startstop.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startstop.ForeColor = System.Drawing.SystemColors.ControlText;
            this.startstop.Location = new System.Drawing.Point(293, 248);
            this.startstop.Name = "startstop";
            this.startstop.Size = new System.Drawing.Size(208, 61);
            this.startstop.TabIndex = 0;
            this.startstop.Text = "start/stop";
            this.startstop.UseVisualStyleBackColor = false;
            this.startstop.Click += new System.EventHandler(this.startstop_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.startstop);
            this.Name = "Form1";
            this.Text = "amk dyno";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button startstop;
    }
}

