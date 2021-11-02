
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;

public class Crosshair : Panel
{
	int fireCounter;
	bool aimingToggle;

	public Crosshair()
	{
		for( int i=0; i<5; i++ )
		{
			var p = Add.Panel( "element" );
			p.AddClass( $"el{i}" );
		}
	}

	public override void Tick()
	{
		base.Tick();
		this.PositionAtCrosshair();

		SetClass( "fire", fireCounter > 0 );

		SetClass("aiming", aimingToggle);

		if ( fireCounter > 0 )
			fireCounter--;
		
        // if (Input.Pressed(InputButton.Attack2))
        // {
        //     switch (aimingToggle)
        //     {
        //         case true:
		// 			aimingToggle = false;
        //             break;

        //         case false:
		// 			aimingToggle = true;
        //             break;
                
        //         default:
        //             Log.Info("You such a sussy");
        //             break;
        //     }
        // }

		if (Input.Down(InputButton.Attack2)) 
		{
			aimingToggle = true;
		}

		if (Input.Released(InputButton.Attack2)) 
		{
			aimingToggle = false;
		}
	}

	[PanelEvent]
	public void FireEvent()
	{
		fireCounter += 2;
	}
}
