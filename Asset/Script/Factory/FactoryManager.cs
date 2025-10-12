using System;
using System.Collections.Generic;
using Godot;
using System.Text.Json;

public partial class FactoryManager : Node3D
{
    //private const string assetsPath = "res://GearEngineer/Asset/Prefab/Factory/";
    //private Dictionary<string, PackedScene> assets = new Dictionary<string, PackedScene>();
    //private Camera3D camera;
    //private bool isFactoryMode = false;
    //private Node3D test;
    //
    //[Export] public int horizontal = 10;
    //[Export] public int vertical = 10;
    
    public void Save()
    {
        //string json = JsonSerializer.Serialize(saveFilePath);
        //using var file = FileAccess.Open(saveFilePath, FileAccess.ModeFlags.Write);
        //file.StoreString(json);
    }

    public void Load()
    {

    }

    //public override void _Ready()
    //{
    //    camera = GetNode<Camera3D>("FactoryCamera");
    //    test = GetAssetsInstantiate("conveyor");
    //    AddChild(test);
    //}
    //PackedScene GetAssets(string fileName)
    //{
    //    if (assets.TryGetValue(fileName, out PackedScene packedScene))
    //    {
    //        return packedScene;
    //    }
    //    else
    //    {
    //        packedScene = GD.Load<PackedScene>(assetsPath + fileName+"/.tscn");
    //        if(packedScene == null){GD.Print($"해당 에셋이 없습니다.{fileName}");}
    //        assets.Add(fileName, packedScene);
    //        return packedScene;
    //    }
    //}
    //
    //Node3D GetAssetsInstantiate(string fileName)
    //{
    //    if (assets.TryGetValue(fileName, out PackedScene packedScene))
    //    {
    //        var target = packedScene.Instantiate<Node3D>();
    //        return target;
    //    }
    //    else
    //    {
    //        packedScene = GD.Load<PackedScene>(assetsPath + fileName+".tscn");
    //        if(packedScene == null){GD.Print($"해당 에셋이 없습니다.{fileName}");}
    //        assets.Add(fileName, packedScene);
    //        var target = packedScene.Instantiate<Node3D>();
    //        return target;
    //    }
    //}
    //
    //public override void _Process(double delta)
    //{
    //    if (isFactoryMode == true)
    //    {
    //        var position = GetMouseWorldPosition(camera);
    //        if (position.HasValue)
    //        {
    //            var local = this.ToLocal(position.Value);
    //            if(local.X < 0) local.X = 0;
    //            else if(local.X >= horizontal - 1) local.X = horizontal - 1;
    //            
    //            if(local.Z < 0) local.Z = 0;
    //            else if(local.Z >= vertical - 1) local.Z = vertical - 1;
    //            
    //            local =new Vector3(Mathf.Round(local.X), Mathf.Round(local.Y), Mathf.Round(local.Z));
    //            test.Position = local;
    //
    //            if (Input.IsMouseButtonPressed(MouseButton.Left))
    //            {
    //                
    //            }
    //        }
    //       
    //    }
    //}
    //
    //public void Generator()
    //{
    //    Node3D cameraTarget = null;
    //    for (int i = -1; i < horizontal; i++)
    //    {
    //        for (int j = -1; j < vertical; j++)
    //        {
    //            if (i == -1 || i == horizontal-1 || j == -1 || j == vertical-1)
    //            {
    //                //위 수평, 왼쪽 수직, 두 수평과 수직이 만나는 모서리를 체크 
    //                if (i == horizontal-1 && j == vertical-1) cameraTarget = CreateEdgeWall(i, j);
    //                else if (i == horizontal-1 &&  j != -1)  CreateHorizontalWall(i, j);
    //                else if (j == vertical-1   &&  i != -1)  CreateVerticalWall(i, j);
    //            }
    //            else
    //            {
    //                var target =GetAssetsInstantiate("ground");
    //                target.Position = new Vector3(i, 0, j);
    //                AddChild(target);
    //            }
    //        }
    //    }
    //    camera.Size = 20;
    //    camera.Position = new Vector3(0, camera.Size *0.5f, 0);
    //    camera.RotationDegrees = new Vector3(-30, -135, 0);
    //    camera.Current = true;
    //    isFactoryMode = true;
    //}
    //
    //Node3D CreateVerticalWall(int h,int v)
    //{
    //    var target =GetAssetsInstantiate("wall");
    //    target.Position = new Vector3(h, 0, v);
    //    AddChild(target);
    //    target.RotationDegrees = new Vector3(0, 0, 0);
    //    return target;
    //}
    //Node3D CreateHorizontalWall(int h,int v)
    //{
    //    var target =GetAssetsInstantiate("wall");
    //    target.Position = new Vector3(h, 0, v);
    //    AddChild(target);
    //    target.RotationDegrees = new Vector3(0, 90, 0);
    //    return target;
    //}
    //Node3D CreateEdgeWall(int h,int v)
    //{
    //    var target =GetAssetsInstantiate("wall_inner");
    //    target.Position = new Vector3(h, 0, v);
    //    AddChild(target);
    //    target.RotationDegrees = new Vector3(0, 0, 0);
    //    return target;
    //}
    //
    //public Vector3? GetMouseWorldPosition(Camera3D camera)
    //{
    //    var viewport = GetViewport();
    //    var mousePos = viewport.GetMousePosition();
    //
    //    // 레이 시작점과 방향 얻기
    //    Vector3 rayOrigin = camera.ProjectRayOrigin(mousePos);
    //    Vector3 rayDir = camera.ProjectRayNormal(mousePos);
    //
    //    // 평면: y = 0
    //    float t = -rayOrigin.Y / rayDir.Y;
    //
    //    if (t < 0)
    //        return null; // 평면과 교차하지 않음 (위쪽만 보는 경우)
    //
    //    Vector3 hitPos = rayOrigin + rayDir * t;
    //    return hitPos;
    //}
    //
    //public void OnButtonPressed()
    //{
    //    Generator();
    //}
}
