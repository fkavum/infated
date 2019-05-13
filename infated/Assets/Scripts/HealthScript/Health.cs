using UnityEngine;
using System.Collections;
using Unity.Collections;
using Infated.Tools;

namespace Infated.CoreEngine
{
	/// <summary>
	/// This class manages the health of an object, pilots its potential health bar, handles what happens when it takes damage,
	/// and what happens when it dies.
	/// </summary>
	public class Health : MonoBehaviour
	{
		/// the current health of the character
		[ReadOnly]

		public GameObject mask;
		public bool isMask = false;
		public int CurrentHealth;
		
		public int Armor;
        public float CurrentHealthInPercentage;
        public bool Invulnerable = false;

		public AudioClip blockSound;
		public AudioClip damageSound;
		[Header("Health")]
		//[Information("Add this component to an object and it'll have health, will be able to get damaged and potentially die.",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		/// the initial amount of health of the object
	    public int InitialHealth = 10;
	    /// the maximum amount of health of the object
	    public int MaximumHealth = 10;
		public int InitialArmor = 0;
		public int MaximumArmor = 8;

		public GameObject armors = null;
		public bool isBlocking = false;

		private CharacterStamina _stamina;

		public float blockStaminaSpend = 10.0f;

        [Header("Damage")]
		//[Information("Here you can specify an effect and a sound FX to instantiate when the object gets damaged, and also how long the object should flicker when hit (only works for sprites).",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		/// the effect that will be instantiated everytime the character touches the ground
		public GameObject DamageEffect;
		// the sound to play when the player gets hit
		public AudioClip DamageSfx;
		// should the sprite (if there's one) flicker when getting damage ?
		public bool FlickerSpriteOnHit = true;
        // whether or not this object can get knockback
        public bool ImmuneToKnockback = false;

		[Header("Death")]
		//[Information("Here you can set an effect to instantiate when the object dies, a force to apply to it (corgi controller required), how many points to add to the game score, if the device should vibrate (only works on iOS and Android), and where the character should respawn (for non-player characters only).",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		/// the effect to instantiate when the object gets destroyed
		public GameObject DeathEffect;
		/// if this is not true, the object will remain there after its death
		public bool DestroyOnDeath = true;
		/// the time (in seconds) before the character is destroyed or disabled
		public float DelayBeforeDestruction = 0f;
		/// if this is true, collisions will be turned off when the character dies
		public bool CollisionsOffOnDeath = true;
		/// the force applied when the character dies
		public Vector2 DeathForce = new Vector2(0,10);
		/// the points the player gets when the object's health reaches zero
		public int PointsWhenDestroyed;
		/// if true, the handheld device will vibrate when the object dies
		public bool VibrateOnDeath;
		/// if this is set to false, the character will respawn at the location of its death, otherwise it'll be moved to its initial position (when the scene started)
		public bool RespawnAtInitialLocation = false;

        // respawn
        public delegate void OnHitDelegate();
        public OnHitDelegate OnHit;

		public delegate void OnReviveDelegate();
		public OnReviveDelegate OnRevive;

		public delegate void OnDeathDelegate();
		public OnDeathDelegate OnDeath;

		protected Vector3 _initialPosition;
		protected Color _initialColor;
		protected Color _flickerColor = new Color32(255, 20, 20, 255); 
		protected Renderer _renderer;
		protected Character _character;
		protected CoreController _controller;
	    //protected MMHealthBar _healthBar;
	    protected Collider2D _collider2D;
		protected bool _initialized = false;
		//protected AutoRespawn _autoRespawn;
		protected Animator _animator;

	    /// <summary>
	    /// On Start, we initialize our health
	    /// </summary>
	    protected virtual void Start()
	    {
			Initialization();
			InitializeSpriteColor ();
	    }

		public void SetBlocking(bool b){
			isBlocking = b;
		}
	    /// <summary>
	    /// Grabs useful components, enables damage and gets the inital color
	    /// </summary>
		protected virtual void Initialization()
		{
			_character = GetComponent<Character>();
			if (gameObject.GetComponentNoAlloc<SpriteRenderer>() != null)
			{
				_renderer = GetComponent<SpriteRenderer>();				
			}
			if (_character != null)
			{
				if (_character.CharacterModel != null)
				{
					if (_character.CharacterModel.GetComponentInChildren<Renderer> ()!= null)
					{
						_renderer = _character.CharacterModel.GetComponentInChildren<Renderer> ();	
					}
				}	
			}

			_stamina = GetComponent<CharacterStamina>();

            // we grab our animator
            if (_character != null)
            {
                if (_character.CharacterAnimator != null)
                {
                    _animator = _character.CharacterAnimator;
                }
                else
                {
                    _animator = GetComponent<Animator>();
                }
            }
            else
            {
                _animator = GetComponent<Animator>();
            }

            if (_animator != null)
            {
                _animator.logWarnings = false;
            }

			_controller = GetComponent<CoreController>();
			_collider2D = GetComponent<Collider2D>();

			_initialPosition = transform.position;
			_initialized = true;
			CurrentHealth = InitialHealth;
			Armor = InitialArmor;
            DamageEnabled();
			UpdateHealthBar (false);
			UpdateArmorBar();
		}

