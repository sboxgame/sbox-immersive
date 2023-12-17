using System.Collections.Generic;
using Sandbox;
public class DebuffManager : Component
{
	public List<BaseDebuff> ActiveDebuffs { get; private set; } = new List<BaseDebuff>();

	public void AddDebuff( BaseDebuff debuff )
	{
		ActiveDebuffs.Add( debuff );
	}

	public void RemoveDebuff( BaseDebuff debuff )
	{
		ActiveDebuffs.Remove( debuff );
	}
	public void UpdateDebuffTime( BaseDebuff debuff )
	{
		
	}

	protected override void OnUpdate()
	{
		/*
		// Update each debuff
		foreach ( var debuff in ActiveDebuffs )
		{
			debuff.Update();
		}
		*/
	}
}
