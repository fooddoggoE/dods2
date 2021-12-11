using Sandbox;
using System;

[Library("dod_smokegrenade", Title = "Smoke Grenade")]
partial class SmokeGrenade : BaseDmWeapon
{
    public override string ViewModelPath => "models/weapons/smokegrenade/v_smoke_us.vmdl";

    public override float PrimaryRate => 1.0f;
    public override int ClipSize => 1;
    public override int Bucket => 3;
	public override AmmoType AmmoType => AmmoType.Knife;

    public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/weapons/m1_garand/w_garand.vmdl" );
		AmmoClip = 1;
	}

	public override bool CanPrimaryAttack()
	{
		return base.CanPrimaryAttack() && Input.Pressed( InputButton.Attack1 );
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;

        if (Input.Down(InputButton.Attack1)) 
        {
		    ViewModelEntity?.SetAnimBool( "attack", true );
        }

        if (Input.Released(InputButton.Attack1)) 
        {
            ViewModelEntity?.SetAnimBool("attack", false);
            
            if ( !TakeAmmo( 1 ) )
            {
                DryFire();
                return;
            }
        }

		(Owner as AnimEntity).SetAnimBool( "b_attack", true );

        ViewModelEntity?.SetAnimBool("live", true);

		//
		// Tell the clients to play the shoot effects
		//
		// ShootEffects();
		// PlaySound( "garand_shoot" );

		//
		// Shoot the bullets
		//
		Rand.SetSeed(Time.Tick);
	}
}