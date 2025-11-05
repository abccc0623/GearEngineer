using System;
using System.Collections.Generic;
using Godot;

namespace Algorithm
{
    public class BSPNode
    {
        public enum DoorDirection
        {
            Left = 0,
            Top = 1,
            Right = 2,
            Bottom = 3
        }

        private const int VERTICAL = 0;
        private const int HORIZONTAL = 1;
        public BSPNode child1;
        public BSPNode child2;
        private int PartitionPlane;
        public Vector2I center;
        public bool isClosed = false;
        public List<BSPNode> linkNode = new List<BSPNode>();
        public Rect2I subspace;

        public BSPNode(Rect2I rect)
        {
            subspace = rect;
            center = new Vector2I((int)(rect.Position.X + (rect.Size.X * 0.5f)),
                (int)(rect.Position.Y + (rect.Size.Y * 0.5f)));
        }


        public void Split()
        {
            if (subspace.Size.X < BSP.minRoomSize / 2 || subspace.Size.Y < BSP.minRoomSize / 2) return;

            //어느 지점으로 자를것인지 여부 가로 또는 세로
            PartitionPlane = Partition();

            var child1Rect = new Rect2I();
            var child2Rect = new Rect2I();
            float min = 0.355f;
            float max = 0.655f;
            while (true)
            {
                float value = 0;
                if (PartitionPlane == VERTICAL)
                {
                    var t1 = (int)Math.Round(subspace.Size.Y * min);
                    var t2 = (int)Math.Round(subspace.Size.Y * max);
                    var splitValue = (int)GD.RandRange(t1, t2);
                    var splitHeight = subspace.Size.Y - splitValue;
                    child1Rect = new Rect2I(subspace.Position.X, subspace.Position.Y, subspace.Size.X, splitHeight);
                    child2Rect = new Rect2I(subspace.Position.X, subspace.Position.Y + splitHeight, subspace.Size.X,
                        subspace.Size.Y - splitHeight);
                    value = AspectRatio(child1Rect.Size.X, child1Rect.Size.Y);
                }
                else
                {
                    var t1 = (int)Math.Round(subspace.Size.X * min);
                    var t2 = (int)Math.Round(subspace.Size.X * max);
                    var splitValue = GD.RandRange(t1, t2);
                    var splitWidth = subspace.Size.X - splitValue;
                    child1Rect = new Rect2I(subspace.Position.X, subspace.Position.Y, splitWidth, subspace.Size.Y);
                    child2Rect = new Rect2I(subspace.Position.X + splitWidth, subspace.Position.Y,
                        subspace.Size.X - splitWidth, subspace.Size.Y);
                    value = AspectRatio(child1Rect.Size.X, child1Rect.Size.Y);
                }

                if (value >= 0.5f && value <= 1.5f) break;
                min += 0.025f;
                max -= 0.025f;
                if (min >= max) break;
            }

            child1 = new BSPNode(child1Rect);
            child2 = new BSPNode(child2Rect);
            if (child1.subspace.Size.X > BSP.minRoomSize / 2 && child1.subspace.Size.Y > BSP.minRoomSize / 2)
            {
                child1.Split();
            }

            if (child2.subspace.Size.X > BSP.minRoomSize / 2 && child2.subspace.Size.Y > BSP.minRoomSize / 2)
            {
                child2.Split();
            }
        }

        private int Partition()
        {
            if (subspace.Size.X > subspace.Size.Y)
            {
                return HORIZONTAL;
            }
            else if (subspace.Size.X < subspace.Size.Y)
            {
                return VERTICAL;
            }
            else
            {
                return (GD.RandRange(0, 1) == 1) ? HORIZONTAL : VERTICAL;
            }
        }

        float AspectRatio(int width, int height)
        {
            int GCD(int a, int b)
            {
                while (b != 0)
                {
                    int temp = b;
                    b = a % b;
                    a = temp;
                }

                return Mathf.Abs(a);
            }

            var gdcValue = GCD((int)width, (int)height);
            var ratio = new Vector2(width / gdcValue, height / gdcValue);
            return (ratio.X / ratio.Y);
        }

        public virtual void Draw()
        {
            var color = new Color(1, 0, 0, 0);
            //나의 중심점
            //DebugDraw3D.DrawSphere(new Vector3(center.X, 0, center.Y));
            //나의 범위
            DebugDraw3D.DrawLine(new Vector3(subspace.Position.X, 0, subspace.Position.Y),
                new Vector3(subspace.Position.X + subspace.Size.X, 0, subspace.Position.Y), color);
            DebugDraw3D.DrawLine(new Vector3(subspace.Position.X, 0, subspace.Position.Y),
                new Vector3(subspace.Position.X, 0, subspace.Position.Y + subspace.Size.Y), color);
            DebugDraw3D.DrawLine(new Vector3(subspace.Position.X, 0, subspace.Position.Y + subspace.Size.Y),
                new Vector3(subspace.Position.X + subspace.Size.X, 0, subspace.Position.Y + subspace.Size.Y), color);
            DebugDraw3D.DrawLine(new Vector3(subspace.Position.X + subspace.Size.X, 0, subspace.Position.Y),
                new Vector3(subspace.Position.X + subspace.Size.X, 0, subspace.Position.Y + subspace.Size.Y), color);

            child1?.Draw();
            child2?.Draw();
            
            for (var i = 0; i < linkNode.Count; i++)
            {
                DebugDraw3D.DrawLine(new Vector3(center.X, 0, center.Y),
                    new Vector3(linkNode[i].center.X, 0, linkNode[i].center.Y), color);
            }
        }

        public void GetChild(List<BSPNode> list)
        {
            if (child1 == null && child2 == null)
            {
                if (isClosed == false) list.Add(this);
            }
            else
            {
                child1?.GetChild(list);
                child2?.GetChild(list);
            }
        }


        public bool IsOverlap(Vector2I target)                                 
        {                                                                    
            // 오른쪽이 상대의 왼쪽보다 왼쪽에 있으면 겹치지 않음                                  
            if (subspace.Position.X + subspace.Size.X < target.X)   
                return false;                                                
                                                                      
            // 왼쪽이 상대의 오른쪽보다 오른쪽에 있으면 겹치지 않음                                 
            if (subspace.Position.X >  target.X)     
                return false;                                                
                                                                      
            // 아래쪽이 상대의 위쪽보다 위에 있으면 겹치지 않음                                   
            if (subspace.Position.Y + subspace.Size.Y < target.Y)   
                return false;                                                
                                                                      
            // 위쪽이 상대의 아래쪽보다 아래에 있으면 겹치지 않음                                  
            if (subspace.Position.Y > target.Y)     
                return false;                                                
                                                                      
            // 위 조건들에 걸리지 않으면 겹침                                             
            return true;                                                     
        }                                                                    
    }
}