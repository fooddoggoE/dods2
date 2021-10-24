using Sandbox;
using System;

[Library( "dod_garand", Title = "M1 Garand" )]
[Hammer.EditorModel( "weapons/M1_Garand/v_garand.vmdl" )]
partial class Garand : BaseDmWeapon
{ 
	public override string ViewModelPath => "models/weapons/m1_garand/v_garand.vmdl";

	public override float PrimaryRate => 7.0f;
	public override float SecondaryRate => 1.0f;
	public override int ClipSize => 10;
	public override float ReloadTime => 1.6f;
	public override int Bucket => 2;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "weapons/rust_smg/rust_smg.vmdl" );
		AmmoClip = 10;
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

		(Owner as AnimEntity).SetAnimBool( "b_attack", true );

		ViewModelEntity?.SetAnimBool( "attack", true );

        if (AmmoClip == 0) 
        {
            ViewModelEntity?.SetAnimBool("empty", true);
        }

        else 
        {
            ViewModelEntity?.SetAnimBool("empty", false);
        }

		//
		// Tell the clients to play the shoot effects
		//
		ShootEffects();
		PlaySound( "garand_shoot" );

		//
		// Shoot the bullets
		//
		Rand.SetSeed(Time.Tick);
		ShootBullet( 0.1f, 1.5f, 5.0f, 3.0f );

	}

	public override void Simulate( Client owner ) 
	{
		if ( TimeSinceDeployed < 0.6f )
			return;

		if ( !IsReloading )
		{
			base.Simulate( owner );
		}

		if ( IsReloading && TimeSinceReload > ReloadTime )
		{
			OnReloadFinish();
		}

        if (AmmoClip == 0) 
        {
            ViewModelEntity?.SetAnimBool("empty", true);
        }
	}

	public override void AttackSecondary()
	{
		// Grenade lob
	}

	[ClientRpc]
	protected override void ShootEffects()
	{
		Host.AssertClient();

		Particles.Create( "particles/pistol_muzzleflash.vpcf", EffectEntity, "muzzle" );
		Particles.Create( "particles/pistol_ejectbrass.vpcf", EffectEntity, "ejection_point" );

		if ( Owner == Local.Pawn )
		{
			new Sandbox.ScreenShake.Perlin(0.5f, 4.0f, 1.0f, 0.5f);
		}

		CrosshairPanel?.CreateEvent( "fire" );
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetParam( "holdtype", 2 ); // TODO this is shit
		anim.SetParam( "aimat_weight", 1.0f );
	}

}
