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

    // 무조건 AttackEndRun 이 체크되는 변수
    public bool confirmAttackEndRun = false;

    // isRun 패스하는 변수
    public bool isPass = false;

    // attackEndRun 패스하는 변수 잔류 병사
    public bool isAttackEndPass = false;

    private float moveSpeed = 2f;
    private float runSpeed = 1f;
    public static event Action<GameObject> OnCharacterControll;
    public UnityEngine.Transform MonsterTransform {  get; set; }
    public RectTransform skillCanvasRectTransform { get; private set; }

    public CharacterEffect characterEffect { get; private set; }

    // Todo : 게임 패배후 재시작시 캐릭터 애니메이션 상태 전환 해줘야함
    // 버그 : 게임 패배후 편성 창 가면 캐릭터가 하나 증가되는 버그

    public Vector3 StopPosition = Vector3.zero;
    private void Awake()
    {
        status = Status.Move;

        animator = GetComponentInChildren<Animator>();

        skillCanvasRectTransform = GetComponentInChildren<Canvas>().GetComponent<RectTransform>();
        characterEffect = GetComponent<CharacterEffect>();
        // 기본 상태 오른쪽 보게 하기
        Flip(true);
    }

    private void OnEnable()
    {
        Flip(true);
        AnimationMove();
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
        skillCanvasRectTransform.rotation = quaternion;

        //transform.localScale = isFlip ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }

    public void RunMode(bool isRun)
    {
        if (isPass && isRun) return;
        else if (!isRun && isAttackEndPass) return;

        int randomInt = UnityEngine.Random.Range(0, 101);

        if (this.isRun) return;

        if (isRun) this.isRun = randomInt > runPercent ? false : true;
        else attackEndRun = randomInt > runPercent ? false : true;

        if (confirmAttackEndRun) attackEndRun = true;
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
        characterEffect.CryParticle.Play();
        characterEffect.RunParticle.Play();

        AnimationMove();
    }

    public void AttackMode()
    {
        if (isRun) return;

        AnimationMove();

        ChangeStatus(Status.Move);
    }
    public void AttackEndRunModeChange()
    {
        if (!attackEndRun) return;

        Flip(false);
        status = Status.Run;
        characterEffect.CryParticle.Play();
        characterEffect.RunParticle.Play();

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
        isPass = false;
        isAttackEndPass = false;
        attackEndRun = false;
        isRun = false;
        confirmAttackEndRun = false;
        Flip(true);
        ChangeStatus(Status.Move);
        gameObject.transform.position = Vector3.zero;

        gameObject.SetActive(true);
        AnimationMove();
        gameObject.SetActive(false);
        StopPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

}
