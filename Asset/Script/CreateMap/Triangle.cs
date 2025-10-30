using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Godot;
using Color = Godot.Color;

namespace GearEngineer.GearEngineer.Asset.Script.CreateMap;

public class Triangle
{
    public Vector2 p1;
    public Vector2 p2;
    public Vector2 p3;
    
    public Vector2 center;
    public Vector2 circumcenter;
    List<Line> lineList = new List<Line>();
    public float radius;
    public Triangle(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        this.p1 = p1;
        this.p2 = p2;
        this.p3 = p3;
        lineList.Add(new Line(p1, p2));
        lineList.Add(new Line(p2, p3));
        lineList.Add(new Line(p3, p1));
        //원의 중심점
        center = new Vector2((p1.X + p2.X + p3.X) / 3, (p1.Y + p2.Y + p3.Y) / 3);
        circumcenter = GetCircumcenter(p1,p2,p3);
        //원의 반지름
        radius = circumcenter.DistanceTo(p1);
    }
    public void Draw()
    {
        DebugDraw3D.DrawLine(new Vector3(p1.X,0,p1.Y),new Vector3(p2.X,0,p2.Y),new Color(0,1,0));
        DebugDraw3D.DrawLine(new Vector3(p2.X,0,p2.Y),new Vector3(p3.X,0,p3.Y),new Color(0,1,0));
        DebugDraw3D.DrawLine(new Vector3(p3.X,0,p3.Y),new Vector3(p1.X,0,p1.Y),new Color(0,1,0));
        
        DrawCircle3D(new Vector3(circumcenter.X,0,circumcenter.Y), radius,new Vector3(0,1,0),new Color(1,0,0));
    }
    
    public void DrawCircle3D(Vector3 center, float radius, Vector3 normal, Color color, int segments = 32)
    {
        Vector3 up = normal.Normalized();
        Vector3 right = up.Cross(Vector3.Forward).Normalized();
        if (right == Vector3.Zero)
            right = up.Cross(Vector3.Right).Normalized();

        Vector3 forward = right.Cross(up).Normalized();

        Vector3[] points = new Vector3[segments + 1];
        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.Tau * i / segments;
            Vector3 point = center + radius * (Mathf.Cos(angle) * right + Mathf.Sin(angle) * forward);
            points[i] = point;
        }
    }

    public bool PointInCircumcircle(Vector2 p)
    {
        const float EPSILON = 0.0001f;
        return circumcenter.DistanceTo(p) < radius - EPSILON;
    }

    public List<Line> GetLines()
    {
        return lineList;
    }

    public bool HasVertex(Vector2 v, float epsilon = 0.001f)
    {
        return p1.DistanceTo(v) < epsilon ||
               p2.DistanceTo(v) < epsilon ||
               p3.DistanceTo(v) < epsilon;
    }
    
    public override bool Equals(object obj)
    {
        if (obj is Triangle other)
        {
            return HasVertex(other.p1) &&
                   HasVertex(other.p2) &&
                   HasVertex(other.p3);
        }
        return false;
    }
    
    public static Vector2 GetCircumcenter(Vector2 a, Vector2 b, Vector2 c)
    {
        float d = 2 * (a.X * (b.Y - c.Y) +
                       b.X * (c.Y - a.Y) +
                       c.X * (a.Y - b.Y));

        if (Mathf.Abs(d) < 0.000001f)
            return Vector2.Zero; // 외접원 존재하지 않음

        float aSq = a.LengthSquared();
        float bSq = b.LengthSquared();
        float cSq = c.LengthSquared();

        float ux = (aSq * (b.Y - c.Y) +
                    bSq * (c.Y - a.Y) +
                    cSq * (a.Y - b.Y)) / d;

        float uy = (aSq * (c.X - b.X) +
                    bSq * (a.X - c.X) +
                    cSq * (b.X - a.X)) / d;
        
        

        return new Vector2(ux, uy);
    }
    
    
    public override int GetHashCode()
    {
        return p1.GetHashCode() ^ p2.GetHashCode() ^ p3.GetHashCode();
    }
}
