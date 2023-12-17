using Sandbox;
using Sandbox.Diagnostics;
using System.Collections.Generic;

[Title( "Interact - Test" )]
[Category( "ImSim" )]
[Icon( "check_box_outline_blank", "red", "white" )]
public class TestInteracter : BaseInteract
{
	public override InteractionType Interaction => InteractionType.Hold;
	protected override void OnStart()
	{
		base.OnStart();

		//StartPos = GameObject.Transform.LocalPosition;
		
	}

	public override void OnUse()
	{
		base.OnUse();


	}

	public override void OnHoldUse()
	{
		base.OnHoldUse();
		var model = GameObject.Components.Get<ModelRenderer>();
		model.Tint = Color.Random;
		//Sound.FromWorld( "error", GameObject.Transform.Position );
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();

	}


}
