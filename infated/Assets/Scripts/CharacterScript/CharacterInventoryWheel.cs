using UnityEngine;
using System.Collections;
using Infated.Tools;

namespace Infated.CoreEngine
{
    /// <summary>
    /// Add this class to a character and it will be able to use ice magic
    /// Mana component is required
    /// </summary>
    [AddComponentMenu("Core Engine/Character/Abilities/Character Ice Magic")]
    public class CharacterInventoryWheel : CharacterAbility
    {
        /// This method is only used to display a helpbox text at the beginning of the ability's inspector
        public override string HelpBoxText() { return "This component handles ice magic. Holding down the ice button charges ice magic power."; }

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
         
        }

        /// <summary>
        /// Every frame we perform a number of checks related to jump
        /// </summary>
        public override void ProcessAbility()
        {
            if (!AbilityPermitted ) { return; }
            
            base.ProcessAbility();
            //Spawn particles at each loop, scaled with the ChargedAmount

        }

        void Update(){
             UpdateGuiValues();
        }

        /// <summary>
        /// Causes the character to start jumping.
        /// </summary>
      
        /// <summary>
        /// Causes the character to stop jumping.
        /// </summary>
        
        private void UpdateGuiValues(){
           // _character.mGuiWriter.setChargeText(Mathf.Floor(ChargedAmount).ToString());
            //_character.mGuiWriter.setManaText(Mana.getManaGuiString());            
        }
        protected override void InitializeAnimatorParameters()
        {
            //Debug.Log("Jump Registered");
            //RegisterAnimatorParameter("IceMagicCast", AnimatorControllerParameterType.Bool);
        }

        /// <summary>
        /// At the end of each cycle, sends Jumping states to the Character's animator
        /// </summary>
        public override void UpdateAnimator()
        {
           // InfAnimator.UpdateAnimatorBool(_animator, "IceMagicCast", this.Charging,_character._animatorParameters);
        }

        /// <summary>
        /// Resets parameters in anticipation for the Character's respawn.
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            //ChargedAmount = 0.0f;
        }

       
    }
}
