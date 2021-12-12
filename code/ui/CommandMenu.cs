using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public class CommandMenu : Panel
{
	public static bool open;

	public CommandMenu()
	{
		StyleSheet.Load( "/ui/CommandMenu.scss" );

		Add.Label( "Voice Command Menu", "header" );

		AddChild<CommandMenuButtons>();
	}

	public override void Tick()
	{
		SetClass("open", Input.Down(InputButton.View));
	}
}

public class CommandMenuButtons : Panel
{
	public CommandMenuButtons()
	{
		var player = Local.Pawn as DeathmatchPlayer;

		Add.Label( "Area Clear", "button" ).AddEventListener( "onclick", () => {
			ConsoleSystem.Run("areaclear");
		} );

		Add.Label( "Area Secure", "button" ).AddEventListener( "onclick", () => {
			ConsoleSystem.Run("areasecure");
		} );

		Add.Label("Attack", "button").AddEventListener("onclick", () => {
			ConsoleSystem.Run("attack");
		});

		Add.Label( "Flank Left", "button" ).AddEventListener( "onclick", () => {
			ConsoleSystem.Run("flankleft");
		} );

		Add.Label( "Flank Right", "button" ).AddEventListener( "onclick", () => {
			ConsoleSystem.Run("flankright");
		} );

		Add.Label("Main Console Commands", "section");

		Add.Label("Thirdperson", "button").AddEventListener("onclick", () => {
			ConsoleSystem.Run("switchview");
		});
	}
}
