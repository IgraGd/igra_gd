using UnityEngine;
using System.Collections;

public class CharacterBaseScript : MonoBehaviour 
{
	#region Variables
	protected Vector3 moveVector;
	protected CharacterController charCtrl;
	protected Animator anim;
	protected Transform pivot;

	protected float currentXAngle;
	protected float currentYAngle;

	public float health;

	#region Bones
	public Transform spine1;
	public Transform spine2;
	public Transform leftLeg;
	public Transform leftKnee;
	public Transform rightLeg;
	public Transform rightKnee;
	public Transform leftArm;
	public Transform leftElbow;
	public Transform rightArm;
	public Transform rightElbow;
	public Transform head;
	#endregion

	#region States
	public bool isGrounded = true; //На земле?
	public bool isStanding = true; //Стоим?
	public bool isCrouching = false; //Сидим?
	public bool isFreeMoving = true; //Смотрим в сторону движения?
	public bool isStrafing = false; //Смотрим вперед всегда?
	public bool isInAir = false; //В воздухе?
	public bool isOneArmed = false; //Вооружен?
	public bool isTwoArmed = false; //Вооружен?
	public bool isWeaponChanging = false; //Меняем оружие?
	public bool isReloading = false; //Перезаряжаемся?
	public bool isAlive;
	#endregion
	#region Speeds
	public float currentSpeed; //Текущая скорость
	public float goalSpeed; //Целевая скорость
	public float rotSpeed = 5; //Скорость вращения
	public float strafeRotSpeed = 5; //Скорость вращения при стрейфе
	public float speedLerpSpeed = 4; //Скорость изменения скорости
	protected float leanLerpSpeed = 4; //Скорость изменения наклона при движении
	#region Free
	#region Stand
	public float idleSpeed = 0; //Скорость стояния
	public float walkSpeed = 1.5f; //Скорость ходьбы
	public float runSpeed = 3.5f; //Скорость бега
	public float sprintSpeed = 5.5f; //Скорость спринта
	#endregion
	#region Crouch
	public float crouchIdleSpeed = 0; //Скорость стояния приседом
	public float crouchWalkSpeed = 1.5f; //Скорость ходьбы сидя
	public float crouchRunSpeed = 3.5f; //Скорость бега сидя
	public float crouchSprintSpeed = 5.5f; //Скорость спринта сидя
	#endregion
	#endregion
	#region Strafe
	#region Stand
	public float strafeIdleSpeed = 0; //Скорость стояния
	public float strafeWalkSpeed = 1.5f; //Скорость ходьбы
	public float strafeRunSpeed = 3.5f; //Скорость бега
	public float strafeSprintSpeed = 5.5f; //Скорость спринта
	#endregion
	#region Crouch
	public float crouchStrafeIdleSpeed = 0; //Скорость стояния приседом
	public float crouchStrafeWalkSpeed = 1.5f; //Скорость ходьбы сидя
	public float crouchStrafeRunSpeed = 3.5f; //Скорость бега сидя
	public float crouchStrafeSprintSpeed = 5.5f; //Скорость спринта сидя
	#endregion
	#endregion
	#region Gravity
	public float currentGravity;
	public float standGravity = -1;
	public float fallGravity = -9;
	public float jumpSpeed = -5;
	public float gravityLerpSpeed = 1;
	#endregion
	#endregion
	#region Capsule parametres
	protected float heightChangeSpeed = 5; //Скорость изменения высоты коллайдера
	protected float standHeight = 1.9f; //Высота коллайдера когда стоим
	protected float standY = 0.95f; //Координата коллайдера при стоянии
	protected float crouchHeight = 1.2f; //Высота коллайдера при сидении
	protected float crouchY = 0.6f; //Координата коллайдера при сидении
	#endregion
	#region Abilities
	public bool canMove; //Можем ли перемещаться
	public bool canRotate; //Можем ли вращаться
	public bool canIdle; //Можем ли стоять
	public bool canWalk; //Можем ли ходить
	public bool canRun; //Можем ли бегать
	public bool canSprint; //Можем ли спринтовать
	public bool canCrouch; //Можем ли сидеть
	public bool canUnCrouch; //Можем ли вставать
	public bool canCrouchIdle; //Можем ли стоять сидя
	public bool canCrouchWalk; //Можем ли ходить сидя
	public bool canCrouchRun; //Можем ли бегать сидя
	public bool canCrouchSprint; //Можем ли спринтовать сидя
	public bool canStrafeIdle; //Можем ли стоять стрейфя
	public bool canStrafeWalk; //Можем ли ходить стрейфя
	public bool canStrafeRun; //Можем ли бегать стрейфя
	public bool canStrafeSprint; //Можем ли спринтовать стрейфя
	public bool canCrouchStrafeIdle; //Можем ли сидя стоять стрейфя
	public bool canCrouchStrafeWalk; //Можем ли перемещаться
	public bool canCrouchStrafeRun; //Можем ли перемещаться
	public bool canCrouchStrafeSprint; //Можем ли перемещаться
	public bool canJump; //Можем ли перемещаться
	public bool canGetAngle; //Можем ли перемещаться

