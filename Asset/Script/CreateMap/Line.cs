using System.Diagnostics;
using Godot;

namespace GearEngineer.GearEngineer.Asset.Script.CreateMap;

public class Line
{
    public Vector2 p1;
    public Vector2 p2;
    public Line(Vector2 p1, Vector2 p2)
    {
        this.p1 = p1;
        this.p2 = p2;
    }
    
    public override bool Equals(object obj)
    {
        if (obj is Line other)
        {
            return (p1 == other.p1) && (p2 == other.p2) || (p1 == other.p2) && (p2 == other.p1);
        }
        return false;
    }

    public override int GetHashCode()
    {
        // 정렬된 해시를 사용해 순서 무시
        int h1 = p1.GetHashCode();
        int h2 = p2.GetHashCode();
        return h1 < h2 ? h1 ^ h2 : h2 ^ h1;
    }

    public void Draw()
    {
        DebugDraw3D.DrawLine(new Vector3(p1.X,0,p1.Y),new Vector3(p2.X,0,p2.Y),new Color(0,1,0));
    }
}