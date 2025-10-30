using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GearEngineer.GearEngineer.Asset.Script.CreateMap;

public partial class DungeonGenerate : Node3D
{
	private MeshLibrary wall;
	private MeshLibrary tile;
	[Export] private float Vertical;
	[Export] private float Horizontal;
	[Export] private int minRoomSize;
	private BSPNode treeNode;

	public static List<RoomNode> roomList = new List<RoomNode>();
	public static Dictionary<BSPNode,List<BSPNode>> BSPTreeNodes = new Dictionary<BSPNode,List<BSPNode>>();
	private HashSet<RoomNode> MST = new HashSet<RoomNode>();
	public override async  void _Ready()
	{
		CallDeferred(nameof(GenerateTree));
	}

	public override void _Process(double delta)
	{
		for (var i = 0; i < roomList.Count; i++)
		{
			roomList[i].Draw();
		}

		//for (var i = 0; i < MST.Count; i++)
		//{
		//	if(i + 1 >= MST.Count)break;
		//	var v1 =MST.ElementAt(i).center;
		//	var v2 =MST.ElementAt(i+1).center;
		//	DebugDraw3D.DrawLine(new Vector3(v1.X,0,v1.Y),new Vector3(v2.X,0,v2.Y));
		//}
	}

	private void GenerateTree()
	{
		//BSP 노드를 생성
		roomList = CreateBSPTree(out treeNode);
		
		//방의 위치만 따로 뺴와서 계산할수 있도록 리스트와 연결
		var points = LinkPoints(roomList);

		//생성된 방 기준으로 삼각형들을 생성
		List<Triangle> triangleList =  DelaunayTriangulations(points);

		////Room과 연결된 삼각형들을 대응시켜줌
		LinkRooms(triangleList);
		//
		Prim(roomList);
	}

	List<Vector2> LinkPoints(List<RoomNode> roomList)
	{
		List<Vector2> points = new List<Vector2>();
		foreach (var roomNode in DungeonGenerate.roomList)
		{
			var point = roomNode.center;
			points.Add(point);
		}
		return points;
	}
	

	List<RoomNode> CreateBSPTree(out BSPNode treeNode)
	{
		//BSP 트리를 생성 (재귀)
		var position = new Vector2(0, 0);
		var size = new Vector2(Horizontal, Vertical);
		treeNode = new BSPNode(new Rect2(position,size),minRoomSize);
		
		List<RoomNode> treeNodes = new List<RoomNode>();
		//만약 내 하위로 자식이 아무도 없다면 그노드는 방이다. 
		foreach (var keyValuePair in BSPTreeNodes)
		{
			if (keyValuePair.Value == null)
			{
				if (keyValuePair.Key is RoomNode room)
				{
					treeNodes.Add(room);
				}
			}
		}
		return treeNodes;
	}
	
	
	

	Node3D CreateGroundCollider()
	{
		Node3D top = new Node3D();
		StaticBody3D body = new StaticBody3D();
		CollisionShape3D shape = new CollisionShape3D();
		top.AddChild(body);
		body.AddChild(shape);
		shape.Shape = new BoxShape3D();
		shape.Scale = new Vector3(treeNode.sizeRect.Size.X, 0.1f, treeNode.sizeRect.Size.Y);
		AddChild(top);
		top.GlobalPosition = new Vector3(treeNode.center.X, 0, treeNode.center.Y);
		return top;
	}

	void CreateGround(Vector3 position)
	{
		position.X += 0.5f;
		position.Z += 0.5f;
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
		instance.RotationDegrees = new Vector3(-90,0,0);
		instance.Scale = new Vector3(100,100,100);		
	}

	void CreateWall(Vector2 original, Vector3 position, Mesh item)
	{
		if(original.X != position.X) return;
		
	}

