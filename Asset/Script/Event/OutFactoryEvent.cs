using Godot;
public partial class OutFactoryEvent : Event
{
    private static FactoryUI factoryUI = null;
    private static FactoryManager factoryManager = null;
    private static Camera3D camera;
    
    protected override void Start()
    {
        if (camera == null)
        {
            camera = GetNode<Camera3D>("/root/Node3D/PlayerCamera");
        }
        if (factoryManager == null)
        {
            factoryManager = GetNode<FactoryManager>("/root/Node3D/MapGenerator");
        }
        if (factoryUI == null)
        {
            factoryUI = factoryManager.GetNode<FactoryUI>("FactoryUI/Control");
        }
        camera.Current = true;
        factoryManager.Hide();
        factoryUI.Hide();
    }

    protected override bool Update(float delta)
    {
        return true;
    }

    protected override void End()
    {
    }
}