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

        protected CharacterMana Mana;
        //private ParticleSystem IceParticles;
        public float DeltaCharge = 0.5f;
        public float MaxChargeAmount = 20.0f;
        public float MinimumCharge = 5.0f;
        protected float ChargedAmount = 0.0f;

        /// <summary>
        /// On Start() we reset our number of jumps
        /// </summary>
        protected override void Initialization()
        {
            base.Initialization();
            Mana = GetComponent<CharacterMana>();
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
    }
}
