using Sandbox;

public class ImSimBlindDebuff : BaseDebuff
{
	public override string DebuffIcon => "😣";
	public override string DebuffName => "Blind";
	public override string DebuffDescription => "You are blind";

	public override float DebuffDuration => 15;

	GameObject ScreenPanel;
	BlindUIDebuff UIBlind;
	protected override void OnAwake()
	{
		base.OnAwake();

		var cam = GameObject.Components.Get<ScreenPanel>(FindMode.InChildren);
		ScreenPanel = cam.GameObject;

		UIBlind = ScreenPanel.Components.Create<BlindUIDebuff>();
	}

	public override void RemoveDebuff()
	{
		base.RemoveDebuff();

		UIBlind.Destroy();

		//ScreenPanel.RemoveComponent<BlindUIDebuff>();
	}

	public override void Tick()
	{
		//create a drunk movment


	}
}
