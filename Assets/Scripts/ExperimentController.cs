using UnityEngine;
using System;
using System.Collections;

public class ExperimentController : MonoBehaviour {

	public bool SbInSensoGame;

	public int iSensoGameRound;

	public LeapButton[] pButtons; // = new LeapButton[4];

	public int SiInputCount = 0;

	public int MaxSensoRounds = 4;

	public bool bPlayedPattern;
	public bool bTouchedAllButtons;
	public bool bDeavtivateButtons;

	public int[] _buttons;

	public Canvas m_gui;
	
	
	
	
	
	public Light m_offLight;
	public Crate m_offCrate;
	public AudioClip m_offMusic;
	public AudioClip m_offSadTombolla;

	// Use this for initialization
	void Start () {
		SbInSensoGame = true;
		bPlayedPattern = false;
		bTouchedAllButtons = false;
		bDeavtivateButtons = false;
		SiInputCount = 0;
		// m_gui = this.GetComponent<Canvas> (); // ("IntroductionGui");

		iSensoGameRound = 1;
		pButtons = this.GetComponentsInChildren<LeapButton>();
		/*
		for (int i = 0; i < pButtons.Length; i++) {
						print (pButtons [i].name);
				}
*/
		StartCoroutine(this.InitializeGameWithPause (10));
	}
	
	// Update is called once per frame
	void Update () {
		if (this.SbInSensoGame) {

			if (this.SiInputCount == this.iSensoGameRound)
				this.bTouchedAllButtons = true;
			if (this.SiInputCount == this.iSensoGameRound && this.iSensoGameRound >= this.MaxSensoRounds) {
				this.SbInSensoGame = false;

				print ("Finished Senso Game");
			}

			if (this.SbInSensoGame && this.bPlayedPattern && this.bTouchedAllButtons && this.iSensoGameRound < this.MaxSensoRounds) {

				//this.PlaySensoRound();
				StartCoroutine(this.StartNextRound(++this.iSensoGameRound));
			}
		}
	}

	public IEnumerator StartNextRound(int round) {
		this.bDeavtivateButtons = true;
		this.SiInputCount = 0;
		this.iSensoGameRound = round;
		this.bPlayedPattern = false;
		this.bTouchedAllButtons = false;
		yield return new WaitForSeconds (2);

		this.PlaySensoRound ();
		}

	public void PlaySensoRound() {

			if (!this.bPlayedPattern) {
				print ("Play senso, round: " + iSensoGameRound);

			Array.Resize<int>(ref this._buttons, this.iSensoGameRound);
				//_buttons = new int[iSensoGameRound];

				// Get the order of buttons
				for (int i = 0; i < iSensoGameRound; i++) {
					// Generate random number
					int _rndNr = this.GetRandomNumber (0, 3);
					// Get the button index with the correct iDigit
					//for (int p = 0; p < this.pButtons.Length; p++) {

						//if (this.pButtons[p].iDigit == _rndNr) {
						//	this._buttons[i] = this.pButtons[p].iDigit;
						//}	
					//}
					this._buttons[i] = _rndNr;
				}

				// Play the order of the buttons
				//for (int i = 0; i < iSensoGameRound; i++) {
				// Play the sandbox method for a button, thereby only the light is activated for a short time
			this.bDeavtivateButtons = true;
				StartCoroutine (this.PlayButtonSandbox ());
				//}
				this.bPlayedPattern = true;

			} else {

				// Now the user presses the buzzers
				// Every button has a specific digit, use SiInputCount to determine which position should be 
				// observed if we are at position SiInputCount, we have to click _buttons[SiInputCount], which will be
				// compare to iDigit of every button.
			}
	}

	/**
	 * @brief
	 * Return a random int value between 'minValue' and 'maxValue'.
	 */
	private int GetRandomNumber(int minValue, int maxValue) {
		return (int)UnityEngine.Random.Range(minValue, maxValue);
	}

	private IEnumerator PlayButtonSandbox() {
		// Got the button, now let's play the sandbox animation

		///TODO: Use StartCoroutine in here!
		for (int i = 0; i < this._buttons.Length; i++) {
			for (int b = 0; b < this.pButtons.Length; b++) {
				if (this.pButtons[b].iDigit == this._buttons[i]) {
					this.pButtons[b].PlaySandbox();
				}
			}

			yield return new WaitForSeconds(1);
		}

		this.bDeavtivateButtons = false;
	}
	
	public IEnumerator InitializeGameWithPause(int seconds) {
		yield return new WaitForSeconds (seconds);

		this.m_gui.enabled = false;

		yield return new WaitForSeconds (2);

		this.PlaySensoRound ();
	}
}
