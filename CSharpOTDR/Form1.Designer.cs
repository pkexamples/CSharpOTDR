namespace CSharpOTDR
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.axGNGraph1 = new AxGNGRAPHLib.AxGNGraph();
			this.cmdOpenFile = new System.Windows.Forms.Button();
			this.pnlControls = new System.Windows.Forms.Panel();
			this.chkAverage = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.axGNGraph1)).BeginInit();
			this.pnlControls.SuspendLayout();
			this.SuspendLayout();
			// 
			// axGNGraph1
			// 
			this.axGNGraph1.Location = new System.Drawing.Point(25, 39);
			this.axGNGraph1.Name = "axGNGraph1";
			this.axGNGraph1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axGNGraph1.OcxState")));
			this.axGNGraph1.Size = new System.Drawing.Size(441, 250);
			this.axGNGraph1.TabIndex = 0;
			// 
			// cmdOpenFile
			// 
			this.cmdOpenFile.Location = new System.Drawing.Point(3, 8);
			this.cmdOpenFile.Name = "cmdOpenFile";
			this.cmdOpenFile.Size = new System.Drawing.Size(78, 42);
			this.cmdOpenFile.TabIndex = 1;
			this.cmdOpenFile.Text = "Open file";
			this.cmdOpenFile.UseVisualStyleBackColor = true;
			this.cmdOpenFile.Click += new System.EventHandler(this.cmdOpenFile_Click);
			// 
			// pnlControls
			// 
			this.pnlControls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlControls.Controls.Add(this.cmdOpenFile);
			this.pnlControls.Location = new System.Drawing.Point(387, 304);
			this.pnlControls.Name = "pnlControls";
			this.pnlControls.Size = new System.Drawing.Size(88, 59);
			this.pnlControls.TabIndex = 3;
			// 
			// chkAverage
			// 
			this.chkAverage.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkAverage.Location = new System.Drawing.Point(303, 312);
			this.chkAverage.Name = "chkAverage";
			this.chkAverage.Size = new System.Drawing.Size(78, 42);
			this.chkAverage.TabIndex = 4;
			this.chkAverage.Text = "Average";
			this.chkAverage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkAverage.UseVisualStyleBackColor = true;
			this.chkAverage.CheckedChanged += new System.EventHandler(this.chkAverage_CheckedChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(497, 375);
			this.Controls.Add(this.chkAverage);
			this.Controls.Add(this.pnlControls);
			this.Controls.Add(this.axGNGraph1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.axGNGraph1)).EndInit();
			this.pnlControls.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private AxGNGRAPHLib.AxGNGraph axGNGraph1;
		private System.Windows.Forms.Button cmdOpenFile;
		private System.Windows.Forms.Panel pnlControls;
		private System.Windows.Forms.CheckBox chkAverage;
	}
}

