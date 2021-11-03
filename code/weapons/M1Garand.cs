using Sandbox;
using System;

[Library( "dod_garand", Title = "M1 Garand" )]
[Hammer.EditorModel( "weapons/m1_garand/w_garand.vmdl" )]
partial class Garand : BaseDmWeapon
{ 
	public override string ViewModelPath => "models/weapons/m1_garand/v_garand.vmdl";

	public override float PrimaryRate => 7.0f;
	public override float SecondaryRate => 1.0f;
	public override int ClipSize => 10;
	public override float ReloadTime => 1.6f;
	public override int Bucket => 2;

    bool Aiming;

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

		if (!Aiming)
			ShootBullet( 0.1f, 1.5f, 15.0f, 3.0f );

		if (Aiming) 
			ShootBullet(0.025f, 1.5f, 15, 3);
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

		// if (Input.Pressed(InputButton.Flashlight)) 
		// {
		// 	PlaySound("us_grenadein");
		// }

        if (AmmoClip == 0) 
        {
            ViewModelEntity?.SetAnimBool("empty", true);
        }

		if (AmmoClip != 0) 
		{
			ViewModelEntity?.SetAnimBool("empty", false);
		}


		// Shitty old thing that barely worked, was toggle but difficult to make

		// if (Input.Pressed(InputButton.Attack2)) 
		// {
		// 	Aiming = !Aiming;
		// }

		// if (Input.Pressed(InputButton.Attack2) && AimingCounter <= 1) 
		// {
		// 	ViewModelEntity?.SetAnimBool("aiming_enter", true);
		// 	ViewModelEntity?.SetAnimBool("aiming", true);

		// 	AimingCounter++;

		// 	// Log.Info(AimingCounter);
		// }

		// if (Input.Pressed(InputButton.Attack2) && AimingCounter > 1) 
		// {
		// 	ViewModelEntity?.SetAnimBool("aiming", false);

		// 	AimingCounter--;
		// 	AimingCounter--;
		// }

		if (Input.Down(InputButton.Attack2) && !Aiming) 
		{
			Aiming = true;
			ViewModelEntity?.SetAnimBool("aiming_enter", true);
			ViewModelEntity?.SetAnimBool("aiming", true);
		}

		if (Input.Released(InputButton.Attack2) && Aiming) 
		{
			Aiming = false;
			ViewModelEntity?.SetAnimBool("aiming", false);
		}

		(Owner as AnimEntity).SetAnimBool("b_aim", Aiming);

		if (AmmoClip == 0 && Input.Pressed(InputButton.Attack1)) 
		{
			PlaySound("weaponempty");
		}
	}

	[ClientRpc]
	public void CannotReloadText() 
	{
		if (Input.Pressed(InputButton.Reload) && AmmoClip != 0) 
		{
			new CannotReload();
		}	
	}

	public override void PostCameraSetup( ref CameraSetup camSetup )
	{
		base.PostCameraSetup( ref camSetup );

		if ( Aiming )
		{
			camSetup.FieldOfView = 35;
		}
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

	public override void BuildInput( InputBuilder owner ) 
	{
		if ( Aiming )
		{
			owner.ViewAngles = Angles.Lerp( owner.OriginalViewAngles, owner.ViewAngles, 0.2f );
		}
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetParam( "holdtype", 1 ); // TODO this is shit
		anim.SetParam( "aimat_weight", 1.0f );
	}

}
