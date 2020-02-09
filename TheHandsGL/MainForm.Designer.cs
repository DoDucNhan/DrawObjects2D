namespace TheHandsGL
{
	partial class mainForm
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
			this.drawBoard = new SharpGL.OpenGLControl();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.btnLine = new System.Windows.Forms.Button();
			this.btnColor = new System.Windows.Forms.Button();
			this.btnWidth = new System.Windows.Forms.Button();
			this.btnCircle = new System.Windows.Forms.Button();
			this.btnRectangle = new System.Windows.Forms.Button();
			this.btnEllipse = new System.Windows.Forms.Button();
			this.btnTriangle = new System.Windows.Forms.Button();
			this.btnPentagon = new System.Windows.Forms.Button();
			this.btnHexagon = new System.Windows.Forms.Button();
			this.btnClear = new System.Windows.Forms.Button();
			this.lbUndo = new System.Windows.Forms.Label();
			this.lbTime = new System.Windows.Forms.Label();
			this.tbTime = new System.Windows.Forms.TextBox();
			this.btnPolygon = new System.Windows.Forms.Button();
			this.lbMode = new System.Windows.Forms.Label();
			this.btnRotateOrScale = new System.Windows.Forms.Button();
			this.btnFlood = new System.Windows.Forms.Button();
			this.btnScanline = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.drawBoard)).BeginInit();
			this.SuspendLayout();
			// 
			// drawBoard
			// 
			this.drawBoard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.drawBoard.Cursor = System.Windows.Forms.Cursors.Cross;
			this.drawBoard.DrawFPS = false;
			this.drawBoard.Location = new System.Drawing.Point(0, 46);
			this.drawBoard.Margin = new System.Windows.Forms.Padding(5);
			this.drawBoard.Name = "drawBoard";
			this.drawBoard.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
			this.drawBoard.RenderContextType = SharpGL.RenderContextType.DIBSection;
			this.drawBoard.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
			this.drawBoard.Size = new System.Drawing.Size(1134, 601);
			this.drawBoard.TabIndex = 0;
			this.drawBoard.OpenGLInitialized += new System.EventHandler(this.drawBoard_OpenGLInitialized);
			this.drawBoard.OpenGLDraw += new SharpGL.RenderEventHandler(this.drawBoard_OpenGLDraw);
			this.drawBoard.Resized += new System.EventHandler(this.drawBoard_Resized);
			this.drawBoard.KeyDown += new System.Windows.Forms.KeyEventHandler(this.drawBoard_KeyDown);
			this.drawBoard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.drawBoard_MouseDown);
			this.drawBoard.MouseMove += new System.Windows.Forms.MouseEventHandler(this.drawBoard_MouseMove);
			this.drawBoard.MouseUp += new System.Windows.Forms.MouseEventHandler(this.drawBoard_MouseUp);
			// 
			// colorDialog
			// 
			this.colorDialog.Color = System.Drawing.Color.White;
			// 
			// btnLine
			// 
			this.btnLine.Location = new System.Drawing.Point(0, 5);
			this.btnLine.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnLine.Name = "btnLine";
			this.btnLine.Size = new System.Drawing.Size(91, 34);
			this.btnLine.TabIndex = 1;
			this.btnLine.Text = "Line";
			this.btnLine.UseVisualStyleBackColor = true;
			this.btnLine.Click += new System.EventHandler(this.btnLine_Click);
			// 
			// btnCircle
			// 
			this.btnCircle.Location = new System.Drawing.Point(91, 5);
			this.btnCircle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnCircle.Name = "btnCircle";
			this.btnCircle.Size = new System.Drawing.Size(91, 34);
			this.btnCircle.TabIndex = 2;
			this.btnCircle.Text = "Circle";
			this.btnCircle.UseVisualStyleBackColor = true;
			this.btnCircle.Click += new System.EventHandler(this.btnCircle_Click);
			// 
			// btnRectangle
			// 
			this.btnRectangle.Location = new System.Drawing.Point(182, 5);
			this.btnRectangle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnRectangle.Name = "btnRectangle";
			this.btnRectangle.Size = new System.Drawing.Size(91, 34);
			this.btnRectangle.TabIndex = 3;
			this.btnRectangle.Text = "Rectangle";
			this.btnRectangle.UseVisualStyleBackColor = true;
			this.btnRectangle.Click += new System.EventHandler(this.btnRectangle_Click);
			// 
			// btnEllipse
			// 
			this.btnEllipse.Location = new System.Drawing.Point(273, 5);
			this.btnEllipse.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnEllipse.Name = "btnEllipse";
			this.btnEllipse.Size = new System.Drawing.Size(91, 34);
			this.btnEllipse.TabIndex = 4;
			this.btnEllipse.Text = "Ellipse";
			this.btnEllipse.UseVisualStyleBackColor = true;
			this.btnEllipse.Click += new System.EventHandler(this.btnEllipse_Click);
			// 
			// btnTriangle
			// 
			this.btnTriangle.Location = new System.Drawing.Point(364, 5);
			this.btnTriangle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnTriangle.Name = "btnTriangle";
			this.btnTriangle.Size = new System.Drawing.Size(91, 34);
			this.btnTriangle.TabIndex = 5;
			this.btnTriangle.Text = "Triangle";
			this.btnTriangle.UseVisualStyleBackColor = true;
			this.btnTriangle.Click += new System.EventHandler(this.btnTriangle_Click);
			// 
			// btnPentagon
			// 
			this.btnPentagon.Location = new System.Drawing.Point(455, 5);
			this.btnPentagon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnPentagon.Name = "btnPentagon";
			this.btnPentagon.Size = new System.Drawing.Size(91, 34);
			this.btnPentagon.TabIndex = 6;
			this.btnPentagon.Text = "Pentagon";
			this.btnPentagon.UseVisualStyleBackColor = true;
			this.btnPentagon.Click += new System.EventHandler(this.btnPentagon_Click);
			// 
			// btnHexagon
			// 
			this.btnHexagon.Location = new System.Drawing.Point(546, 5);
			this.btnHexagon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnHexagon.Name = "btnHexagon";
			this.btnHexagon.Size = new System.Drawing.Size(91, 34);
			this.btnHexagon.TabIndex = 7;
			this.btnHexagon.Text = "Hexagon";
			this.btnHexagon.UseVisualStyleBackColor = true;
			this.btnHexagon.Click += new System.EventHandler(this.btnHexagon_Click);
			// 
			// btnPolygon
			// 
			this.btnPolygon.Location = new System.Drawing.Point(637, 5);
			this.btnPolygon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnPolygon.Name = "btnPolygon";
			this.btnPolygon.Size = new System.Drawing.Size(91, 34);
			this.btnPolygon.TabIndex = 8;
			this.btnPolygon.Text = "Polygon";
			this.btnPolygon.UseVisualStyleBackColor = true;
			this.btnPolygon.Click += new System.EventHandler(this.btnPolygon_Click);
			// 
			// btnClear
			// 
			this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClear.Location = new System.Drawing.Point(1043, 5);
			this.btnClear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(91, 34);
			this.btnClear.TabIndex = 9;
			this.btnClear.Text = "ClearAll";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// btnColor
			// 
			this.btnColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnColor.Location = new System.Drawing.Point(0, 652);
			this.btnColor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnColor.Name = "btnColor";
			this.btnColor.Size = new System.Drawing.Size(91, 34);
			this.btnColor.TabIndex = 10;
			this.btnColor.Text = "Color";
			this.btnColor.UseVisualStyleBackColor = true;
			this.btnColor.Click += new System.EventHandler(this.btnColor_Click);
			// 
			// btnWidth
			// 
			this.btnWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
			this.btnWidth.Location = new System.Drawing.Point(91, 652);
			this.btnWidth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnWidth.Name = "btnWidth";
			this.btnWidth.Size = new System.Drawing.Size(91, 34);
			this.btnWidth.TabIndex = 11;
			this.btnWidth.Text = "Width: 1.0";
			this.btnWidth.UseVisualStyleBackColor = true;
			this.btnWidth.Click += new System.EventHandler(this.btnWidth_Click);
			// 
			// btnRotateOrScale
			// 
			this.btnRotateOrScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnRotateOrScale.Location = new System.Drawing.Point(182, 652);
			this.btnRotateOrScale.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnRotateOrScale.Name = "btnRotateOrScale";
			this.btnRotateOrScale.Size = new System.Drawing.Size(91, 34);
			this.btnRotateOrScale.TabIndex = 12;
			this.btnRotateOrScale.Text = "Rotate";
			this.btnRotateOrScale.UseVisualStyleBackColor = true;
			this.btnRotateOrScale.Click += new System.EventHandler(this.btnRotateOrScale_Click);
			// 
			// lbUndo
			// 
			this.lbUndo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lbUndo.AutoSize = true;
			this.lbUndo.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
			this.lbUndo.Location = new System.Drawing.Point(323, 661);
			this.lbUndo.Name = "lbUndo";
			this.lbUndo.Size = new System.Drawing.Size(96, 17);
			this.lbUndo.TabIndex = 13;
			this.lbUndo.Text = "Undo: Ctrl + Z";
			// 
			// lbMode
			// 
			this.lbMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lbMode.AutoSize = true;
			this.lbMode.Location = new System.Drawing.Point(452, 661);
			this.lbMode.Name = "lbMode";
			this.lbMode.Size = new System.Drawing.Size(96, 17);
			this.lbMode.TabIndex = 14;
			this.lbMode.Text = "Mode: Picking";
			// 
			// lbTime
			// 
			this.lbTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lbTime.AutoSize = true;
			this.lbTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
			this.lbTime.Location = new System.Drawing.Point(973, 661);
			this.lbTime.Name = "lbTime";
			this.lbTime.Size = new System.Drawing.Size(43, 17);
			this.lbTime.TabIndex = 15;
			this.lbTime.Text = "Time:";
			// 
			// tbTime
			// 
			this.tbTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.tbTime.Location = new System.Drawing.Point(1022, 658);
			this.tbTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tbTime.Name = "tbTime";
			this.tbTime.ReadOnly = true;
			this.tbTime.Size = new System.Drawing.Size(100, 22);
			this.tbTime.TabIndex = 16;
			// 
			// btnFlood
			// 
			this.btnFlood.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFlood.Location = new System.Drawing.Point(952, 5);
			this.btnFlood.Margin = new System.Windows.Forms.Padding(4);
			this.btnFlood.Name = "btnFlood";
			this.btnFlood.Size = new System.Drawing.Size(91, 34);
			this.btnFlood.TabIndex = 17;
			this.btnFlood.Text = "Flood";
			this.btnFlood.UseVisualStyleBackColor = true;
			this.btnFlood.Click += new System.EventHandler(this.btnFlood_Click);
			// 
			// btnScanline
			// 
			this.btnScanline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnScanline.Location = new System.Drawing.Point(861, 5);
			this.btnScanline.Name = "btnScanline";
			this.btnScanline.Size = new System.Drawing.Size(91, 34);
			this.btnScanline.TabIndex = 18;
			this.btnScanline.Text = "Scanline";
			this.btnScanline.UseVisualStyleBackColor = true;
			this.btnScanline.Click += new System.EventHandler(this.btnScanline_Click);
			// 
			// mainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1134, 690);
			this.Controls.Add(this.btnScanline);
			this.Controls.Add(this.btnFlood);
			this.Controls.Add(this.btnRotateOrScale);
			this.Controls.Add(this.lbMode);
			this.Controls.Add(this.btnPolygon);
			this.Controls.Add(this.tbTime);
			this.Controls.Add(this.lbTime);
			this.Controls.Add(this.lbUndo);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.btnHexagon);
			this.Controls.Add(this.btnPentagon);
			this.Controls.Add(this.btnTriangle);
			this.Controls.Add(this.btnEllipse);
			this.Controls.Add(this.btnRectangle);
			this.Controls.Add(this.btnCircle);
			this.Controls.Add(this.btnWidth);
			this.Controls.Add(this.btnColor);
			this.Controls.Add(this.btnLine);
			this.Controls.Add(this.drawBoard);
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.MinimumSize = new System.Drawing.Size(890, 686);
			this.Name = "mainForm";
			this.Text = "TheHandsGL";
			((System.ComponentModel.ISupportInitialize)(this.drawBoard)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private SharpGL.OpenGLControl drawBoard;
		private System.Windows.Forms.ColorDialog colorDialog;
		private System.Windows.Forms.Button btnLine;
		private System.Windows.Forms.Button btnColor;
		private System.Windows.Forms.Button btnWidth;
		private System.Windows.Forms.Button btnCircle;
		private System.Windows.Forms.Button btnRectangle;
		private System.Windows.Forms.Button btnEllipse;
		private System.Windows.Forms.Button btnTriangle;
		private System.Windows.Forms.Button btnPentagon;
		private System.Windows.Forms.Button btnHexagon;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Label lbUndo;
		private System.Windows.Forms.Label lbTime;
		private System.Windows.Forms.TextBox tbTime;
		private System.Windows.Forms.Button btnPolygon;
		private System.Windows.Forms.Label lbMode;
		private System.Windows.Forms.Button btnRotateOrScale;
		private System.Windows.Forms.Button btnFlood;
		private System.Windows.Forms.Button btnScanline;
	}
}
