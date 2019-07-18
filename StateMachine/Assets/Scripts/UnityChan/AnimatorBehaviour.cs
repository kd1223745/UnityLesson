using UnityEngine;
using System;

public class AnimatorBehaviour : StateMachineBehaviour
{
    float enterTime = 0.0f;
    public float NormalizedTime { get; private set; }
    public bool IsTransition { get; private set; }
    public Action EndCallBack { private get; set; }
    = () => {/*Debug.LogWarning("empty animator callback");*/
    };

    public virtual void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NormalizedTime = 0.0f;
        IsTransition = animator.IsInTransition(layerIndex);
        enterTime = Time.time;
        StateEnter(animator, stateInfo, layerIndex);
    }

    public virtual void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (IsTransition == false)
        {
            NormalizedTime = ((Time.time - enterTime) * stateInfo.speed) / stateInfo.length;
        }
        IsTransition = animator.IsInTransition(layerIndex);
        StateUpdate(animator, stateInfo, layerIndex);
    }

    public virtual void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        StateExit(animator, stateInfo, layerIndex);
    }

    public void ResetTime()
    {
        enterTime = Time.time;
        NormalizedTime = 0.0f;
        EndCallBack();
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
