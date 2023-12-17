using Sandbox;
using Sandbox.Diagnostics;
using System.Collections.Generic;

[Title( "Interact - Spray" )]
[Category( "ImSim" )]
[Icon( "check_box_outline_blank", "red", "white" )]
public class SprayInteract : BaseInteract
{
	public override InteractionType Interaction => InteractionType.Hold;

	[Property]
	private float EndDistance { get; set; } = 100;

	[Property]
	private GameObject nozzle { get; set; }

	[Property]
	private bool IsBeingUsed { get; set; } = false;

	private string spraySound = null;

	//private Sound spraySoundInstance;

	protected override void OnStart()
	{
		base.OnStart();
	}

	public override void OnUse()
	{
		base.OnUse();

		spraySound = "spray.loop";
	}

	public override void OnHoldUse()
	{
		base.OnHoldUse();
		Gizmo.Draw.Line( nozzle.Transform.Position, nozzle.Transform.Position + nozzle.Transform.Rotation.Forward * EndDistance );	
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();

		if ( !nozzle.IsValid() ) return;
		
		var partsys = nozzle.Components.Get<ParticleSystem>(false);
		//partsys.Enabled = IsInteracting;

		if ( IsInteracting && !IsBeingUsed )
		{
			IsBeingUsed = true;
			//spraySoundInstance = Sound.Play( spraySound, nozzle.Transform.Position );
		}
		
		if(!IsInteracting )
		{
			IsBeingUsed = false;
			//spraySoundInstance.Stop();
			spraySound = null;
		}
		
	}


}
