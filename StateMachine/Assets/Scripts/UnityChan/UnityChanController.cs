using UnityEngine;

//AddComponentしたときにGameObjectに
//RequireComponentで指定したComponentがなければ
//AddComponentしてくれる命令

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]

public class UnityChanController : MonoBehaviour
{
    enum AnimationState : int
    {
        Idle,
        Run,
        Jump
    }

    StateMachine<AnimationState> stateMachine = new StateMachine<AnimationState>();

    Animator                 animator = null;
    AnimatorBehaviour        animatorBehaviour=null;
    Rigidbody                rigidbody = null;

    [SerializeField] float rotateSpeed = 1.0f;
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] float jumpPower = 5.0f;

    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        InitializeComponent();
        InitializeState();
    }

    void InitializeState()
    {
        stateMachine.Add(AnimationState.Idle
            , IdleUpdate
            , IdleInitialize
            );
        stateMachine.Add(AnimationState.Run
            , RunUpdate
            , RunInitialize
            );
        stateMachine.Add(AnimationState.Jump
            , JumpUpdate
            , JumpInitialize
            );
        stateMachine.ChangeState(AnimationState.Idle);
    }

    void InitializeComponent()
    {
        animator     = GetComponent<Animator>();
        rigidbody    = GetComponent<Rigidbody>();
        animatorBehaviour = animator.GetBehaviour<AnimatorBehaviour>();
    }

    void IdleUpdate()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            stateMachine.ChangeState(AnimationState.Idle);
        }
        if (Input.GetKey(KeyCode.W))
        {
            stateMachine.ChangeState(AnimationState.Run);
        }
        if(Input.GetKey(KeyCode.Space))
        {
            stateMachine.ChangeState(AnimationState.Jump);
        }
        DirectionChange();
    }

    void IdleInitialize()
    {
        animator.CrossFadeInFixedTime("Idle", 0.0f);
    }

    void DirectionChange()
    {
        float rot = 0.0f;
        if (Input.GetKey(KeyCode.A))
        {
            rot -= rotateSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rot += rotateSpeed;
        }
        transform.Rotate(0.0f, rot, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
        var animeState = animator.GetCurrentAnimatorStateInfo(0);
        //Unityのデフォルトで取得できるアニメーション再生時間
        Debug.LogFormat($"before normalizeTime{animeState.normalizedTime}");
        //新たに作り直して0～1の間で取得できるようにした
        //アニメーション再生時間
        Debug.LogFormat($"after normalizeTime = {animatorBehaviour.NormalizedTime}");
        if (animatorBehaviour.NormalizedTime > 1.0f)
        {
            animatorBehaviour.ResetTime();
        }

    }

    void JumpInitialize()
    {
        animator.CrossFadeInFixedTime("Jump", 0.0f);
        rigidbody.AddForce(0.0f, jumpPower, 0.0f, ForceMode.Impulse);
        animatorBehaviour.EndCallBack = () => { stateMachine.ChangeState(AnimationState.Idle); };
    }

    void JumpUpdate()
    {
        //var animeState = animator.GetCurrentAnimatorStateInfo(0);
        //if (animeState.normalizedTime > 1.0f)
        //{
        //    stateMachine.ChangeState(AnimationState.Idle);
        //}

        if (Input.GetKey(KeyCode.W))
        {
            if (animatorBehaviour.NormalizedTime > 0.65f)
            {
                stateMachine.ChangeState(AnimationState.Run);
            }
            else
            {
                Move(moveSpeed / 10);
            }
        }
    }

    void JumpEnd()
    {
        animatorBehaviour.EndCallBack = () => { };
    }

    void RunInitialize()
    {
        animator.CrossFadeInFixedTime("Run", 0.0f);
    }

    void RunUpdate()
    {
        if (Input.GetKey(KeyCode.W) == false)
        {
            stateMachine.ChangeState(AnimationState.Idle);
            return;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            stateMachine.ChangeState(AnimationState.Jump);
        }
       Move(moveSpeed/5);
        DirectionChange();
    }

    void Move(float aMoveSpeed)
    {
        velocity = transform.TransformDirection(new Vector3(0, 0, aMoveSpeed));
        var position = transform.position + velocity;
        rigidbody.MovePosition(position);
    }


}
