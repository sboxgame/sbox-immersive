public abstract class BaseInteract : Component
{
	public enum InteractionType
	{
		Hold,
		Touch,		
		Use,
		Screen,
		None
	}

	[Property]
	[Group( "Interaction" )]
	public virtual bool CanInteract => true;

	[Property]
	[Group( "Interaction" )]
	public virtual bool IsPhysicsInteract => true;

	[Property]
	[Group( "Interaction" )]
	public virtual InteractionType Interaction => InteractionType.Hold;

	public virtual bool HandleOwnHoldingState => false;

	public GameObject Interacter { get; set; }

	public bool IsInteracting { get; private set; }

	public virtual void OnUse()
	{
		//Log.Info( "OnUse" );
	}

	public virtual void OnHoldUse()
	{
		IsInteracting = true;
	}

	public virtual void OnHoldUseEnd()
	{
		IsInteracting = false;
	}
	public virtual void OnStartHolding()
	{
	}

	public virtual void OnEndHolding()
	{
		Interacter = null;
	}

	protected override void OnEnabled()
	{

	}

	protected override void OnUpdate()
	{

	}

}
