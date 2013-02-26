using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Condition : MonoBehaviour 
{		
	public int ID;
	public string Name = "";
	public float Duration = 5f;
	public float TickInterval = 1f;
	public bool Prolongable;
	public List<Effect> Effects;
	
	private float currentTickTime = 0f;

	void Start() 
	{
		
	}

	void Update() 
	{		
		
	}
	
	//Do something to target when effect is applied
	public void ApplyToTarget( PlayerStats target )
	{
		print( "Applying " + Name + " for " + Duration + "s" );
	}

	//Do something to target each update-phase when effect is on.
	public void Continuous( PlayerStats target, float timePassed )
	{
		currentTickTime += timePassed;
		if ( currentTickTime >= TickInterval )
		{
			currentTickTime = 0f;
			print( "Continuing " + Name );
			foreach( var effect in Effects )
			{
				//if ( effect.DamagePerTick > 0 )
					//target.ApplyDamage( effect.DamagePerTick );
			}
		}
	}

	//Do something to target when effect expires.
	public void Expire( PlayerStats target )
	{
		print( "Expiring " + Name + " at " + Duration + "s" );
		foreach( var effect in Effects )
		{
			//if ( effect.DamagePerTick > 0 )
				//target.ApplyDamage( effect.DamagePerTick );
		}
	}
}