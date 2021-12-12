
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Threading.Tasks;

namespace Sandbox.UI
{
	public partial class KillFeedCustomEntry : Panel
	{
		public Label Left { get; internal set; }
		public Label Right { get; internal set; }
		public Panel Icon { get; internal set; }

		public RealTimeSince TimeSinceBorn = 0;

		public KillFeedCustomEntry()
		{
			Left = Add.Label( "", "left" );
			Icon = Add.Panel( "icon" );
			Right = Add.Label( "", "right" );
		}

		public override void Tick() 
		{
			base.Tick();

			if ( TimeSinceBorn > 6 ) 
			{ 
				Delete();
			}
		}

	}
}
