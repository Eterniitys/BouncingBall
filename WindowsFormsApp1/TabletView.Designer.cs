using Emgu.CV.UI;

namespace BouncingBall {
	partial class TabletView {

		/// <summary>
		/// Variable nécessaire au concepteur.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Nettoyage des ressources utilisées.
		/// </summary>
		/// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Code généré par le Concepteur Windows Form

		/// <summary>
		/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
		/// le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.lbl_format = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.lbl = new System.Windows.Forms.Label();
			this.pictureBox2 = new Emgu.CV.UI.ImageBox();
			this.useHough = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			this.SuspendLayout();
			// 
			// lbl_format
			// 
			this.lbl_format.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lbl_format.AutoSize = true;
			this.lbl_format.Location = new System.Drawing.Point(3, 306);
			this.lbl_format.Name = "lbl_format";
			this.lbl_format.Size = new System.Drawing.Size(109, 13);
			this.lbl_format.TabIndex = 12;
			this.lbl_format.Text = "largeur : {}, hauteur {}";
			this.lbl_format.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(521, 328);
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
			this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
			this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
			this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
			// 
			// timer
			// 
			this.timer.Enabled = true;
			this.timer.Interval = Properties.Settings.Default.iGameTick;
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// lbl
			// 
			this.lbl.AutoSize = true;
			this.lbl.Location = new System.Drawing.Point(3, 9);
			this.lbl.Name = "lbl";
			this.lbl.Size = new System.Drawing.Size(205, 13);
			this.lbl.TabIndex = 2;
			this.lbl.Text = "The application try to connect to a broker.";
			// 
			// pictureBox2
			// 
			this.pictureBox2.Enabled = false;
			this.pictureBox2.Location = new System.Drawing.Point(6, 25);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(288, 225);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox2.TabIndex = 13;
			this.pictureBox2.TabStop = false;
			// 
			// useHough
			// 
			this.useHough.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.useHough.AutoSize = true;
			this.useHough.Location = new System.Drawing.Point(431, 12);
			this.useHough.Name = "useHough";
			this.useHough.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.useHough.Size = new System.Drawing.Size(78, 17);
			this.useHough.TabIndex = 14;
			this.useHough.Text = "Use hough";
			this.useHough.UseVisualStyleBackColor = true;
			this.useHough.CheckedChanged += new System.EventHandler(this.useHough_CheckedChanged);
			// 
			// TabletView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(521, 328);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.lbl);
			this.Controls.Add(this.lbl_format);
			this.Controls.Add(this.useHough);
			this.Controls.Add(this.pictureBox1);
			this.Name = "TabletView";
			this.Text = "TabView";
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TabletteView_KeyUp);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label lbl_format;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.Label lbl;
		private Emgu.CV.UI.ImageBox pictureBox2;
		private System.Windows.Forms.CheckBox useHough;
	}
}

