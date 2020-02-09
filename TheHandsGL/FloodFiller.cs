using System.Collections.Generic;
using System.Drawing;

namespace TheHandsGL
{
	static class FloodFiller
	{
		static List<Point> Neighbor(Point point)
		{
			List<Point> neighborList = new List<Point>();

			neighborList.Add(new Point(point.X + 1, point.Y));
			neighborList.Add(new Point(point.X - 1, point.Y));
			neighborList.Add(new Point(point.X, point.Y + 1));
			neighborList.Add(new Point(point.X, point.Y - 1));

			return neighborList;
		}

		public static void Fill(Shape shape, Color fillColor, ref bool isShapesChanged)
		{
			shape.fillColor = fillColor;

			Queue<Point> queue = new Queue<Point>();
			queue.Enqueue(new Point((int)(shape.center.Item1), (int)(shape.center.Item2)));

			while (queue.Count > 0)
			{
				Point point = queue.Dequeue();

				if (shape.fillPoints.IndexOf(point) == -1 && shape.rasterPoints.IndexOf(point) == -1)
				{
					shape.fillPoints.Add(point);
					List<Point> neighborList = Neighbor(point);

					for (int i = 0; i < neighborList.Count; i++)
						if (shape.fillPoints.IndexOf(neighborList[i]) == -1 && shape.rasterPoints.IndexOf(neighborList[i]) == -1)
							queue.Enqueue(neighborList[i]);
				}
			}

			shape.isColored = true;
			isShapesChanged = true;
		}
	}
}
