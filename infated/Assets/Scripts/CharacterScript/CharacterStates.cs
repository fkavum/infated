using UnityEngine;
using System.Collections;

    /// <summary>
    /// The various states you can use to check if your character is doing something at the current frame
    /// by Renaud Forestié
    /// </summary>

public class CharacterStates 
	{
    /// the possible character types : player controller or AI (controlled by the computer)
    public enum CharacterTypes { Player, AI }
    /// the possible initial facing direction for your character
    public enum FacingDirections { Left, Right }
    /// the possible directions you can force your character to look at after its spawn
    public enum SpawnFacingDirections { Default, Left, Right }


    /// The possible character conditions
    public enum CharacterConditions
		{
			Normal,
			ControlledMovement,
			Frozen,
			Paused,
			Dead
		}

		/// The possible Movement States the character can be in. These usually correspond to their own class, 
		/// but it's not mandatory
		public enum MovementStates 
		{
			Null,
			Idle,
			Walking,
			Falling,
			Running,
			Crouching,
			Crawling, 
			Dashing,
			LookingUp,
			WallClinging,
			Jetpacking,
			Diving,
			Gripping,
			Dangling,
			Jumping,
			Pushing,
			DoubleJumping,
			WallJumping,
			LadderClimbing,
            SwimmingIdle,
            Gliding,
            Flying
		}
	}
