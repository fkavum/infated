using UnityEngine;
using System.Collections;
using Infated.Tools;

namespace Infated.CoreEngine
{	

	/// <summary>
	/// Add this class to a character so it can use weapons
	/// Note that this component will trigger animations (if their parameter is present in the Animator), based on 
	/// the current weapon's Animations
	/// Animator parameters : defined from the Weapon's inspector
	/// </summary>
	public class CharacterAttack : CharacterAbility 
	{

		/// This method is only used to display a helpbox text at the beginning of the ability's inspector
		public override string HelpBoxText() { return "This component will allow your character to pickup and use weapons. What the weapon will do is defined in the Weapon classes. This just describes the behaviour of the 'hand' holding the weapon, not the weapon itself. Here you can set an initial weapon for your character to start with, allow weapon pickup, and specify a weapon attachment (a transform inside of your character, could be just an empty child gameobject, or a subpart of your model."; }

		public GameObject weapon;
		public int Damage = 10;
		public ChargingMagicType Buff = ChargingMagicType.NONE;
		private BoxCollider2D _damageAreaCollider;
        public Animator CharacterAnimator { get; set; }
        private bool attacking = false;

		private CharacterStamina _characterStamina = null;
		// Initialization
		protected override void Initialization () 
		{
			base.Initialization();
			_characterStamina = GetComponent<CharacterStamina>();
			Setup ();
		}

		/// <summary>
		/// Grabs various components and inits stuff
		/// </summary>
		public virtual void Setup()
		{
			_character = gameObject.GetComponentNoAlloc<Character> ();
            CharacterAnimator = _animator;
			_damageAreaCollider = weapon.GetComponent<BoxCollider2D>();
        }

		/// <summary>
		/// Every frame we check if it's needed to update the ammo display
		/// </summary>
		public override void ProcessAbility()
		{
			base.ProcessAbility ();
		}

		/// <summary>
		/// Gets input and triggers methods based on what's been pressed
		/// </summary>
		protected override void HandleInput ()
		{			

			if ((_inputManager.ShootAxis == InfInput.ButtonStates.ButtonDown))
			{
                ShootStart();
			}

            if ((_inputManager.ShootAxis == InfInput.ButtonStates.ButtonUp))
            {
                ShootStop();
            }        
        }
						
		/// <summary>
		/// Causes the character to start shooting
		/// </summary>
		public virtual void ShootStart()
		{
			// if the Shoot action is enabled in the permissions, we continue, if not we do nothing.  If the player is dead we do nothing.
			if ( !AbilityPermitted
				|| (_condition.CurrentState != CharacterStates.CharacterConditions.Normal)
				|| (_movement.CurrentState == CharacterStates.MovementStates.LadderClimbing))
			{
				return;
			}

            attacking = true;
			_characterStamina.spendStamina(5.0f);
		}
		
		/// <summary>
		/// Causes the character to stop shooting
		/// </summary>
		public virtual void ShootStop()
		{
			// if the Shoot action is enabled in the permissions, we continue, if not we do nothing
			if (!AbilityPermitted
                || (_movement == null))
			{
				return;		
			}		

            attacking = false;
		}

		protected override void InitializeAnimatorParameters()
		{
			RegisterAnimatorParameter ("Attacking", AnimatorControllerParameterType.Bool);
		}

		/// <summary>
		/// Override this to send parameters to the character's animator. This is called once per cycle, by the Character
		/// class, after Early, normal and Late process().
		/// </summary>
		public override void UpdateAnimator()
		{
		    InfAnimator.UpdateAnimatorBool(_animator,"Attacking", attacking,_character._animatorParameters);
		}

		public void SpendStamina(float amount){
			float x = _characterStamina.spendStamina(amount);
			if(x == 0){
				attacking = false;
			}
		}
        protected override void OnHit()
        {
            base.OnHit();
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            ShootStop();
        }
		void Update(){

		}



		private void CanHurt(float canhurt){
			if(canhurt > 0)
				weapon.GetComponent<AttackHitCheck>().canHurt = true;
			else
				weapon.GetComponent<AttackHitCheck>().canHurt = false;
		}
    }
}