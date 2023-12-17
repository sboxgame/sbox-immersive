using Sandbox;
using Sandbox.Diagnostics;
using System.Collections.Generic;
using static Sandbox.Gizmo;

[Title( "Interact - Magnet" )]
[Category( "ImSim" )]
[Icon( "check_box_outline_blank", "red", "white" )]
public class MagnetInteracter : BaseInteract
{
	public override InteractionType Interaction => InteractionType.Hold;

	public override bool HandleOwnHoldingState => false;
	protected override void OnStart()
	{
		base.OnStart();
	}

	public override void OnHoldUse()
	{
		base.OnHoldUse();
	}

	public override void OnHoldUseEnd()
	{
		base.OnHoldUseEnd();

	}

	protected override void OnUpdate()
	{
		base.OnUpdate();

		var tr = Scene.PhysicsWorld.Trace.Ray( GameObject.Transform.Position, GameObject.Transform.Position + GameObject.Transform.Rotation.Forward * 10 )
			.WithTag( "metal" )
			.Run();

		if ( tr.Body != null )
		{
			var physicsBody = GameObject.Components.Get<Rigidbody>();
			var velocity = physicsBody.Velocity;
			Vector3.SmoothDamp( GameObject.Transform.Position, tr.EndPosition, ref velocity, 0.2f, RealTime.Delta );
			physicsBody.Velocity = velocity;
			physicsBody.AngularVelocity = Vector3.Zero;
			physicsBody.Gravity = false;
		}
	}

	public override void OnEndHolding()
	{
		base.OnEndHolding();

		var physicsBody = GameObject.Components.Get<Rigidbody>();
		physicsBody.Gravity = true;

	}
}
