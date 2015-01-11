using UnityEngine;
using System.Collections;

public class ExperimentController : MonoBehaviour {

	public bool SbInSensoGame;

	public int iSensoGameRound;

	private GameObject[] pButtons = new GameObject[4];

	static public int SiInputCount = 0;

	// Use this for initialization
	void Start () {
		SbInSensoGame = true;

		iSensoGameRound = 1;
		
		// Fetch all Buttons
		pButtons[0] = transform.FindChild ("RedButton").gameObject;
		pButtons[1] = transform.FindChild ("GreenButton").gameObject;
		pButtons[2] = transform.FindChild ("BlueButton").gameObject;
		pButtons[3] = transform.FindChild ("YellowButton").gameObject;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void PlaySensoRound() {
		int[] _buttons = new int[iSensoGameRound];

		// Get the order of buttons
		for (int i = 0; i < iSensoGameRound; i++) {
			_buttons[i] = this.GetRandomNumber(0, 3);
		}

		// Play the order of the buttons
		for (int i = 0; i < iSensoGameRound; i++) {
			// Play the sandbox method for a button, thereby only the light is activated for a short time
			this.PlayButtonSandbox(this.pButtons[_buttons[i]]);
		}

		// Now the user presses the buzzers
		// Every button has a specific digit, use SiInputCount to determine which position should be 
		// observed if we are at position SiInputCount, we have to click _buttons[SiInputCount], which will be
		// compare to iDigit of every button.
	}

	/**
	 * @brief
	 * Return a random int value between 'minValue' and 'maxValue'.
	 */
	private int GetRandomNumber(int minValue, int maxValue) {
		return (int)Random.Range(minValue, maxValue);
	}

	private void PlayButtonSandbox(GameObject obj) {
		// Got the button, now let's play the sandbox animation

		// Set the material
		obj.renderer.material.shader = Shader.Find("Self-Illumin/Diffuse");

		// play sound

		// Reset the material
		obj.renderer.material.shader = Shader.Find("Diffuse");
	}
}
