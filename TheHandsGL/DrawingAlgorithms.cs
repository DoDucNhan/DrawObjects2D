using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TheHandsGL
{
	static class DrawingAlgorithms
	{
		//Lớp "DrawingAlgorithms", định nghĩa các thuật toán vẽ hình

		public static void Line(Shape newShape, Point pStart, Point pEnd)
		{
			//Vẽ đường thẳng bằng 2 điểm chuột pStart và pEnd

			//Vẽ từ điểm có hoành độ nhỏ hơn
			if (pStart.X > pEnd.X)
				(pStart, pEnd) = (pEnd, pStart);

			//Tịnh tiến sao cho pStart trùng với (0, 0), vector tịnh tiến là move
			Point move = new Point(pStart.X, pStart.Y);
			(pStart.X, pStart.Y) = (0, 0);
			(pEnd.X, pEnd.Y) = (pEnd.X - move.X, pEnd.Y - move.Y);

			//Nếu dx bằng 0, đường thẳng đứng
			if (pEnd.X == 0)
			{
				for (int i = Math.Min(0, pEnd.Y); i <= Math.Max(0, pEnd.Y); i++)
					newShape.rasterPoints.Add(new Point(move.X, i + move.Y));
				return;
			}

			//Thuật toán Bresenham
			int dy2 = 2 * pEnd.Y, dx2 = 2 * pEnd.X;
			float m = (float)dy2 / dx2;
			bool negativeM = false, largeM = false;

			//Nếu m < 0, đối xứng qua trục x = 0
			if (m < 0)
			{
				pEnd.Y = -pEnd.Y;
				dy2 = -dy2;
				m = -m;
				negativeM = true;
			}

			//Nếu m > 1, đối xứng qua trục y = x
			if (m > 1)
			{
				(pEnd.X, pEnd.Y) = (pEnd.Y, pEnd.X);
				(dy2, dx2) = (dx2, dy2);
				largeM = true;
			}

			//Tính p0
			int p = dy2 - pEnd.X;

			//List chứa các điểm vẽ
			List<Point> points = new List<Point>();

			int x = 0, y = 0;
			points.Add(new Point(x, y));
			while (x < pEnd.X)
			{
				if (p > 0)
				{
					x++;
					y++;
					p += dy2 - dx2;
				}
				else
				{
					x++;
					p += dy2;
				}
				points.Add(new Point(x, y));
			}

			//Đối xứng lại qua trục y = x (nếu cần)
			if (largeM == true)
				for (int i = 0; i < points.Count; i++)
					points[i] = new Point(points[i].Y, points[i].X);

			//Đối xứng lại qua trục y = 0 (nếu cần)
			if (negativeM == true)
				for (int i = 0; i < points.Count; i++)
					points[i] = new Point(points[i].X, -points[i].Y);

			//Thêm tất cả vào tập điểm vẽ
			for (int i = 0; i < points.Count; i++)
				newShape.rasterPoints.Add(new Point(points[i].X + move.X, points[i].Y + move.Y));

			//Tạo tâm
			double centerX = 0, centerY = 0;
			for (int i = 0; i < newShape.controlPoints.Count; i++)
			{
				centerX += newShape.controlPoints[i].X;
				centerY += newShape.controlPoints[i].Y;
			}
			centerX /= newShape.controlPoints.Count;
			centerY /= newShape.controlPoints.Count;
			newShape.center = new Tuple<double, double>(centerX, centerY);

			//Tạo điểm xoay và co giãn
			double dx = newShape.controlPoints[1].X - centerX, dy = newShape.controlPoints[1].Y - centerY;
			double len = Math.Sqrt(dx * dx + dy * dy);
			AffineTransform transformer = new AffineTransform();
			transformer.Translate(30 * dx / len, 30 * dy / len);
			newShape.extraPoint = transformer.Transform(newShape.controlPoints[1]);

			points.Clear();
		}

		public static void Circle(Shape newShape, Point pStart, Point pEnd)
		{
			//Vẽ đường tròn bằng 2 điểm chuột pStart và pEnd

			//Tính tập điểm theo chiều kim đồng hồ
			//Nhưng winform tính điểm (0, 0) từ góc trên xuống => ngược chiều kim đồng hồ

			//Tính bán kính r
			double r = Math.Sqrt(Math.Pow(pStart.X - pEnd.X, 2) + Math.Pow(pStart.Y - pEnd.Y, 2)) / 2;

			//Tính p0
			double decision = 5 / 4 - r;

			//Điểm đầu (0, r)
			int x = 0;
			int y = (int)r;

			//Tâm đường tròn, cũng chính là vector tịnh tiến
			Point pCenter = new Point((pStart.X + pEnd.X) / 2, (pStart.Y + pEnd.Y) / 2);

			//Tập điểm vẽ ở 1/8 và đối xứng của 1/8 qua trục y = x
			List<Point> partsOfCircle = new List<Point>();

			//Thêm điểm đầu vào tập điểm vẽ
			partsOfCircle.Add(new Point(x, y));
			newShape.rasterPoints.Add(new Point(x + pCenter.X, y + pCenter.Y));

			int x2 = x * 2, y2 = y * 2;

			//Thuật toán Midpoint
			while (y > x)
			{
				if (decision < 0)
				{
					decision += x2 + 3;
					x++;
					x2 += 2;
				}
				else
				{
					decision += x2 - y2 + 5;
					x++;
					y--;
					x2 += 2;
					y2 -= 2;
				}

				//Tịnh tiến mỗi điểm theo tâm, rồi thêm vào tập điểm vẽ
				partsOfCircle.Add(new Point(x, y));
				newShape.rasterPoints.Add(new Point(x + pCenter.X, y + pCenter.Y));
			}

			//Đối xứng qua trục y = x, rồi tịnh tiến theo tâm
			Point p;
			int i, size = partsOfCircle.Count();
			for (i = size - 1; i >= 0; i--)
			{
				p = partsOfCircle[i];
				partsOfCircle.Add(new Point(p.Y, p.X));
				newShape.rasterPoints.Add(new Point(p.Y + pCenter.X, p.X + pCenter.Y));
			}

			//Đối xứng 1/4 qua trục y = 0, rồi tịnh tiến theo tâm
			size = partsOfCircle.Count();
			for (i = size - 1; i >= 0; i--)
			{
				p = partsOfCircle[i];
				partsOfCircle.Add(new Point(p.X, -p.Y));
				newShape.rasterPoints.Add(new Point(p.X + pCenter.X, -p.Y + pCenter.Y));
			}

			//Đối xứng 1/2 qua trục x = 0, rồi tịnh tiến theo tâm
			size = partsOfCircle.Count();
			for (i = size - 1; i >= 0; i--)
			{
				p = partsOfCircle[i];
				newShape.rasterPoints.Add(new Point(-p.X + pCenter.X, p.Y + pCenter.Y));
			}

			//Tạo tâm
			double centerX = 0, centerY = 0;
			for (i = 0; i < newShape.controlPoints.Count; i++)
			{
				centerX += newShape.controlPoints[i].X;
				centerY += newShape.controlPoints[i].Y;
			}
			centerX /= newShape.controlPoints.Count;
			centerY /= newShape.controlPoints.Count;
			newShape.center = new Tuple<double, double>(centerX, centerY);

			//Tạo điểm xoay và co giãn
			double dx = pEnd.X - centerX, dy = pEnd.Y - centerY;
			double len = Math.Sqrt(dx * dx + dy * dy);
			AffineTransform transformer = new AffineTransform();
			transformer.Translate(30 * dx / len, 30 * dy / len);
			newShape.extraPoint = transformer.Transform(pEnd);

			partsOfCircle.Clear();
		}

		public static void Rectangle(Shape newShape, Point pStart, Point pEnd)
		{
			//Vẽ hình chữ nhật bằng 2 điểm chuột pStart và pEnd

			//Tính tọa độ 2 điểm còn lại của hình chữ nhật
			Point p1 = new Point(pEnd.X, pStart.Y);
			Point p2 = new Point(pStart.X, pEnd.Y);

			//Tập 4 điểm điều khiển
			newShape.controlPoints.Clear();
			newShape.controlPoints.Add(pStart);
			newShape.controlPoints.Add(p1);
			newShape.controlPoints.Add(pEnd);
			newShape.controlPoints.Add(p2);

			//Vẽ hình bằng hàm vẽ đa giác
			Polygon(newShape);
		}

		public static void Ellipse(Shape newShape, Point pStart, Point pEnd)
		{
			//Tính tâm ellipse
			Point pCenter = new Point((pStart.X + pEnd.X) / 2, (pStart.Y + pEnd.Y) / 2);

			//Tính đường kính rX
			double rX = Math.Sqrt(Math.Pow(pStart.X - pEnd.X, 2) + Math.Pow(pStart.Y - pStart.Y, 2)) / 2;

			//Tính đường kính rY
			double rY = Math.Sqrt(Math.Pow(pStart.X - pStart.X, 2) + Math.Pow(pStart.Y - pEnd.Y, 2)) / 2;

			//Điểm đầu (0, rY)
			int x = 0;
			int y = (int)rY;

			//Tập điểm ở 1/4
			List<Point> oneFourth = new List<Point>();

			//Thêm điểm đầu vào tập điểm vẽ
			oneFourth.Add(new Point(x, y));
			newShape.rasterPoints.Add(new Point(x + pCenter.X, y + pCenter.Y));

			//Các thông số cơ bản
			double rX2 = rX * rX, rY2 = rY * rY;
			double rX2y = 2 * rX2 * y;
			double rY2x = 2 * rY2 * x;
			double decision = rY2 - rX2 * rY + rX2 / 4;

			while (rY2x < rX2y)
			{
				if (decision < 0)
				{
					x++;
					rY2x += 2 * rY2;
					decision += rY2x + rY2;
				}
				else
				{
					x++;
					y--;
					rY2x += 2 * rY2;
					rX2y -= 2 * rX2;
					decision += rY2x - rX2y + rY2;
				}

				//Tịnh tiến mỗi điểm theo tâm, rồi thêm vào tập điểm vẽ
				oneFourth.Add(new Point(x, y));
				newShape.rasterPoints.Add(new Point(x + pCenter.X, y + pCenter.Y));
			}

			//xLast, yLast
			rX2y = 2 * rX2 * y;
			rY2x = 2 * rY2 * x;
			decision = rY2 * Math.Pow((x + (1 / 2)), 2) + rX2 * Math.Pow((y - 1), 2) - rX2 * rY2;

			while (y >= 0)
			{
				if (decision > 0)
				{
					y--;
					rX2y -= 2 * rX2;
					decision -= rX2y + rX2;
				}
				else
				{
					x++;
					y--;
					rY2x += 2 * rY2;
					rX2y -= 2 * rX2;
					decision += rY2x - rX2y + rX2;
				}

				//Tịnh tiến mỗi điểm theo tâm, rồi thêm vào tập điểm vẽ
				oneFourth.Add(new Point(x, y));
				newShape.rasterPoints.Add(new Point(x + pCenter.X, y + pCenter.Y));
			}

			//Đối xứng 1/4 qua trục x = 0, rồi thêm vào tập điểm vẽ
			int size = oneFourth.Count();
			for (int i = size - 1; i >= 0; i--)
			{
				Point p = oneFourth[i];
				oneFourth.Add(new Point(p.X, -p.Y));
				newShape.rasterPoints.Add(new Point(p.X + pCenter.X, -p.Y + pCenter.Y));
			}

			//Đối xứng 1/2 qua trục y = 0, rồi thêm vào tập điểm vẽ
			size = oneFourth.Count();
			for (int i = size - 1; i >= 0; i--)
			{
				Point p = oneFourth[i];
				oneFourth.Add(new Point(-p.X, p.Y));
				newShape.rasterPoints.Add(new Point(-p.X + pCenter.X, p.Y + pCenter.Y));
			}

			//Tạo tâm
			double centerX = 0, centerY = 0;
			for (int i = 0; i < newShape.controlPoints.Count; i++)
			{
				centerX += newShape.controlPoints[i].X;
				centerY += newShape.controlPoints[i].Y;
			}
			centerX /= newShape.controlPoints.Count;
			centerY /= newShape.controlPoints.Count;
			newShape.center = new Tuple<double, double>(centerX, centerY);

			//Tạo điểm xoay và co giãn
			double dx = pEnd.X - centerX, dy = pEnd.Y - centerY;
			double len = Math.Sqrt(dx * dx + dy * dy);
			AffineTransform transformer = new AffineTransform();
			transformer.Translate(25 * dx / len, 25 * dy / len);
			newShape.extraPoint = transformer.Transform(pEnd);
		}

		public static void Triangle(Shape newShape, Point pStart, Point pEnd)
		{
			//Vẽ tam giác đều bằng 2 điểm chuột pStart và pEnd

			//Cứ xoay pEnd quanh pStart 60 độ sẽ được một điểm điều khiển mới
			AffineTransform transformer = new AffineTransform();
			transformer.Translate(-pStart.X, -pStart.Y);
			transformer.Rotate(60 * Math.PI / 180);
			transformer.Translate(pStart.X, pStart.Y);
			
			Point p = transformer.Transform(pEnd);

			//Tập 3 điểm điều khiển
			newShape.controlPoints.Clear();
			newShape.controlPoints.Add(pStart);
			newShape.controlPoints.Add(pEnd);
			newShape.controlPoints.Add(p);

			//Vẽ hình bằng hàm vẽ đa giác
			Polygon(newShape);
		}

		public static void Pengtagon(Shape newShape, Point pStart, Point pEnd)
		{
			//Vẽ ngũ giác đều bằng 2 điểm chuột pStart và pEnd

			//Cứ xoay pEnd quanh pStart 72 độ sẽ được một điểm điều khiển mới
			AffineTransform transformer = new AffineTransform();
			transformer.Translate(-pStart.X, -pStart.Y);
			transformer.Rotate(72 * Math.PI / 180);
			transformer.Translate(pStart.X, pStart.Y);

			Point p1 = transformer.Transform(pEnd);
			Point p2 = transformer.Transform(p1);
			Point p3 = transformer.Transform(p2);
			Point p4 = transformer.Transform(p3);

			//Tập 5 điểm điều khiển
			newShape.controlPoints.Clear();
			newShape.controlPoints.Add(pEnd);
			newShape.controlPoints.Add(p1);
			newShape.controlPoints.Add(p2);
			newShape.controlPoints.Add(p3);
			newShape.controlPoints.Add(p4);

			//Vẽ hình bằng hàm vẽ đa giác
			Polygon(newShape);
		}

		public static void Hexagon(Shape newShape, Point pStart, Point pEnd)
		{
            //Vẽ ngũ giác đều bằng 2 điểm chuột pStart và pEnd

            // Tìm ma trận xoay một góc 60 độ với tâm quay là điểm bắt đầu.
            AffineTransform transformer = new AffineTransform();
            transformer.Translate(-pStart.X, -pStart.Y);
            transformer.Rotate(60 * Math.PI / 180);
            transformer.Translate(pStart.X, pStart.Y);

            Point p1 = transformer.Transform(pEnd);
            Point p2 = transformer.Transform(p1);
            Point p3 = transformer.Transform(p2);
            Point p4 = transformer.Transform(p3);
            Point p5 = transformer.Transform(p4);

            //Tập 6 điểm điều khiển
            newShape.controlPoints.Clear();
            newShape.controlPoints.Add(pEnd);
            newShape.controlPoints.Add(p1);
            newShape.controlPoints.Add(p2);
            newShape.controlPoints.Add(p3);
            newShape.controlPoints.Add(p4);
            newShape.controlPoints.Add(p5);

            //Vẽ hình bằng hàm vẽ đa giác
            Polygon(newShape);
        }

		public static void Polygon(Shape newShape)
		{
			//Nối các điểm kề nhau
			for (int i = 0; i < newShape.controlPoints.Count - 1; i++)
				Line(newShape, newShape.controlPoints[i], newShape.controlPoints[i + 1]);
			//Nối điểm cuối với điểm đầu
			Line(newShape, newShape.controlPoints.Last(), newShape.controlPoints[0]);

			//Tạo tâm
			double centerX = 0, centerY = 0;
			for (int i = 0; i < newShape.controlPoints.Count; i++)
			{
				centerX += newShape.controlPoints[i].X;
				centerY += newShape.controlPoints[i].Y;
			}
			centerX /= newShape.controlPoints.Count;
			centerY /= newShape.controlPoints.Count;
			newShape.center = new Tuple<double, double>(centerX, centerY);

			//Tạo điểm xoay và co giãn
			double dx = newShape.controlPoints[1].X - centerX, dy = newShape.controlPoints[1].Y - centerY;
			double len = Math.Sqrt(dx * dx + dy * dy);
			AffineTransform transformer = new AffineTransform();
			transformer.Translate(25 * dx / len, 25 * dy / len);
			newShape.extraPoint = transformer.Transform(newShape.controlPoints[1]);
		}
	}
}
