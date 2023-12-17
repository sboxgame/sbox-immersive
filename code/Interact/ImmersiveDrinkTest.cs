using Sandbox;

[Title( "Interact - Drink" )]	
[Category( "ImSim" )]
[Icon( "check_box_outline_blank", "red", "white" )]
public class ImmersiveDrinkTest : BaseInteract
{
	public override InteractionType Interaction => InteractionType.Use;
	public override bool IsPhysicsInteract => false;

	public override void OnUse()
	{
		Interacter.Components.Create<ImSimDrunkDebuff>();
		Interacter.Components.Create<ImSimBlindDebuff>();
	}

	public override void OnHoldUse()
	{
		base.OnHoldUse();

		var drink = Interacter.Components.Get<ImmersivePlayerStats>();
		if ( drink.CurrentPlayerStats[ImmersivePlayerStats.PlayerStats.Thirst] <= 1 )
		{
			drink.IncrementStat( ImmersivePlayerStats.PlayerStats.Thirst, 0.1f );
			drink.DecrementStat( ImmersivePlayerStats.PlayerStats.Bladder, 0.1f );
		}
		else
		{
		//drink.PlayerDrinks();
			//Log.Info( drink.CurrentPlayerStats[ImmersivePlayerStats.PlayerStats.Thirst] );
		}
	}

	protected override void OnUpdate()
	{

	}
}