	#region Commandos
	#region Beret
	public bool canKillKnife;
	/*public bool can
	public bool
	public bool
	public bool
	public bool
	public bool
	public bool
	public bool
	public bool
	public bool
	public bool
	public bool*/
	#endregion


	#endregion
	#endregion
	#endregion
	#region Methods
	#region Animator
	protected void SetAnimationBools()
	{
		anim.SetBool ("isStrafing", isStrafing);
	}
	#region Free
	protected void GetMoveAnimation()
	{
		anim.SetFloat ("moveSpeed", currentSpeed);
		anim.SetFloat ("moveHeight", (charCtrl.height - 1.2f) / 0.7f);
	}
	protected void GetLean()
	{
		if (moveVector != Vector3.zero)
			anim.SetFloat ("moveLean", Mathf.Lerp (anim.GetFloat ("moveLean"), currentXAngle / 45.0f, leanLerpSpeed * Time.deltaTime));
		else
			anim.SetFloat ("moveLean", Mathf.Lerp (anim.GetFloat ("moveLean"), 0, leanLerpSpeed * Time.deltaTime));
	}
	protected void GetWeaponAnimation()
	{
		anim.SetBool ("isTwoArmed", isTwoArmed);
		anim.SetBool ("isOneArmed", isOneArmed);
		if (isTwoArmed) 
		{
			if (isFreeMoving)
				anim.SetLayerWeight (1, 1);
			else
				anim.SetLayerWeight (1, 0);
		} 
		else 
		{
			anim.SetLayerWeight (1, 0);
		}
	}
	#endregion
	#region Strafe
	protected void GetStrafeFloats(float horizontal,float vertical)
	{
		anim.SetFloat ("moveX", Mathf.Lerp (anim.GetFloat ("moveX"), horizontal, speedLerpSpeed * Time.deltaTime));
		anim.SetFloat ("moveY", Mathf.Lerp (anim.GetFloat ("moveY"), vertical, speedLerpSpeed * Time.deltaTime));
	}
	protected void GetVerticalAim(Transform one,Transform two)
	{
		//currentYAngle = GetAngle(one.forward, two.forward);
		Debug.Log (one.localEulerAngles.x);
		if (one.localEulerAngles.x > 71)
			currentYAngle = -one.localEulerAngles.x + 360.0f;
		else
			currentYAngle = -one.localEulerAngles.x;
		//currentYAngle = Vector3.Angle (one.forward, two.forward);
		//currentYAngle *= Mathf.Sign (one.forward.z * two.forward.x - one.forward.x * two.forward.z);
		anim.SetFloat ("aimRotation", currentYAngle);
	}
	#endregion
	#endregion
	#region Controller
	protected void CheckHealth()
	{
		if (health <= 0) 
		{
			anim.enabled = false;
			charCtrl.enabled = false;
			spine1.GetComponent<Rigidbody> ().isKinematic = false;
			spine2.GetComponent<Rigidbody> ().isKinematic = false;
			leftLeg.GetComponent<Rigidbody> ().isKinematic = false;
			leftKnee.GetComponent<Rigidbody> ().isKinematic = false;
			rightLeg.GetComponent<Rigidbody> ().isKinematic = false;
			rightKnee.GetComponent<Rigidbody> ().isKinematic = false;
			leftArm.GetComponent<Rigidbody> ().isKinematic = false;
			leftElbow.GetComponent<Rigidbody> ().isKinematic = false;
			rightArm.GetComponent<Rigidbody> ().isKinematic = false;
			rightElbow.GetComponent<Rigidbody> ().isKinematic = false;
			head.GetComponent<Rigidbody> ().isKinematic = false;
			isAlive = false;
		}
	}
	protected void SetDead()
	{
		anim.enabled = false;
		charCtrl.enabled = false;
		spine1.GetComponent<Rigidbody> ().isKinematic = false;
		spine2.GetComponent<Rigidbody> ().isKinematic = false;
		leftLeg.GetComponent<Rigidbody> ().isKinematic = false;
		leftKnee.GetComponent<Rigidbody> ().isKinematic = false;
		rightLeg.GetComponent<Rigidbody> ().isKinematic = false;
		rightKnee.GetComponent<Rigidbody> ().isKinematic = false;
		leftArm.GetComponent<Rigidbody> ().isKinematic = false;
		leftElbow.GetComponent<Rigidbody> ().isKinematic = false;
		rightArm.GetComponent<Rigidbody> ().isKinematic = false;
		rightElbow.GetComponent<Rigidbody> ().isKinematic = false;
		head.GetComponent<Rigidbody> ().isKinematic = false;
		isAlive = false;
	}
	protected void CheckStates()
	{
		isGrounded = charCtrl.isGrounded;
		isStanding = !isCrouching;
		isFreeMoving = !isStrafing;
	}
	protected void GetAngle()
	{
		if (canGetAngle) 
		{
			currentXAngle = GetAngle (anim.transform.forward, moveVector);
			currentYAngle = GetAngle (anim.transform.forward, pivot.forward);
		} 
		else 
		{
			currentXAngle = 0;
			currentYAngle = 0;
		}
	}
	protected void GetSpeed(Vector3 vector)
	{
		if (isStrafing) 
		{
			if (!isCrouching) 
			{
				goalSpeed = strafeIdleSpeed;
				if (vector != Vector3.zero) 
				{
					if (canStrafeWalk)
						goalSpeed = strafeWalkSpeed;
				}
				if (Input.GetButton ("Run")) 
				{
					if (canStrafeRun)
						goalSpeed = strafeRunSpeed;
				}
				if (Input.GetButton ("Sprint")) 
				{
					if (canStrafeSprint)
						goalSpeed = strafeSprintSpeed;
				}
				if (vector == Vector3.zero) 
				{
					if (canIdle)
						goalSpeed = strafeIdleSpeed;
				}
			} 
			else 
			{
				goalSpeed = crouchStrafeIdleSpeed;
				if (vector != Vector3.zero) 
				{
					if(canCrouchStrafeWalk)
						goalSpeed = crouchStrafeWalkSpeed;
				}
				if (Input.GetButton ("Run")) 
				{
					if (canCrouchStrafeRun)
						goalSpeed = crouchStrafeRunSpeed;
				}
				if (Input.GetButton ("Sprint")) 
				{
					if (canCrouchStrafeSprint)
						goalSpeed = crouchStrafeSprintSpeed;
				}
				if (vector == Vector3.zero) 
				{
					if (canCrouchStrafeIdle)
						goalSpeed = crouchStrafeIdleSpeed;
				}
			}
		}
		else if (isFreeMoving) 
		{
			if (!isCrouching) 
			{
				goalSpeed = idleSpeed;
				if (vector != Vector3.zero) 
				{
					if (canWalk)
						goalSpeed = walkSpeed;
				}
				if (Input.GetButton ("Run")) 
				{
					if (canRun)
						goalSpeed = runSpeed;
				}
				if (Input.GetButton ("Sprint")) 
				{
					if (canSprint)
						goalSpeed = sprintSpeed;
				}
				if (vector == Vector3.zero) 
				{
					if (canIdle)
						goalSpeed = idleSpeed;
				}
			} 
			else 
			{
				goalSpeed = crouchIdleSpeed;
				if (vector != Vector3.zero) 
				{
					if(canWalk)
						goalSpeed = crouchWalkSpeed;;
				}
				if (Input.GetButton ("Run")) 
				{
					if (canCrouchRun)
						goalSpeed = crouchRunSpeed;
				}
				if (Input.GetButton ("Sprint")) 
				{
					if (canCrouchSprint)
						goalSpeed = crouchSprintSpeed;
				}
				if (vector == Vector3.zero) 
				{
					if(canCrouchIdle)
						goalSpeed = crouchIdleSpeed;
				}
			}
		}
		currentSpeed = Mathf.Lerp (currentSpeed, goalSpeed, Time.deltaTime * speedLerpSpeed);
	}
	protected void RegulateCrouch()
	{
		if (isCrouching) 
		{
			charCtrl.height = Mathf.Lerp (charCtrl.height, crouchHeight, heightChangeSpeed * Time.deltaTime);
			charCtrl.center = new Vector3 (charCtrl.center.x, Mathf.Lerp (charCtrl.center.y, crouchY, heightChangeSpeed * Time.deltaTime), charCtrl.center.z);
			isCrouching = true;
		} 
		else 
		{
			charCtrl.height = Mathf.Lerp (charCtrl.height, standHeight, heightChangeSpeed * Time.deltaTime);
			charCtrl.center = new Vector3 (charCtrl.center.x, Mathf.Lerp (charCtrl.center.y, standY, heightChangeSpeed * Time.deltaTime), charCtrl.center.z);
			isCrouching = false;
		}
	}
	protected void Gravity()
	{
		if (!charCtrl.isGrounded)
			currentGravity = Mathf.Lerp (currentGravity, fallGravity, gravityLerpSpeed * Time.deltaTime);
		else
			currentGravity = Mathf.Lerp (currentGravity, standGravity, gravityLerpSpeed * Time.deltaTime);
		charCtrl.Move (Vector3.down * currentGravity * Time.deltaTime);
	}
	protected void Jump()
	{
		if (canJump)
		{
			if (Input.GetButtonDown ("Jump")) 
			{
				if (isGrounded && isFreeMoving) 
				{
					currentGravity = jumpSpeed;
					isInAir = true;
				}
			} 
		}
	}
	protected void RegulateStates()
	{
		if (isGrounded) 
		{
			if (Input.GetButtonDown ("Weapon")) 
			{
				if (!isWeaponChanging && !isReloading) 
				{
					isOneArmed = !isOneArmed;
					isTwoArmed = !isTwoArmed;
				}
			}
			if (Input.GetButton ("Fire2"))
				isStrafing = true;
			else
				isStrafing = false;

			if (Input.GetButtonDown ("Crouch")) 
			{
				if (isCrouching) 
				{
					if (!Physics.SphereCast (new Ray (transform.position, Vector3.up), 0.4f, 1.5f)) 
					{
						if (canUnCrouch)
							isCrouching = false;
					}
				}
				else if (canCrouch)
					isCrouching = true;
			}
		}
	}

