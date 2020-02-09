using SharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace TheHandsGL
{
	class Shape
	{
		//Lớp "Shape", đối tượng hình vẽ
		public enum shapeType { NONE, LINE, CIRCLE, RECTANGLE, ELLIPSE, TRIANGLE, PENTAGON, HEXAGON, POLYGON }

		//Màu nét vẽ
		Color color;
		//Độ dày nét vẽ
		float width;
		//Loại hình vẽ
		public shapeType type;

		//Tập điểm điều kiển. Lưu ý, để vẽ đúng, các điểm điều khiển phải đúng thứ tự
		public List<Point> controlPoints;
		//Tập điểm vẽ
		public List<Point> rasterPoints;
		//Điểm xoay và co giãn
		public Point extraPoint;
		//Tâm
		public Tuple<double, double> center;

		//Biến đánh dấu có tô màu hay không
		public bool isColored;
		//Màu tô
		public Color fillColor;
		//Tập điểm tô
		public List<Point> fillPoints;

		public Shape(Color userColor, float userWidth, shapeType userType)
		{
			//Hàm khởi tạo
			color = userColor;
			width = userWidth;
			type = userType;

			rasterPoints = new List<Point>();
			controlPoints = new List<Point>();
			extraPoint = new Point(-1, -1);

			isColored = false;
			fillColor = Color.Black;
			fillPoints = new List<Point>();
		}

		public void Draw(OpenGL gl)
		{
			//Hàm vẽ từng pixel

			//Set màu nét vẽ
			gl.Color(color.R, color.G, color.B, color.A);
			//Set độ dày nét vẽ
			gl.PointSize(width);

			//Liệt kê tập điểm vẽ cho OpenGL
			gl.Begin(OpenGL.GL_POINTS);
				for (int i = 0; i < rasterPoints.Count; i++)
					gl.Vertex(rasterPoints[i].X, gl.RenderContextProvider.Height - rasterPoints[i].Y);
			gl.End();
		}

		public void Fill(OpenGL gl)
		{
			//Hàm tô tưng pixel

			//Set màu tô
			gl.Color(fillColor.R, fillColor.G, fillColor.B);

			//Liệt kê tập điểm tô cho OpenGL
			gl.Begin(OpenGL.GL_POINTS);
				for (int j = 0; j < fillPoints.Count; j++)
					gl.Vertex(fillPoints[j].X, gl.RenderContextProvider.Height - fillPoints[j].Y);
			gl.End();
		}

		public Shape Clone()
		{
			//Hàm sao chép hình

			Shape newShape = new Shape(color, width, type);

			for (int i = 0; i < controlPoints.Count; i++)
				newShape.controlPoints.Add(new Point(controlPoints[i].X, controlPoints[i].Y));

			for (int i = 0; i < rasterPoints.Count; i++)
				newShape.rasterPoints.Add(new Point(rasterPoints[i].X, rasterPoints[i].Y));

			newShape.extraPoint = new Point(extraPoint.X, extraPoint.Y);
			newShape.center = new Tuple<double, double>(center.Item1, center.Item2);

			newShape.isColored = isColored;
			newShape.fillColor = fillColor;

			return newShape;
		}
	}
}
