using System;

namespace Algorithm
{
    public class AstarNode
    {
        public int x = 0;
        public int y = 0;
        public float c = 0.0f; //총 점수
        public float g = 0.0f; //현재 위치까지의 이동값
        public float h = 0.0f; //휴리 스틱
        public AstarNode parent;
        
        public AstarNode(int x, int y, float cost,AstarNode parent)
        {
            this.x = x;
            this.y = y;
            this.g = cost;
            this.parent = parent;
        }

        public void CreateHeuristic(int endPointX, int endPointY)
        {
            h = GetManhattanDistance(x, y, endPointX, endPointY);
            c = g + h;
        }

        int GetManhattanDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x2 - x1) + Math.Abs(y2 - y1);
        }
        
        public override bool Equals(object obj)
        {
            if (obj is not AstarNode other) return false;
            return x == other.x && y == other.y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }
    }    
}

