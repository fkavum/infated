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
    public class CharacterIceMagic : CharacterAbility
    {
        /// This method is only used to display a helpbox text at the beginning of the ability's inspector
        public override string HelpBoxText() { return "This component handles ice magic. Holding down the ice button charges ice magic power."; }

        private CharacterMana Mana;
        //private ParticleSystem IceParticles;
        public float DeltaCharge = 2.5f;
        public float MaxChargeAmount = 100.0f;
        public GameObject IceBlock;
        public int BlockOffset = 1;
        public float MinimumCharge = 5.0f;
        public float MinimumBlockHeight = 0.5f;
        public float MaximumBlockHeight = 2.0f;
        private float ChargedAmount = 0.0f;
        private bool Charging = false;


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
            if(Charging && _inputManager.ActionButton.State.CurrentState == InfInput.ButtonStates.ButtonDown){
                if(ChargedAmount > MinimumCharge) {
                    if (_userProfiler != null) { 
                    _userProfiler.profileIceMagic(ChargedAmount);
                    }
                    CreateIceBlock();
                    
                }
            }
        }

        /// <summary>
        /// Every frame we perform a number of checks related to jump
        /// </summary>
        public override void ProcessAbility()
        {
            if (!AbilityPermitted || !Charging) { return; }
            
            base.ProcessAbility();

            if (ChargedAmount < MaxChargeAmount)
            {
                if(Mana.spendMana(DeltaCharge))
                    Charge();
            }
            //Spawn particles at each loop, scaled with the ChargedAmount

        }

        void Update(){
             UpdateGuiValues();
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
            Mana.setCharging(true);
            //IceParticles.emit()
            UpdateGuiValues();
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
            Mana.setCharging(false);
            UpdateGuiValues();
        }
        private void Charge(){
            if(ChargedAmount == MaxChargeAmount) return;
            
            ChargedAmount += DeltaCharge;
            if(ChargedAmount > MaxChargeAmount){
                ChargedAmount = MaxChargeAmount;
            }
        }
        private void UpdateGuiValues(){
            _character.mGuiWriter.setChargeText(Mathf.Floor(ChargedAmount).ToString());
            _character.mGuiWriter.setManaText(Mana.getManaGuiString());            
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
        }
    }
}
