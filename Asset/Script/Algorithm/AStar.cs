using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;


public class AStar
{
    enum Direction
    {
        LEFT = 0,
        RIGHT = 1,
        TOP = 2,
        BOTTOM = 3,
    }


    private List<Vector2I> obstacle = new List<Vector2I>();
    public List<AstarNode> closeList = new List<AstarNode>();
    public PriorityQueue<AstarNode, float> openQueue = new PriorityQueue<AstarNode, float>();

    private Vector2I startPoint = new Vector2I(0, 0);
    private Vector2I endPoint = new Vector2I(0, 0);
    private float GlobalCost = 1;


    public List<Vector2I> FindPath4Dir(Vector2I startPoint, Vector2I endPoint, List<Vector2I> obstacleList)
    {
        obstacle = obstacleList;
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        List<AstarNode> D = new List<AstarNode>(4);


        AstarNode start = new AstarNode(this.startPoint.X, this.startPoint.Y, 0,null);
        while (start.x != endPoint.X || start.y != endPoint.Y)
        {
            //시작점
            FindOpenNode(start);

            //현재 OpenList에서 거리비용이 가장 짧은 노드를 찾는다.
            var target = openQueue.Dequeue();
            closeList.Add(target);
            start = target;
        }

        Stack<AstarNode> path = new Stack<AstarNode>();
        List<Vector2I> roadList = new List<Vector2I>();
        while (start.parent != null)
        {
            path.Push(start.parent);
            start = start.parent;
        }
        path.Pop();
        while (path.Count != 0)
        {
            var target = path.Pop();
            roadList.Add(new Vector2I(target.x, target.y));
        }

        return roadList;
    }

    void FindOpenNode(AstarNode target)
    {
        //4방향 위치를 구함
        AstarNode[] D = new AstarNode[4];
        D[(int)Direction.RIGHT] = new AstarNode(target.x + 1, target.y, target.g + GlobalCost,target);
        D[(int)Direction.LEFT] = new AstarNode(target.x - 1, target.y, target.g + GlobalCost,target);
        D[(int)Direction.TOP] = new AstarNode(target.x, target.y + 1, target.g + GlobalCost,target);
        D[(int)Direction.BOTTOM] = new AstarNode(target.x, target.y - 1, target.g + GlobalCost,target);

        //현재 4방향이 장애물에 걸리는지 체크한다.
        for (var i = 0; i < 4; i++)
        {
            //맵 밖으로 나가는걸 방지
            if(D[i].x < 0 || D[i].y < 0 )continue;
            
            //이미 닫친 노드는 가지 않음
            bool alreadyClosed = closeList.Any(n => n.x == D[i].x && n.y == D[i].y);
            if (alreadyClosed) continue;
            
            var findTarget = obstacle.Find(vector2I => vector2I.X == D[i].x && vector2I.Y == D[i].y);
            if (findTarget == Vector2I.Zero)
            {
                D[i].CreateHeuristic(endPoint.X, endPoint.Y);
                openQueue.Enqueue(D[i],D[i].c);
            }
        }
    }


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
    }
}