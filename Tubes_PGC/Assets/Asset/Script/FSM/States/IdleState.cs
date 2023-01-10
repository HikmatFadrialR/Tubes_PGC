using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tubes_PGC.Assets.Asset.Script.FSM.States 
{

    [CreateAssetMenu(fileName = "IdleState", menuName="Tubes_PGC/States/Idle", order=1)]
   public  class IdleState : AbstractFSMState 
    {
        public override bool EnterState()
        {
            base.EnterState();
            Debug.Log("ENTERED IDLE STATE");
            return true;
        }

        public override void UpdateState()
        {
            Debug.Log("UPDATING IDLE STATE");
        }

        public override bool ExitState()
        {
            base.ExitState();
            Debug.Log("EXITING IDLE STATE");
            return true;
        }
    }
}
