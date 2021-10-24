
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;

[Library]
public partial class CannotReload : Panel
{
	public static CannotReload Instance;

	public CannotReload()
	{
		Instance = this;

		StyleSheet.Load( "/ui/SpawnMenu.scss" );
	}

	public override void Tick()
	{
		base.Tick();

		Parent.SetClass( "spawnmenuopen", Input.Down( InputButton.Menu ) );
	}

	public override void OnHotloaded()
	{
		base.OnHotloaded();
	}
}
