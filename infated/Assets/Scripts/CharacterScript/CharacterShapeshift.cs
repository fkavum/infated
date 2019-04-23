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
        public float _TransformingTime = 2.0f;
        private bool PardusMode = false;
        private bool isTransformingNow = false;
        private float timer = 0.0f;

        private const int NUMBER_OF_ABILITIES_TO_DISABLE = 6;        
        // Get the abilitiy references that you want to disable while shapeshifting transformation
        private CharacterAbility[] abilitiesToDisable = new CharacterAbility[NUMBER_OF_ABILITIES_TO_DISABLE];
        private bool[] abilityPrevStates = new bool[NUMBER_OF_ABILITIES_TO_DISABLE];

        /// <summary>
        /// On Start() we reset our number of jumps
        /// </summary>
        protected override void Initialization()
        {
            base.Initialization();
            abilitiesToDisable[0] = GetComponent<CharacterHorizontalMovement>();
            abilitiesToDisable[1] = GetComponent<CharacterIceMagic>();
            abilitiesToDisable[2] = GetComponent<CharacterJump>();
            //abilitiesToDisable[4] = GetComponent<CharacterWallClimb>();
            //abilitiesToDisable[5] = GetComponent<CharacterFireMagic>();
        }

        /// <summary>
        /// At the beginning of each cycle we check if we've just pressed or released the jump button
        /// </summary>
        protected override void HandleInput()
        {
            if (!isTransformingNow && _inputManager.ShapeshiftButton.State.CurrentState == InfInput.ButtonStates.ButtonUp)
            {
                StartShapeshift();
                _userProfiler.shapeShiftCount++;
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
            RegisterAnimatorParameter("Transforming", AnimatorControllerParameterType.Bool);
        }

        /// <summary>
        /// At the end of each cycle, sends Jumping states to the Character's animator
        /// </summary>
        public override void UpdateAnimator()
        {
            InfAnimator.UpdateAnimatorBool(_animator, "Shapeshift", this.PardusMode);
            InfAnimator.UpdateAnimatorBool(_animator, "Transforming", this.isTransformingNow);
        }

        /// <summary>
        /// Resets parameters in anticipation for the Character's respawn.
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            PardusMode = false;
            isTransformingNow = false;
            timer = 0.0f;
        }
        private void StartShapeshift(){
            isTransformingNow = true;
            _movement.ChangeState(CharacterStates.MovementStates.Idle);
            for(int i = 0; i < NUMBER_OF_ABILITIES_TO_DISABLE; i++){
                if(abilitiesToDisable[i]  != null){
                    abilityPrevStates[i] = abilitiesToDisable[i].AbilityPermitted;
                    abilitiesToDisable[i].PermitAbility(false);
                    abilitiesToDisable[i].Reset();
                }
            }
        }
        private void Shapeshift(){
            PardusMode = !PardusMode;
            isTransformingNow = false;
            for(int i = 0; i < NUMBER_OF_ABILITIES_TO_DISABLE; i++){
                if(abilitiesToDisable[i]  != null){
                    abilitiesToDisable[i].PermitAbility(abilityPrevStates[i]);
                }
            }
            timer = 0.0f;
        }

        void Update(){
            if(isTransformingNow == true){
                if(timer < _TransformingTime){
                    timer += Time.deltaTime;
                }
                else{
                    Shapeshift();
                }
            }
            else{
                timer = 0.0f;
            }
        }
    }
}