	#region Free
	public void Rotate()
	{
		if (canRotate) 
		{
			if (moveVector != Vector3.zero)
				anim.transform.rotation = Quaternion.Lerp (anim.transform.rotation, Quaternion.LookRotation (moveVector, charCtrl.transform.up), Time.deltaTime * rotSpeed);
		}
	}
	protected void Move()
	{
		if (canMove)
			charCtrl.Move (anim.transform.forward * currentSpeed * Time.deltaTime);
	}
	#endregion
	#region Strafe
	public void StrafeRotate(Transform goal)
	{
		if (canRotate)
			anim.transform.rotation = Quaternion.Lerp (anim.transform.rotation, Quaternion.LookRotation (goal.forward, goal.up), Time.deltaTime * strafeRotSpeed);
	}
	protected void StrafeMove(Vector3 vector)
	{
		if (canMove)
			charCtrl.Move (vector * currentSpeed * Time.deltaTime);
	}
	#endregion
	#endregion
	#region Math
	protected Vector3 GetVectorFromKeyboard()
	{
		Vector3 vector = new Vector3 (0, 0, 0);
		vector += Input.GetAxisRaw ("Horizontal") * Vector3.right;
		vector += Input.GetAxisRaw ("Vertical") * Vector3.forward;
		return vector;
	}
	protected Vector3 GetNormalizedVectorFromKeyboard()
	{
		Vector3 vector = new Vector3 (0, 0, 0);
		vector += Input.GetAxisRaw ("Horizontal") * Vector3.right;
		vector += Input.GetAxisRaw ("Vertical") * Vector3.forward;
		vector.Normalize ();
		return vector;
	}
	protected Vector3 GetRelativeVector(Vector3 vector,Transform pivot)
	{
		return pivot.TransformDirection (vector);
	}
	protected float GetAngle(Vector3 a, Vector3 b)
	{
		float angle;
		angle = Vector3.Angle (a, b);
		angle *= Mathf.Sign (a.z * b.x - a.x * b.z);
		return angle;
	}
	#endregion
	protected void DrawLines()
	{
		Debug.DrawRay (transform.position, Vector3.down * charCtrl.skinWidth + Vector3.down * 0.05f, Color.red);
		Debug.DrawRay (transform.position, Vector3.up, Color.yellow);
	}
	#endregion
}