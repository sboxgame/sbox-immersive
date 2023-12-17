using Sandbox;

[Title( "Interact - Switch" )]
[Category( "ImSim" )]
[Icon( "check_box_outline_blank", "red", "white" )]
public class SwitchInteract : BaseInteract
{
	public override InteractionType Interaction => InteractionType.Touch;
	public bool IsOn { get; set; } = true;

	public override bool IsPhysicsInteract => false;

	[Property]
	public GameObject SwitchedObject { get; set; }

	public override void OnUse()
	{
		base.OnUse();

		IsOn = !IsOn;

		var model = GameObject.Components.Get<ModelRenderer>();

		model.BodyGroups = ulong.Parse( IsOn ? "2" : "1" );
		
		if(IsOn)
		{
			Sound.Play( "switch.on", GameObject.Transform.Position );
			SwitchedObject.Enabled = true;
		}
		else
		{
			Sound.Play( "switch.off", GameObject.Transform.Position );
			SwitchedObject.Enabled = false;
		}
	}

	protected override void OnUpdate()
	{

	}
}
