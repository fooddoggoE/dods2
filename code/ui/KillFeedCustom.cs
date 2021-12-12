
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Threading.Tasks;

namespace Sandbox.UI
{
	public partial class KillFeedCustom : Sandbox.UI.KillFeed
	{
		public KillFeedCustom()
		{
			StyleSheet.Load( "/ui/killfeed/KillFeedCustom.scss" );
		}

		public override Panel AddEntry( long lsteamid, string left, long rsteamid, string right, string method )
		{
			Log.Info( $"{left} killed {right} using {method}" );

			var e = Current.AddChild<KillFeedCustomEntry>();

			e.AddClass( method );

			e.Left.Text = left;
			e.Left.SetClass( "me", lsteamid == Local.PlayerId );

			e.Right.Text = right;
			e.Right.SetClass( "me", rsteamid == Local.PlayerId );

			return e;
		}
	}
}
