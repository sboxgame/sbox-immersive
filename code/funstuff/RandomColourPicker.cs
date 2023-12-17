using Sandbox;

public sealed class RandomColourPicker : Component, Component.ExecuteInEditor
{
	[Property] public Gradient Gradient { get; set; } = new Gradient( new Gradient.ColorFrame( 0.0f, Color.Cyan ), new Gradient.ColorFrame( 1.0f, Color.Red ) );

	float delta = 0.0f;

	protected override void OnStart()
	{
		base.OnStart();
		delta = Game.Random.Float( 0.0f, 1.0f );

		/*
		var color = Gradient.Evaluate( delta );
		GameObject.ForEachComponent<ITintable>( "ChangeColor", true, t =>
		{
			t.Color = color;
		} );
		*/
	}
}
