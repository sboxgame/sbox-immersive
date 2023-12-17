using Sandbox;

[Title( "Interact - Bed" )]
[Category( "ImSim" )]
[Icon( "check_box_outline_blank", "red", "white" )]
public class ImmersiveBedTest : BaseInteract
{
	public override InteractionType Interaction => InteractionType.Use;
	
	[Property]
	GameObject HeadLocation { get; set; }

	CameraComponent playerHead { get; set; }

	GameObject EyePos { get; set; }
	public override bool IsPhysicsInteract => false;

	bool playersHeadIsOnBed = false;

	Vector3 lastHeadPos = Vector3.Zero;

	public override void OnUse()
	{
		base.OnUse();

		if ( !playersHeadIsOnBed )
		{
			Log.Info( "You are now sleeping" );
			lastHeadPos = EyePos.Transform.Position; // Save last position
			EyePos.Transform.Position = HeadLocation.Transform.Position;
			EyePos.Transform.Rotation = HeadLocation.Transform.Rotation;
			playersHeadIsOnBed = true;
		}
		else
		{
			Log.Info( "You are now awake" );
			EyePos.Transform.Position = lastHeadPos; // Return to last position
			playersHeadIsOnBed = false;
		}
	}

	public override void OnHoldUse()
	{
		base.OnHoldUse();
		if ( !playersHeadIsOnBed )
		{
			var peetime = Interacter.Components.Get<ImmersivePlayerController>();
			EyePos = peetime.Eye;
			// lastHeadPos is set when the player first uses the bed
		}
	}

 protected override void OnUpdate()
	{
		if ( playersHeadIsOnBed && EyePos != null )
		{
			// Update the position and rotation continuously in case of any changes
			EyePos.Transform.Position = HeadLocation.Transform.Position;
			EyePos.Transform.Rotation = HeadLocation.Transform.Rotation;
			var peetime = Interacter.Components.Get<ImmersivePlayerStats>();
			peetime.IncrementStat( ImmersivePlayerStats.PlayerStats.Energy, 0.1f );
		}
	}
}
