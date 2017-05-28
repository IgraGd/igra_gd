using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pobegushki : MonoBehaviour {
	public float speed = 1.0F;
	public CharacterController controller;
	private Vector3 moveDirection;
	private Vector3 moveDirectionNo_y;
	public float gravity = 20.0F;
	public GameObject cam;
	public float jumpSpeed = 8f;
	public float jumpLimit = 5f;
	public float jumpedSource_y = 0;
	public bool facingCamera = false;
	private Animator animator;
	private bool jumpFinished = false;
	private bool secondJumpUsed = false;
	private bool secondJumpFinished = false;
	private bool jumpReleased = false;
	private float v = 0f;
	private float h = 0f;
	private Vector3 camRotation;

	void Start () {
		controller = GetComponent<CharacterController>();
		cam = GameObject.Find("Camera");
		animator = GetComponent<Animator>();
	}

	void Update () {
		h = Input.GetAxisRaw("Horizontal");
		v = Input.GetAxisRaw("Vertical");

		if (v > 0.1f) {
			facingCamera = false;
		}

		if (v < -0.1f) {
			facingCamera = true;
		}

		moveDirection = new Vector3(h, 0, v);
		moveDirection = cam.transform.TransformDirection(moveDirection);
		moveDirection.y = 0.0f;
		//moveDirection = transform.TransformDirection(moveDirection);
		moveDirection.Normalize();
		if (controller.isGrounded) {
			jumpedSource_y = transform.position.y;
			jumpFinished = false;
			secondJumpFinished = false;
			secondJumpUsed = false;
			jumpReleased = false;
		}
		if (Input.GetButton("Jump")) {
			if ((jumpFinished == true) && (secondJumpFinished == false) && (jumpReleased == true)) {
				if (secondJumpUsed == false) {
					jumpedSource_y = transform.position.y;
					secondJumpUsed = true;
				} else {
					if (((jumpedSource_y+jumpLimit) > transform.position.y) && (secondJumpFinished == false)) {
						moveDirection.y = jumpSpeed;
					} else {
						secondJumpFinished = true;
					}
				}
			}
			if (((jumpedSource_y+jumpLimit) > transform.position.y) && (jumpFinished == false)) {
				moveDirection.y = jumpSpeed;
			} else {
				jumpFinished = true;
			}
		} else {
			jumpReleased = true;
			if (jumpFinished == false) {
				jumpFinished = true;
			} else {
				if ((secondJumpFinished == false) && (secondJumpUsed == true)) {
					secondJumpFinished = true;
				}
			}
		}
		moveDirection *= speed;
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
		moveDirectionNo_y = moveDirection;
		moveDirectionNo_y.y = 0f;

		if (facingCamera == true) {
			camRotation = cam.transform.TransformDirection(-Vector3.forward);
			animator.SetFloat("h", -h);
			animator.SetFloat("v", -v);
		} else {
			camRotation = cam.transform.TransformDirection(Vector3.forward);
			animator.SetFloat("h", h);
			animator.SetFloat("v", v);
		}
		camRotation.y = 0;
		transform.rotation = Quaternion.LookRotation(camRotation, Vector3.up);
	}
}
