using Sandbox;

public sealed class ImmersiveUse : Component
{
	[Property]
	public GameObject Eye { get; set; }
	
	[Property]
	private GameObject Camera { get; set; }
	public GameObject currentlyCarriedObject = null;

	[Property]
	private ImmersiveUIInteract UIInteract { get; set; } = null;

	public float distance = 0;
	private Rotation localRotation;
	private Vector3 localMassCenter;
	Rotation targetRotation;
	Rotation HoldRot;
	
	public bool isHolding = false;

	
	protected override void OnUpdate()
	{
		Eye.Transform.Rotation = Camera.Transform.Rotation;

		var tr = Scene.PhysicsWorld.Trace.Ray( Eye.Transform.Position, Eye.Transform.Position + Eye.Transform.Rotation.Forward * 100 )
			.WithoutTags( "player" )
			.Run();

		Gizmo.Draw.Color = Color.Red;
		Gizmo.Draw.Line( Eye.Transform.Position, tr.EndPosition );

		if(tr.Body.IsValid() && currentlyCarriedObject == null)
		{
			var gameObject = tr.Body.GameObject as GameObject;
			var interact = gameObject.Components.Get<BaseInteract>(FindMode.InSelf);

			if ( interact != null && interact.Interaction == BaseInteract.InteractionType.Hold )
			{
				UIInteract.CurrentInteraction = BaseInteract.InteractionType.Hold;				
			}
			else if ( interact != null && interact.Interaction == BaseInteract.InteractionType.Touch )
			{
				UIInteract.CurrentInteraction = BaseInteract.InteractionType.Touch;
			}
			else if ( interact != null && interact.Interaction == BaseInteract.InteractionType.Use )
			{
				UIInteract.CurrentInteraction = BaseInteract.InteractionType.Use;
			}
			else if ( interact != null && interact.Interaction == BaseInteract.InteractionType.Screen )
			{
				UIInteract.CurrentInteraction = BaseInteract.InteractionType.Screen;
			}
		}
		else
		{
			UIInteract.CurrentInteraction = BaseInteract.InteractionType.None;
		}

		distance += Input.MouseWheel.y * 5;
		distance = Math.Clamp( distance, 20, 50 );

		if ( Input.Pressed( "use" ) )
		{
			if ( currentlyCarriedObject == null )
			{
				if ( tr.Body.IsValid() )
				{
					var interactedObject = tr.Body.GameObject as GameObject;
					var interactComponent = interactedObject.Components.Get<BaseInteract>();
					var physicsComponent = interactedObject.Components.Get<Rigidbody>();

					if ( interactComponent != null && physicsComponent.IsValid() )
					{
						// Pick up the object
						currentlyCarriedObject = interactedObject;

						// Get the local rotation so we can convert it back to a global target rotation
						localRotation = Eye.Transform.Rotation.Inverse * physicsComponent.Transform.Rotation;

						// Keep the mass center to remove it from target position
						localMassCenter = tr.Body.LocalMassCenter;

						// Distance from the eye to mass center
						distance = Eye.Transform.Position.Distance( tr.Body.MassCenter );

						targetRotation = Eye.Transform.Rotation.Inverse * physicsComponent.Transform.Rotation;
						HoldRot = physicsComponent.Transform.Rotation;
					}
				}
				else
				{
					Sound.Play( "player_used" );
				}
			}
			else
			{
				if ( currentlyCarriedObject != null )
				{
					// Release the object
					currentlyCarriedObject = null;
				}
			}
		}

		if ( Input.Pressed( "attack1" ) )
		{
			HandlePressInteraction( tr );
		}

		if (Input.Down("attack1"))
		{
			if ( currentlyCarriedObject != null )
			{
				var Interact = currentlyCarriedObject.Components.Get<BaseInteract>(FindMode.EverythingInSelfAndChildren);
				Interact.Interacter = GameObject;
				Interact.OnHoldUse();
			}
			else if( tr.Body.IsValid() )
			{
				var gameObject = tr.Body.GameObject as GameObject;
				var Interact = gameObject.Components.Get<BaseInteract>( FindMode.EverythingInSelfAndChildren );
				
				if ( Interact!= null && !Interact.IsPhysicsInteract)
				{
					Interact.Interacter = GameObject;
					Interact.OnHoldUse();
				}
			
			}
		}
		else if(Input.Released("attack1"))
		{
			if ( currentlyCarriedObject != null )
			{
				var Interact = currentlyCarriedObject.Components.Get<BaseInteract>();
				Interact.OnHoldUseEnd();
			}

		}

		if ( Input.Pressed( "flashlight" ) )
		{
			if ( currentlyCarriedObject != null )
			{
				
				//throw the object
				var physicsBody = currentlyCarriedObject.Components.Get<Rigidbody>();
				physicsBody.Velocity = Eye.Transform.Rotation.Forward * 500;
				physicsBody.AngularVelocity = Vector3.Zero;
				physicsBody.Gravity = true;
				var interactComponent = currentlyCarriedObject.Components.Get<BaseInteract>();

				//ReleaseCurrentlyHeldObject();

				interactComponent.OnEndHolding();

				currentlyCarriedObject = null;
				isHolding = false;
			}
		}
		
		if(currentlyCarriedObject == null)
		{
			var depth = Eye.Components.Get<DepthOfField>();
			depth.FocalDistance = 2000;
			UIInteract.hasObject = false;
		}
	}

	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();

