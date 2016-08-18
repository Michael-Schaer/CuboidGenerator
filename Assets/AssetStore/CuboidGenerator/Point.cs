using UnityEngine;

namespace GeneratedCuboids
{
    public class Point
    {
        private int x;
        private int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static float Distance(Point a, Point b)
        {
            float dx = (a.x - b.x);
            float dy = (a.y - b.y);
            return Mathf.Sqrt(dx * dx + dy * dy);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Point other = obj as Point;
            return (other.X == this.X && other.Y == this.Y);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }
    }
}
