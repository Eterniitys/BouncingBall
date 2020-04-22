namespace BouncingBall
{
	partial class TabletteView
	{
		/// <summary>
		/// Variables nécessaire au développer
		/// </summary>
		private Tablet tab;

		/// <summary>
		/// Variable nécessaire au concepteur.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Nettoyage des ressources utilisées.
		/// </summary>
		/// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Code généré par le Concepteur Windows Form

		/// <summary>
		/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
		/// le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		private void InitializeComponent()
		{
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lbl_1 = new System.Windows.Forms.Label();
			this.lbl_format = new System.Windows.Forms.Label();
			this.lbl_angle = new System.Windows.Forms.Label();
			this.lbl_y = new System.Windows.Forms.Label();
			this.spin_btn_x = new System.Windows.Forms.NumericUpDown();
			this.lbl_x = new System.Windows.Forms.Label();
			this.spin_btn_y = new System.Windows.Forms.NumericUpDown();
			this.spin_btn_angle = new System.Windows.Forms.NumericUpDown();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.spin_btn_x)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spin_btn_y)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spin_btn_angle)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.45038F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 88.54962F));
			this.tableLayoutPanel1.Controls.Add(this.lbl_1, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.lbl_format, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.lbl_angle, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lbl_y, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.spin_btn_x, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lbl_x, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.spin_btn_y, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.spin_btn_angle, 1, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 321);
			this.tableLayoutPanel1.MaximumSize = new System.Drawing.Size(0, 100);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(524, 100);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// lbl_1
			// 
			this.lbl_1.AutoSize = true;
			this.lbl_1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbl_1.Location = new System.Drawing.Point(3, 78);
			this.lbl_1.Name = "lbl_1";
			this.lbl_1.Size = new System.Drawing.Size(53, 22);
			this.lbl_1.TabIndex = 13;
			this.lbl_1.Text = "Format";
			this.lbl_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbl_format
			// 
			this.lbl_format.AutoSize = true;
			this.lbl_format.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbl_format.Location = new System.Drawing.Point(62, 78);
			this.lbl_format.Name = "lbl_format";
			this.lbl_format.Size = new System.Drawing.Size(459, 22);
			this.lbl_format.TabIndex = 12;
			this.lbl_format.Text = "largeur : {}, hauteur {}";
			this.lbl_format.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbl_angle
			// 
			this.lbl_angle.AutoSize = true;
			this.lbl_angle.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbl_angle.Location = new System.Drawing.Point(3, 52);
			this.lbl_angle.Name = "lbl_angle";
			this.lbl_angle.Size = new System.Drawing.Size(53, 26);
			this.lbl_angle.TabIndex = 9;
			this.lbl_angle.Text = "angle";
			this.lbl_angle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbl_y
			// 
			this.lbl_y.AutoSize = true;
			this.lbl_y.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbl_y.Location = new System.Drawing.Point(3, 26);
			this.lbl_y.Name = "lbl_y";
			this.lbl_y.Size = new System.Drawing.Size(53, 26);
			this.lbl_y.TabIndex = 8;
			this.lbl_y.Text = "posY";
			this.lbl_y.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// spin_btn_x
			// 
			this.spin_btn_x.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.spin_btn_x.Cursor = System.Windows.Forms.Cursors.Default;
			this.spin_btn_x.InterceptArrowKeys = false;
			this.spin_btn_x.Location = new System.Drawing.Point(62, 3);
			this.spin_btn_x.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.spin_btn_x.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
			this.spin_btn_x.Name = "spin_btn_x";
			this.spin_btn_x.Size = new System.Drawing.Size(94, 20);
			this.spin_btn_x.TabIndex = 4;
			this.spin_btn_x.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.spin_btn_x.ValueChanged += new System.EventHandler(this.spin_btn_x_ValueChanged);
			// 
			// lbl_x
			// 
			this.lbl_x.AutoSize = true;
			this.lbl_x.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbl_x.Location = new System.Drawing.Point(3, 0);
			this.lbl_x.Name = "lbl_x";
			this.lbl_x.Size = new System.Drawing.Size(53, 26);
			this.lbl_x.TabIndex = 7;
			this.lbl_x.Text = "posX";
			this.lbl_x.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// spin_btn_y
			// 
			this.spin_btn_y.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.spin_btn_y.Cursor = System.Windows.Forms.Cursors.Default;
			this.spin_btn_y.InterceptArrowKeys = false;
			this.spin_btn_y.Location = new System.Drawing.Point(62, 29);
			this.spin_btn_y.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.spin_btn_y.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
			this.spin_btn_y.Name = "spin_btn_y";
			this.spin_btn_y.Size = new System.Drawing.Size(94, 20);
			this.spin_btn_y.TabIndex = 11;
			this.spin_btn_y.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.spin_btn_y.ValueChanged += new System.EventHandler(this.spin_btn_y_ValueChanged);
			// 
			// spin_btn_angle
			// 
			this.spin_btn_angle.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.spin_btn_angle.Cursor = System.Windows.Forms.Cursors.Default;
			this.spin_btn_angle.InterceptArrowKeys = false;
			this.spin_btn_angle.Location = new System.Drawing.Point(62, 55);
			this.spin_btn_angle.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.spin_btn_angle.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
			this.spin_btn_angle.Name = "spin_btn_angle";
			this.spin_btn_angle.Size = new System.Drawing.Size(94, 20);
			this.spin_btn_angle.TabIndex = 10;
			this.spin_btn_angle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.spin_btn_angle.ValueChanged += new System.EventHandler(this.spin_btn_angle_ValueChanged);
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(524, 321);
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			// 
			// TabletteView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(524, 421);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "TabletteView";
			this.Text = "TabView";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.spin_btn_x)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spin_btn_y)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spin_btn_angle)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lbl_x;
		private System.Windows.Forms.NumericUpDown spin_btn_x;
		private System.Windows.Forms.Label lbl_y;
		private System.Windows.Forms.NumericUpDown spin_btn_y;
		private System.Windows.Forms.Label lbl_angle;
		private System.Windows.Forms.NumericUpDown spin_btn_angle;
		private System.Windows.Forms.Label lbl_1;
		private System.Windows.Forms.Label lbl_format;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}

