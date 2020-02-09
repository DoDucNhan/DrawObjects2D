using SharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace TheHandsGL
{
	public partial class mainForm : Form
	{
		//Biến đánh dấu đang vẽ hình
		bool isDrawing = false;
		//Biến đanh dấu đang vẽ đa giác
		bool isPolygonDrawing = false;
		//Biến đánh dấu đang biến đổi Affine
		bool isTransforming = false;
		//Biến đánh dấu đang xoay hình hay co giãn hình, 0 là xoay, 1 là co giãn
		int rotateOrScale = 0;

		//Màu đang chọn
		Color userColor = Color.White;
		//Độ dày nét vẽ đang chọn
		float userWidth = 1.0f;
		//Loại hình vẽ đang chọn để vẽ
		Shape.shapeType userType = Shape.shapeType.NONE;

		//Tọa độ chuột
		Point pStart = new Point(0, 0), pEnd = new Point(0, 0);
		//Tập các hình đã vẽ
		List<Shape> shapes = new List<Shape>();
		//Biến đánh dấu tập hình vẽ bị thay đổi => phải vẽ lại
		bool isShapesChanged = true;

		//Sai số pixel bình phương, dùng trong chức năng chọn lại hình
		const double epsilon = 50.0;
		//Số thứ tự của hình đang được chọn lại, -1 là không chọn gì
		int choosingShape = -1;
		//Backup cho hình đang được chọn
		Shape backupShape;
		//Số thứ tự của điểm vẽ đang được chọn lại, -1 là không chọn gì
		int choosingRaster = -1;
		//Số thứ tự của điểm điều khiển đang được chọn lại, -2 là không chọn gì, -1 là chọn điểm xoay và co giãn
		int choosingControl = -2;

		public mainForm()
		{
			InitializeComponent();
		}

		private void drawBoard_OpenGLInitialized(object sender, EventArgs e)
		{
			//Sự kiện "khởi tạo", xảy ra khi chương trình vừa được khởi chạy

			//Lấy đối tượng OpenGL
			OpenGL gl = drawBoard.OpenGL;

			//Set màu nền (đen)
			gl.ClearColor(0, 0, 0, 1);

			//Xóa toàn bộ drawBoard
			gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
		}

		private void drawBoard_Resized(object sender, EventArgs e)
		{
			//Sự kiện "thay đổi kích thước cửa sổ"

			//Lấy đối tượng OpenGL
			OpenGL gl = drawBoard.OpenGL;

			//Set viewport theo kích thước mới
			gl.Viewport(0, 0, drawBoard.Width, drawBoard.Height);

			//Chọn chế độ chiếu
			gl.MatrixMode(OpenGL.GL_PROJECTION);
			//Chiếu theo kích thước mới
			gl.LoadIdentity();
			gl.Ortho2D(0, drawBoard.Width, 0, drawBoard.Height);

			//Vẽ lại hình
			isShapesChanged = true;
		}

		private void drawBoard_OpenGLDraw(object sender, RenderEventArgs args)
		{
			//Sự kiện "vẽ", xảy ra liên tục và lặp vô hạn lần

			//Lấy đối tượng OpenGL
			OpenGL gl = drawBoard.OpenGL;

			//Chỉ vẽ khi tập hình vẽ bị thay đổi
			if (isShapesChanged)
			{
				//Xóa toàn bộ drawBoard
				gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

				//Vẽ lại tất cả hình
				if (shapes.Count > 0)
				{
					for (int i = 0; i < shapes.Count - 1; i++)
					{
						shapes[i].Draw(gl);

						if (shapes[i].fillColor != Color.Black)
							shapes[i].Fill(gl);
					}

					//Đo thời gian vẽ hình cuối cùng
					Stopwatch watch = Stopwatch.StartNew();
					shapes.Last().Draw(gl);
					watch.Stop();
					tbTime.Text = watch.ElapsedTicks.ToString() + " ticks";

					if (shapes.Last().fillColor != Color.Black)
						shapes.Last().Fill(gl);
				}

				gl.Flush();
				isShapesChanged = false;
			}

			//Vẽ điểm điều khiển của hình đang được chọn lại
			if (choosingShape >= 0)
			{
				gl.PointSize(5.0f);
				gl.Begin(OpenGL.GL_POINTS);

					//Điểm điều khiến
					gl.Color(230.0, 230.0, 0);
					for (int i = 0; i < shapes[choosingShape].controlPoints.Count; i++)
						gl.Vertex(shapes[choosingShape].controlPoints[i].X, gl.RenderContextProvider.Height - shapes[choosingShape].controlPoints[i].Y);

					//Điểm xoay và co giãn
					gl.Color(0, 100.0, 100.0);
					gl.Vertex(shapes[choosingShape].extraPoint.X, gl.RenderContextProvider.Height - shapes[choosingShape].extraPoint.Y);
				
				gl.End();
				gl.Flush();
			}
		}

		private void drawBoard_MouseDown(object sender, MouseEventArgs e)
		{
			//Sự kiện "nhấn chuột"
			pStart = pEnd = e.Location;

			//Nhấn chuột trái => vẽ hình mới, hoặc chọn lại hình đã vẽ, hoặc biến đổi Affine
			if (e.Button == MouseButtons.Left)
			{
				//Nếu userType là NONE => chọn lại hình đã vẽ, hoặc biến đổi Affine
				if (userType == Shape.shapeType.NONE)
				{
					Thread thread = new Thread
					(
						delegate()
						{
							//Nếu một hình nào đó đang được chọn => biến đổi Affine
							if (choosingShape >= 0)
							{
								//Tìm điểm điều khiển hoặc điểm xoay và co giãn gần với tọa độ chuột nhất
								double minDistance = 999999999999999999999999.0;
								int closestControl = -2;

								int dx, dy;
								double distance;

								for (int j = 0; j < shapes[choosingShape].controlPoints.Count; j++)
								{
									dx = shapes[choosingShape].controlPoints[j].X - e.Location.X;
									dy = shapes[choosingShape].controlPoints[j].Y - e.Location.Y;
									distance = dx * dx + dy * dy;

									if (distance < minDistance)
									{
										minDistance = distance;
										closestControl = j;
									}
								}

								dx = shapes[choosingShape].extraPoint.X - e.Location.X;
								dy = shapes[choosingShape].extraPoint.Y - e.Location.Y;
								distance = dx * dx + dy * dy;

								if (distance < minDistance)
								{
									minDistance = distance;
									closestControl = -1;
								}

								//Nếu khoảng cách nhỏ nhất nhỏ hơn epsilon => chọn điểm này
								if (minDistance <= epsilon)
								{
									choosingControl = closestControl;
									isTransforming = true;
									return;
								}

								//Ngược lại => hủy chọn hình và chọn lại hình khác
								choosingShape = choosingRaster = -1;
								choosingControl = -2;
							}

							//Nếu không hình nào đang được chọn => chọn lại hình
							if (choosingShape == -1)
							{
								//Tìm điểm vẽ gần với tọa độ chuột nhất
								double minDistance = 999999999999999999999999.0;
								int closestRaster = -1;

								int dx, dy;
								double distance;

								for (int i = 0; i < shapes.Count; i++)
									for (int j = 0; j < shapes[i].rasterPoints.Count; j++)
									{
										dx = shapes[i].rasterPoints[j].X - e.Location.X;
										dy = shapes[i].rasterPoints[j].Y - e.Location.Y;
										distance = dx * dx + dy * dy;

										if (distance < minDistance)
										{
											choosingShape = i;
											minDistance = distance;
											closestRaster = j;
										}
									}

								//Nếu khoảng cách nhỏ nhất nhỏ hơn epsilon => chọn hình này
								if (minDistance <= epsilon)
								{
									choosingRaster = closestRaster;
									backupShape = shapes[choosingShape].Clone();
									isTransforming = true;
									isShapesChanged = true;
									return;
								}

								//Ngược lại => không chọn hình nào hết
								choosingShape = choosingRaster = -1;
								choosingControl = -2;
								isShapesChanged = true;
								return;
							}
						}
					);
					thread.IsBackground = true;
					thread.Start();
					return;
				}

				//Nếu userType khác NONE => vẽ hình mới
				choosingShape = -1;
				isDrawing = true;
				tbTime.Text = "";

				//Nếu userType khác POLYGON => tạo hình vẽ mới, thêm vào danh sách
				if (userType != Shape.shapeType.POLYGON)
				{
					shapes.Add(new Shape(userColor, userWidth, userType));
					shapes.Last().controlPoints.Add(pStart);
					shapes.Last().controlPoints.Add(pEnd);
				}
				//Nếu userType là POLYGON => xét 2 trường hợp bên dưới
				else if (userType == Shape.shapeType.POLYGON)
				{
					//Nếu bắt đầu vẽ => tạo hình vẽ mới
					if (isPolygonDrawing == false)
					{
						isPolygonDrawing = true;
						shapes.Add(new Shape(userColor, userWidth, userType));
						shapes.Last().controlPoints.Add(pStart);
						shapes.Last().controlPoints.Add(pEnd);
					}
					//Nếu đang vẽ => không tạo ra hình mới, mà thêm tọa độ chuột vào tập điểm điều khiển
					else
					{
						shapes.Last().controlPoints.Add(pEnd);
					}
				}
			}

			//Nhấn chuột phải => nếu đang vẽ POLYGON => kết thúc quá trình vẽ
			else if (isPolygonDrawing)
			{
				isDrawing = isPolygonDrawing = false;
			}
		}

		private void drawBoard_MouseUp(object sender, MouseEventArgs e)
		{
			//Sự kiện "nhả chuột", kết thúc quá trình vẽ
			if (userType != Shape.shapeType.POLYGON)
				isDrawing = false;
			if (isTransforming == true)
			{
				//Nếu hình có tô màu => tô màu lại
				if (shapes[choosingShape].fillColor != Color.Black && shapes[choosingShape].fillPoints.Count == 0)
				{
					shapes[choosingShape].fillPoints.Clear();
					if (shapes[choosingShape].controlPoints.Count < 3)
					{
						Thread thread = new Thread
						(
							() => FloodFiller.Fill(shapes[choosingShape], shapes[choosingShape].fillColor, ref isShapesChanged)
						);
						thread.IsBackground = true;
						thread.Start();
					}
					else
					{
						Thread thread = new Thread
						(
							() => ScanlineFiller.Fill(shapes[choosingShape], shapes[choosingShape].fillColor, ref isShapesChanged)
						);
						thread.IsBackground = true;
						thread.Start();
					}
				}

				backupShape = shapes[choosingShape].Clone();
				isTransforming = false;
			}
		}

		private void drawBoard_MouseMove(object sender, MouseEventArgs e)
		{
			//Sự kiện "kéo chuột", xảy ra liên tục khi người dùng nhấn giữ chuột và kéo đi
			pEnd = e.Location;

			//Nếu đang vẽ => liên tục vẽ lại theo tọa độ pEnd mới
			if (isDrawing)
			{
				Thread thread = new Thread
				(
					delegate()
					{
						//Cập nhật điểm điều kiển cuối cùng ứng với pEnd
						shapes.Last().controlPoints[shapes.Last().controlPoints.Count - 1] = pEnd;

						//Xóa tập điểm vẽ, vẽ lại tập điểm mới
						shapes.Last().rasterPoints.Clear();

						switch (userType)
						{
							case Shape.shapeType.LINE:
								DrawingAlgorithms.Line(shapes.Last(), pStart, pEnd);
								break;
							case Shape.shapeType.CIRCLE:
								DrawingAlgorithms.Circle(shapes.Last(), pStart, pEnd);
								break;
							case Shape.shapeType.RECTANGLE:
								DrawingAlgorithms.Rectangle(shapes.Last(), pStart, pEnd);
								break;
							case Shape.shapeType.ELLIPSE:
								DrawingAlgorithms.Ellipse(shapes.Last(), pStart, pEnd);
								break;
							case Shape.shapeType.TRIANGLE:
								DrawingAlgorithms.Triangle(shapes.Last(), pStart, pEnd);
								break;
							case Shape.shapeType.PENTAGON:
								DrawingAlgorithms.Pengtagon(shapes.Last(), pStart, pEnd);
								break;
							case Shape.shapeType.HEXAGON:
								DrawingAlgorithms.Hexagon(shapes.Last(), pStart, pEnd);
								break;
							case Shape.shapeType.POLYGON:
								DrawingAlgorithms.Polygon(shapes.Last());
								break;
						}
						//Vẽ lại hình
						isShapesChanged = true;
					}
				);
				thread.IsBackground = true;
				thread.Start();
			}

			//Nếu đang biến đổi Affine => thực hiện các phép biến đổi lên tập điểm điều khiển
			else if (isTransforming)
			{
				Thread thread = new Thread
				(
					delegate()
					{
						//Tắt tô màu để biến đổi Affine
						if (shapes[choosingShape].isColored == true)
						{
							shapes[choosingShape].isColored = false;
							shapes[choosingShape].fillPoints.Clear();
						}

						AffineTransform transformer = new AffineTransform();

						//Nếu đang chọn điểm xoay và co giãn => phép xoay hình hoặc co giãn hình
						if (choosingControl == -1)
						{
							//Phép xoay hình
							if (rotateOrScale == 0)
							{
								//Tính góc xoay
								Tuple<double, double> vecA = new Tuple<double, double>(backupShape.extraPoint.X - backupShape.center.Item1, backupShape.extraPoint.Y - backupShape.center.Item2);
								Tuple<double, double> vecB = new Tuple<double, double>(pEnd.X - backupShape.center.Item1, pEnd.Y - backupShape.center.Item2);
								double lenA = Math.Sqrt(vecA.Item1 * vecA.Item1 + vecA.Item2 * vecA.Item2);
								double lenB = Math.Sqrt(vecB.Item1 * vecB.Item1 + vecB.Item2 * vecB.Item2);
								double phi = Math.Acos((vecA.Item1 * vecB.Item1 + vecA.Item2 * vecB.Item2) / (lenA * lenB));
								if (vecA.Item1 * vecB.Item2 - vecA.Item2 * vecB.Item1 < 0)
									phi = -phi;

								//Thiết lập phép xoay
								transformer.LoadIdentity();
								transformer.Translate(-backupShape.center.Item1, -backupShape.center.Item2);
								transformer.Rotate(phi);
								transformer.Translate(backupShape.center.Item1, backupShape.center.Item2);

								//Biến đổi tập điểm điều khiển
								for (int i = 0; i < shapes[choosingShape].controlPoints.Count; i++)
									shapes[choosingShape].controlPoints[i] = transformer.Transform(backupShape.controlPoints[i]);
							}
							//Phép co giãn hình
							else if (rotateOrScale == 1)
							{
								//Tính hệ số co giãn
								Tuple<double, double> vecA = new Tuple<double, double>(backupShape.extraPoint.X - backupShape.center.Item1, backupShape.extraPoint.Y - backupShape.center.Item2);
								Tuple<double, double> vecB = new Tuple<double, double>(pEnd.X - backupShape.center.Item1, pEnd.Y - backupShape.center.Item2);
								double sx = vecB.Item1 / vecA.Item1;
								double sy = vecB.Item2 / vecA.Item2;
								double s = Math.Max(sx, sy);

								//Thiết lập phép co giãn
								transformer.LoadIdentity();
								transformer.Translate(-backupShape.center.Item1, -backupShape.center.Item2);
								transformer.Scale(s, s);
								transformer.Translate(backupShape.center.Item1, backupShape.center.Item2);

								//Biến đổi tập điểm điều khiển
								for (int i = 0; i < shapes[choosingShape].controlPoints.Count; i++)
									shapes[choosingShape].controlPoints[i] = transformer.Transform(backupShape.controlPoints[i]);
							}
						}
						//Nếu đang chọn điểm điều khiển => thay đổi tọa độ điểm điều khiển
						else if (choosingControl >= 0)
						{
							shapes[choosingShape].controlPoints[choosingControl] = pEnd;
						}
						//Nếu đang chọn một điểm vẽ của hình => phép tịnh tiến hình
						else if (choosingRaster >= 0)
						{
							//Thiết lập phép tịnh tiến
							transformer.LoadIdentity();
							transformer.Translate(pEnd.X - pStart.X, pEnd.Y - pStart.Y);

							//Biến đổi tập điểm điều khiển
							for (int i = 0; i < shapes[choosingShape].controlPoints.Count; i++)
								shapes[choosingShape].controlPoints[i] = transformer.Transform(backupShape.controlPoints[i]);
						}

						//Xóa tập điểm vẽ, vẽ lại tập điểm mới
						shapes[choosingShape].rasterPoints.Clear();

						Point controlPoint0 = shapes[choosingShape].controlPoints[0];
						Point controlPoint1 = shapes[choosingShape].controlPoints[1];

						switch (shapes[choosingShape].type)
						{
							case Shape.shapeType.LINE:
								DrawingAlgorithms.Line(shapes[choosingShape], controlPoint0, controlPoint1);
								break;
							case Shape.shapeType.CIRCLE:
								DrawingAlgorithms.Circle(shapes[choosingShape], controlPoint0, controlPoint1);
								break;
							case Shape.shapeType.RECTANGLE:
								DrawingAlgorithms.Polygon(shapes[choosingShape]);
								break;
							case Shape.shapeType.ELLIPSE:
								DrawingAlgorithms.Ellipse(shapes[choosingShape], controlPoint0, controlPoint1);
								break;
							case Shape.shapeType.TRIANGLE:
								DrawingAlgorithms.Polygon(shapes[choosingShape]);
								break;
							case Shape.shapeType.PENTAGON:
								DrawingAlgorithms.Polygon(shapes[choosingShape]);
								break;
							case Shape.shapeType.HEXAGON:
								DrawingAlgorithms.Polygon(shapes[choosingShape]);
								break;
							case Shape.shapeType.POLYGON:
								DrawingAlgorithms.Polygon(shapes[choosingShape]);
								break;
						}

						//Vẽ lại hình
						isShapesChanged = true;
					}
				);
				thread.IsBackground = true;
				thread.Start();
			}
		}

		private void btnLine_Click(object sender, EventArgs e)
		{
			//Sự kiện "chọn vẽ đường thẳng"
			userType = Shape.shapeType.LINE;
			lbMode.Text = "Mode: Line (ESC to cancel)";
			btnRotateOrScale.Enabled = false;
		}

		private void btnCircle_Click(object sender, EventArgs e)
		{
			//Sự kiện "chọn vẽ đường tròn"
			userType = Shape.shapeType.CIRCLE;
			lbMode.Text = "Mode: Circle (ESC to cancel)";
			btnRotateOrScale.Enabled = false;
		}

		private void btnRectangle_Click(object sender, EventArgs e)
		{
			//Sự kiện "chọn vẽ hình chữ nhật"
			userType = Shape.shapeType.RECTANGLE;
			lbMode.Text = "Mode: Rectangle (ESC to cancel)";
			btnRotateOrScale.Enabled = false;
		}

		private void btnEllipse_Click(object sender, EventArgs e)
		{
			//Sự kiện "chọn vẽ ellipse"
			userType = Shape.shapeType.ELLIPSE;
			lbMode.Text = "Mode: Ellipse (ESC to cancel)";
			btnRotateOrScale.Enabled = false;
		}

		private void btnTriangle_Click(object sender, EventArgs e)
		{
			//Sự kiện "chọn vẽ tam giác đều"
			userType = Shape.shapeType.TRIANGLE;
			lbMode.Text = "Mode: Triangle (ESC to cancel)";
			btnRotateOrScale.Enabled = false;
		}

		private void btnPentagon_Click(object sender, EventArgs e)
		{
			//Sự kiện "chọn vẽ ngũ giác đều"
			userType = Shape.shapeType.PENTAGON;
			lbMode.Text = "Mode: Pentagon (ESC to cancel)";
			btnRotateOrScale.Enabled = false;
		}

		private void btnHexagon_Click(object sender, EventArgs e)
		{
			//Sự kiện "chọn vẽ lục giác đều"
			userType = Shape.shapeType.HEXAGON;
			lbMode.Text = "Mode: Hexagon (ESC to cancel)";
			btnRotateOrScale.Enabled = false;
		}

		private void btnPolygon_Click(object sender, EventArgs e)
		{
			//Sự kiện "chọn vẽ đa giác"
			userType = Shape.shapeType.POLYGON;
			lbMode.Text = "Mode: Polygon (ESC to cancel)";
			btnRotateOrScale.Enabled = false;
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			//Sự kiện "xóa toàn bộ"
			shapes.Clear();
			isDrawing = isPolygonDrawing = false;
			choosingShape = choosingControl = choosingRaster = -1;
			isShapesChanged = true;
			btnRotateOrScale.Enabled = false;
		}

		private void btnColor_Click(object sender, EventArgs e)
		{
			//Sự kiện "chọn màu"
			if (colorDialog.ShowDialog() == DialogResult.OK)
				userColor = colorDialog.Color;
		}

		private void btnWidth_Click(object sender, EventArgs e)
		{
			//Sự kiện "thay đổi độ dày nét vẽ"
			userWidth += 0.5f;
			if (userWidth > 3)
				userWidth = 1.0f;
			btnWidth.Text = "Width: " + userWidth.ToString("0.0");
		}

		private void btnRotateOrScale_Click(object sender, EventArgs e)
		{
			if (btnRotateOrScale.Text == "Rotate")
			{
				btnRotateOrScale.Text = "Scale";
				rotateOrScale = 1;
			}
			else
			{
				btnRotateOrScale.Text = "Rotate";
				rotateOrScale = 0;
			}
		}

		private void btnFlood_Click(object sender, EventArgs e)
		{
			if (choosingShape >= 0 && shapes[choosingShape].fillColor != userColor)
			{
				if (shapes[choosingShape].type != Shape.shapeType.CIRCLE && shapes[choosingShape].type != Shape.shapeType.ELLIPSE && shapes[choosingShape].controlPoints.Count < 3)
					return;

				Thread thread = new Thread
				(
					() => FloodFiller.Fill(shapes[choosingShape], userColor, ref isShapesChanged)
				);
				thread.IsBackground = true;
				thread.Start();
			}
		}

		private void btnScanline_Click(object sender, EventArgs e)
		{
			if (shapes[choosingShape].type != Shape.shapeType.CIRCLE && shapes[choosingShape].type != Shape.shapeType.ELLIPSE && shapes[choosingShape].controlPoints.Count < 3)
				return;

			if (choosingShape >= 0 && shapes[choosingShape].fillColor != userColor)
			{
				Thread thread = new Thread
				(
					() => ScanlineFiller.Fill(shapes[choosingShape], userColor, ref isShapesChanged)
				);
				thread.IsBackground = true;
				thread.Start();
			}
		}

		private void drawBoard_KeyDown(object sender, KeyEventArgs e)
		{
			//Sự kiện "bấm bàn phím"
			if (e.KeyCode == Keys.Z && e.Control && shapes.Count > 0)
			{
				//Ctrl + Z => undo
				shapes.RemoveAt(shapes.Count - 1);
				isDrawing = isPolygonDrawing = false;
				choosingShape = choosingRaster = -1;
				choosingControl = -2;
				isShapesChanged = true;
			}
			else if (e.KeyCode == Keys.Escape)
			{
				//Esc => hủy bỏ thao tác
				if (isDrawing)
				{
					shapes.RemoveAt(shapes.Count - 1);
					isDrawing = isPolygonDrawing = false;
					choosingShape = choosingRaster = -1;
					choosingControl = -2;
					isShapesChanged = true;
				}
				else
				{
					lbMode.Text = "Mode: Picking";
					userType = Shape.shapeType.NONE;
					isDrawing = isPolygonDrawing = false;
					choosingShape = choosingRaster = -1;
					choosingControl = -2;
					isShapesChanged = true;
					btnRotateOrScale.Enabled = true;
				}
			}
		}
	}
}
