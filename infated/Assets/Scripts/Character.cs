using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Infated.Tools;
using Infated.CoreEngine;


public class Character : MonoBehaviour {
   
    // State Machines
    public InfStateMachine<CharacterStates.MovementStates> MovementState;
    public InfStateMachine<CharacterStates.CharacterConditions> ConditionState;

    // associated camera and input manager
    //public CameraController SceneCamera { get; protected set; }
    public InputManager LinkedInputManager { get; protected set; }

    public Animator _animator { get; protected set; }
    public List<string> _animatorParameters { get; set; }

    protected CoreController _controller;
    protected SpriteRenderer _spriteRenderer;
    protected Color _initialColor;
    protected CharacterAbility[] _characterAbilities;
    protected float _originalGravity;
    //protected Health _health;
    protected bool _spawnDirectionForced = false;
    //protected AIBrain _aiBrain;
    //protected DamageOnTouch _damageOnTouch;


    /// if this is true, the character is currently facing right
    public bool IsFacingRight { get; set; }
    public CharacterStates.CharacterTypes CharacterType = CharacterStates.CharacterTypes.AI;
    // Only used if the character is player-controlled. The PlayerID must match an input manager's PlayerID. It's also used to match Unity's input settings. So you'll be safe if you keep to Player1, Player2, Player3 or Player4
    public string PlayerID = "";
    /// the various states of the character
    public CharacterStates CharacterState { get; protected set; }

    [Header("Direction")]
    /// true if the player is facing right
    public CharacterStates.FacingDirections InitialFacingDirection = CharacterStates.FacingDirections.Right;


    [Header("Events")]
    // If this is true, the Character's state machine will emit events when entering/exiting a state
    public bool SendStateChangeEvents = false;
    public GameObject CharacterModel;
    /// if true, the model will be flipped on direction change. By default it'll be a sprite inversion, and a localscale change if you're using a model (or Spine, or anything other than 'regular' sprites)
    public bool FlipOnDirectionChange = true;
    /// the FlipValue will be used to multiply the model's transform's localscale on flip. Usually it's -1,1,1, but feel free to change it to suit your model's specs
    public Vector3 FlipValue = new Vector3(-1, 1, 1);


    protected virtual void Awake()
    {
        Initialization();
    }

    protected virtual void Initialization()
    {

        MovementState = new InfStateMachine<CharacterStates.MovementStates>(gameObject,SendStateChangeEvents);
        // returns enum as "Walking", "Idle" etc.
        ConditionState = new InfStateMachine<CharacterStates.CharacterConditions>(gameObject, SendStateChangeEvents);


        if (InitialFacingDirection == CharacterStates.FacingDirections.Left)
        {
            IsFacingRight = false;
        }
        else
        {
            IsFacingRight = true;
        }

        // we get the current input manager
        GetInputManager();
        //// we get the main camera
        //if (Camera.main == null) { return; }
        //SceneCamera = Camera.main.GetComponent<CameraController>();
        // we store our components for further use 
        CharacterState = new CharacterStates();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _controller = GetComponent<CoreController>();
        _characterAbilities = GetComponents<CharacterAbility>();
        //_aiBrain = GetComponent<AIBrain>();
        //_health = GetComponent<Health>();
        //_damageOnTouch = GetComponent<DamageOnTouch>();

        //if (CharacterAnimator != null)
        //{
        //    _animator = CharacterAnimator;
        //}
        //else
        //{
        //    _animator = GetComponent<Animator>();
        //}

        //if (_animator != null)
        //{
        //    InitializeAnimatorParameters();
        //}

        //_originalGravity = _controller.Parameters.Gravity;

        //ForceSpawnDirection();

    }


    /// <summary>
    /// Gets (if it exists) the InputManager matching the Character's Player ID
    /// </summary>
    protected virtual void GetInputManager()
    {
        if (CharacterType == CharacterStates.CharacterTypes.AI)
        {
            return;
        }

        // we get the corresponding input manager
        if (!string.IsNullOrEmpty(PlayerID))
        {
            LinkedInputManager = null;
            InputManager[] foundInputManagers = FindObjectsOfType(typeof(InputManager)) as InputManager[];
            foreach (InputManager foundInputManager in foundInputManagers)
            {
                if (foundInputManager.PlayerID == PlayerID)
                {
                    LinkedInputManager = foundInputManager;
                }
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}


    /// <summary>
    /// This is called every frame.
    /// </summary>
    protected virtual void Update()
    {
        EveryFrame();
        

    }

    /// <summary>
    /// We do this every frame. This is separate from Update for more flexibility.
    /// </summary>
    protected virtual void EveryFrame()
    {
        Debug.Log(ConditionState.CurrentState);
        HandleCharacterStatus();

        // we process our abilities
        EarlyProcessAbilities();
        ProcessAbilities();
        LateProcessAbilities();

        // we send our various states to the animator.		 
        //UpdateAnimators();

    }

    /// <summary>
    /// Calls all registered abilities' Early Process methods
    /// </summary>
    protected virtual void EarlyProcessAbilities()
    {
        foreach (CharacterAbility ability in _characterAbilities)
        {
            if (ability.enabled && ability.AbilityInitialized)
            {
                ability.EarlyProcessAbility();
            }
        }
    }
    /// <summary>
    /// Calls all registered abilities' Process methods
    /// </summary>
    protected virtual void ProcessAbilities()
    {
        foreach (CharacterAbility ability in _characterAbilities)
        {
            if (ability.enabled && ability.AbilityInitialized)
            {
                ability.ProcessAbility();
            }
        }
    }

    /// <summary>
    /// Calls all registered abilities' Late Process methods
    /// </summary>
    protected virtual void LateProcessAbilities()
    {
        foreach (CharacterAbility ability in _characterAbilities)
        {
            if (ability.enabled && ability.AbilityInitialized)
            {
                ability.LateProcessAbility();
            }
        }
    }

    /// <summary>
    /// Handles the character status.
    /// </summary>
    protected virtual void HandleCharacterStatus()
    {
        // if the character is dead, we prevent it from moving horizontally		
        if (ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead)
        {
            _controller.SetHorizontalForce(0);
            return;
        }

        // if the character is frozen, we prevent it from moving
        if (ConditionState.CurrentState == CharacterStates.CharacterConditions.Frozen)
        {
            _controller.GravityActive(false);
            _controller.SetForce(Vector2.zero);
        }
    }

    /// <summary>
    /// Flips the character and its dependencies (jetpack for example) horizontally
    /// </summary>
    public virtual void Flip(bool IgnoreFlipOnDirectionChange = false)
    {
        // if we don't want the character to flip, we do nothing and exit
        if ((!FlipOnDirectionChange) && !IgnoreFlipOnDirectionChange)
        {
            return;
        }

        // Flips the character horizontally
        if (CharacterModel != null)
        {
            CharacterModel.transform.localScale = Vector3.Scale(CharacterModel.transform.localScale, FlipValue);
        }
        else
        {
            // if we're sprite renderer based, we revert the flipX attribute
            if (_spriteRenderer != null)
            {
                _spriteRenderer.flipX = !_spriteRenderer.flipX;
            }
        }

        IsFacingRight = !IsFacingRight;

        // we tell all our abilities we should flip
        foreach (CharacterAbility ability in _characterAbilities)
        {
            if (ability.enabled)
            {
                ability.Flip();
            }
        }
    }

}
