﻿using BouncingBall.Properties;

namespace BrokerApplication {
	partial class MapView {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lbl_angle = new System.Windows.Forms.Label();
			this.lbl_goal = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// timer
			// 
			this.timer.Enabled = true;
			this.timer.Interval = 20;
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(524, 321);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
			// 
			// lbl_angle
			// 
			this.lbl_angle.AutoSize = true;
			this.lbl_angle.BackColor = System.Drawing.Color.Transparent;
			this.lbl_angle.ForeColor = System.Drawing.Color.White;
			this.lbl_angle.Location = new System.Drawing.Point(12, 9);
			this.lbl_angle.Name = "lbl_angle";
			this.lbl_angle.Size = new System.Drawing.Size(111, 13);
			this.lbl_angle.TabIndex = 1;
			this.lbl_angle.Text = "There is no player yet.";
			// 
			// lbl_goal
			// 
			this.lbl_goal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lbl_goal.AutoSize = true;
			this.lbl_goal.BackColor = System.Drawing.Color.Transparent;
			this.lbl_goal.ForeColor = System.Drawing.Color.White;
			this.lbl_goal.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.lbl_goal.Location = new System.Drawing.Point(12, 282);
			this.lbl_goal.Name = "lbl_goal";
			this.lbl_goal.Size = new System.Drawing.Size(49, 30);
			this.lbl_goal.TabIndex = 2;
			this.lbl_goal.Text = "lbl_goal\r\n2";
			this.lbl_goal.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.lbl_goal.UseCompatibleTextRendering = true;
			// 
			// MapView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.ClientSize = new System.Drawing.Size(524, 321);
			this.Controls.Add(this.lbl_goal);
			this.Controls.Add(this.lbl_angle);
			this.Controls.Add(this.pictureBox1);
			this.Name = "MapView";
			this.Text = "Map";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label lbl_angle;
		private System.Windows.Forms.Label lbl_goal;
	}
}