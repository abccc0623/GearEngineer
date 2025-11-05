using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Algorithm;

public partial class DungeonGenerate : Node3D
{
    private AStar astar = new AStar();
    private BSP bsp = new BSP();
    private DelaunayTriangulations DT = new DelaunayTriangulations();

    private MeshLibrary wall;
    private MeshLibrary tile;
    [Export] private int Vertical;
    [Export] private int Horizontal;
    [Export] private int minRoomSize;
    [Export] private int minRoomCount;

    List<BSPNode> BSPNodeList = new List<BSPNode>();
    
    public override void _Ready()
    {
        wall = GD.Load<MeshLibrary>("res://GearEngineer/Asset/MeshLibrary/Tile.tres");
        CallDeferred(nameof(GenerateTree));
    }

    public override void _Process(double delta)
    {
        astar.Draw();
    }

    private void GenerateTree()
    {
        //사각형들을 쪼개 방을 생성
        bsp.Split(new Rect2I(0, 0, Horizontal, Vertical), minRoomSize, minRoomCount);
        //방들중에서 필요없는 방들을 제거
        bsp.CloseRoom(minRoomCount);
        //방들 중 가장 하위 노드만 가져온다.
        BSPNodeList = bsp.GetLastChildren();
        //가장 하위 노드의 중간지점들만 가져온다.
        var points = bsp.GetLastChildrenInCenter();
        //중간지점으로 삼각형들을 생성
        var triangles = DT.Create(points, new Vector2I(Horizontal / 2, Vertical / 2));
        
        //이렇게 나온 삼각형들로 노드들을 연결시킴
        foreach (var triangle in triangles)
        {
            var node1 = bsp.Overlap(triangle.p1);
            var node2 = bsp.Overlap(triangle.p2);
            var node3 = bsp.Overlap(triangle.p3);
            
            //점1
            var find1 = node1.linkNode.Find(node => node == node2);
            var find2 =node1.linkNode.Find(node => node == node3);
            if(find1 == null) node1.linkNode.Add(node2);
            if(find2 == null) node1.linkNode.Add(node3);
            
            //점2
            var find3 = node2.linkNode.Find(node => node == node1);
            var find4 =node2.linkNode.Find(node => node == node3);
            if(find3 == null) node2.linkNode.Add(node1);
            if(find4 == null) node2.linkNode.Add(node3);
            
            //점3
            var find5 = node3.linkNode.Find(node => node == node1);
            var find6 = node3.linkNode.Find(node => node == node2);
            if(find5 == null) node3.linkNode.Add(node1);
            if(find6 == null) node3.linkNode.Add(node2);
        }
        
        new Prim(BSPNodeList);
        
        astar.Clear();
        astar.SettingOutLine(0,0,Horizontal,Vertical,AStar.PointType.outline);
        for (var i = 0; i < BSPNodeList.Count; i++)
        {
            var rt = BSPNodeList[i].subspace;
            astar.SettingOutLine(rt.Position.X,rt.Position.Y,rt.Size.X,rt.Size.Y,AStar.PointType.breakableWall,1);
        }
        
        //Astar로 방끼리 연결시킴
        for (var i = 0; i < BSPNodeList.Count; i++)
        {
            for (var j = 0; j < BSPNodeList[i].linkNode.Count; j++)
            {
                if (BSPNodeList[i].isClosed == true) continue;
                var path = astar.FindPath4Dir(
                    BSPNodeList[i].center,
                    BSPNodeList[i].linkNode[j].center);
                astar.SettingType(path, AStar.PointType.road);
            }
        }
        CreateGroundCollider();
        CreateWall();
    }
    
    void CreateGround(Vector3 position)
    {
        MeshInstance3D instance = new MeshInstance3D();
        AddChild(instance);
        var rendom = GD.RandRange(0, 100);
        if (rendom >= 0 && rendom < 2)
        {
            instance.Mesh = tile.GetItemMesh(25);
        }
        else
        {
            instance.Mesh = tile.GetItemMesh(24);
        }

        instance.GlobalPosition = position;
        instance.RotationDegrees = new Vector3(-90, 0, 0);
        instance.Scale = new Vector3(100, 100, 100);
    }

    Vector2I GetStartPoint()
    {
        return BSPNodeList[0].center;
    }

    void CreateWall()
    {
        var test = astar.Get();
        foreach (var keyValuePair in test)
        {
            if (keyValuePair.Value == (int)AStar.PointType.road)
            {
                var mesh = wall.GetItemMesh(24);
                MeshInstance3D instance = new MeshInstance3D();
                instance.Mesh = mesh;
                float x = keyValuePair.Key.X;
                float y = keyValuePair.Key.Y;
                AddChild(instance);
                instance.GlobalPosition = new Vector3(x, 0, y);
                instance.Scale = new Vector3(50, 50, 50);
                instance.RotationDegrees = new Vector3(-90, 0, 0);
            }
        }
    }
    
    Node3D CreateGroundCollider()
    {
        Node3D top = new Node3D();
        StaticBody3D body = new StaticBody3D();
        CollisionShape3D shape = new CollisionShape3D();
        top.AddChild(body);
        body.AddChild(shape);
        shape.Shape = new BoxShape3D();
        shape.Scale = new Vector3(Horizontal, 0.1f, Vertical);
        AddChild(top);
        top.GlobalPosition = new Vector3(Horizontal/2, 0, Vertical/2);
        return top;
    }
}