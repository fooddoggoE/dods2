using Sandbox;
using System;
using System.Linq;

namespace DOD 
{
	public partial class DODGame : Game 
	{
		public DODGame() 
		{

		}

		public override void ClientJoined(Client cl) 
		{
			base.ClientJoined(cl);

			var pawn = new DODPlayer();
			pawn.Respawn();

			cl.Pawn = pawn;
		}
	}
}
