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
    public class CharacterShapeshift : CharacterAbility
    {
        /// This method is only used to display a helpbox text at the beginning of the ability's inspector
        public override string HelpBoxText() { return "This component enables turning into a pardus."; }
        private bool PardusMode = false;

        /// <summary>
        /// On Start() we reset our number of jumps
        /// </summary>
        protected override void Initialization()
        {
            base.Initialization();
        }

        /// <summary>
        /// At the beginning of each cycle we check if we've just pressed or released the jump button
        /// </summary>
        protected override void HandleInput()
        {
            if (_inputManager.ShapeshiftButton.State.CurrentState == InfInput.ButtonStates.ButtonUp)
            {
                PardusMode = !PardusMode;
            }
        }

        /// <summary>
        /// Every frame we perform a number of checks related to jump
        /// </summary>
        public override void ProcessAbility()
        {
            if (!AbilityPermitted) { return; }
            
            base.ProcessAbility();
        }



        protected override void InitializeAnimatorParameters()
        {
            //Debug.Log("Jump Registered");
            RegisterAnimatorParameter("Shapeshift", AnimatorControllerParameterType.Bool);
        }

        /// <summary>
        /// At the end of each cycle, sends Jumping states to the Character's animator
        /// </summary>
        public override void UpdateAnimator()
        {
            InfAnimator.UpdateAnimatorBool(_animator, "Shapeshift", this.PardusMode);
        }

        /// <summary>
        /// Resets parameters in anticipation for the Character's respawn.
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            PardusMode = false;
        }
    }
}
