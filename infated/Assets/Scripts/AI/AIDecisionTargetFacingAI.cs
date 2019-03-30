using Infated.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Infated.CorgiEngine
{
    /// <summary>
    /// This decision will return true if the Brain's current target is facing this character.
    /// </summary>
    public class AIDecisionTargetFacingAI : AIDecision
    {
        protected Character _targetCharacter;

        /// <summary>
        /// On Decide we check whether the Target is facing us
        /// </summary> s
        /// <returns></returns>
        public override bool Decide()
        {
            return EvaluateTargetFacingDirection();
        }

        /// <summary>
        /// Returns true if the Brain's Target is facing us (this will require that the Target has a Character component)
        /// </summary>
        /// <returns></returns>
        protected virtual bool EvaluateTargetFacingDirection()
        {
            if (_brain.Target == null)
            {
                return false;
            }

            _targetCharacter = _brain.Target.gameObject.GetComponentNoAlloc<Character>();
            if (_targetCharacter != null)
            {
                if (_targetCharacter.IsFacingRight && (this.transform.position.x > _targetCharacter.transform.position.x))
                {
                    return true;
                }
                if (!_targetCharacter.IsFacingRight && (this.transform.position.x < _targetCharacter.transform.position.x))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