		/// <summary>
		/// Stores the inital color of the Character's sprite.
		/// </summary>
		protected virtual void InitializeSpriteColor()
		{
            if (!FlickerSpriteOnHit)
            {
                return;
            }
			if (_renderer != null)
			{
				if (_renderer.material.HasProperty("_Color"))
				{
					_initialColor = _renderer.material.color;
				}
			}
		}

		/// <summary>
		/// Restores the original sprite color
		/// </summary>
		protected virtual void ResetSpriteColor()
		{
			if (_renderer != null)
			{
				if (_renderer.material.HasProperty("_Color"))
				{
					_renderer.material.color = _initialColor;
				}
			}
		}

		/// <summary>
		/// Called when the object takes damage
		/// </summary>
		/// <param name="damage">The amount of health points that will get lost.</param>
		/// <param name="instigator">The object that caused the damage.</param>
		/// <param name="flickerDuration">The time (in seconds) the object should flicker after taking the damage.</param>
		/// <param name="invincibilityDuration">The duration of the short invincibility following the hit.</param>
		public virtual void Damage(int damage,GameObject instigator, float flickerDuration, float invincibilityDuration, bool ignoreArmor = false)
		{
			// if the object is invulnerable, we do nothing and exit
			if (Invulnerable)
			{
				return;
			}

			// if we're already below zero, we do nothing and exit
			if ((CurrentHealth <= 0) && (InitialHealth != 0))
			{
				return;
			}

			if(isBlocking){
				PlaySound(blockSound);
				DamageDisabled();
				StartCoroutine(DamageEnabled(invincibilityDuration));
				Vector2 knockbackForce = new Vector2(3.0f, 0.0f);	
				if(_character.IsFacingRight) knockbackForce.x *= -1;
				_controller.SetForce(knockbackForce);	
				_stamina.spendStamina(blockStaminaSpend);
				return;
			}
			// we decrease the character's health by the damage
			float previousHealth = CurrentHealth;
			float rng = Random.Range(0.0f, 1.0f);
			ignoreArmor = (rng > 0.75f) ? true : ignoreArmor;
			CurrentHealth -= (ignoreArmor) ? damage : (damage - Armor);
			PlaySound(damageSound);
            if (OnHit != null)
            {
                OnHit();
            }

			if (CurrentHealth < 0)
			{
				CurrentHealth = 0;
			}

			// we prevent the character from colliding with Projectiles, Player and Enemies
			if (invincibilityDuration > 0)
			{
				DamageDisabled();
				StartCoroutine(DamageEnabled(invincibilityDuration));	
			}

			// we trigger a damage taken event
            // TODO:: Event Manager will do something
			//MMEventManager.TriggerEvent(new MMDamageTakenEvent(_character, instigator, CurrentHealth, damage, previousHealth));

			if (_animator != null)
			{
				_animator.SetTrigger ("Damage");	
			}

			// we play the sound the player makes when it gets hit
			PlayHitSfx();
					
			// When the character takes damage, we create an auto destroy hurt particle system
	        if (DamageEffect!= null)
	        { 
	    		Instantiate(DamageEffect,transform.position,transform.rotation);
	        }

			if (FlickerSpriteOnHit && !isBlocking)
			{
				// We make the character's sprite flicker
				if (_renderer != null)
				{
					StartCoroutine(Blink(flickerDuration, 0.05f, 0.05f));	
				}	
			}

            //CharacterStates.CharacterTypes.AI
            if (_character.CharacterType == CharacterStates.CharacterTypes.Player)
            {
                UpdateHealthBar(true);
            }

			// if health has reached zero
			if (CurrentHealth <= 0)
			{
				// we set its health to zero (useful for the healthbar)
				CurrentHealth = 0;
				if (_character != null)
				{
                    //Debug.Log("Character's Health is " + CurrentHealth);

					//if (_character.CharacterType == Character.CharacterTypes.Player)
					//{
					//	LevelManager.Instance.KillPlayer(_character);
					//	return;
					//}
                }

				Kill();
			}
		}
		protected virtual void PlaySound(AudioClip sfx)
        {
            // we create a temporary game object to host our audio source
            GameObject temporaryAudioHost = new GameObject("TempAudio");
            // we set the temp audio's position

            // we add an audio source to that host
            AudioSource audioSource = temporaryAudioHost.AddComponent<AudioSource>() as AudioSource;
            // we set that audio source clip to the one in paramaters
            audioSource.clip = sfx;
            // we set the audio source volume to the one in parameters
            audioSource.volume = 100;
            // we start playing the sound
            audioSource.Play();

            Destroy(temporaryAudioHost, sfx.length);
        }
		/// <summary>
		/// Kills the character, vibrates the device, instantiates death effects, handles points, etc
		/// </summary>
		public virtual void Kill()
		{

			// we prevent further damage
			DamageDisabled();

			// instantiates the destroy effect
			if (DeathEffect!=null)
			{
				GameObject instantiatedEffect=(GameObject)Instantiate(DeathEffect,transform.position,transform.rotation);
	            instantiatedEffect.transform.localScale = transform.localScale;
			}

			// Adds points if needed.
			if(PointsWhenDestroyed != 0)
			{
				// we send a new points event for the GameManager to catch (and other classes that may listen to it too)
                // TODO:
				//MMEventManager.TriggerEvent (new CorgiEnginePointsEvent (PointsMethods.Add, PointsWhenDestroyed));
			}

			if (_animator != null)
			{
				_animator.SetTrigger ("Death");	
			}

			// if we have a controller, removes collisions, restores parameters for a potential respawn, and applies a death force
			if (_controller != null)
			{
				// we make it ignore the collisions from now on
				if (CollisionsOffOnDeath)
				{
					_controller.CollisionsOff();	
					if (_collider2D != null)
					{
						_collider2D.enabled = false;
					}
				}

				// we reset our parameters
				_controller.ResetParameters();

				// we apply our death force
				if (DeathForce != Vector2.zero)
				{
					_controller.GravityActive(true);
					_controller.SetForce(DeathForce);		
				}
			}

			if (OnDeath != null)
			{
				OnDeath ();
			}

			// if we have a character, we want to change its state
			if (_character != null)
			{
				// we set its dead state to true
				_character.ConditionState.ChangeState(CharacterStates.CharacterConditions.Dead);
				_character.Reset ();

				// if this is a player, we quit here
				if (_character.CharacterType == CharacterStates.CharacterTypes.Player)
				{
					return;
				}
			}

			if (DelayBeforeDestruction > 0f)
			{
				Invoke ("DestroyObject", DelayBeforeDestruction);
			}
			else
			{
				// finally we destroy the object
				DestroyObject();	
			}
		}

