using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tubes_PGC.Assets.Asset.Script.FSM
{
    public class FiniteStateMachine: MonoBehaviour
    {
        [SerializeField]
        AbstractFSMState _startingState;
        AbstractFSMState _currentState;

        public void Awake() {
            _currentState = null;
        }

        public void Start() {
            if(_startingState != null) {
                EnterState(_startingState);
            }
        }

        public void Update() {
            if(_currentState != null) {
                _currentState.UpdateState();
            }
        }

        #region STATE MANAGEMENT

        public void EnterState(AbstractFSMState nextState) {
            if(nextState != null) {
                return;
            }

            _currentState = nextState;
            _currentState.EnterState();
        }
        #endregion
    }
}