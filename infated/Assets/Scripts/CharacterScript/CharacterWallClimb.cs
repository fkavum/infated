using UnityEngine;
using System.Collections;
using Infated.Tools;

namespace Infated.CoreEngine
{
    /// <summary>
    /// Add this class to a character and it will be able to use ice magic
    /// Mana component is required
    /// </summary>
    [AddComponentMenu("Core Engine/Character/Abilities/Character Wall Climb")]
    public class CharacterWallClimb : CharacterAbility
    {
        /// This method is only used to display a helpbox text at the beginning of the ability's inspector
        public override string HelpBoxText() { return "This component is required for wall climbing."; }

        public float WallClimbDistance = 0.1f;
        public float StaminaCost = 2.0f;
        private CharacterStamina _Stamina; 
        private bool isClimbing = false;
        
        protected override void Initialization()
        {
            base.Initialization();
            _Stamina = GetComponent<CharacterStamina>();
        }

        protected override void HandleInput()
        {   

            if (_inputManager.WallClimbButton.State.CurrentState == InfInput.ButtonStates.ButtonDown)
            {
                if(CheckWallClimbConditions()){
                    StartWallClimb();
                }
            }
            if (_inputManager.WallClimbButton.State.CurrentState == InfInput.ButtonStates.ButtonUp)
            {
                StopWallClimb();
            }

        }
        public override void ProcessAbility(){
                base.ProcessAbility();
                if(!AbilityPermitted) return;

                if(CheckWallClimbConditions()){
                    _controller.SetVerticalForce(2.0f);
                }
                else if(isClimbing){
                    StopWallClimb();
                }
        }
        protected override void InitializeAnimatorParameters()
        {
            RegisterAnimatorParameter("WallClimbing", AnimatorControllerParameterType.Bool);
        }
        public override void UpdateAnimator()
        {
            InfAnimator.UpdateAnimatorBool(_animator, "WallClimbing", isClimbing, _character._animatorParameters);
        }

        void Update(){
            if(_animator.GetBool("Shapeshift")){
                AbilityPermitted = true;
            }
            else{
                StopWallClimb();
                AbilityPermitted = false;
            }

        }

        private void StartWallClimb(){
            isClimbing = true;
            _movement.ChangeState(CharacterStates.MovementStates.pWallClimbing);
            Debug.Log("Enter");
        }

        private void StopWallClimb(){
            isClimbing = false;
            _movement.ChangeState(CharacterStates.MovementStates.Idle);
            Debug.Log("Exit");
        }

        private bool CheckWallClimbConditions(){
            //Near a wall
            //Has enough stamina
            Vector3 position = _character.transform.position;
            Vector2 direction;
            if(_character.IsFacingRight){
                direction = Vector2.right;
            }
            else{
                direction = Vector2.left;
            }

            RaycastHit2D hit = Physics2D.Raycast(position, direction);
            // We spend stamina in this if condition
            if(Mathf.Abs(hit.transform.position.x - position.x) < WallClimbDistance && _Stamina.spendStamina(StaminaCost) > 0){
                return true;
            }
            else
                return false;
        }
        public override void Reset()
        {
            base.Reset();
        }
    }
}
