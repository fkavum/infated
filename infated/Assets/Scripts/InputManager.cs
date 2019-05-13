using UnityEngine;
using System.Collections;
using Infated.Tools;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Infated.CoreEngine
{	
	/// <summary>
	/// This persistent singleton handles the inputs and sends commands to the player.
	/// IMPORTANT : this script's Execution Order MUST be -100.
	/// You can define a script's execution order by clicking on the script's file and then clicking on the Execution Order button at the bottom right of the script's inspector.
	/// See https://docs.unity3d.com/Manual/class-ScriptExecution.html for more details
	/// </summary>
	[AddComponentMenu("Infated Engine/Managers/Input Manager")]
	public class InputManager : Singleton<InputManager>
	{
		[Header("Status")]
		/// set this to false to prevent input to be detected
		public bool InputDetectionActive = true;

		[Header("Player binding")]
		/// a string identifying the target player(s). You'll need to set this exact same string on your Character, and set its type to Player
		public string PlayerID = "Player1";
		/// the possible modes for this input manager
		public enum InputForcedMode { None, Mobile, Desktop }
		/// the possible kinds of control used for movement
		public enum MovementControls { Joystick, Arrows }
		[Header("Mobile controls")]
		/// if this is set to true, the InputManager will try to detect what mode it should be in, based on the current target device
		public bool AutoMobileDetection = true;
		/// use this to force desktop (keyboard, pad) or mobile (touch) mode
		public InputForcedMode ForcedMode;
		/// if this is true, mobile controls will be hidden in editor mode, regardless of the current build target or the forced mode
		public bool HideMobileControlsInEditor = false;
		/// use this to specify whether you want to use the default joystick or arrows to move your character
		public MovementControls MovementControl = MovementControls.Joystick;
        /// if this is true, we're currently in mobile mode
        public bool IsMobile = false;

		[Header("Movement settings")]
		/// If set to true, acceleration / deceleration will take place when moving / stopping
		public bool SmoothMovement=true;
		/// the minimum horizontal and vertical value you need to reach to trigger movement on an analog controller (joystick for example)
		public Vector2 Threshold = new Vector2(0.1f, 0.4f);

		public InfInput.IInfButton WallClimbButton { get; protected set; }
		public InfInput.IInfButton MovementToggleButton { get; protected set; }
		/// the Shapehsifting button (by the which if you do not understand that from the name)
		public InfInput.IInfButton ShapeshiftButton { get; protected set; }
        /// the action button (use doors, levers, interactions, cast magic)
        public InfInput.IInfButton ActionButton { get; protected set; }
        /// the jump button, used for jumps
        public InfInput.IInfButton JumpButton { get; protected set; }
		public InfInput.IInfButton EnchantButton { get; protected set; }
        /// Attacks
        public InfInput.IInfButton LightAttackButton { get; protected set; }
        public InfInput.IInfButton BlockButton { get; protected set; }
        /// the swim button, used to swim
        /// the activate button, used for interactions with zones
        public InfInput.IInfButton InteractButton { get; protected set; }
        /// the jetpack button
		/// the dash button
		public InfInput.IInfButton DashButton { get; protected set; }
		/// the pause button
		public InfInput.IInfButton PauseButton { get; protected set; }
		/// the switch weapon button
		/// the shoot axis, used as a button (non analogic)
		public InfInput.ButtonStates ShootAxis { get; protected set; }
		/// the primary movement value (used to move the character around)
		public Vector2 PrimaryMovement {get { return _primaryMovement; } }
		/// the secondary movement (usually the right stick on a gamepad), used to aim
		public Vector2 SecondaryMovement {get { return _secondaryMovement; } }

		protected List<InfInput.IInfButton> ButtonList;
        protected Vector2 _primaryMovement = new Vector2(1f,1f); // Vector2.zero;
        protected Vector2 _secondaryMovement = new Vector2(1f,1f); //Vector2.zero;
		public float _magicCharge;
		protected string _axisHorizontal;
		protected string _axisVertical;
		protected string _axisSecondaryHorizontal;
		protected string _axisSecondaryVertical;
		protected string _axisShoot;
		protected string _axisMagic;

	    /// <summary>
	    /// On Start we look for what mode to use, and initialize our axis and buttons
	    /// </summary>
	    protected virtual void Start()
		{
			//ControlsModeDetection();
			InitializeButtons ();
			InitializeAxis();
		}

        public virtual void Test(string teststring)
        {

        }

		///// <summary>
		///// Turns mobile controls on or off depending on what's been defined in the inspector, and what target device we're on
		///// </summary>
		//public virtual void ControlsModeDetection()
		//{
		//	if (GUIManager.Instance!=null)
		//	{
		//		GUIManager.Instance.SetMobileControlsActive(false);
		//		IsMobile=false;
		//		if (AutoMobileDetection)
		//		{
		//			#if UNITY_ANDROID || UNITY_IPHONE
		//			GUIManager.Instance.SetMobileControlsActive(true,MovementControl);
		//			IsMobile = true;
		//			 #endif
		//		}
		//		if (ForcedMode==InputForcedMode.Mobile)
		//		{
		//			GUIManager.Instance.SetMobileControlsActive(true,MovementControl);
		//			IsMobile = true;
		//		}
		//		if (ForcedMode==InputForcedMode.Desktop)
		//		{
		//			GUIManager.Instance.SetMobileControlsActive(false);
		//			IsMobile = false;					
		//		}
		//		if (HideMobileControlsInEditor)
		//		{
		//			#if UNITY_EDITOR
		//				GUIManager.Instance.SetMobileControlsActive(false);
		//				IsMobile = false;	
		//			#endif
		//		}
		//	}
		//}

		/// <summary>
		/// Initializes the buttons. If you want to add more buttons, make sure to register them here.
		/// </summary>
		protected virtual void InitializeButtons()
		{
			ButtonList = new List<InfInput.IInfButton> ();
	
			ButtonList.Add(WallClimbButton = new InfInput.IInfButton(PlayerID, "WallClimb", WallClimbButtonDown, WallClimbButtonPressed, WallClimbButtonUp));
			ButtonList.Add(MovementToggleButton = new InfInput.IInfButton(PlayerID, "MovementToggle", MovementToggleButtonDown, MovementToggleButtonPressed, MovementToggleButtonUp));
			ButtonList.Add(ShapeshiftButton = new InfInput.IInfButton(PlayerID, "Shapeshift", ShapeshiftButtonDown, ShapeshiftButtonPressed, ShapeshiftButtonUp));
            ButtonList.Add(LightAttackButton = new InfInput.IInfButton(PlayerID, "LightAttack", LightAttackButtonDown, LightAttackButtonPressed, LightAttackButtonUp));
            ButtonList.Add(BlockButton = new InfInput.IInfButton(PlayerID, "Block", BlockButtonDown, BlockButtonPressed, BlockButtonUp));
            ButtonList.Add(ActionButton = new InfInput.IInfButton(PlayerID, "Action", ActionButtonDown, ActionButtonPressed, ActionButtonUp));
            ButtonList.Add(JumpButton = new InfInput.IInfButton(PlayerID, "Jump", JumpButtonDown, JumpButtonPressed, JumpButtonUp));
            ButtonList.Add(EnchantButton = new InfInput.IInfButton(PlayerID, "Enchant", EnchantButtonDown, EnchantButtonPressed, EnchantButtonUp));
			ButtonList.Add(InteractButton = new InfInput.IInfButton (PlayerID, "Interact", InteractButtonDown, InteractButtonPressed, InteractButtonUp));
			ButtonList.Add(DashButton = new InfInput.IInfButton(PlayerID, "Dash", DashButtonDown, DashButtonPressed, DashButtonUp));
        	ButtonList.Add(PauseButton = new InfInput.IInfButton (PlayerID, "Pause", PauseButtonDown, PauseButtonPressed, PauseButtonUp));
		}

		/// <summary>
		/// Initializes the axis strings.
		/// </summary>
		protected virtual void InitializeAxis()
		{
			_axisHorizontal = PlayerID+"_Horizontal";
			_axisVertical = PlayerID+"_Vertical";
			_axisSecondaryHorizontal = PlayerID+"_SecondaryHorizontal";
			_axisSecondaryVertical = PlayerID+"_SecondaryVertical";
			_axisShoot = PlayerID+"_ShootAxis";
			_axisMagic = PlayerID+"_MagicCast";
		}

		/// <summary>
		/// On LateUpdate, we process our button states
		/// </summary>
		protected virtual void LateUpdate()
		{
			ProcessButtonStates();
		}

	    /// <summary>
	    /// At update, we check the various commands and update our values and states accordingly.
	    /// </summary>
	    protected virtual void Update()
		{		
			if (!IsMobile && InputDetectionActive)
			{	
				SetMovement();	
				SetSecondaryMovement ();
				SetMagic();
				SetShootAxis ();
				GetInputButtons ();
			}									
		}

		/// <summary>
		/// If we're not on mobile, watches for input changes, and updates our buttons states accordingly
		/// </summary>
		protected virtual void GetInputButtons()
		{
			foreach(InfInput.IInfButton button in ButtonList)
			{
				if (Input.GetButton(button.ButtonID))
				{
					button.TriggerButtonPressed ();
				}
				if (Input.GetButtonDown(button.ButtonID))
				{
					button.TriggerButtonDown ();
				}
				if (Input.GetButtonUp(button.ButtonID))
				{
					button.TriggerButtonUp ();
				}
			}
		}

		/// <summary>
		/// Called at LateUpdate(), this method processes the button states of all registered buttons
		/// </summary>
		public virtual void ProcessButtonStates()
		{
 
			// for each button, if we were at ButtonDown this frame, we go to ButtonPressed. If we were at ButtonUp, we're now Off
			foreach (InfInput.IInfButton button in ButtonList)
			{
				if (button.State.CurrentState == InfInput.ButtonStates.ButtonDown)
				{
					button.State.ChangeState(InfInput.ButtonStates.ButtonPressed);				
				}	
				if (button.State.CurrentState == InfInput.ButtonStates.ButtonUp)
				{
					button.State.ChangeState(InfInput.ButtonStates.Off);				
				}	
			}
		}

		/// <summary>
		/// Called every frame, if not on mobile, gets primary movement values from input
		/// </summary>
		public virtual void SetMovement()
		{
			if (!IsMobile && InputDetectionActive)
			{   
				if (SmoothMovement)
				{
					_primaryMovement.x = Input.GetAxis(_axisHorizontal);
					_primaryMovement.y = Input.GetAxis(_axisVertical);		
				}
				else
				{
					_primaryMovement.x = Input.GetAxisRaw(_axisHorizontal);
					_primaryMovement.y = Input.GetAxisRaw(_axisVertical);		
				}
			}
		}
		public virtual void SetMagic()
		{
			_magicCharge = Input.GetAxisRaw(_axisMagic);	
		}
		/// <summary>
		/// Called every frame, if not on mobile, gets secondary movement values from input
		/// </summary>
		public virtual void SetSecondaryMovement()
		{
            
			if (!IsMobile && InputDetectionActive)
			{
				if (SmoothMovement)
				{
					_secondaryMovement.x = Input.GetAxis(_axisSecondaryHorizontal);
					_secondaryMovement.y = Input.GetAxis(_axisSecondaryVertical);		
				}
				else
				{
					_secondaryMovement.x = Input.GetAxisRaw(_axisSecondaryHorizontal);
					_secondaryMovement.y = Input.GetAxisRaw(_axisSecondaryVertical);		
				}
			}
		}

		/// <summary>
		/// Called every frame, if not on mobile, gets shoot axis values from input
		/// </summary>
		protected virtual void SetShootAxis()
		{
			if (!IsMobile && InputDetectionActive)
			{
				ShootAxis = InfInput.ProcessAxisAsButton (_axisShoot, Threshold.y, ShootAxis);
			}
		}

		/// <summary>
		/// If you're using a touch joystick, bind your main joystick to this method
		/// </summary>
		/// <param name="movement">Movement.</param>
		public virtual void SetMovement(Vector2 movement)
		{
			if (IsMobile && InputDetectionActive)
			{
				_primaryMovement.x = movement.x;
				_primaryMovement.y = movement.y;	
			}
		}

		/// <summary>
		/// If you're using a touch joystick, bind your secondary joystick to this method
		/// </summary>
		/// <param name="movement">Movement.</param>
		public virtual void SetSecondaryMovement(Vector2 movement)
		{
			if (IsMobile && InputDetectionActive)
			{
				_secondaryMovement.x = movement.x;
				_secondaryMovement.y = movement.y;	
			}
		}

		/// <summary>
		/// If you're using touch arrows, bind your left/right arrows to this method
		/// </summary>
		/// <param name="">.</param>
		public virtual void SetHorizontalMovement(float horizontalInput)
		{
			if (IsMobile && InputDetectionActive)
			{
				_primaryMovement.x = horizontalInput;
			}
		}

		/// <summary>
		/// If you're using touch arrows, bind your secondary down/up arrows to this method
		/// </summary>
		/// <param name="">.</param>
		public virtual void SetVerticalMovement(float verticalInput)
		{
			if (IsMobile && InputDetectionActive)
			{
				_primaryMovement.y = verticalInput;
			}
		}

		/// <summary>
		/// If you're using touch arrows, bind your secondary left/right arrows to this method
		/// </summary>
		/// <param name="">.</param>
		public virtual void SetSecondaryHorizontalMovement(float horizontalInput)
		{
			if (IsMobile && InputDetectionActive)
			{
				_secondaryMovement.x = horizontalInput;
			}
		}

		/// <summary>
		/// If you're using touch arrows, bind your down/up arrows to this method
		/// </summary>
		/// <param name="">.</param>
		public virtual void SetSecondaryVerticalMovement(float verticalInput)
		{
			if (IsMobile && InputDetectionActive)
			{
				_secondaryMovement.y = verticalInput;
			}
        }


        public virtual void WallClimbButtonDown() { WallClimbButton.State.ChangeState(InfInput.ButtonStates.ButtonDown); }
        public virtual void WallClimbButtonPressed() { WallClimbButton.State.ChangeState(InfInput.ButtonStates.ButtonPressed); }
        public virtual void WallClimbButtonUp() { WallClimbButton.State.ChangeState(InfInput.ButtonStates.ButtonUp); }

        public virtual void MovementToggleButtonDown() { MovementToggleButton.State.ChangeState(InfInput.ButtonStates.ButtonDown); }
        public virtual void MovementToggleButtonPressed() { MovementToggleButton.State.ChangeState(InfInput.ButtonStates.ButtonPressed); }
        public virtual void MovementToggleButtonUp() { MovementToggleButton.State.ChangeState(InfInput.ButtonStates.ButtonUp); }

        public virtual void ShapeshiftButtonDown() { ShapeshiftButton.State.ChangeState(InfInput.ButtonStates.ButtonDown); }
        public virtual void ShapeshiftButtonPressed() { ShapeshiftButton.State.ChangeState(InfInput.ButtonStates.ButtonPressed); }
        public virtual void ShapeshiftButtonUp() { ShapeshiftButton.State.ChangeState(InfInput.ButtonStates.ButtonUp); }

        public virtual void BlockButtonDown() { BlockButton.State.ChangeState(InfInput.ButtonStates.ButtonDown); }
        public virtual void BlockButtonPressed() { BlockButton.State.ChangeState(InfInput.ButtonStates.ButtonPressed); }
        public virtual void BlockButtonUp() { BlockButton.State.ChangeState(InfInput.ButtonStates.ButtonUp); }

        public virtual void LightAttackButtonDown() { LightAttackButton.State.ChangeState(InfInput.ButtonStates.ButtonDown); }
        public virtual void LightAttackButtonPressed() { LightAttackButton.State.ChangeState(InfInput.ButtonStates.ButtonPressed); }
        public virtual void LightAttackButtonUp() { LightAttackButton.State.ChangeState(InfInput.ButtonStates.ButtonUp); }

        public virtual void JumpButtonDown()        { JumpButton.State.ChangeState(InfInput.ButtonStates.ButtonDown); }
        public virtual void JumpButtonPressed()     { JumpButton.State.ChangeState(InfInput.ButtonStates.ButtonPressed); }
        public virtual void JumpButtonUp()          { JumpButton.State.ChangeState(InfInput.ButtonStates.ButtonUp); }

		public virtual void EnchantButtonDown()        { EnchantButton.State.ChangeState(InfInput.ButtonStates.ButtonDown); }
        public virtual void EnchantButtonPressed()     { EnchantButton.State.ChangeState(InfInput.ButtonStates.ButtonPressed); }
        public virtual void EnchantButtonUp()          { EnchantButton.State.ChangeState(InfInput.ButtonStates.ButtonUp); }

        public virtual void ActionButtonDown() { ActionButton.State.ChangeState(InfInput.ButtonStates.ButtonDown); }
        public virtual void ActionButtonPressed() { ActionButton.State.ChangeState(InfInput.ButtonStates.ButtonPressed); }
        public virtual void ActionButtonUp() { ActionButton.State.ChangeState(InfInput.ButtonStates.ButtonUp); }

        public virtual void InteractButtonDown()	{ InteractButton.State.ChangeState (InfInput.ButtonStates.ButtonDown); }
		public virtual void InteractButtonPressed()	{ InteractButton.State.ChangeState (InfInput.ButtonStates.ButtonPressed); }
		public virtual void InteractButtonUp()		{ InteractButton.State.ChangeState (InfInput.ButtonStates.ButtonUp); }

        public virtual void DashButtonDown()        { DashButton.State.ChangeState(InfInput.ButtonStates.ButtonDown); }
        public virtual void DashButtonPressed()     { DashButton.State.ChangeState(InfInput.ButtonStates.ButtonPressed); }
        public virtual void DashButtonUp()          { DashButton.State.ChangeState(InfInput.ButtonStates.ButtonUp); }

		public virtual void PauseButtonDown()		{ PauseButton.State.ChangeState (InfInput.ButtonStates.ButtonDown); }
		public virtual void PauseButtonPressed()	{ PauseButton.State.ChangeState (InfInput.ButtonStates.ButtonPressed); }
		public virtual void PauseButtonUp()			{ PauseButton.State.ChangeState (InfInput.ButtonStates.ButtonUp); }

	}
}