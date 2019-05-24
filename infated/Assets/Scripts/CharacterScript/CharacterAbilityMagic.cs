using UnityEngine;
using System.Collections;
using Infated.Tools;

namespace Infated.CoreEngine
{
    /// <summary>
    /// Add this class to a character and it will be able to use ice magic
    /// Mana component is required
    /// </summary>
    [AddComponentMenu("Core Engine/Character/Abilities/Character Ability Magic")]
    public class CharacterAbilityMagic : CharacterAbility
    {
        /// This method is only used to display a helpbox text at the beginning of the ability's inspector
        public override string HelpBoxText() { return "This component is base for ice and fire majik."; }

        public GameObject particle;
        protected CharacterMana Mana;
        protected CharacterJump _jump;
        protected CharacterHorizontalMovement _hMovement;
        protected CharacterAttack _attack;
        //private ParticleSystem IceParticles;
        public float DeltaCharge = 0.5f;
        public float MaxChargeAmount = 20.0f;
        public float MinimumCharge = 5.0f;
        public float ChargedAmount = 0.0f;
        public float BuffDuration = 0.0f;

        /// <summary>
        /// On Start() we reset our number of jumps
        /// </summary>
        protected override void Initialization()
        {
            base.Initialization();
            Mana = GetComponent<CharacterMana>();
            _jump = GetComponent<CharacterJump>();
            _hMovement = GetComponent<CharacterHorizontalMovement>();
            _attack = GetComponent<CharacterAttack>();
        }

        /// <summary>
        /// Every frame we perform a number of checks related to jump
        /// </summary>
        public override void ProcessAbility()
        {
            if (!AbilityPermitted || !Mana.Charging) { return; }
            
            base.ProcessAbility();

            if (ChargedAmount < MaxChargeAmount)
            {
                Charge(Mana.spendMana(DeltaCharge));
            }
            //Spawn particles at each loop, scaled with the ChargedAmount
            UpdateGuiValues();
        }

        void Update(){
             UpdateGuiValues();
             if(BuffDuration < 0){
                 DebuffSword();
             }
             else
                BuffDuration -= Time.deltaTime;

        }
        /// <summary>
        /// Causes the character to start jumping.
        /// </summary>
        public virtual void StartMagicCharge()
        {

            if(Mana.getChargingMagicType() != ChargingMagicType.NONE){
                return;
            }
            
            ChargedAmount += Mana.spendMana(DeltaCharge);
            Mana.Charging = true;
            //IceParticles.emit()
            // we start our sounds
            PlayAbilityStartSfx();
            UpdateGuiValues();

        }

        /// <summary>
        /// Causes the character to stop jumping.
        /// </summary>
        public virtual void EndMagicCharge()
        {
            
            //IceParticles.stopEmit()
            ChargedAmount = 0.0f;
            Mana.Charging = false;
            UpdateGuiValues();
        }

        protected void BuffSword(ChargingMagicType type){
            _attack.Buff = type;
            BuffDuration = (ChargedAmount / MaxChargeAmount) * 0.2f;
        }
        protected void DebuffSword(){
            _attack.Buff = ChargingMagicType.NONE;
        }
        private void Charge(float amount){
            if(ChargedAmount == MaxChargeAmount) return;
            
            ChargedAmount += amount;
            if(ChargedAmount > MaxChargeAmount){
                ChargedAmount = MaxChargeAmount;
            }
        }
        protected virtual void UpdateGuiValues(){
            _character.mGuiWriter.setChargeText(Mathf.Floor(ChargedAmount).ToString());
            _character.mGuiWriter.setManaText(Mana.getManaGuiString(), Mana.getManaPercentage()); 
        }
        public override void Reset()
        {
            base.Reset();
            ChargedAmount = 0.0f;
        }

        protected override void InitializeAnimatorParameters()
		{
            //Debug.Log("Jump Registered");
			RegisterAnimatorParameter ("Charging", AnimatorControllerParameterType.Bool);
            RegisterAnimatorParameter ("Casting", AnimatorControllerParameterType.Bool);
		}

		/// <summary>
		/// At the end of each cycle, sends Jumping states to the Character's animator
		/// </summary>
		public override void UpdateAnimator()
		{
	
            InfAnimator.UpdateAnimatorBool(_animator, "Charging", Mana.Charging,_character._animatorParameters);
		}

        public void Decharge(){
            InfAnimator.UpdateAnimatorBool(_animator, "Casting", true,_character._animatorParameters);
        }
        public void StoppedCast(){
            InfAnimator.UpdateAnimatorBool(_animator, "Casting", false,_character._animatorParameters);
        }

    }
}
