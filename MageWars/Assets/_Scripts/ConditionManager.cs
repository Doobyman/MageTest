using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ConditionManager : MonoBehaviour {
	
	// link to playerstats
	private PlayerStats playerStats;
	
	// list of active conditions
	private List<Condition> activeConditions = new List<Condition>();
	
	// list of active condition IDs. stacking effects will be represented as 1 ID
	private List<int> activeConditionIDs = new List<int>();
	
	//Mapping between condition-instance and time until expiration.
	private Dictionary<Condition, float> timeRemaining = new Dictionary<Condition, float>();
	
	void Awake()
	{
		playerStats = GetComponent( "PlayerStats" ) as PlayerStats;
	}
	
	void Start() 
	{
		
	}
	
	void Update() 
	{
		float timePassed = Time.deltaTime;
		
		//Countdown durations
		foreach( var key in timeRemaining.Keys.ToList() )
		{
			timeRemaining[ key ] -= timePassed;
		}

		//If some effect has expired, remove it and stop tracking the remaining time.
  		foreach( Condition condition in activeConditions.ToList() )
		{
			if( timeRemaining[ condition ] <= 0 )
			{
				activeConditionIDs.Remove( condition.ID );
				activeConditions.Remove( condition );
				timeRemaining.Remove( condition );
				condition.Expire( playerStats );
			}
		}

  		//Keep doing whatever the effect is supposed to do
  		foreach( Condition condition in activeConditions )
		{
			condition.Continuous( playerStats, timePassed );
  		}
	}
	
	public void ReceiveCondition( Condition condition )
	{
		// if previously unaffected
		if ( !activeConditionIDs.Contains( condition.ID ) )
		{
			//Add effect
			activeConditions.Add( condition );
			activeConditionIDs.Add( condition.ID );
			timeRemaining.Add( condition, condition.Duration );
			condition.ApplyToTarget( playerStats );
		}
		else if ( condition.Prolongable )	//Affected already, not stackable, but prolongable
		{
			// Prolong the condition
			foreach( Condition con in activeConditions.ToList() )
			{
				if( con.ID == condition.ID )
				{
					timeRemaining[ con ] = condition.Duration;
					break;
				}
			}
		}
	}
}
