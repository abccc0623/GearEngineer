using Godot;
using System.Text.Json;

public partial class FactoryManager : Node
{
    [Export] public Vector2 startPosition = Vector2.Zero;
    [Export] public int horizontal = 0;
    [Export] public int vertical = 0;
    //[Export] private PackedScene floorObject = null;
    [Export] private PackedScene wall = null;
    [Export] private PackedScene wallOuter = null;
    
    [Export] private PackedScene outPutObject = null;
    [Export] public string saveFilePath = "user://save.json";
    
    public void Save()
    {
        string json = JsonSerializer.Serialize(saveFilePath);
        using var file = FileAccess.Open(saveFilePath, FileAccess.ModeFlags.Write);
        file.StoreString(json);
    }

    public void Load()
    {
        if (!FileAccess.FileExists(saveFilePath)) return;

        using var file = FileAccess.Open(saveFilePath, FileAccess.ModeFlags.Read);
        string json = file.GetAsText();

        //saveData = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json);
        //GD.Print("로드 완료: ", json);
    }
    public override void _Ready()
    {
        
    }
    
    public void OnButtonPressed()
    {
        foreach (Node child in GetChildren())   child.QueueFree(); 
        MeshInstance3D floor = new MeshInstance3D();
        floor.Position = new Vector3((horizontal *-0.5f)+0.5f,-1,(vertical *-0.5f)+0.5f);
        floor.Mesh = new QuadMesh();
        floor.Scale = new Vector3(horizontal, vertical, 1);
        floor.RotationDegrees = new Vector3(-90,0,0);
        AddChild(floor);
        
        for (int i = 0; i < horizontal; i++)
        {
            for (int j = 0;j < vertical; j++)
            {
                var createPosition = new Vector3(-i,0,-j);
                if (j == 1 && i == 0)
                {
                    var target = outPutObject.Instantiate<Node3D>();
                    target.Position = createPosition;
                    target.RotationDegrees = new Vector3(0, 90, 0);
                    AddChild(target);
                }
                else if (j == horizontal - 1 && i == 0)
                {
                    //왼쪽 위
                    var target = wallOuter.Instantiate<Node3D>();
                    target.RotationDegrees = new Vector3(0, 90, 0);
                    target.Position = createPosition;
                    AddChild(target);
                }
                else if (j == horizontal - 1 && i == vertical-1)
                {
                    //오른쪽 위
                    var target = wallOuter.Instantiate<Node3D>();
                    target.RotationDegrees = new Vector3(0, 180, 0);
                    target.Position = createPosition;
                    AddChild(target);
                }
                else if (i == 0)
                {
                    var target = wall.Instantiate<Node3D>();
                    target.RotationDegrees = new Vector3(0, 90, 0);
                    target.Position = createPosition;
                    AddChild(target);
                }
                else if (j == vertical - 1)
                {
                    var target = wall.Instantiate<Node3D>();
                    target.RotationDegrees = new Vector3(0, 180, 0);
                    target.Position = createPosition;
                    AddChild(target);
                }
            }
        }
    }
}
