namespace BouncingBall
{
	partial class TabletteView
	{
		/// <summary>
		/// Variables nécessaire au développer
		/// </summary>
		private Tablette tab;

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
			this.lbl_angle = new System.Windows.Forms.Label();
			this.lbl_y = new System.Windows.Forms.Label();
			this.spin_btn_x = new System.Windows.Forms.NumericUpDown();
			this.lbl_x = new System.Windows.Forms.Label();
			this.spin_btn_y = new System.Windows.Forms.NumericUpDown();
			this.spin_btn_angle = new System.Windows.Forms.NumericUpDown();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.spin_btn_x)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spin_btn_y)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spin_btn_angle)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.45038F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 88.54962F));
			this.tableLayoutPanel1.Controls.Add(this.lbl_angle, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lbl_y, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.spin_btn_x, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lbl_x, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.spin_btn_y, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.spin_btn_angle, 1, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(524, 100);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// lbl_angle
			// 
			this.lbl_angle.AutoSize = true;
			this.lbl_angle.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbl_angle.Location = new System.Drawing.Point(3, 66);
			this.lbl_angle.Name = "lbl_angle";
			this.lbl_angle.Size = new System.Drawing.Size(53, 34);
			this.lbl_angle.TabIndex = 9;
			this.lbl_angle.Text = "angle";
			this.lbl_angle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbl_y
			// 
			this.lbl_y.AutoSize = true;
			this.lbl_y.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbl_y.Location = new System.Drawing.Point(3, 33);
			this.lbl_y.Name = "lbl_y";
			this.lbl_y.Size = new System.Drawing.Size(53, 33);
			this.lbl_y.TabIndex = 8;
			this.lbl_y.Text = "posY";
			this.lbl_y.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// spin_btn_x
			// 
			this.spin_btn_x.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.spin_btn_x.Cursor = System.Windows.Forms.Cursors.Default;
			this.spin_btn_x.InterceptArrowKeys = false;
			this.spin_btn_x.Location = new System.Drawing.Point(62, 6);
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
			this.lbl_x.Size = new System.Drawing.Size(53, 33);
			this.lbl_x.TabIndex = 7;
			this.lbl_x.Text = "posX";
			this.lbl_x.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// spin_btn_y
			// 
			this.spin_btn_y.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.spin_btn_y.Cursor = System.Windows.Forms.Cursors.Default;
			this.spin_btn_y.InterceptArrowKeys = false;
			this.spin_btn_y.Location = new System.Drawing.Point(62, 39);
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
			this.spin_btn_angle.Location = new System.Drawing.Point(62, 73);
			this.spin_btn_angle.Name = "spin_btn_angle";
			this.spin_btn_angle.Size = new System.Drawing.Size(94, 20);
			this.spin_btn_angle.TabIndex = 10;
			this.spin_btn_angle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.spin_btn_angle.ValueChanged += new System.EventHandler(this.spin_btn_angle_ValueChanged);
			// 
			// TabletteView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(524, 321);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "TabletteView";
			this.Text = "TabView";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.spin_btn_x)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spin_btn_y)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spin_btn_angle)).EndInit();
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
	}
}

