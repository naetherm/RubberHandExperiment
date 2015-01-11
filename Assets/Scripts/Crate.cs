using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {

	public bool bIsOpen;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!this.bIsOpen) {
			this.OpenFront();
		}
	}

	void OpenFront() {
		// Play animation for open the front door
		this.animation.Play ("CratePlateOpen");

		// Play sound
		this.audio.Play ();

		this.bIsOpen = true;
	}
}
