using UnityEngine;
using System.Collections;
using Infated.Tools;

namespace Infated.CoreEngine
{	
	/// <summary>
	/// Add this class to a character and it'll be able to jump
	/// Animator parameters : Jumping (bool), DoubleJumping (bool), HitTheGround (bool)
	/// </summary>
	[AddComponentMenu("Corgi Engine/Character/Abilities/Character Jump")] 
	public class CharacterBlock : CharacterAbility
	{	
		/// This method is only used to display a helpbox text at the beginning of the ability's inspector
		public override string HelpBoxText() { return "This component handles blocking"; }

		public CharacterStamina _stamina = null;
		public Health _hp = null;
		protected CharacterHorizontalMovement _characterHorizontalMovement = null;
		protected override void Initialization()
	    {
			base.Initialization();
			//_characterWallJump = GetComponent<CharacterWalljump>();
			//_characterCrouch = GetComponent<CharacterCrouch>();
			//_characterButtonActivation = GetComponent<CharacterButtonActivation>();
			_characterHorizontalMovement = GetComponent<CharacterHorizontalMovement> ();
			_stamina = GetComponent<CharacterStamina>();
			_hp = GetComponent<Health>();
		}	

        
		/// <summary>
		/// At the beginning of each cycle we check if we've just pressed or released the jump button
		/// </summary>
		protected override void HandleInput()
		{

			if (_inputManager.BlockButton.State.CurrentState == InfInput.ButtonStates.ButtonDown)
			{
				_hp.SetBlocking(true);
				_characterHorizontalMovement.MovementForbidden = true;
			}
			if (_inputManager.BlockButton.State.CurrentState == InfInput.ButtonStates.ButtonUp)
			{
				_hp.SetBlocking(false);
				_characterHorizontalMovement.MovementForbidden = false;
				_character.MovementState.ChangeState(CharacterStates.MovementStates.Idle);
			}
		}	

		
		/// <summary>
		/// Every frame we perform a number of checks related to jump
		/// </summary>
		public override void ProcessAbility()
	    {

	    }



		protected override void InitializeAnimatorParameters()
		{
            //Debug.Log("Jump Registered");
			RegisterAnimatorParameter ("Blocking", AnimatorControllerParameterType.Bool);
		}

		/// <summary>
		/// At the end of each cycle, sends Jumping states to the Character's animator
		/// </summary>
		public override void UpdateAnimator()
		{
			InfAnimator.UpdateAnimatorBool(_animator, "Blocking", _hp.isBlocking, _character._animatorParameters);
		}

		/// <summary>
		/// Resets parameters in anticipation for the Character's respawn.
		/// </summary>
		public override void Reset()
		{
			base.Reset ();
		}
	}
}
