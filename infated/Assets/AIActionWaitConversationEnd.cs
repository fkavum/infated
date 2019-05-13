using Infated.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Infated.CorgiEngine
{
    /// <summary>
    /// This decision will return true if the Brain's current target is facing this character.
    /// </summary>
    public class AIActionWaitConversationEnd : AIDecision
    {
        protected Character _targetCharacter;
        private bool isConversationEnded = false;
        /// <summary>
        /// On Decide we check whether the Target is facing us
        /// </summary> s
        /// <returns></returns>
        public override bool Decide()
        {
            return isConversationEnded;
        }

        /// <summary>
        /// Returns true if the Brain's Target is facing us (this will require that the Target has a Character component)
        /// </summary>
        public void setConversationEnd()
        {
            isConversationEnded = true;
        }
    }
}