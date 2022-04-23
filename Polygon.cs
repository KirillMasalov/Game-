using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game_
{
    public class Polygon
    {
        public List<Point> Points;
        public Polygon(params int[] coordinates)
        {
            if (coordinates.Length == 0 || coordinates.Length == 1
                || coordinates.Length == 2)
                throw new ArgumentException("Недостаточно точек для построения полигона");
            if (coordinates.Length % 2 == 1)
                throw new ArgumentException("Нечетное количество координат");
            Points = new List<Point>();
            for (var i = 0; i < coordinates.Length; i+=2)
                Points.Add(new Point(coordinates[i], coordinates[i + 1]));
            if (Points.First() != Points.Last())
                Points.Add(Points.Last());
        }

        public Point[] GetPoints()
        {
            return Points.ToArray();
        }
    }
}
