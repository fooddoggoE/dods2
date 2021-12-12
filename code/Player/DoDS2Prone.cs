
namespace Sandbox
{
	[Library]
	public class DoDS2Prone : BaseNetworkable
	{
		public BasePlayerController Controller;

		public bool IsActive; // replicate

		public DoDS2Prone( BasePlayerController controller )
		{
			Controller = controller;
		}

		public virtual void PreTick() 
		{
			bool wants = Input.Down( InputButton.Flashlight );

			var player = Local.Pawn as DeathmatchPlayer;

			// player.SetAnimBool("b_prone");

			if ( wants != IsActive ) 
			{
				if ( wants ) TryProne();
				else TryUnProne();
			}

			if ( IsActive )
			{
				Controller.SetTag( "proned" );
				Controller.EyePosLocal *= 0.25f;
			}
		}

		protected virtual void TryProne()
		{
			IsActive = true;
		}

		protected virtual void TryUnProne()
		{
			var pm = Controller.TraceBBox( Controller.Position, Controller.Position, originalMins, originalMaxs );
			if ( pm.StartedSolid ) return;

			IsActive = false;
		}

		// Uck, saving off the bbox kind of sucks
		// and we should probably be changing the bbox size in PreTick
		Vector3 originalMins;
		Vector3 originalMaxs;

		public virtual void UpdateBBox( ref Vector3 mins, ref Vector3 maxs, float scale )
		{
			originalMins = mins;
			originalMaxs = maxs;

			if ( IsActive )
				maxs = maxs.WithZ( 16 * scale );
		}

		//
		// Coudl we do this in a generic callback too?
		//
		public virtual float GetWishSpeed()
		{
			if ( !IsActive ) return -1;
			return 42.0f;
		}
	}
}
