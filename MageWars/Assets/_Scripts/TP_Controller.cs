using UnityEngine;
using System.Collections;

public class TP_Controller : MonoBehaviour
{
	public static CharacterController CharacterController;
	public static TP_Controller Instance;
	
	private GameObject currentTarget;
	private PlayerStats currentTargetStats;
	private ConditionManager currentTargetConditionManager;
	private PlayerStats playerStats;
	private ConditionList conditionList;
	private ConditionManager conditionManager;
	
	void Awake() 
	{
		CharacterController = GetComponent( "CharacterController" ) as CharacterController;
		playerStats = GetComponent( "PlayerStats" ) as PlayerStats;
		conditionList = GameObject.Find( "Spells" ).GetComponent<ConditionList>();
		conditionManager = GetComponent( "ConditionManager" ) as ConditionManager;
		Instance = this;
		TP_Camera.UseExistingOrCreateNewMainCamera();
	}

	void Update() 
	{
		if ( Camera.mainCamera == null )
			return;
		
		GetLocomotionInput();
		HandleActionInput();
		
		TP_Motor.Instance.UpdateMotor();
	}
	
	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 1000, 20), "Player Health: " + playerStats.GetCurrentHealth().ToString() + " / " + playerStats.TotalHealth.ToString() );
		if ( currentTargetStats != null ) GUI.Label(new Rect(10, 25, 1000, 20), "Target Health: " + currentTargetStats.GetCurrentHealth().ToString() + " / " + currentTargetStats.TotalHealth.ToString() );
	}
	
	void GetLocomotionInput()
	{
		var deadZone = 0.1f;
		
		TP_Motor.Instance.VerticalVelocity = TP_Motor.Instance.MoveVector.y;
		TP_Motor.Instance.MoveVector = Vector3.zero;

		if ( Input.GetAxis( "Vertical" ) > deadZone || Input.GetAxis( "Vertical" ) < -deadZone )
			TP_Motor.Instance.MoveVector += new Vector3( 0, 0, Input.GetAxis( "Vertical" ) );
		
		if ( Input.GetAxis( "Horizontal" ) > deadZone || Input.GetAxis( "Horizontal" ) < -deadZone )
			TP_Motor.Instance.MoveVector += new Vector3( Input.GetAxis( "Horizontal" ), 0, 0 );
		
		TP_Animator.Instance.DetermineCurrentMoveDirection();
	}
	
	void HandleActionInput()
	{
		if ( Input.GetButton( "Jump" ) )
		{
			Jump();	
		}
		
		if ( Input.GetKey( KeyCode.Escape ) )
		{
			Escape();
		}
		
		if ( Input.GetMouseButtonUp( 0 ) )
		{
			Click();
		}
		
		if ( Input.GetKeyUp( KeyCode.T ) )
		{
			ApplyCondition();
		}
	}
	
	void Jump()
	{
		TP_Motor.Instance.Jump();
	}
			
	void Click()
	{
		var ray = Camera.mainCamera.ScreenPointToRay( Input.mousePosition );
		RaycastHit hitInfo;
		if ( Physics.Raycast( ray, out hitInfo ) )
		{
			if ( hitInfo.collider.tag == "Enemy" )
			{
				currentTarget = hitInfo.collider.gameObject;
				currentTargetStats = currentTarget.GetComponent( "PlayerStats" ) as PlayerStats;
				currentTargetConditionManager = currentTarget.GetComponent( "ConditionManager" ) as ConditionManager;
			}
		}
	}
	
	void Escape()
	{
		if ( currentTarget != null )
		{
			currentTarget = null;
			currentTargetStats = null;
			currentTargetConditionManager = null;
		}
	}
	
	void ApplyCondition()
	{
		if ( currentTarget != null ) currentTargetConditionManager.ReceiveCondition( conditionList.Conditions[0] );
		else conditionManager.ReceiveCondition( conditionList.Conditions[0] );
	}
}