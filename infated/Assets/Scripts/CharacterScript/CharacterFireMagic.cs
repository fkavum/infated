using UnityEngine;
using System.Collections;
using Infated.Tools;

namespace Infated.CoreEngine
{
    /// <summary>
    /// Add this class to a character and it will be able to use ice magic
    /// Mana component is required
    /// </summary>
    [AddComponentMenu("Core Engine/Character/Abilities/Character Fire Magic")]
    public class CharacterFireMagic : CharacterAbilityMagic
    {
        /// This method is only used to display a helpbox text at the beginning of the ability's inspector
        public override string HelpBoxText() { return "This component handles fire magic. Holding down the fire button charges fire magic power."; }

        public GameObject FireObject;

         public float FireOffset = 0.1f;

        void Update(){
            if(!Mana.Charging){
                UpdateGuiValues();
                particle.gameObject.SetActive(false);
            }
            else if(Mana.getChargingMagicType() == ChargingMagicType.FIRE){
                particle.gameObject.SetActive(true);
                UpdateGuiValues();
            }
            else{
                
            }
                
            
        }
        
        public override void StartMagicCharge()
        {
            base.StartMagicCharge();
            Mana.setCharging(true, ChargingMagicType.FIRE);
            //IceParticles.emit()
        }
        public override void EndMagicCharge()
        {
            base.EndMagicCharge();
            Mana.setCharging(false, ChargingMagicType.NONE);
        }
        protected override void HandleInput()
        {   

            if (_inputManager._magicCharge > 0 && !Mana.Charging) 
            {
                StartMagicCharge();
            }
            if (_inputManager._magicCharge == 0)
            {
                EndMagicCharge();
            }
            if(_inputManager.EnchantButton.State.CurrentState == InfInput.ButtonStates.ButtonDown){
                if(Mana.CurrentChargeType == ChargingMagicType.FIRE) base.BuffSword(ChargingMagicType.FIRE);
                EndMagicCharge();
            }
        
            if(Mana.Charging && _inputManager.ActionButton.State.CurrentState == InfInput.ButtonStates.ButtonDown
            && _character.MovementState.CurrentState != CharacterStates.MovementStates.Casting){
                if(ChargedAmount > MinimumCharge) {
                    /*
                    if (_userProfiler != null) { 
                    _userProfiler.profileIceMagic(ChargedAmount);
                    }
                    */
                    Decharge();
                    CreateFire();
                     
                }
            }
        }
        public override void ProcessAbility(){
            if(Mana.getChargingMagicType() == ChargingMagicType.FIRE)
                base.ProcessAbility();
        }
        protected override void InitializeAnimatorParameters()
        {
            base.InitializeAnimatorParameters();
            RegisterAnimatorParameter("FireMagicCharging", AnimatorControllerParameterType.Bool);
        }
        public override void UpdateAnimator()
        {
            bool thisCharged = false;
            if(ChargedAmount > 0.0f)
                thisCharged = true;

            InfAnimator.UpdateAnimatorBool(_animator, "FireMagicCharging", Mana.Charging && thisCharged,_character._animatorParameters);
        }
        
        protected override void UpdateGuiValues(){
            base.UpdateGuiValues();
            if(Mana.Charging)
                _character.mGuiWriter.setChargeTypeText("Fire");
            else
                _character.mGuiWriter.setChargeTypeText("Discharged");
        }

        private void CreateFire(){
             Vector3 position = _character.transform.position;
            if(_character.IsFacingRight){
                position.x += FireOffset;
            }
            else{
                position.x -= FireOffset;
            }

            GameObject FireInstance = Instantiate(FireObject, position, Quaternion.identity);

            Transform Fire = FireInstance.transform.GetChild(0);
            if(_character.IsFacingRight){
                Fire.GetComponent<SpriteRenderer>().flipX = false;            
                Fire.GetComponent<Rigidbody2D>().velocity = Vector2.right * 10;
            } 
            else{
                Fire.GetComponent<SpriteRenderer>().flipX = true; 
                Fire.GetComponent<Rigidbody2D>().velocity = Vector2.left * 10;
            }
            Fire.GetComponent<FireMagicInteractions>().isFacingRight = _character.IsFacingRight;
            
            EndMagicCharge();
        }
    }
}
