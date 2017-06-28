using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	private Transform pivot;
	private Transform firstPivot;
	private Transform secondPivot;
	private Transform camTrans;
	private RaycastHit sphereCastHit;

	public Weapon weap;

	public bool shoulderRight = true;
	public float shoulderVar = 1;

	public float distance;
	public bool isCollided;

	public float goalPivot;
	public float goalFirst;
	public float goalSecond;
	public float goalHyp;
	public float maxRotation;
	public float minRotation;
	public float realVertRotation;
	public float colliderRadius = 0.4f;
	#region Speeds
	public float horRotateSpeed;
	public float vertRotateSpeed;
	public float firstLerpSpeed;
	public float secondLerpSpeed;
	public float collideLerpSpeed;
	#endregion
	#region StateVars
	public float standPivot = 1.7f;
	public float standFirst = 1;
	public float standSecond = -3;
	public float standHyp;

	public float crouchPivot = 1;
	public float crouchFirst = 0.5f;
	public float crouchSecond = -2;
	public float crouchHyp;

	public float strafePivot;
	public float strafeFirst;
	public float strafeSecond;
	public float strafeHyp;

	public float aimPivot;
	public float aimFirst;
	public float aimSecond;
	public float aimHyp;

	public float vehiclePivot;
	public float vehicleFirst;
	public float vehicleSecond;
	public float vehicleHyp;

	public float FPSPivot;
	public float FPSFirst;
	public float FPSSecond;
	public float FPSHyp;
	#endregion
	#region States
	public bool isStanding;
	public bool isCrouching;
	public bool isAiming;
	public bool isStrafing;
	public bool isInVehicle;
	public bool isFPS;
	#endregion
	void OnEnable ()
	{
		GetObjects ();
		GetHypotenuse ();
		SetStartPivotVars (standFirst, standSecond);
	}
	void Update () 
	{
		CheckStates ();

		SetShoulder ();
		GetPivotVars ();
		GetRealVertRotation ();
		Rotate ();
		GetColliderVars ();
		SetVars ();
		Draw();
	}
	private void GetObjects()
	{
		pivot = transform.GetChild (0);
		firstPivot = pivot.GetChild (0);
		secondPivot = firstPivot.GetChild (0);
		camTrans = secondPivot.GetChild (0);
	}
	private void GetHypotenuse()
	{
		standHyp = Mathf.Sqrt (standFirst * standFirst + standSecond * standSecond);
		strafeHyp = Mathf.Sqrt (strafeFirst * strafeFirst + strafeSecond * strafeSecond);
		aimHyp = Mathf.Sqrt (aimFirst * aimFirst + aimSecond * aimSecond);
		crouchHyp = Mathf.Sqrt (crouchFirst * crouchFirst + crouchSecond * crouchSecond);
		FPSHyp = Mathf.Sqrt (FPSFirst * FPSFirst + FPSSecond * FPSSecond);
		vehicleHyp = Mathf.Sqrt (vehicleFirst * vehicleFirst + vehicleSecond * vehicleSecond);
	}
	private void GetPivotVars()
	{
		if (isStanding) 
		{
			goalPivot = standPivot;
			goalFirst = standFirst * shoulderVar;
			goalSecond = standSecond;
			goalHyp = standHyp;
		}
		else if (isCrouching) 
		{
			goalPivot = crouchPivot;
			goalFirst = crouchFirst * shoulderVar;
			goalSecond = crouchSecond;
			goalHyp = crouchHyp;
		}
		else if (isAiming) 
		{
			goalPivot = aimPivot;
			goalFirst = aimFirst * shoulderVar;
			goalSecond = aimSecond;
			goalHyp = aimHyp;
		} 
		else if (isStrafing) 
		{
			goalPivot = strafePivot;
			goalFirst = strafeFirst * shoulderVar;
			goalSecond = strafeSecond;
			goalHyp = strafeHyp;
		} 
		else if (isInVehicle) 
		{
			goalPivot = vehiclePivot;
			goalFirst = vehicleFirst * shoulderVar;
			goalSecond = vehicleSecond;
			goalHyp = vehicleHyp;
		} 
		else if (isFPS) 
		{
			goalPivot = FPSPivot;
			goalFirst = FPSFirst * shoulderVar;
			goalSecond = FPSSecond;
			goalHyp = FPSHyp;
		}
	}
	private void GetRealVertRotation()
	{
		if (secondPivot.localRotation.eulerAngles.x > 180.0f)
			realVertRotation = secondPivot.localRotation.eulerAngles.x - 360.0f;
		else
			realVertRotation = secondPivot.localRotation.eulerAngles.x;
	}
	private void SetStartPivotVars(float x,float y)
	{
		secondPivot.localPosition = new Vector3 (x, 0, 0);
		camTrans.localPosition = new Vector3 (0, 0, y);
	}
	private void Rotate()
	{
		if (Input.GetAxis ("Mouse Y") > 0) 
		{
			if (realVertRotation - Time.deltaTime * vertRotateSpeed * Input.GetAxis ("Mouse Y") > minRotation)
				secondPivot.Rotate (Vector3.left * Input.GetAxis ("Mouse Y") * vertRotateSpeed * Time.deltaTime);
			else
				secondPivot.localRotation = Quaternion.Euler (minRotation, 0, 0);
		} 
		else if (Input.GetAxis ("Mouse Y") == 0)
		{
			secondPivot.localRotation = Quaternion.Euler (Mathf.Clamp (realVertRotation, minRotation, maxRotation), 0, 0);
		}
		else if(Input.GetAxis ("Mouse Y") < 0)
		{
			if (realVertRotation - Time.deltaTime * vertRotateSpeed * Input.GetAxis ("Mouse Y") < maxRotation)
				secondPivot.Rotate (Vector3.left * Input.GetAxis ("Mouse Y") * vertRotateSpeed * Time.deltaTime);
			else
				secondPivot.localRotation = Quaternion.Euler (maxRotation, 0, 0);
		}
		firstPivot.Rotate (Vector3.up * Input.GetAxis ("Mouse X") * horRotateSpeed * Time.deltaTime);
	}
	private void GetColliderVars()
	{
		Physics.SphereCast (firstPivot.position, colliderRadius, camTrans.position - firstPivot.position, out sphereCastHit);
		distance = sphereCastHit.distance;
		isCollided = false;
		if (sphereCastHit.transform != null) 
		{
			if (distance < goalHyp) 
			{
				isCollided = true;
				goalFirst = goalFirst * sphereCastHit.distance / goalHyp;
				goalSecond = goalSecond * sphereCastHit.distance / goalHyp;
			}
		} 
	}
	private void SetVars()
	{
		firstPivot.localPosition = new Vector3 (0, Mathf.Lerp (firstPivot.localPosition.y, goalPivot, Time.deltaTime * firstLerpSpeed), 0);
		secondPivot.localPosition = new Vector3 (Mathf.Lerp (secondPivot.localPosition.x, goalFirst, Time.deltaTime * firstLerpSpeed), 0, 0);
		camTrans.localPosition = new Vector3 (0, 0, Mathf.Lerp (camTrans.localPosition.z, goalSecond, Time.deltaTime * secondLerpSpeed));
	}
	private void SetShoulder()
	{
		if (Input.GetButtonDown ("Shoulder")) 
		{
			shoulderRight = !shoulderRight;
			if (shoulderRight)
				shoulderVar = 1;
			else
				shoulderVar = -1;
		}
	}
	private void CheckStates()
	{
		if (isStrafing)
			isStanding = false;
	}
	private void SetTarget()
	{
		
	}
	private void Draw()
	{
		Debug.DrawLine (firstPivot.position, secondPivot.position, Color.blue);
		Debug.DrawLine (secondPivot.position, camTrans.position, Color.blue);
		Debug.DrawLine (firstPivot.position, camTrans.position, Color.red);
	}
}