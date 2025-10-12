using Godot;
public partial class DebugManager : Node
{
	public void EnterFactory()
	{
		EventManager.Play<CameraFadeOutEvent>();
		EventManager.Play<EnterFactoryEvent>();
		EventManager.Play<CameraFadeInEvent>();
	}
	
	public void OutFactory()
	{
		EventManager.Play<CameraFadeOutEvent>();
		EventManager.Play<OutFactoryEvent>();
		EventManager.Play<CameraFadeInEvent>();
	}
}
