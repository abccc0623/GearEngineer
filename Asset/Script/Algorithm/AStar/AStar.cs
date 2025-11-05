using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Algorithm;
using Godot;


namespace Algorithm
{
    public class AStar
    {
        public enum PointType
        {
            NONE = 0,
            outline = 1,
            wall = 2,
            breakableWall = 3,
            road = 4,
        }

        //private List<Vector2I> obstacle = new List<Vector2I>();
        public List<AstarNode> closeList = new List<AstarNode>();
        public PriorityQueue<AstarNode, float> openQueue = new PriorityQueue<AstarNode, float>();

        private Vector2I startPoint = new Vector2I(0, 0);
        private Vector2I endPoint = new Vector2I(0, 0);
        private float GlobalCost = 5;

        //public async void FindPath4Dir(Vector2I startPoint, Vector2I endPoint, List<Vector2I> obstacleList)
        Dictionary<Vector2I,int> obstacle = new Dictionary<Vector2I,int>();
        public void Clear()
        {
            obstacle.Clear();
            closeList.Clear();
            openQueue.Clear();
        }

        public void SettingOutLine(int startPointX, int startPointY, int width, int height,PointType type ,int thickness = 1)
        {
            int x1 = startPointX;
            int y1 = startPointY;
            int x2 = startPointX + width;
            int y2 = startPointY + height;
            for (int t = 0; t < thickness; t++)
            {
                int offset = t;
                // 상단
                for (int x = x1 + offset; x <= x2 - offset; x++)
                    obstacle.TryAdd(new Vector2I(x, y1 + offset), (int)type);
                // 하단
                for (int x = x1 + offset; x <= x2 - offset; x++)
                    obstacle.TryAdd(new Vector2I(x, y2 - offset), (int)type);
                // 좌측
                for (int y = y1 + offset; y <= y2 - offset; y++)
                    obstacle.TryAdd(new Vector2I(x1 + offset, y), (int)type);
                // 우측
                for (int y = y1 + offset; y <= y2 - offset; y++)
                    obstacle.TryAdd(new Vector2I(x2 - offset, y), (int)type);
            }
        }

        public Dictionary<Vector2I,int> Get()
        {
            return obstacle;
        }
       
        public void SettingType(List<Vector2I> points,PointType type)
        {
            for (var i = 0; i < points.Count; i++)
            {
                if (obstacle.ContainsKey(points[i]))
                {
                    if (obstacle[points[i]] != (int)PointType.outline)
                    {
                        obstacle[points[i]] = (int)type;
                    }
                }
                else
                {
                    //없는 값이면 그냥 추가
                    obstacle.Add(points[i],(int)type);
                }
            }
        }

        public void Draw()
        {
            Color targetColor = new Color();
            foreach (var keyValuePair in obstacle)
            {
                switch (keyValuePair.Value)
                {
                    case (int)PointType.NONE: targetColor = new Color(0,0,1,255); break;
                    case (int)PointType.outline: targetColor = new Color(1,0,0,255); break;
                    case (int)PointType.wall: targetColor = new Color(0,1,0,255); break;
                    case (int)PointType.breakableWall: targetColor = new Color(0,1, 1,255); break;
                    case (int)PointType.road: targetColor = new Color(1,1, 1,255); break;
                }
                DebugDraw3D.DrawSphere(new Vector3(keyValuePair.Key.X, 0, keyValuePair.Key.Y), 0.5f, targetColor);
            }
        }

        
        
        public List<Vector2I> FindPath4Dir(Vector2I startPoint, Vector2I endPoint)
        {
            openQueue.Clear();
            closeList.Clear();
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            List<AstarNode> D = new List<AstarNode>(4);

            AstarNode start = new AstarNode(this.startPoint.X, this.startPoint.Y, 0, null);
            openQueue.Enqueue(start,0);
            while (openQueue.Count > 0)
            {
                var target = openQueue.Dequeue();
                closeList.Add(target);

                if (target.x == endPoint.X && target.y == endPoint.Y)
                {
                    start = target;
                    break;
                }

                FindOpenNode(target);
            }

            Stack<AstarNode> path = new Stack<AstarNode>();
            List<Vector2I> roadList = new List<Vector2I>();
            while (start != null)
            {
                path.Push(start);
                start = start.parent;
            }

            if (path.Count != 0) path.Pop();
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
            D[1] = new AstarNode(target.x + 1, target.y, target.g + GlobalCost, target);
            D[0] = new AstarNode(target.x - 1, target.y, target.g + GlobalCost, target);
            D[2] = new AstarNode(target.x, target.y + 1, target.g + GlobalCost, target);
            D[3] = new AstarNode(target.x, target.y - 1, target.g + GlobalCost, target);

            //현재 4방향이 장애물에 걸리는지 체크한다.
            for (var i = 0; i < 4; i++)
            {
                var point = new Vector2I(D[i].x, D[i].y);
                //맵 밖으로 나가는걸 방지
                if(obstacle.ContainsKey(point) == true)
                {
                    switch (obstacle[point])
                    {
                        case (int)PointType.outline:
                        case (int)PointType.wall:
                            continue;
                        case (int)PointType.breakableWall:
                            D[i].g += 5;
                            break;
                        case (int)PointType.road:
                            D[i].g -= 3;
                            break;
                    }
                }
                

                bool alreadyOpen = openQueue.UnorderedItems
                    .Any(n => n.Element.x == D[i].x && n.Element.y == D[i].y);
                if (alreadyOpen) continue;
                
                //이미 닫친 노드는 가지 않음
                bool alreadyClosed = closeList.Any(n => n.x == D[i].x && n.y == D[i].y);
                if (alreadyClosed) continue;
                
                D[i].CreateHeuristic(endPoint.X, endPoint.Y);
                openQueue.Enqueue(D[i], D[i].c);
            }
        }
    }
}