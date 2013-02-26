using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {
	public int TotalHealth;
	public int TotalMana;
	
	private int currentHealth;
	private int currentMana;

	// Use this for initialization
	void Start () {
		currentHealth = TotalHealth;
		currentMana = TotalMana;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public int GetCurrentHealth()
	{
		return currentHealth;
	}
	
	public int GetCurrentMana()
	{
		return currentMana;
	}
	
	public void ApplyDamage( int damage )
	{
		print( "Taking Damage! " + damage );
		currentHealth -= damage;
	}
}
