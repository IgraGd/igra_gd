using UnityEngine;
using System.Collections;

public class PlayerScript : CharacterBaseScript {

	public Weapon weap;

	public Vector3 target;
	public RaycastHit targetHit;

	public Transform camTran;

	public CameraScript camra;
	public Material heroMaterial;

	public Transform oneWeapon;
	public Transform twoWeapon;

	public Transform oneWeaponHand;
	public Transform twoWeaponHand;

	public Transform oneWeaponHolder;
	public Transform twoWeaponHolder;

	public float currentWeapon = 0;

	void OnEnable()
	{
		charCtrl = GetComponent<CharacterController> ();
		pivot = transform.GetChild (1).GetChild (0).GetChild (0);
		anim = transform.GetChild (0).GetComponent<Animator> ();
		camra = transform.GetChild (1).GetComponent<CameraScript> ();
		camTran = pivot.GetChild (0);
	}
	void Update () 
	{
		Gravity ();
		RegulateStates ();
		CheckStates ();
		GetWeapon ();
		SetWeapon ();
		if (isFreeMoving) 
		{
			if (isGrounded) 
			{
				#region Controller
				Jump ();
				moveVector = GetNormalizedVectorFromKeyboard ();
				moveVector = GetRelativeVector (moveVector, pivot);
				GetAngle ();
				RegulateCrouch ();
				Rotate ();
				#endregion
				#region Animator
				GetMoveAnimation ();
				GetLean ();
				GetWeaponAnimation();
				anim.SetLayerWeight (2, 0);
				#endregion
			} 
			GetSpeed (moveVector);
			Move ();
		} 
		else if (isStrafing) 
		{
			if (isGrounded) 
			{
				#region Controller

				Jump ();
				moveVector = GetNormalizedVectorFromKeyboard ();
				moveVector = GetRelativeVector (moveVector, pivot);
				RegulateCrouch ();
				StrafeRotate (pivot);
				#endregion
				#region Animator
				GetStrafeFloats (GetVectorFromKeyboard().x, GetVectorFromKeyboard().z);
				GetMoveAnimation();
				GetWeaponAnimation();
				GetVerticalAim(camTran,pivot);
				anim.SetLayerWeight (2, 1);
				#endregion
			} 
			GetSpeed (moveVector);
			StrafeMove (moveVector);
			if (isOneArmed || isTwoArmed) 
			{
				Shoot ();
			}
		}
		SetStateToCamera ();
		SetTransparent ();
		SetAnimationBools ();
	}

