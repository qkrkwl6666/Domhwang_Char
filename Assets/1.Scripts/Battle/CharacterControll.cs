using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CharacterControll : MonoBehaviour
{
    public enum Status
    {
        Idle,
        Move,
        Run,
        Attack,
        Back,
        Fly,
    }

    Animator animator;

    public Status status;

    public int runPercent;
    public bool isRun = false;
    public bool attackEndRun = false;

    private float moveSpeed = 3f;
    private float runSpeed = 12f;
    public static event Action<GameObject> OnCharacterControll;
    public UnityEngine.Transform MonsterTransform {  get; set; }

    public Vector3 StopPosition = Vector3.zero;
    private void Awake()
    {
        status = Status.Move;

        animator = GetComponentInChildren<Animator>();

        // 기본 상태 오른쪽 보게 하기
        Flip(true);
    }

    private void OnEnable()
    {
        Flip(true);
    }

    private void Start()
    {
        animator.SetBool("Move", true);
    }

    private void Update()
    {

    }
    private void FixedUpdate()
    {

        switch (status)
        {
            case Status.Idle:

                break;

            case Status.Move:
                UpdateMoveToMonster();
                break;

            case Status.Run:
                UpdateLeftMove();
                if(transform.position.x < -15)
                {
                    gameObject.SetActive(false);
                }
                break;

            case Status.Attack:

                break;

            case Status.Back:
                {
                    UpdateIdlePointMove();
                }
                break;

            case Status.Fly:
                UpdateFly();
                break;

        }
    }

    public void Flip(bool isFlip)
    {
        //transform.rotation = isFlip ? Quaternion.AngleAxis(-180, Vector3.up) : Quaternion.AngleAxis(0, Vector3.up);

        Quaternion quaternion = Quaternion.identity;
        quaternion.eulerAngles = isFlip ? new Vector3(0f, -180, 0) : Vector3.zero;
        transform.rotation = quaternion;

        //transform.localScale = isFlip ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }

    public void RunMode(bool isRun)
    {
        int randomInt = UnityEngine.Random.Range(0, 101);

        if (isRun) this.isRun = randomInt > runPercent ? false : true;
        else attackEndRun = randomInt > runPercent ? false : true;
    }

    public void ChangeStatus(Status status)
    {
        this.status = status;
    }

    public void RunModeChange()
    {
        if (!isRun) return;

        Flip(false);
        status = Status.Run;

        AnimationMove();
    }

    public void AttackMode()
    {
        if (isRun) return;

        AnimationMove();

        ChangeStatus(Status.Move);

        //RunMode(false);
    }
    public void AttackEndRunModeChange()
    {
        if (!attackEndRun) return;

        Flip(false);
        status = Status.Run;

        AnimationMove();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Run 
        if (other.tag == "RunCollider")
        {
            // float randomTime = UnityEngine.Random.Range(0.5f, 1f); (수정 전) 랜덤한 도망 타이밍 산출
            float randomTime = UnityEngine.Random.Range(0, 2); // (수정 후) 특정한 두 타이밍 산출
            Invoke("RunModeChange", randomTime);
        }

        // 몬스터
        if (other.tag == "Monster")
        {
            status = Status.Attack;

            animator.SetBool("Move", false);

            animator.SetBool("Attack",true);
        }
    }

    public void AnimationMove()
    {
        animator.SetBool("Attack", false);
        animator.SetBool("Idle", false);
        animator.SetBool("Move", true);
    }

    public void UpdateMoveToMonster()
    {
        Vector3 dir = MonsterTransform.position - transform.position;

        dir = new Vector3(-1f * dir.x, dir.y, dir.z);
        dir.Normalize();

        transform.Translate(dir * Time.deltaTime * moveSpeed);
    }

    public void UpdateLeftMove()
    {
        transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * runSpeed);
    }

    public void UpdateFly()
    {
        Vector3 flyDir = new Vector3(-1, 0.5f, 0);

        flyDir.Normalize();

        transform.Translate(flyDir * Time.deltaTime * 10f, Space.World);

        // Todo : 회전 추가

        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation.z += Time.deltaTime * 720f;
        transform.rotation = Quaternion.Euler(currentRotation);
    }

    public void UpdateIdlePointMove()
    {
        Vector3 dir = StopPosition - transform.position;

        var distance = Vector3.Distance(StopPosition, transform.position);

        if (distance < 0.1)
        {
            ChangeStatus(Status.Idle);
            animator.SetBool("Move", false);
            animator.SetBool("Idle", true);
            Flip(true);
            OnCharacterControll?.Invoke(gameObject);
            return;
        }
        
        dir.Normalize();

        transform.Translate(dir * Time.deltaTime * moveSpeed);

    }

    public void CharacterAwake()
    {
        attackEndRun = false;
        isRun = false;
        Flip(true);
        ChangeStatus(Status.Move);

        gameObject.SetActive(true);
        AnimationMove();
        gameObject.SetActive(false);
        StopPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

}
