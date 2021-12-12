using Sandbox;
using System;
using System.Linq;

/// <summary>
/// This is the heart of the gamemode. It's responsible
/// for creating the player and stuff.
/// </summary>
[Library( "dods2", Title = "DoDS2" )]
partial class DoDS2 : Game
{
	[ServerCmd("switchview")]
	public static void SwitchView()
	{
		if ( ConsoleSystem.Caller.Pawn is not DeathmatchPlayer p ) return;

		if ( p.Camera is not FirstPersonCamera )
		{
			p.Camera = new FirstPersonCamera();
		}
		else
		{
			p.Camera = new ThirdPersonCamera();
		}
	}

	public DoDS2()
	{
		//
		// Create the HUD entity. This is always broadcast to all clients
		// and will create the UI panels clientside. It's accessible 
		// globally via Hud.Current, so we don't need to store it.
		//
		if ( IsServer )
		{
			new DeathmatchHud();
		}

		
	}

	public override void PostLevelLoaded()
	{
		base.PostLevelLoaded();

		ItemRespawn.Init();
	}

	public override void ClientJoined( Client cl )
	{
		base.ClientJoined( cl );

		var player = new DeathmatchPlayer();
		player.Respawn();

		cl.Pawn = player;
	}
}
