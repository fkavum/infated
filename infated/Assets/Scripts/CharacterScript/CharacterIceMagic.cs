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
    public class CharacterIceMagic : CharacterAbilityMagic
    {
        /// This method is only used to display a helpbox text at the beginning of the ability's inspector
        public override string HelpBoxText() { return "This component handles ice magic. Holding down the ice button charges ice magic power."; }

        public GameObject IceBlock;
        public int BlockOffset = 1;
        public float MinimumBlockHeight = 0.5f;
        public float MaximumBlockHeight = 2.0f;
        void Update(){
            if(!Mana.Charging){
                UpdateGuiValues();
            }
            else if(Mana.getChargingMagicType() == ChargingMagicType.ICE)
                UpdateGuiValues();
        }
        
        public override void StartMagicCharge()
        {
            base.StartMagicCharge();
            Mana.setCharging(true, ChargingMagicType.ICE);
            //IceParticles.emit()
        }
        public override void EndMagicCharge()
        {
            base.EndMagicCharge();
            Mana.setCharging(false, ChargingMagicType.NONE);
        }
        protected override void HandleInput()
        {    

            if (_inputManager._magicCharge < 0 && !Mana.Charging)
            {
                StartMagicCharge();
            }
            if (_inputManager._magicCharge == 0)
            {

                EndMagicCharge();
            }

            if(Mana.Charging && _inputManager.ActionButton.State.CurrentState == InfInput.ButtonStates.ButtonDown){
                if(ChargedAmount > MinimumCharge) {
                    
                    if (_userProfiler != null) { 
                    _userProfiler.profileIceMagic(ChargedAmount);
                    }
                    CreateIceBlock();
                     
                }
            }
        }
        public override void ProcessAbility(){
            if(Mana.getChargingMagicType() == ChargingMagicType.ICE)
                base.ProcessAbility();
        }
        protected override void InitializeAnimatorParameters()
        {
            RegisterAnimatorParameter("IceMagicCast", AnimatorControllerParameterType.Bool);
        }
        public override void UpdateAnimator()
        {
            bool thisCharged = false;
            if(ChargedAmount > 0.0f)
                thisCharged = true;
            InfAnimator.UpdateAnimatorBool(_animator, "IceMagicCast", Mana.Charging && thisCharged,_character._animatorParameters);
        }
        
        protected override void UpdateGuiValues(){
            base.UpdateGuiValues();
            if(Mana.Charging)
                _character.mGuiWriter.setChargeTypeText("Ice");
            else
                _character.mGuiWriter.setChargeTypeText("Discharged");
        }
        private void CreateIceBlock(){
            Vector3 position = _character.transform.position;
            if(_character.IsFacingRight){
                position.x += BlockOffset;
            }
            else{
                position.x -= BlockOffset;
            }

            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down);
            if(hit.collider != null){
                GameObject iceBlockInstance = Instantiate(IceBlock, hit.point, Quaternion.identity);
                float height = ((ChargedAmount - MinimumCharge) * (MaximumBlockHeight - MinimumBlockHeight) / (MaxChargeAmount - MinimumCharge)) + MinimumBlockHeight;
                
                iceBlockInstance.transform.localScale = new Vector3(1, height, 1);
                ChargedAmount = 0;
            }
            EndMagicCharge();
        }
    }
}
