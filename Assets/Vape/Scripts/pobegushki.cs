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
	private bool jumpFinished = false;
	private bool secondJumpUsed = false;
	private bool secondJumpFinished = false;
	private bool jumpReleased = false;

	void Start () {
		controller = GetComponent<CharacterController>();
		cam = GameObject.Find("Camera");
	}

	void Update () {
		moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
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
		transform.rotation = Quaternion.LookRotation(moveDirectionNo_y, Vector3.up);
	}
}
