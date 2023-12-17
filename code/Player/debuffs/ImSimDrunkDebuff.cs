using Sandbox;

public class ImSimDrunkDebuff : BaseDebuff
{
	public override string DebuffIcon => "🥴";
	public override string DebuffName => "Drunk";
	public override string DebuffDescription => "You are drunk";

	GameObject Player;

	CameraComponent Camera;

	private float baseFov;
	private float timeElapsed;

	protected override void OnAwake()
	{
		base.OnAwake();
		Player = GameObject.Components.Get<GameObject>();

		// Get the camera component and store the base FOV
		Camera = GameObject.Components.Get<CameraComponent>( FindMode.Enabled );
		if ( Camera != null )
		{
			baseFov = Camera.FieldOfView;
		}

		var stats = GameObject.Components.Get<ImmersivePlayerStats>();
		stats.CurrentState = ImmersivePlayerStats.PlayerState.Drunk;

	}

	public override void Tick()
	{
		//create a drunk movment

		var controller = GameObject.Components.Get<ImmersivePlayerController>();
		controller.WishVelocity = controller.WishVelocity + new Vector3( Game.Random.Float( -10000, 10000 ), Game.Random.Float( -10000, 10000 ), 0 );
		var cc = GameObject.Components.Get<CharacterController>();
		
		cc.Punch( new Vector3( Game.Random.Float( -2, 2 ), Game.Random.Float( -2, 2 ), 0 ) );
		var body = GameObject.Components.Get<PhysicsBody>();
		//body.Velocity = body.Velocity + new Vector3( Game.Random.Float( -1000, 1000 ), Game.Random.Float( -1000, 1000 ), 0 );

		if ( Camera != null )
		{
			timeElapsed += Time.Delta;
			float fovChange = (float)Math.Sin( timeElapsed ) * 12; // Adjust '5' to control the effect intensity
			Camera.FieldOfView = baseFov + fovChange;
		}
	}
	public override void RemoveDebuff()
	{
		var stats = GameObject.Components.Get<ImmersivePlayerStats>();
		stats.CurrentState = ImmersivePlayerStats.PlayerState.Neutral;

		if ( Camera != null )
		{
			Camera.FieldOfView = baseFov;
		}

		base.RemoveDebuff();
	}
}
