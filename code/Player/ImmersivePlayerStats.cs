using Sandbox;
using System.Collections.Generic;

public sealed class ImmersivePlayerStats : Component
{
	public enum PlayerState
	{
		Happy,
		Neutral,
		Angry,
		Sad,
		Upset,
		Bored,
		Tired,
		Hungry,
		Thirsty,
		Bladder,
		Drunk
	}

	[Property]
	public PlayerState CurrentState { get; set; } = PlayerState.Neutral;

	public IDictionary<PlayerStats, float> CurrentPlayerStats { get; private set; }
	public enum PlayerStats
	{ Health, Hunger, Thirst, Energy, Bladder, Money };

	public float StartingHealth { get; set; } = 0.55f;

	public float StartingHunger { get; set; } = 0.65f;

	public float StartingThirst { get; set; } = 0.85f;

	public float StartingEnergy { get; set; } = 0.75f;

	public float StartingMoney { get; set; } = 100.0f;

	public float StartingBladder { get; set; } = 1.0f;


	protected override void OnStart()
	{
		base.OnStart();

		CurrentPlayerStats = new Dictionary<PlayerStats, float>();
		SetStartingStats();
	}


	protected override void OnUpdate()
	{
		Random random = new Random();
		foreach ( var key in CurrentPlayerStats.Keys.ToList() )
		{
			if ( key != PlayerStats.Money )
			{
				float decrement = (float)random.NextDouble() * 0.0001f;
				CurrentPlayerStats[key] = ClampStat( CurrentPlayerStats[key] - decrement );
			}

			//IncrementStat( key, 0.001f );
		}

		if(Input.Down("slot2"))
		{
			IncrementStat( PlayerStats.Health, 0.1f );
			IncrementStat( PlayerStats.Hunger, 0.1f );
			
		}

		UpdatePlayerStateBasedOnLowestStat();
	}

	private void UpdatePlayerStateBasedOnLowestStat()
	{
		var lowestStat = CurrentPlayerStats.OrderBy( stat => stat.Value ).First();
/*
		// Only update the state if the lowest stat is below 50%
		if ( lowestStat.Value < 0.5 )
		{
			switch ( lowestStat.Key )
			{
				case PlayerStats.Health:
					CurrentState = PlayerState.Sad; // Example state for low health
					break;
				case PlayerStats.Hunger:
					CurrentState = PlayerState.Hungry;
					break;
				case PlayerStats.Thirst:
					CurrentState = PlayerState.Thirsty;
					break;
				case PlayerStats.Energy:
					CurrentState = PlayerState.Tired;
					break;
				case PlayerStats.Bladder:
					CurrentState = PlayerState.Bladder; // Adjust this state as needed
					break;
				// Add other cases as necessary
				default:
					CurrentState = PlayerState.Neutral;
					break;
			}
		}
*/
	}


	public void IncrementStat( PlayerStats stat, float amount )
	{
		//if ( Input.Down( "slot" + (int)stat ) )
		//{
		//	if ( stat == PlayerStats.Money )
		//	{
		//		CurrentPlayerStats[stat] += 1.0f; // Different increment for Money
		//	}
		//	else
		//	{

		//	}
		//}

		CurrentPlayerStats[stat] = ClampStat( CurrentPlayerStats[stat] + amount / 100 );
	}

	public void DecrementStat( PlayerStats stat, float amount )
	{
		CurrentPlayerStats[stat] = ClampStat( CurrentPlayerStats[stat] - amount / 100 );
	}

	private float ClampStat( float value )
	{
		return Math.Clamp( value, 0.0f, 1.0f );
	}
	

	[Property]
	public GameObject peeobject { get; set; }

	public void PlayerUsesToilet()
	{
		CurrentPlayerStats[PlayerStats.Bladder] -= 0.001f;
		CurrentPlayerStats[PlayerStats.Thirst] += 0.00001f;

		var peed = SceneUtility.Instantiate(peeobject.Scene,GameObject.Transform.Position , GameObject.Transform.Rotation);

	}

	public void SetStartingStats()
	{

		CurrentPlayerStats[PlayerStats.Health] = StartingHealth;
		CurrentPlayerStats[PlayerStats.Hunger] = StartingHunger;
		CurrentPlayerStats[PlayerStats.Thirst] = StartingThirst;
		CurrentPlayerStats[PlayerStats.Energy] = StartingEnergy;
		CurrentPlayerStats[PlayerStats.Bladder] = StartingBladder;
		CurrentPlayerStats[PlayerStats.Money] = StartingMoney;
		
		foreach ( var stat in CurrentPlayerStats )
		{
			Log.Info( stat.Key + " " + stat.Value );
		}

	}
	
}
