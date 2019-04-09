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
    public class CharacterIceMagic : CharacterAbility
    {
        /// This method is only used to display a helpbox text at the beginning of the ability's inspector
        public override string HelpBoxText() { return "This component handles ice magic. Holding down the ice button charges ice magic power."; }

        private CharacterMana Mana;
        //private ParticleSystem IceParticles;
        public float DeltaCharge = 2.5f;
        private float ChargedAmount = 0.0f;
        private bool Charging = false;
        public float MaxChargeAmount = 20.0f;


        /// <summary>
        /// On Start() we reset our number of jumps
        /// </summary>
        protected override void Initialization()
        {
            base.Initialization();
            Mana = GetComponent<CharacterMana>();
        }

        /// <summary>
        /// At the beginning of each cycle we check if we've just pressed or released the jump button
        /// </summary>
        protected override void HandleInput()
        {
            if (_inputManager.IceMagicButton.State.CurrentState == InfInput.ButtonStates.ButtonDown)
            {
                StartMagicCharge();
            }
            if (_inputManager.IceMagicButton.State.CurrentState == InfInput.ButtonStates.ButtonUp)
            {
                EndMagicCharge();
            }
        }

        /// <summary>
        /// Every frame we perform a number of checks related to jump
        /// </summary>
        public override void ProcessAbility()
        {
            if (!AbilityPermitted) { return; }
            
            base.ProcessAbility();

            if (Mana.spendMana(DeltaCharge))
            {
                ChargedAmount += DeltaCharge;
            }
            
            //Spawn particles at each loop, scaled with the ChargedAmount

        }



        /// <summary>
        /// Causes the character to start jumping.
        /// </summary>
        public virtual void StartMagicCharge()
        {
            if (!Mana.spendMana(DeltaCharge))
            {
                return;
            }
            ChargedAmount += DeltaCharge;
            Charging = true;
            //IceParticles.emit()

            // we start our sounds
            PlayAbilityStartSfx();

        }

        /// <summary>
        /// Causes the character to stop jumping.
        /// </summary>
        public virtual void EndMagicCharge()
        {
            //IceParticles.stopEmit()
            ChargedAmount = 0.0f;
            Charging = false;
        }

        protected override void InitializeAnimatorParameters()
        {
            //Debug.Log("Jump Registered");
            RegisterAnimatorParameter("IceMagicCast", AnimatorControllerParameterType.Bool);
        }

        /// <summary>
        /// At the end of each cycle, sends Jumping states to the Character's animator
        /// </summary>
        public override void UpdateAnimator()
        {
            InfAnimator.UpdateAnimatorBool(_animator, "IceMagicCast", this.Charging,_character._animatorParameters);
        }

        /// <summary>
        /// Resets parameters in anticipation for the Character's respawn.
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            ChargedAmount = 0.0f;
        }
    }
}