		if ( !currentlyCarriedObject.IsValid() )
			return;

		var physicsBody = currentlyCarriedObject.Components.Get<Rigidbody>();
		if ( !physicsBody.IsValid() )
			return;

		// Target position is a distance away from the eye, minus center of mass offset
		var currentPosition = physicsBody.Transform.Position;
		var targetPosition = Eye.Transform.Position + Eye.Transform.Rotation.Forward * distance;
		targetPosition -= physicsBody.Transform.Rotation * localMassCenter;

		// Calculate the velocity needed to move from current to target position
		var velocity = physicsBody.Velocity;
		Vector3.SmoothDamp( currentPosition, targetPosition, ref velocity, 0.2f, Time.Delta );
		physicsBody.Velocity = velocity;

		// Add the eye rotation back onto the local rotation to make it a global rotation
		var currentRotation = physicsBody.Transform.Rotation;
		//targetRotation = Eye.Transform.Rotation * localRotation;

		var eyerot = Rotation.From( new Angles( 0.0f, Camera.Transform.Rotation.y, 0.0f ) );

		if ( Input.Down( "attack2" ) )
		{
			DoRotation( eyerot, Input.MouseDelta );
		}

		HoldRot = Camera.Transform.Rotation * targetRotation;

		// Calculate the velocity needed to move from current to target rotation
		var angvelocity = physicsBody.AngularVelocity;
		Rotation.SmoothDamp( currentRotation, HoldRot, ref angvelocity, 0.075f, Time.Delta );
		physicsBody.AngularVelocity = angvelocity;

		var depth = Eye.Components.Get<DepthOfField>();
		if ( distance <= 25 )
		{
			depth.FocalDistance = distance * 2;
		}
		else
		{
			depth.FocalDistance = 2000;
		}
	}

	public void DoRotation( Rotation eye, Vector3 input )
	{
		var localRot = eye;
		localRot *= Rotation.FromAxis( Vector3.Up, input.x * 0.125f );
		localRot *= Rotation.FromAxis( Vector3.Right, input.y * 0.125f );
		localRot = eye.Inverse * localRot;

		targetRotation = localRot * targetRotation;
	}

	private void HandlePressInteraction( PhysicsTraceResult tr )
	{
		if ( !isHolding && tr.Body.IsValid() )
		{
			var interactedObject = tr.Body.GameObject as GameObject;
			var interactComponent = interactedObject.Components.Get<BaseInteract>();

			if ( interactComponent != null && !interactComponent.IsPhysicsInteract )
			{
				// Simple interaction with the object
				interactComponent.Interacter = GameObject;
				interactComponent.OnUse();
			}
		}
	}
}
