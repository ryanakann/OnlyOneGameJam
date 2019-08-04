using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
		private PlatformerCharacter2D m_Character;
		private bool m_Jump;
		private bool m_HoldJump; 
		private bool crouch;

        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

			m_HoldJump = CrossPlatformInputManager.GetButton("Jump");
		}


        private void FixedUpdate()
        {
            // Read the inputs.
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump, m_HoldJump);
            m_Jump = false;
        }

		private void OnTriggerEnter2D (Collider2D collision) {
			if (collision.CompareTag("SlowDown")) {
				//print("Slow down!");
				crouch = true;
			}
		}

		private void OnTriggerExit2D (Collider2D collision) {
			if (collision.CompareTag("SlowDown")) {
				crouch = false;
			}
		}
	}
}
