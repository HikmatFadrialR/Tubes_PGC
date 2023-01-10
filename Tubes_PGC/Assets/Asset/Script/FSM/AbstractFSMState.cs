using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExecutionState {
    NONE,
    ACTIVE,
    COMPLETED,
    TERMINATED,
};

public abstract class AbstractFSMState : ScriptableObject
{
    ExecutionState _executionState;

    public virtual void OnEnable() {
        ExecutionState = ExecutionState.NONE;
    }

    public virtual bool EnterState() {
        ExecutionState = ExecutionState.ACTIVE;
        return true;
    }

    public abstract void UpdateState();

    public virtual bool ExitState() {
        ExecutionState = ExecutionState.COMPLETED;
        return true;
    }

    public ExecutionState ExecutionState
    {
        get {
            return _executionState;
        }
        protected set {
            _executionState = value;
        }
    }
}
