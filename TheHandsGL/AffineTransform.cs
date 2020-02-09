using System;
using System.Collections.Generic;
using System.Drawing;

namespace TheHandsGL
{
	class AffineTransform
	{
		//Lớp "AffineTransform", định nghĩa các phép biến đổi Affine 2D (3x3)

		//Ma trận biến đổi Affine
		List<double> transformMatrix;

		public AffineTransform()
		{
			//Hàm khởi tạo (khởi tạo bằng ma trận đơn vị)
			transformMatrix = new List<double> { 1, 0, 0, 0, 1, 0, 0, 0, 1 };
		}

		public void Multiply(List<double> matrix)
		{
			//Hàm nhân một ma trận khác với ma trận hiện hành
			List<double> retMatrix = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
			for (int i = 0; i < 3; i++)
				for (int j = 0; j < 3; j++)
					for (int k = 0; k < 3; k++)
						retMatrix[i * 3 + j] += matrix[i * 3 + k] * transformMatrix[k * 3 + j];
			transformMatrix = retMatrix;
		}

		public void LoadIdentity()
		{
			//Reset ma trận hiện hành về ma trận đơn vị
			transformMatrix = new List<double> { 1, 0, 0, 0, 1, 0, 0, 0, 1 };
		}

		public void Translate(double dx, double dy)
		{
			//Hàm tạo ma trận tịnh tiến và nhân với ma trận hiện hành
			List<double> transformMatrix = new List<double> { 1, 0, dx, 0, 1, dy, 0, 0, 1 };
			Multiply(transformMatrix);
		}

		public void Scale(double sx, double sy)
		{
			//Hàm tạo ma trận co giãn và nhân với ma trận hiện hành
			List<double> transformMatrix = new List<double> { sx, 0, 0, 0, sy, 0, 0, 0, 1 };
			Multiply(transformMatrix);
		}

		public void Rotate(double phi)
		{
			//Hàm tạo ma trận xoay và nhân với mà trận hiện hành
			double cosPhi = Math.Cos(phi), sinPhi = Math.Sin(phi);
			List<double> rotateMatrix = new List<double> { cosPhi, -sinPhi, 0, sinPhi, cosPhi, 0, 0, 0, 1 };
			Multiply(rotateMatrix);
		}

		public Point Transform(Point p)
		{
			//Hàm áp dụng phép biến đổi lên một điểm
			List<double> oriPoint = new List<double> { p.X, p.Y, 1.0 };
			List<double> retPoint = new List<double> { 0, 0, 0 };
			for (int i = 0; i < 3; i++)
				for (int j = 0; j < 3; j++)
					retPoint[i] += transformMatrix[i * 3 + j] * oriPoint[j];
			return new Point((int)(Math.Round(retPoint[0])), (int)(Math.Round(retPoint[1])));
		}
	}
}
