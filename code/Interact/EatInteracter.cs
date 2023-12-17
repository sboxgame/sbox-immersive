using Sandbox;
using Sandbox.Diagnostics;
using System.Collections.Generic;

[Title( "Interact - Eat" )]
[Category( "ImSim" )]
[Icon( "check_box_outline_blank", "red", "white" )]
public class EatInteracter : BaseInteract
{
	public override InteractionType Interaction => InteractionType.Hold;

	public bool IsEating { get; set; } = false;

	[Property]
	public int numberofbites { get; set; } = 5;

	public override void OnHoldUse()
	{
		base.OnHoldUse();

		var usecomp = Interacter.Components.Get<ImmersiveUse>(FindMode.EnabledInSelf);
		var distance = usecomp.distance;

		if ( distance <= 25 )
		{
			if ( !IsEating )
			{
				IsEating = true;
				numberofbites -= 1;
				//Sound.FromWorld( "Error", GameObject.Transform.Position );
				Log.Info( "Eating" );

				GameObject.Transform.Scale = GameObject.Transform.Scale * numberofbites / 5;

				var stats = Interacter.Components.Get<ImmersivePlayerStats>();
				stats.IncrementStat( ImmersivePlayerStats.PlayerStats.Hunger, 5.0f );

			}
			if ( numberofbites <= 0 )
			{
				usecomp.currentlyCarriedObject = null;
				usecomp.isHolding = false;
				GameObject.Destroy();
				Log.Info( "Finished Eating" );
			}
		}
	}

	public override void OnHoldUseEnd()
	{
		base.OnHoldUseEnd();

		IsEating = false;
	}


	protected override void OnUpdate()
	{
		base.OnUpdate();

	}


}
