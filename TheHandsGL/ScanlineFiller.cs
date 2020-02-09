using System;
using System.Collections.Generic;
using System.Drawing;

namespace TheHandsGL
{
	class Edge
	{
		public Point pBefore, pAfter; //go clockwise
		public int slope; //check if parallel to Ox
		public int yLower;

		public Edge(Point p_before, Point p_after)
		{
			pBefore = p_before;
			pAfter = p_after;
			slope = p_before.Y == p_after.Y ? 0 : 1;
			yLower = p_before.Y < p_after.Y ? p_before.Y : p_after.Y;
		}
	}

	class AEL
	{
		public int yUpper;
		public float xIntersect;
		public float reciSlope;

		public AEL(int _yUpper, float _xIntersect, float _reciSlope)
		{
			yUpper = _yUpper;
			xIntersect = _xIntersect;
			reciSlope = _reciSlope;
		}
	}

	static class ScanlineFiller
	{
		static List<AEL> _ael = new List<AEL>();
		static List<Point> extremePoints = new List<Point>();
		static List<Edge> _edgeList = new List<Edge>();
		static int yMin, yMax;

		static void pushEdge(Edge edge)
		{
			int _yUpper = edge.pBefore.Y > edge.pAfter.Y ? edge.pBefore.Y : edge.pAfter.Y; //y of point has y_upper
			float _xIntersect = edge.pBefore.Y > edge.pAfter.Y ? edge.pAfter.X : edge.pBefore.X; //x of point has y_lower
			float _reciSlope = (float)(edge.pBefore.X - edge.pAfter.X) / (edge.pBefore.Y - edge.pAfter.Y);
			AEL aelTemp = new AEL(_yUpper, _xIntersect, _reciSlope);
			_ael.Add(aelTemp);
		}

		static int partition(int low, int high)
		{
			AEL pivot = _ael[high];

			// index of smaller element 
			int i = (low - 1);
			for (int j = low; j < high; j++)
			{
				// If current element is smaller  
				// than the pivot 
				if (_ael[j].xIntersect < pivot.xIntersect)
				{
					i++;

					// swap arr[i] and arr[j] 
					AEL temp = _ael[i];
					_ael[i] = _ael[j];
					_ael[j] = temp;
				}
			}

			// swap arr[i+1] and arr[high] (or pivot) 
			AEL temp1 = _ael[i + 1];
			_ael[i + 1] = _ael[high];
			_ael[high] = temp1;

			return i + 1;
		}

		static void quickSort(int low, int high)
		{
			/* The main function that implements QuickSort() 
			arr[] --> Array to be sorted, 
			low --> Starting index, 
			high --> Ending index */
			if (low < high)
			{
				/* pi is partitioning index, arr[pi] is  
				now at right place */
				int pi = partition(low, high);

				// Recursively sort elements before 
				// partition and after partition 
				quickSort(low, pi - 1);
				quickSort(pi + 1, high);
			}
		}

		static void findExtremePoints(List<Point> vertex)
		{
			Point p1, p2, p3;
			int i, size = vertex.Count;
			for (i = 0; i < size; i++)
			{
				//Duyệt theo chiều kim đồng hồ và kiểm tra mỗi 3 điểm
				if (i == size - 2)
				{
					p1 = vertex[i];
					p2 = vertex[i + 1];
					p3 = vertex[0];
				}
				else if (i == size - 1)
				{
					p1 = vertex[i];
					p2 = vertex[0];
					p3 = vertex[1];
				}
				else
				{
					p1 = vertex[i];
					p2 = vertex[i + 1];
					p3 = vertex[i + 2];
				}

				int delta12 = p2.Y - p1.Y;
				int delta23 = p3.Y - p2.Y;
				if (delta12 * delta23 < 0)
					extremePoints.Add(p2);
			}
		}

		static void getY_MinMax(List<Point> vertex)
		{
			int i, size = vertex.Count;

			yMax = yMin = vertex[0].Y;
			for (i = 1; i < size; i++)
			{
				if (yMax < vertex[i].Y)
					yMax = vertex[i].Y;

				if (yMin > vertex[i].Y)
					yMin = vertex[i].Y;
			}
		}

		static void refineData(List<Point> vertex)
		{
			getY_MinMax(vertex);
			Point p1, p2;
			int i, size = vertex.Count;
			findExtremePoints(vertex);
			for (i = 0; i < size; i++)
			{
				if (i == size - 1)
				{
					p1 = vertex[i];
					p2 = vertex[0];
				}
				else
				{
					p1 = vertex[i];
					p2 = vertex[i + 1];
				}

				//Tạo list các cạnh
				Edge eTemp = new Edge(p1, p2);
				if (eTemp.slope == 0)
					continue;
				else
				{
					//Nếu không phải điểm cực trị thì y - 1
					if (extremePoints.IndexOf(eTemp.pBefore) == -1)
						eTemp.pBefore.Y--;
					if (extremePoints.IndexOf(eTemp.pAfter) == -1)
						eTemp.pAfter.Y--;

					_edgeList.Add(eTemp);
				}
			}
		}

		public static void Fill(Shape shape, Color fillColor, ref bool isShapesChanged)
		{
			shape.fillColor = fillColor;
			if (shape.controlPoints.Count < 3)
				return;

			int i, j, k;
			//refine data
			refineData(shape.controlPoints);

			for (i = yMin; i <= yMax; i++)
			{
				//fill edge table with no edge table
				for (j = 0; j < _edgeList.Count;)
				{
					if (i == _edgeList[j].yLower)
					{
						pushEdge(_edgeList[j]);
						_edgeList.RemoveAt(j);
					}
					else
						j++;
				}

				//Sort ascending x_intersect
				quickSort(0, _ael.Count - 1);

				//Fill color
				for (j = 0; j < _ael.Count; j += 2)
					for (k = (int)Math.Round(_ael[j].xIntersect); k <= (int)Math.Round(_ael[j + 1].xIntersect); k++)
						shape.fillPoints.Add(new Point(k, i));

				for (j = 0; j < _ael.Count;)
				{
					//Remove edge has y_upper = y
					if (_ael[j].yUpper == i)
						_ael.RemoveAt(j);
					else //Update x intersect
					{
						_ael[j].xIntersect += _ael[j].reciSlope;
						j++;
					}
				}
			}
			shape.isColored = true;
			isShapesChanged = true;
			extremePoints.Clear();
		}
	}
}
