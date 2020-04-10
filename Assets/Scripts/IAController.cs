using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : MonoBehaviour {

	private enum AvatarState {Idle, Pushing, Result};
	public int animationTransitionRate; // the higher the slower
	public int gameMode = 1; //1=A, 2=B

	private int messyCounter;
	private Animator animator;
	private AvatarState state;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator> ();
		state = AvatarState.Idle;
		messyCounter = animationTransitionRate;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (messyCounter == 0) {

			if (state == AvatarState.Result) {
				Debug.Log ("is in state result");

				state = AvatarState.Idle;
			}

			if (state == AvatarState.Pushing) {
				Debug.Log ("is in state pushing");

				int result = -1;

				if (gameMode == 1) 
					result = Random.Range (0, 2);

				if (gameMode == 2) 
					result = Random.Range (0, 6);

				if (result == 0)
					animator.SetTrigger ("hasWon");
				else
					animator.SetTrigger ("hasLose");
				
				state = AvatarState.Result;

				messyCounter = animationTransitionRate;
			}

			if (state == AvatarState.Idle) {
				Debug.Log ("is in state idle");

				animator.SetTrigger ("hasPushed");
				state = AvatarState.Pushing;

				messyCounter = animationTransitionRate / 2;
			}

		} else {
			messyCounter -= 1;
		}
		//Debug.Log (messyCounter);

	}
}
