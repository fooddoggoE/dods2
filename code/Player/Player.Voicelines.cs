using Sandbox;
using System;
using System.Linq;

partial class DeathmatchPlayer
{
    [ClientRpc]
	public void PlaySoundFromEntity(string name, Entity entity)
	{
		Sound.FromEntity(name, entity);
	}

    [ServerCmd("areaclear")]
	public static void AreaClear()
	{
		var player = ConsoleSystem.Caller.Pawn;
		Sound.FromEntity("us_areaclear", player);
	}

	[ServerCmd("areasecure")]
	public static void AreaSecure()
	{
		var player = ConsoleSystem.Caller.Pawn;
		Sound.FromEntity("us_areasecure", player);
	}

	[ServerCmd("attack")]
	public static void Attack()
	{
		var player = ConsoleSystem.Caller.Pawn;
		Sound.FromEntity("us_attack", player);
	}

	[ServerCmd("flankleft")]
	public static void FlankLeft()
	{
		var player = ConsoleSystem.Caller.Pawn;
		Sound.FromEntity("us_flankleft", player);
	}

	[ServerCmd("flankright")]
	public static void FlankRight()
	{
		var player = ConsoleSystem.Caller.Pawn;
		Sound.FromEntity("us_flankright", player);
	}
}