
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public class Vitals : Panel
{
	public Label Health;

	public Vitals()
	{
		Health = Add.Label( "100", "health" );
		// Indicator = Add.Panel( "indicator" );
	}

	public override void Tick()
	{
		if (Local.Pawn is DeathmatchPlayer player) 
		{
			var health = player.Health;

			// Indicator.Style.Set("height", $"{percentage * 100}%");
		}
	}
}
