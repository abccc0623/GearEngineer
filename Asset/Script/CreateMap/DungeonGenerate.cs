using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
	
	
	public override void _Ready()
	{
		CallDeferred(nameof(GenerateTree));
	}

	public override void _Process(double delta)
	{
		for (var i = 0; i < roomList.Count; i++)
		{
			roomList[i].Draw();
		}

		DelaunayTriangulations();
	}

	private void GenerateTree()
	{
		//BSP 노드를 생성
		CreateBSPTree();
		//생성된 노드에서 인접한 노드를 서로 연결
		LinkNode();

		
		//CreateGroundCollider();

		//생성된 방 기준으로 땅 콜라이더를 생성함
		//LinkNodeGround();
	}

	void CreateBSPTree()
	{
		//BSP 트리를 생성 (재귀)
		var position = new Vector2(0, 0);
		var size = new Vector2(Horizontal, Vertical);
		treeNode = new BSPNode(new Rect2(position,size),minRoomSize);
		
		//만약 내 하위로 자식이 아무도 없다면 그노드는 방이다. 
		foreach (var keyValuePair in BSPTreeNodes)
		{
			if (keyValuePair.Value == null)
			{
				if (keyValuePair.Key is RoomNode room)
				{
					roomList.Add(room);
				}
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

	void DelaunayTriangulations()
	{
		//1.모두를 포함하는 거대한 삼각형을 만든다.(BSP로 가장큰 사각형기준으로 방을 만들었으니 중심점은 treenNode의 중심점) 
		var center = treeNode.center;
		var span = Mathf.Min(treeNode.sizeRect.Size.X, treeNode.sizeRect.Size.Y);
		Vector2 p1 = new Vector2(center.X - 2 * span, center.Y - span);
		Vector2 p2 = new Vector2(center.X + 2 * span, center.Y - span);
		Vector2 p3 = new Vector2(center.X, center.Y + 2 * span);
		
		List<Vector3> point = new List<Vector3>();
		point.Add(new Vector3(p1.X,0,p1.Y));
		point.Add(new Vector3(p2.X,0,p2.Y));
		point.Add(new Vector3(p3.X,0,p3.Y));
		
		DebugDraw3D.DrawLine(point[0],point[1],new Color(0,1,0));
		DebugDraw3D.DrawLine(point[1],point[2],new Color(0,1,0));
		DebugDraw3D.DrawLine(point[2],point[0],new Color(0,1,0));
	}
	
	
	
	
	void LinkNode()
	{
		for (var i = 0; i < roomList.Count; i++)
		{
			var thisNode = roomList[i];
			for (var j = 0; j < roomList.Count; j++)
			{
				//나와 인접한 노드인지 체크
				var targetNode = roomList[j];
				if (thisNode.AreRectsEdgeAdjacent(targetNode.sizeRect,8))
				{
					thisNode.linkRooms.Add(targetNode);
				}
			}
		}
	}
	/*

	void LinkNodeGround()
	{
		for (var i = 0; i < BSPNode.roomList.Count; i++)
		{
			if(BSPNode.roomList[i].isClose == true) continue;
			
			//CreateCollider(BSPNode.roomList[i]);
		}
	}

	void BSP()
	{
		Queue<BSPNode> queue = new Queue<BSPNode>();
		List<BSPNode> close = new List<BSPNode>();
		BSPNode end = FindFarthestRoom(BSPNode.roomList[0]);
		close.Add(BSPNode.roomList[0]);
		queue.Enqueue(BSPNode.roomList[0]);
		while (queue.Count > 0)
		{
			var target = queue.Dequeue();
			foreach (var bspNode in target.childList)
			{
				var find = close.Find(node => node == target);
				if (find == null) queue.Enqueue(bspNode);
			}
		}
	}

	BSPNode FindFarthestRoom(BSPNode start)
	{
		BSPNode endNode = null;
		float maxDistance = float.MinValue;
		Vector3 startPosition = new Vector3(start.position.X, 0, start.position.Y);
		for (var i = 0; i < BSPNode.roomList.Count; i++)
		{
			BSPNode targetNode = BSPNode.roomList[i];
			Vector3 target = new Vector3(targetNode.position.X, 0, targetNode.position.Y);
			var direction = startPosition.DistanceTo(target);
			if (direction > maxDistance)
			{
				maxDistance = direction;
				endNode = targetNode;
			}
		}
		return endNode;
	}
	*/
}
