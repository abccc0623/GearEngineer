using System;
using System.Collections.Generic;
using Godot;

namespace Algorithm
{
    public class BSP
    {
        static public int minRoomSize;
        static public int minRoomCount;
        static private Rect2I subspace;
        static private BSPNode treeNode;

        public void Split(Rect2I rect, int minRoomSize, int minRoomCount)
        {
            BSP.subspace = rect;
            BSP.minRoomSize = minRoomSize;
            BSP.minRoomCount = minRoomCount;
            treeNode = new BSPNode(rect);

            treeNode.Split();
        }

        public void Draw()
        {
            treeNode.Draw();
        }

        public List<BSPNode> GetLastChildren()
        {
            List<BSPNode> LastChildren = new List<BSPNode>();
            treeNode.GetChild(LastChildren);
            return LastChildren;
        }

        public List<Vector2I> GetLastChildrenInCenter()
        {
            List<BSPNode> LastChildren = new List<BSPNode>();
            List<Vector2I> PointList = new List<Vector2I>();
            treeNode.GetChild(LastChildren);
            for (var i = 0; i < LastChildren.Count; i++)
            {
                PointList.Add(LastChildren[i].center);
            }

            return PointList;
        }

        public void CloseRoom(int minRoomCount)
        {
            var childList = GetLastChildren();
            Random random = new Random();
            while (childList.Count != minRoomCount)
            {
                var deleteindex = random.Next(childList.Count);
                childList[deleteindex].isClosed = true;
                childList.RemoveAt(deleteindex);
            }
        }

        public BSPNode Overlap(Vector2I target)
        {
            //나와 겹치면
            BSPNode nowNode = treeNode;

            while (true)
            {
                if (nowNode != null)
                {
                    if (nowNode.child1 == null && nowNode.child2 == null)
                    {
                        break;
                    }
                    else if (nowNode.child1.IsOverlap(target))
                    {
                        nowNode = nowNode.child1;
                    }
                    else if (nowNode.child2.IsOverlap(target))
                    {
                        nowNode = nowNode.child2;
                    }
                }
            }

            if (nowNode.isClosed == true) return null;
            
            return nowNode;
        }        
    }
}