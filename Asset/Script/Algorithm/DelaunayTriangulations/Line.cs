using Godot;

namespace Algorithm
{
    public class Line
    {
        public Vector2I p1;
        public Vector2I p2;
        public Line(Vector2I p1, Vector2I p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }
        public void Draw()
        {
            DebugDraw3D.DrawLine(new Vector3(p1.X,0,p1.Y),new Vector3(p2.X,0,p2.Y),new Color(0,1,0));
        }
        
        public override bool Equals(object obj)
        {
            if (obj is not Line other) return false;

            // A-B == B-A 도 같은 선으로 간주
            return (p1 == other.p1 && p2 == other.p2) || (p1 == other.p2 && p2 == other.p1);
        }

        public override int GetHashCode()
        {
            // 순서와 관계없이 동일한 해시값 보장
            int hash1 = p1.GetHashCode() ^ p2.GetHashCode();
            int hash2 = p2.GetHashCode() ^ p1.GetHashCode();
            return hash1 ^ hash2;
        }
    }    
}
