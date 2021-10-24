﻿using Sandbox;


[Library( "dm_pistol", Title = "Pistol" )]
[Hammer.EditorModel( "weapons/rust_pistol/rust_pistol.vmdl" )]
partial class Pistol : BaseDmWeapon
{ 
	public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";

	public override float PrimaryRate => 15.0f;
	public override float SecondaryRate => 1.0f;
	public override float ReloadTime => 3.0f;

	public override int Bucket => 1;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "weapons/rust_pistol/rust_pistol.vmdl" );
		AmmoClip = 12;
	}

	public override bool CanPrimaryAttack()
	{
		return base.CanPrimaryAttack() && Input.Pressed( InputButton.Attack1 );
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		if ( !TakeAmmo( 1 ) )
		{
			DryFire();
			return;
		}

		//
		// Tell the clients to play the shoot effects
		//
		ShootEffects();
		PlaySound( "rust_pistol.shoot" );

		//
		// Shoot the bullets
		//
		//Rand.SetSeed( Time.Tick );
		ShootBullet( 0.2f, 1.5f, 9.0f, 3.0f );

	}

	public override bool CanReload() 
	{
		// Log.Info(AmmoClip);

		if (AmmoClip == 0 && Input.Pressed(InputButton.Reload)) 
		{
			return true;
		}

		if (AmmoClip != 0 && Input.Pressed(InputButton.Reload)) 
		{
			Sandbox.UI.ChatBox.Say("You cannot reload your Pistol unless it's at 0 ammo remaining or some shit idk");
		}

		return false;
	}
}