using Sandbox;

namespace DOD 
{
	public partial class DODPlayer : Player 
	{
		public override void Respawn() 
		{
			base.Respawn();

			SetModel("models/american_soldier.vmdl");

			CameraMode = new ThirdPersonCamera();
			Animator = new StandardPlayerAnimator();
			Controller = new WalkController();
		}
	}
}
