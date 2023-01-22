using bloodlines.entities.vampire.Data;
using Sandbox;

public interface ISignData
{
	public SignData SignData { get; set; }

	public void InitializeHUD( Entity activator );
}
