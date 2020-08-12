using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {

	public bool bIsOpen;

	// Use this for initialization
	void Start () {
		this.bIsOpen = false;
	}
	
	// Update is called once per frame
	void Update () {
		// Nothing to do here
	}

	public void OpenFront() {
		if (!this.bIsOpen) {
				// Play animation for open the front door
				this.animation.Play ("CratePlateOpen");

				// Play sound
				this.audio.Play ();

				this.bIsOpen = true;
		}
	}
}