	private void SetStateToCamera()
	{
		camra.isStanding = isStanding;
		camra.isCrouching = isCrouching;
		camra.isStrafing = isStrafing;
	}
	private void SetTransparent()
	{
		if (camra.isCollided) 
			heroMaterial.color = new Color (heroMaterial.color.r, heroMaterial.color.g, heroMaterial.color.b, Mathf.Clamp01 (camra.distance ));
		else
			heroMaterial.color = new Color (heroMaterial.color.r, heroMaterial.color.g, heroMaterial.color.b, 1);
	}
	private void GetWeapon()
	{
		if (Input.GetKeyDown (KeyCode.Alpha3))
			currentWeapon = 2;
		else if (Input.GetKeyDown (KeyCode.Alpha2))
			currentWeapon = 1;
		else if (Input.GetKeyDown (KeyCode.Alpha1))
			currentWeapon = 0;
	}
	private void SetWeapon()
	{
		if (currentWeapon == 0) 
		{
			isOneArmed = false;
			isTwoArmed = false;
			oneWeapon.parent = oneWeaponHolder;
			oneWeapon.transform.localPosition = Vector3.zero;
			oneWeapon.localRotation = Quaternion.Euler (Vector3.zero);
			twoWeapon.parent = twoWeaponHolder;
			twoWeapon.transform.localPosition = Vector3.zero;
			twoWeapon.localRotation = Quaternion.Euler (Vector3.zero);
		} 
		else if (currentWeapon == 1) 
		{
			isOneArmed = true;
			isTwoArmed = false;
			oneWeapon.parent = oneWeaponHand;
			oneWeapon.transform.localPosition = Vector3.zero;
			oneWeapon.localRotation = Quaternion.Euler (Vector3.zero);
			twoWeapon.parent = twoWeaponHolder;
			twoWeapon.transform.localPosition = Vector3.zero;
			twoWeapon.localRotation = Quaternion.Euler (Vector3.zero);
			weap = oneWeapon.GetComponent<Weapon> ();
		}
		else if (currentWeapon == 2)
		{
			isOneArmed = false;
			isTwoArmed = true;
			twoWeapon.parent = oneWeaponHand;
			twoWeapon.transform.localPosition = Vector3.zero;
			twoWeapon.localRotation = Quaternion.Euler (Vector3.zero);
			oneWeapon.parent = oneWeaponHolder;
			oneWeapon.transform.localPosition = Vector3.zero;
			oneWeapon.localRotation = Quaternion.Euler (Vector3.zero);
			weap = twoWeapon.GetComponent<Weapon> ();
		}
	}
	private void Shoot()
	{
		Physics.Raycast (camTran.position, camTran.transform.forward * weap.range, out targetHit);

		if (targetHit.transform == null)
			target = camTran.position + camTran.transform.forward * weap.range;
		else
			target = targetHit.point;

		if (weap.type == "one") 
		{
			if (Input.GetButtonDown ("Fire1")) 
			{
				if (Physics.Raycast (weap.transform.Find ("dulo").position, target-weap.transform.Find ("dulo").position, out targetHit, weap.range)) 
				{
					if (targetHit.transform.CompareTag ("Enemy")) 
					{
						if (targetHit.transform.GetComponent<EnemyAI> ().isAlive) 
						{
							if (targetHit.transform.GetComponent<EnemyAI> ().health - weap.strength <= 0) 
							{
								targetHit.transform.GetComponent<EnemyAI> ().health -= weap.strength;
								targetHit.transform.SendMessage ("SetDead");
								Physics.Raycast (weap.transform.Find ("dulo").position, target - weap.transform.Find ("dulo").position, out targetHit, weap.range);
								targetHit.transform.GetComponent<Rigidbody> ().AddForce ((target - weap.transform.Find ("dulo").position) * weap.strength,ForceMode.Impulse);
							} 
							else 
							{
								targetHit.transform.GetComponent<EnemyAI> ().health -= weap.strength;
							}
						}
					}
					else if(targetHit.transform.GetComponent<Rigidbody>()!=null)
						targetHit.transform.GetComponent<Rigidbody> ().AddForce ((target - weap.transform.Find ("dulo").position) * weap.strength,ForceMode.Impulse);
				}
			}
		}
		else if (weap.type == "auto") 
		{
			if (Input.GetButton ("Fire1")) 
			{
				if (Physics.Raycast (weap.transform.Find ("dulo").position, target-weap.transform.Find ("dulo").position, out targetHit, weap.range)) 
				{
					if (targetHit.transform.CompareTag ("Enemy")) 
					{
						if (targetHit.transform.GetComponent<EnemyAI> ().isAlive) 
						{
							if (targetHit.transform.GetComponent<EnemyAI> ().health - weap.strength <= 0) 
							{
								targetHit.transform.GetComponent<EnemyAI> ().health -= weap.strength;
								targetHit.transform.SendMessage ("SetDead");
								Physics.Raycast (weap.transform.Find ("dulo").position, target - weap.transform.Find ("dulo").position, out targetHit, weap.range);
								targetHit.transform.GetComponent<Rigidbody> ().AddForce ((target - weap.transform.Find ("dulo").position) * weap.strength,ForceMode.Impulse);
							} 
							else 
							{
								targetHit.transform.GetComponent<EnemyAI> ().health -= weap.strength;
							}
						}
					}
					else if(targetHit.transform.GetComponent<Rigidbody>()!=null)
						targetHit.transform.GetComponent<Rigidbody> ().AddForce ((target - weap.transform.Find ("dulo").position) * weap.strength,ForceMode.Impulse);
				}
			}
		}


		Debug.DrawLine (weap.transform.Find("dulo").position, target, Color.yellow);
	}
}