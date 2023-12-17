using Sandbox;

[Title( "Interact - Toilet" )]
[Category( "ImSim" )]
[Icon( "check_box_outline_blank", "red", "white" )]
public class immersiveInteractToilet : BaseInteract
{
	public override bool IsPhysicsInteract => false;


	public override void OnHoldUse()
	{
		base.OnHoldUse();

		var peetime = Interacter.Components.Get<ImmersivePlayerStats>();
		if ( peetime.CurrentPlayerStats[ImmersivePlayerStats.PlayerStats.Bladder] <= 1 )
		{
			peetime.IncrementStat( ImmersivePlayerStats.PlayerStats.Bladder, 0.1f );


		}
	}

	protected override void OnUpdate()
	{

	}
}
