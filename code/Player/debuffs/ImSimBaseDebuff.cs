using Sandbox;
using Sandbox.Diagnostics;
using System;
using System.Collections.Generic;

public abstract class BaseDebuff : Component
{
	public virtual string DebuffIcon => "materials/gizmo/charactercontroller.png";
	public virtual string DebuffName => "Debuff";
	public virtual string DebuffDescription => "This is a Debuff";
	public virtual float DebuffDuration => 15.0f;

	public RealTimeSince DebuffTime { get; private set; }
	protected override void OnAwake()
	{
		base.OnAwake();
		DebuffTime = 0;

		var debuffManager = GameObject.Components.Get<DebuffManager>();
		debuffManager?.AddDebuff( this );
	}

	protected override void OnUpdate()
	{
		if ( DebuffTime > DebuffDuration )
		{
			RemoveDebuff();
		}

		// Update the debuff time in the debuff manager
		var debuffManager = GameObject.Components.Get<DebuffManager>();
		debuffManager?.UpdateDebuffTime( this );
		
		Tick();
	}

	public virtual void Tick()
	{

	}
	
	public virtual void RemoveDebuff()
	{
		var debuffManager = GameObject.Components.Get<DebuffManager>();
		debuffManager?.RemoveDebuff( this );

		Destroy();
	}

}
