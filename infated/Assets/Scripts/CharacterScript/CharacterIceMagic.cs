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
                particle.gameObject.SetActive(false);
            }
            else if(Mana.getChargingMagicType() == ChargingMagicType.ICE){
                particle.gameObject.SetActive(true);
                UpdateGuiValues();
            }
            else{
                
            }
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
            if(_inputManager.EnchantButton.State.CurrentState == InfInput.ButtonStates.ButtonDown){
                if(Mana.CurrentChargeType == ChargingMagicType.ICE) base.BuffSword(ChargingMagicType.ICE);
                EndMagicCharge();
            }

            if(Mana.Charging && _inputManager.ActionButton.State.CurrentState == InfInput.ButtonStates.ButtonDown 
            && _character.MovementState.CurrentState != CharacterStates.MovementStates.Casting){
                if(ChargedAmount > MinimumCharge) {
                    
                    if (_userProfiler != null) { 
                    _userProfiler.profileIceMagic(ChargedAmount);
                    }
                    Decharge();
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
            base.InitializeAnimatorParameters();
            RegisterAnimatorParameter("IceMagicCharging", AnimatorControllerParameterType.Bool);
        }
        public override void UpdateAnimator()
        {
            bool thisCharged = false;
            if(ChargedAmount > 0.0f)
                thisCharged = true;

            InfAnimator.UpdateAnimatorBool(_animator, "IceMagicCharging", Mana.Charging && thisCharged,_character._animatorParameters);
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
            Debug.Log("My Pos: " + position.x + " " + position.y);
            if(_character.IsFacingRight){
                position.x += BlockOffset;
            }
            else{
                position.x -= BlockOffset;
            }

            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down);
            if(hit.collider != null){
                Debug.Log("Target Pos: " + hit.collider.transform.position.x + " " + hit.collider.transform.position.y);
                Vector2 target = hit.point;
                GameObject iceBlockInstance = Instantiate(IceBlock, target, Quaternion.identity);
                IceBlockConcatanate iceBlockScript = iceBlockInstance.GetComponent<IceBlockConcatanate>();
                iceBlockScript.amount = (int)Mathf.Floor((ChargedAmount / MaxChargeAmount) * 3);

                //iceBlockInstance.transform.localScale = new Vector3(1, height, 1);
                ChargedAmount = 0;
            }
            EndMagicCharge();
        }
    }
}
