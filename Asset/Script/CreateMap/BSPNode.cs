using System.Collections.Generic;
using Godot;

namespace GearEngineer.GearEngineer.Asset.Script.CreateMap;

public partial class BSPNode : RefCounted
{
    private const int VERTICAL = 0;
    private const int HORIZONTAL = 1;
    
    private int PartitionPlane;
    protected Rect2 rect;
    private Color randomColor;
    private int minRoomSize = 1000;

    public bool isRoom = false;
    public Rect2 sizeRect { get => rect;}
    public Vector2 center = new Vector2();
    
    public BSPNode(Rect2 rect,int minRoomSize)
    {
        randomColor = new Color(1,0,0);
        this.minRoomSize = minRoomSize;
        this.rect = rect;
        PartitionPlane = Partition();
        center = new Vector2I( (int)(rect.Position.X + (rect.Size.X * 0.5f)), (int)(rect.Position.Y  + (rect.Size.Y *0.5f)));
        Split();
    }

    private int Partition()
    {
        var aspect_ratio = rect.Size.X / rect.Size.Y;
        if (aspect_ratio > 1.25f)
        {
            return VERTICAL;
        }
        else if (aspect_ratio < 0.75f)
        {
            return HORIZONTAL;
        }
        else
        {
            return (GD.RandRange(0,1) == 1) ? HORIZONTAL : VERTICAL;
        }
    }

    private void Split()
    {
        DungeonGenerate.BSPTreeNodes.Add(this,null);
        
       if(this is RoomNode ) return;
        
        
        Rect2 childRect1 = new Rect2();
        Rect2 childRect2 = new Rect2();
        float min = 0.4f;
        float max = 0.6f;
        
        if (PartitionPlane == VERTICAL)
        {
            SplitHorizontal(min,max,out childRect1,out childRect2);
        }
        else if (PartitionPlane == HORIZONTAL)
        {
            SplitVertical(min,max,out childRect1,out childRect2);
        }
        
        List<BSPNode> children = new List<BSPNode>();
        var node1 = (rect.Size.X < minRoomSize || rect.Size.Y < minRoomSize) ? new RoomNode(childRect1,minRoomSize) : new BSPNode(childRect1,minRoomSize);
        var node2 = (rect.Size.X < minRoomSize || rect.Size.Y < minRoomSize) ? new RoomNode(childRect2,minRoomSize) : new BSPNode(childRect2,minRoomSize);
        children.Add(node1);          
        children.Add(node2);
        DungeonGenerate.BSPTreeNodes[this] = children;
    }

    public virtual void Draw()
    {
        //나의 중심점
        DebugDraw3D.DrawSphere(new Vector3(center.X, 0,center.Y));
        //나의 범위
        DebugDraw3D.DrawLine(new Vector3(rect.Position.X, 0, rect.Position.Y), new Vector3(rect.Position.X + rect.Size.X, 0, rect.Position.Y), randomColor);
        DebugDraw3D.DrawLine(new Vector3(rect.Position.X, 0, rect.Position.Y), new Vector3(rect.Position.X, 0, rect.Position.Y +rect.Size.Y), randomColor);
        DebugDraw3D.DrawLine(new Vector3(rect.Position.X, 0, rect.Position.Y +rect.Size.Y), new Vector3(rect.Position.X + rect.Size.X, 0, rect.Position.Y +rect.Size.Y), randomColor);
        DebugDraw3D.DrawLine(new Vector3(rect.Position.X + rect.Size.X, 0, rect.Position.Y), new Vector3(rect.Position.X + rect.Size.X, 0, rect.Position.Y +rect.Size.Y), randomColor);
    }

    float AspectRatio(float width, float height)
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
        var gdcValue = GCD((int)width,(int)height);
        var ratio = new Vector2(width/ gdcValue, height/ gdcValue);
        return ratio.X/ ratio.Y;
    }

    void SplitVertical(float min,float max,out Rect2 childRect1,out Rect2 childRect2)
    {
        while (true)
        {
            var t1 = rect.Size.Y * min;
            var t2 = rect.Size.Y * max;
            var splitValue = (float)GD.RandRange(t1, t2);
            var splitHeight = (float)rect.Size.Y - splitValue;
            childRect1 = new Rect2(rect.Position.X, rect.Position.Y, rect.Size.X, splitHeight);
            childRect2 = new Rect2(rect.Position.X, rect.Position.Y + splitHeight, rect.Size.X, rect.Size.Y - splitHeight);
                
            var value = AspectRatio(childRect1.Size.X ,childRect1.Size.Y);
            if(value >= 0.50f && value <= 1.5f)break;
            min += 0.025f;
            max -= 0.025f;
            if (min >= max) break;
        }
    }

    void SplitHorizontal(float min,float max,out Rect2 childRect1,out Rect2 childRect2)
    {
        while (true)
        {
            var t1 = rect.Size.X * min;
            var t2 = rect.Size.X * max;
            var splitValue = (float)GD.RandRange(t1, t2);
            var splitWidth = (float)rect.Size.X - splitValue;
            childRect1 = new Rect2(rect.Position.X, rect.Position.Y, splitWidth, rect.Size.Y);
            childRect2 = new Rect2(rect.Position.X + splitWidth, rect.Position.Y, rect.Size.X - splitWidth, rect.Size.Y);
                
            var value = AspectRatio(childRect1.Size.X ,childRect1.Size.Y);
            if(value >= 0.750f && value <= 1.25f)break;
            min += 0.025f;
            max -= 0.025f;
            if (min >= max) break;
        }
    }

    public bool AreRectsEdgeAdjacent(Rect2 target, float minOverlapLength = 1.0f)
    {
        float aLeft = rect.Position.X;
        float aRight = rect.Position.X + rect.Size.X;
        float aTop = rect.Position.Y;
        float aBottom = rect.Position.Y + rect.Size.Y;

        float bLeft = target.Position.X;
        float bRight = target.Position.X + target.Size.X;
        float bTop = target.Position.Y;
        float bBottom = target.Position.Y + target.Size.Y;

        // 겹치는 구간 길이 계산
        float overlapY = Mathf.Min(aBottom, bBottom) - Mathf.Max(aTop, bTop);
        float overlapX = Mathf.Min(aRight, bRight) - Mathf.Max(aLeft, bLeft);

        // 수직 방향으로 붙어 있고 Y축 겹치는 길이가 기준 이상
        bool verticalMatch =
            (aRight == bLeft || bRight == aLeft) &&
            (overlapY >= minOverlapLength);

        // 수평 방향으로 붙어 있고 X축 겹치는 길이가 기준 이상
        bool horizontalMatch =
            (aBottom == bTop || bBottom == aTop) &&
            (overlapX >= minOverlapLength);

        return verticalMatch || horizontalMatch;
    }
}
