using UnityEngine;
using System.Collections;

public class SingleSlideDoorScript : MonoBehaviour {

	public Animator animationController;
	public bool isOpened;

	void OnEnable () 
	{
		animationController = GetComponent<Animator> ();
	}

	void Update () 
	{
		animationController.SetBool ("isOpened", isOpened);
	}
}