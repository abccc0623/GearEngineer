
using Godot;

public partial class EnterFactoryEvent : Event
{
    private static FactoryUI factoryUI = null;
    private static FactoryManager factoryManager = null;
    private static Camera3D camera;
    protected override void Start()
    {
        //if (factoryManager == null)
        //{
        //    factoryManager = GetNode<FactoryManager>("/root/Node3D/MapGenerator");
        //    factoryManager.Generator();
        //}
        //if (camera == null)
        //{
        //    camera = factoryManager.GetNode<Camera3D>("FactoryCamera");
        //}
        //if (factoryUI == null)
        //{
        //    factoryUI = factoryManager.GetNode<FactoryUI>("FactoryUI/Control");
        //}
        //factoryUI.Show();
        //factoryManager.Show();
        //camera.Current = true;
    }

    protected override bool Update(float delta)
    {
        return true;
    }

    protected override void End()
    {
        
    }
}