	List<Triangle> DelaunayTriangulations(List<Vector2> points)
	{
		//1.모두를 포함하는 거대한 삼각형을 만든다.(BSP로 가장큰 사각형기준으로 방을 만들었으니 중심점은 treenNode의 중심점) 
		var center = treeNode.center;
		var span = Mathf.Min(treeNode.sizeRect.Size.X, treeNode.sizeRect.Size.Y);
		Vector2 p1 = new Vector2(center.X - 2 * span, center.Y - span);
		Vector2 p2 = new Vector2(center.X + 2 * span, center.Y - span);
		Vector2 p3 = new Vector2(center.X, center.Y + 2 * span);
		Triangle bigTriangle = new Triangle(p1,p2,p3); 
		
		var triangleList = new List<Triangle>();
		triangleList.Add(bigTriangle);
		
		
		for (var i = 0; i < points.Count; i++)
		{
			//현재 만들어진 삼각형들중에 점을 추가했을 때 그안에 포함되는 점이있다면 그 삼각형을 bad리스트에 넣는다.
			List<Triangle> badTriangles = new List<Triangle>();
			for (var t = 0; t < triangleList.Count; t++)
			{
				if (triangleList[t].PointInCircumcircle(points[i]))
				{
					badTriangles.Add(triangleList[t]);
				}
			}
			
			//삼각형의 변들을 조사
			List<Line> polygon = new List<Line>();
			foreach (var badTriangle in badTriangles)
			{	
				//삼각형들에 변을 가져오고 이미 있는 변이면 제거하고 처음나온 변이면 추가
				var lines = badTriangle.GetLines();
				foreach (var line in lines)
				{
					if (polygon.Contains(line))
					{
						polygon.Remove(line);
					}
					else
					{
						polygon.Add(line);
					}
				}
			}
		
			//이삼각형에서 조사할것은 끝났으니 제거한다.
			foreach (var bad in badTriangles) triangleList.Remove(bad);
		
			//가져온 변 기준으로 새로운 삼각형을 생성
			foreach (var edge in polygon)
			{
				Triangle newTriangle = new Triangle(edge.p1, edge.p2, roomList[i].center);
				triangleList.Add(newTriangle);
			}
		}
		
		triangleList.RemoveAll(tri =>
			tri.HasVertex(p1) || tri.HasVertex(p2) || tri.HasVertex(p3)
		);

		return triangleList;
	}

	void LinkRooms(List<Triangle> triangleList)
	{
		//삼각형에 대응하는 룸을 가져오자
		for (var t = 0; t < triangleList.Count; t++)
		{
			RoomNode r1 = null;
			RoomNode r2 = null;
			RoomNode r3 = null;
			
			for (var i = 0; i < roomList.Count; i++)
			{
				if (triangleList[t].p1.DistanceTo(roomList[i].center) <= 0.001f) r1 = roomList[i];
				if (triangleList[t].p2.DistanceTo(roomList[i].center) <= 0.001f) r2 = roomList[i];
				if (triangleList[t].p3.DistanceTo(roomList[i].center) <= 0.001f)  r3 = roomList[i];
				if (r1 != null && r2 != null && r3 != null) break;
			}
			r1.linkRooms.Add(r2);
			r1.linkRooms.Add(r3);
			r2.linkRooms.Add(r1);
			r2.linkRooms.Add(r3);
			r3.linkRooms.Add(r1);
			r3.linkRooms.Add(r2);
		}
	}

	async Task Prim(List<RoomNode> roomList)
	{
		List<(RoomNode from, RoomNode to)> connections = new List<(RoomNode, RoomNode)>();
		MST = new HashSet<RoomNode>();
		MST.Add(roomList[0]);
		//시작 지점을 설정
		
		//모든 노드를 다돌았을 때 종료 
		while (MST.Count != roomList.Count)
		{
			await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
			//해당 노드에 가장 비용이 적은 노드를 가져온다.
			var minLength = float.MaxValue;
			RoomNode nextRoom = null;
			RoomNode startRoom = null;
			foreach (var roomNode in MST)
			{
				for (var i = 0; i < roomNode.linkRooms.Count; i++)
				{
					//만약 이미 지나온 노드라면 건너 뛴다.
					if(MST.Contains(roomNode.linkRooms[i]))continue;
					var candidate = roomNode.linkRooms[i];
					if (candidate == roomNode || MST.Contains(candidate))
						continue;
					
					//가장 비용이 적은 노드를 찾는다.
					var targetLength =roomNode.center.DistanceTo(roomNode.linkRooms[i].center);
					if (targetLength < minLength)
					{
						minLength = targetLength;
						nextRoom = roomNode.linkRooms[i];
						startRoom = roomNode;
					}
				}
			}
			if (nextRoom == null)
			{
				GD.PrintErr("Prim 실패: 연결 가능한 노드가 없습니다.");
				break;
			}
			//비용이 가장 작은 노드를 MST에 다시 넣어줌.
			connections.Add((startRoom,nextRoom));
			MST.Add(nextRoom);
		}
		
		for (var i = 0; i < roomList.Count; i++) roomList[i].linkRooms.Clear();
		
		foreach (var valueTuple in connections)
		{
			var start =valueTuple.from;
			var end = valueTuple.to;
			
			start.linkRooms.Add(end);
			end.linkRooms.Add(start);
		}
	}	
}
