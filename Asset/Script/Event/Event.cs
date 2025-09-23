using Godot;

public  abstract partial class Event : RefCounted
{
    private bool isStartFunctionPlay = false;
    private bool isUpdateFunctionEnd = false;
    private EventManager manager;


    public void InnerStart(EventManager manager)
    {
        this.manager = manager;
        if (isStartFunctionPlay == false)
        {
            isStartFunctionPlay = true;
            Start();
        }
    }
    public void InnerUpdate(float delta)
    {
        if (isStartFunctionPlay == true)   isUpdateFunctionEnd = Update(delta);
    }
    public bool InnerEnd()
    {
        var eventEnd = isUpdateFunctionEnd;
        if (eventEnd)
        {
            End();
        }
        return eventEnd;
    }
    
    protected abstract void Start();
    protected abstract bool Update(float delta);
    protected abstract void End();

    protected Node GetNode(string path)
    {
        return manager.GetNode(path);
    }
    protected T GetNode<T>(string path) where T : Node
    {
        return manager.GetNode<T>(path);
    }
}

