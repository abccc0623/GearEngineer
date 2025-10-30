using System.Collections.Generic;
using Godot;

namespace GearEngineer.GearEngineer.Asset.Script.CreateMap;

public partial class RoomNode : BSPNode
{
    public List<RoomNode> linkRooms = new List<RoomNode>();
    public RoomNode(Rect2 rect, int minRoomSize) : base(rect, minRoomSize) {}

    public override void Draw()
    {
        base.Draw();
        
        //나와 인접한 노드를 연결
        for (var i = 0; i < linkRooms.Count; i++)
        {
            DebugDraw3D.DrawLine(new Vector3(center.X, 0, center.Y), new Vector3(linkRooms[i].center.X, 0, linkRooms[i].center.Y), new Color(0,1,0));
        }
    }

   
}