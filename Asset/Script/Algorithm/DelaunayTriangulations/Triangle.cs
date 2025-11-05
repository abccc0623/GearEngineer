using System;
using System.Collections.Generic;
using Godot;

namespace Algorithm
{
    public class Triangle
    {
        public Vector2I p1;
        public Vector2I p2;
        public Vector2I p3;
        public Vector2I center;
        private Vector2I circumcenter;
        private float radius;
        private Line line1;
        private Line line2;
        private Line line3;
        
        public Triangle(Vector2I a,Vector2I b,Vector2I c)
        {
            this.p1 = a;
            this.p2 = b;
            this.p3 = c;
            line1 = new Line(p1, p2);
            line2 = new Line(p2, p3);
            line3 = new Line(p3, p1);
            
            circumcenter = GetCircumcenter(p1,p2,p3);
            radius = circumcenter.DistanceTo(p1);
        }

        //꼭지점을 모두 포함하는 외접원을 구함
        public Vector2I GetCircumcenter(Vector2I a, Vector2I b, Vector2I c)
        {
            float d = 2 * (a.X * (b.Y - c.Y) +
                           b.X * (c.Y - a.Y) +
                           c.X * (a.Y - b.Y));

            if (Mathf.Abs(d) < 0.000001f)
                return Vector2I.Zero; // 외접원 존재하지 않음

            float aSq = a.LengthSquared();
            float bSq = b.LengthSquared();
            float cSq = c.LengthSquared();

            float ux = (aSq * (b.Y - c.Y) +
                        bSq * (c.Y - a.Y) +
                        cSq * (a.Y - b.Y)) / d;

            float uy = (aSq * (c.X - b.X) +
                        bSq * (a.X - c.X) +
                        cSq * (b.X - a.X)) / d;
        
            return new Vector2I( (int)Math.Round(ux) ,(int)Math.Round(uy));
        }

        public bool PointInCircumcircle(Vector2I p)
        {
            return circumcenter.DistanceTo(p) < radius;
        }
        
        public void Draw()
        {
            line1.Draw();
            line2.Draw();
            line3.Draw();
        }

        public List<Line> GetLines()
        {
            List<Line> lines = new List<Line>();
            lines.Add(line1);
            lines.Add(line2);
            lines.Add(line3);
            return lines;
        }

        public bool RemovePoint(Vector2I point)
        {
            if (p1.X == point.X && p1.Y == point.Y)
            {
                return true;
            }
            if (p2.X == point.X && p2.Y == point.Y)
            {
                return true;
            }
            if (p3.X == point.X && p3.Y == point.Y)
            {
                return true;
            }
            return false;
        }
    }    
}

