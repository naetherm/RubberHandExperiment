using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LeapButton : MonoBehaviour
{
	
		Vector3 m_originalPosition; // default position of the button when not pressed.

		public ExperimentController SController;
		public AudioClip WrongSoundClip;
		public int iDigit = -1;
	
		// the furthest "down" you can push the button.
		public float m_maxDepth = 0.15f;
	
		// how far down the button has to be to be considered pressed.
		public float m_onDepth = 0.1f;
	
		// multiplier for how fast the button springs back up.
		public float m_springFactor = 0.05f;
	
		// tells us how far to go back up if there's a finger in the region
		public Vector3 m_regionCollisionPoint = Vector3.zero;
	
		// whether or not a finger/palm is colliding in the region.
		public bool m_regionCollision = false;
	
		// toggle button or held down button?
		public bool m_toggleButton = true;

	
		// For toggle buttons, tracks to see if the button is currently in the down state.
		bool m_isDown = false;
		bool m_buttonIsClicked = false;
	bool m_buttonIsDown = false;
	
		// flag to see if I can change the button state again, is reset
		// after the button passes the onDepth threshold.
		bool m_toggleAble = true;
		int m_collisionCount = 0;
	
		void Start ()
		{
				// Get the original position of the button
				m_originalPosition = transform.position;
		}

	void Update() {
		if (SController.bDeavtivateButtons)
			return;

		if (this.m_buttonIsClicked && !this.m_buttonIsDown) {
			renderer.material.shader = Shader.Find ("Self-Illumin/Diffuse");
			this.m_buttonIsDown = true;
			if (SController.SbInSensoGame) {
				
				//this.m_isDown = true;


				
				if (SController.bPlayedPattern && SController._buttons [SController.SiInputCount] != this.iDigit) { //  && !SController.bTouchedAllButtons
					// Reset the Game Controller
					audio.PlayOneShot (WrongSoundClip);

					//SController.bDeavtivateButtons = true;

					StartCoroutine(SController.StartNextRound(1));
				} else {
					audio.Play ();
					++SController.SiInputCount;
				}
				
				
			} else {
				
				this.OffGameBehaviour();
			}


		}
	}

		private bool IsHand (Collider other)
		{
				if (other.transform.parent && other.transform.parent.parent && other.transform.parent.parent.GetComponent<HandModel> ())
						return true;
				else
						return false;
		}
	
		public bool IsButtonOn ()
		{
				return m_isDown;
		}

		void OnMouseDown ()
		{
				///TODO: Not necessary anymore, but for later review.
				//transform.position += transform.forward * 2.0f;
		}
	
		void OnTriggerStay (Collider other)
		{
				if (IsHand (other)) {
						//print ("Pushed button!");

						// compute the penetration depth.
						float penDepth = Vector3.Dot (other.transform.position - transform.position, transform.up);
						penDepth = (transform.lossyScale.y + other.transform.lossyScale.y) / 2.0f - penDepth;
	
						// push downwards along the view axis.
						transform.position -= transform.up * penDepth;
	
						// compute the current depth value.
						float currentDepth = Vector3.Dot (transform.position - m_originalPosition, -transform.up);
	
						//print (currentDepth);
						//print (m_onDepth);
						if (currentDepth > m_onDepth) {
								// make the button "glow" when it's pressed down.

								//				audio.Play();
								

								this.m_buttonIsClicked = true;
				//this.m_isDown = true;
				
				// NO NEED ANYMORE
								if (m_toggleAble && m_toggleButton) {
										m_isDown = !m_isDown;
										m_toggleAble = false;
								}
						} else {
								//this.m_isDown = false;
				this.m_buttonIsClicked = false;
				this.m_buttonIsDown = false;
			}

						transform.position = m_originalPosition - transform.up * m_maxDepth;
			//m_collisionCount++;
				}
		}

	public void OffGameBehaviour() {

			switch (this.iDigit) {

				case 0: {
					SController.m_offCrate.OpenFront();
				} break;
				case 1: {
					SController.m_offLight.enabled = !SController.m_offLight.enabled;
				} break;
				case 2: {
					audio.Stop();
					audio.PlayOneShot(SController.m_offMusic);
				} break;
				case 3: {
					audio.PlayOneShot(SController.m_offSadTombolla);
				} break;
			}
		}
		
		public void PlaySandbox ()
		{ // IEnumerator
				print ("Play sandbox method!");

				renderer.material.shader = Shader.Find ("Self-Illumin/Diffuse");
		
				audio.Play ();

				StartCoroutine (this.WaitSandbox (3));
		}

		public IEnumerator WaitSandbox (int seconds)
		{
				yield return new WaitForSeconds (seconds);
		
				renderer.material.shader = Shader.Find ("Diffuse");
		}

	
		void FixedUpdate ()
		{
				// by default set the shader to not be self illuminated.
				if (!m_isDown) {
					renderer.material.shader = Shader.Find ("Diffuse");
				}

				

		
				if (m_regionCollision) {
						float penDepth = Vector3.Dot (m_regionCollisionPoint - transform.position, transform.up);
						transform.position += transform.up * penDepth;
						m_regionCollision = false;
				} else {
						transform.position += transform.up * m_springFactor;
				}
		
				// clamp to down position if button is pressed down
				if (m_isDown && Vector3.Dot (transform.position - m_originalPosition, -transform.up) < m_onDepth) {
						transform.position = m_originalPosition - transform.up * m_onDepth;
						// just traveled up and had to be clamped, which means it should be toggleable again.
				}
		
				// clamp to original position (so buttons don't levetate)
				if (Vector3.Dot (transform.position - m_originalPosition, -transform.up) < 0) {
						transform.position = m_originalPosition;	
				}

				if (transform.position == this.m_originalPosition) {
					//this.m_buttonIsClicked = false;
				}
		
				if (m_collisionCount == 0)
						m_toggleAble = true;
				m_collisionCount = 0;
		}
}