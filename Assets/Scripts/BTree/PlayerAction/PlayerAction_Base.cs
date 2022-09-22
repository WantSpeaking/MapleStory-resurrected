using ms;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace ms_Unity
{

	[Category ("PlayerAction")]
	public class PlayerAction_Base : ActionTask
	{
		public Player player => ms.Stage.get ().get_player ();
	}
}