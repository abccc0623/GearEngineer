
public partial class SceneChangeEvent : Event
{
    protected override void Start()
    {
        var scenePath = parameterList[0].ToString();
        GetRoot().ChangeSceneToFile(scenePath);
    }

    protected override bool Update(float delta)
    {
        return true;
    }

    protected override void End()
    {
        
    }
}