	    /// <summary>
	    /// Destroys the object, or tries to, depending on the character's settings
	    /// </summary>
	    protected virtual void DestroyObject()
		{
            

            if (!DestroyOnDeath)
			{
				return;
			}
            gameObject.SetActive(false);
        }

		/// <summary>
		/// Plays a sound when the character is hit
		/// </summary>
		protected virtual void PlayHitSfx()
	    {
			if (DamageSfx!=null)
			{
				//SoundManager.Instance.PlaySound(DamageSfx,transform.position);
			}
	    }

	    /// <summary>
	    /// Resets the character's health to its max value
	    /// </summary>
	    public virtual void ResetHealthToMaxHealth()
	    {
			CurrentHealth = MaximumHealth;
			UpdateHealthBar (false);
	    }	

	    /// <summary>
	    /// Updates the character's health bar progress.
	    /// </summary>
		protected virtual void UpdateHealthBar(bool show)
        {

            if(show)
            {   
                CurrentHealthInPercentage = (float) CurrentHealth / MaximumHealth;
				
				if(isMask){
					Vector3 scale = mask.transform.localScale;
					scale.x = 40 * CurrentHealthInPercentage;
					mask.transform.localScale = scale;
				}
                else{
					_character.mGuiWriter.setHp(CurrentHealthInPercentage);
				}
                //Debug.Log(CurrentHealthInPercentage);
            }

	    }

		private void UpdateArmorBar(){
			
			if(armors == null)
				return;
			
			int armorCount = 0;
			foreach(Transform child in armors.transform){
				if(child.name == (armorCount).ToString()){
					child.GetComponent<SpriteRenderer>().enabled = true;
					//Debug.Log(child.name + " enabled.");
				}
				if(armorCount != Armor - 1)
					armorCount+=1;
			}

		}
		void Update(){
			UpdateHealthBar(true);
		}

	    /// <summary>
	    /// Prevents the character from taking any damage
	    /// </summary>
	    public virtual void DamageDisabled()
	    {
			Invulnerable = true;
	    }

	    /// <summary>
	    /// Allows the character to take damage
	    /// </summary>
	    public virtual void DamageEnabled()
	    {
	    	Invulnerable = false;
	    }

		/// <summary>
	    /// makes the character able to take damage again after the specified delay
	    /// </summary>
	    /// <returns>The layer collision.</returns>
	    public virtual IEnumerator DamageEnabled(float delay)
		{
			yield return new WaitForSeconds (delay);
			Invulnerable = false;
		}

		/// <summary>
		/// When the object is enabled (on respawn for example), we restore its initial health levels
		/// </summary>
		protected virtual void OnEnable()
		{
			CurrentHealth = InitialHealth;
			DamageEnabled();
			UpdateHealthBar(false);
		}
		IEnumerator Blink(float duration, float timeOn, float timeOff){
			float time = 0.0f;
			while (time < duration){
				_renderer.enabled = false;
				yield return new WaitForSeconds(timeOn);
				_renderer.enabled = true;
				yield return new WaitForSeconds(timeOff);
				time+= Time.deltaTime;
			}
		}
	}
}