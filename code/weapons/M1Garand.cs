using Sandbox;
using System;

[Library( "dod_garand", Title = "M1 Garand" )]
[Hammer.EditorModel( "weapons/m1_garand/v_garand.vmdl" )]
partial class Garand : BaseDmWeapon
{ 
	public override string ViewModelPath => "models/weapons/m1_garand/v_garand.vmdl";

	public override float PrimaryRate => 7.0f;
	public override float SecondaryRate => 1.0f;
	public override int ClipSize => 10;
	public override float ReloadTime => 1.6f;
	public override int Bucket => 2;

    [Net]
    public bool Aiming {get; set;}

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/weapons/m1_garand/w_garand.vmdl" );
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
		ShootBullet( 0.1f, 1.5f, 15.0f, 3.0f );

	}

    public override bool CanReload() 
    {
        if (AmmoClip == 0 && Input.Pressed(InputButton.Reload)) 
        {
            return true;
        }

		if (AmmoClip != 0 && Input.Pressed(InputButton.Reload)) 
		{
			Log.Info($"lmao, {Client} is a dumb dumb, tried to reload with more than 0 ammo in his clip, he still has {AmmoClip} bullet(s) left!!!");
		}

		return false;
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

		if (Input.Pressed(InputButton.Attack2)) 
		{
			Aiming = !Aiming;
		}

		(Owner as AnimEntity).SetAnimBool("b_aim", Aiming);

		if (AmmoClip == 0 && Input.Pressed(InputButton.Attack1)) 
		{
			PlaySound("weaponempty");
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

		Particles.Create( "particles/muzzleflash.vpcf", EffectEntity, "muzzle" );
		Particles.Create( "particles/large_bullet_casing.vpcf", EffectEntity, "ejection_point" );

		if ( Owner == Local.Pawn )
		{
			new Sandbox.ScreenShake.Perlin(0.5f, 4.0f, 1.0f, 0.5f);
		}

		CrosshairPanel?.CreateEvent( "fire" );
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetParam( "holdtype", 1 ); // TODO this is shit
		anim.SetParam( "aimat_weight", 1.0f );
	}

}
