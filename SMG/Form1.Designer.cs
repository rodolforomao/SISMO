namespace SMG
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
            this.btnMonitoramento = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnMonitoramento
            // 
            this.btnMonitoramento.Location = new System.Drawing.Point(225, 210);
            this.btnMonitoramento.Name = "btnMonitoramento";
            this.btnMonitoramento.Size = new System.Drawing.Size(146, 23);
            this.btnMonitoramento.TabIndex = 0;
            this.btnMonitoramento.Text = "Realizar Monitoramento";
            this.btnMonitoramento.UseVisualStyleBackColor = true;
            this.btnMonitoramento.Click += new System.EventHandler(this.btnMonitoramento_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 261);
            this.Controls.Add(this.btnMonitoramento);
            this.Name = "Form1";
            this.Text = "SMG - Sistema de Monitoramento Gerencial";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnMonitoramento;
    }
}

