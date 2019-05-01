﻿using UnityEngine;
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

        public float WallClimbDistance = 0.6f;
        public float StaminaCost = 2.0f;
        private CharacterStamina _Stamina; 
        private bool isClimbing = false;
        private bool isNearWall = false;
        protected override void Initialization()
        {
            base.Initialization();
            _Stamina = GetComponent<CharacterStamina>();
        }

        protected override void HandleInput()
        {   

            if (_inputManager.WallClimbButton.State.CurrentState == InfInput.ButtonStates.ButtonDown)
            {
                if(isNearWall){
                    StartWallClimb();
                }
            }
            if (_inputManager.WallClimbButton.State.CurrentState == InfInput.ButtonStates.ButtonUp)
            {
                //If we are still climbing i.e. not jumped off before releasing the button or finishing the climb
                if(isClimbing){
                    StopWallClimb();
                    _movement.ChangeState(CharacterStates.MovementStates.Landing);
                }
            }

        }
        public override void ProcessAbility(){
                base.ProcessAbility();
                if(!AbilityPermitted) 
                    return;

                if(isClimbing && isNearWall){
                    _controller.SetVerticalForce(10.0f);
                }
                else if(isClimbing && !isNearWall){
                    StopWallClimb();
                    _movement.ChangeState(CharacterStates.MovementStates.pWallClimbFinish);
                }
        }
        protected override void InitializeAnimatorParameters()
        {
            RegisterAnimatorParameter("WallClimbing", AnimatorControllerParameterType.Bool);
            RegisterAnimatorParameter("NearWall", AnimatorControllerParameterType.Bool);
            RegisterAnimatorParameter("FinishingClimbing", AnimatorControllerParameterType.Bool);
        }
        public override void UpdateAnimator()
        {
            InfAnimator.UpdateAnimatorBool(_animator, "WallClimbing", isClimbing, _character._animatorParameters);
            InfAnimator.UpdateAnimatorBool(_animator, "NearWall", isNearWall, _character._animatorParameters);
            InfAnimator.UpdateAnimatorBool(_animator, "FinishingClimbing", (_movement.CurrentState == CharacterStates.MovementStates.pWallClimbFinish), _character._animatorParameters);
        }

        void Update(){
            NearWallCheck();
            if(_movement.CurrentState == CharacterStates.MovementStates.pWallClimbFinish){
                //_controller.SetVerticalForce(-1 * _controller.Parameters.Gravity);
                if(_character.IsFacingRight)
                    _controller.SetHorizontalForce(2.0f);
                else
                    _controller.SetHorizontalForce(-2.0f);
            }
            Debug.Log(_movement.CurrentState);
        }

        private void StartWallClimb(){
            isClimbing = true;
            _movement.ChangeState(CharacterStates.MovementStates.pWallClimbing);
        }

        private void StopWallClimb(){
            isClimbing = false;
        }

        private void NearWallCheck(){
            Vector3 position = _character.transform.position;
            
            Vector2 direction;
            if(_character.IsFacingRight){
                direction = Vector2.right;
                position.x += 1;
            }
            else{
                direction = Vector2.left;
                position.x -= 1;
            }

            RaycastHit2D[] hits = Physics2D.RaycastAll(position, direction, WallClimbDistance, LayerMask.NameToLayer("Platform"), Mathf.Infinity, Mathf.Infinity);
            

            foreach(RaycastHit2D hit in hits){
                if(hit.collider != null){
                    isNearWall = true;
                    return;
                }
            }
            isNearWall = false;
            
        }
        public override void Reset()
        {
            base.Reset();
        }
    }
}