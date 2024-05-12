using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControll : MonoBehaviour
{
    public enum Status
    {
        Idle,
        Move,
        Run,
        Attack,
        Back,
        MoveToIdle,
    }

    Animator animator;

    public int attack;

    public Status status;

    public int runPercent;
    public bool isRun = false;
    public bool attackEndRun = false;

    public float moveSpeed = 2f;
    public float runSpeed = 4f;

    public Vector3 StopPosition { get; set; } = Vector3.zero;

    private void Awake()
    {
        status = Status.Move;

        animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        attack = GetComponent<CharacterInfo>().Atk;
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
                UpdateRightMove(moveSpeed);
                break;
            case Status.Run:
                UpdateLeftMove(runSpeed);
                break;
            case Status.Attack:

                break;

            case Status.Back:
                {
                    var distance = Vector3.Distance(StopPosition, transform.position);
                    if (distance < 1)
                    {
                        ChangeStatus(Status.Idle);
                        animator.SetBool("Move", false);
                        animator.SetBool("Idle", true);
                        Flip(true);
                    }
                    UpdateLeftMove(moveSpeed);
                }

                break;

            case Status.MoveToIdle:
                {
                    var distance = Vector3.Distance(StopPosition, transform.position);

                    if (distance < 1)
                    {
                        ChangeStatus(Status.Idle);
                        animator.SetBool("Move", false);
                        animator.SetBool("Idle", true);
                    }
                    UpdateRightMove(moveSpeed);
                }

                break;

        }
    }

    public void Flip(bool isFlip)
    {
        transform.localScale = isFlip ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }

    public void Attack()
    {

    }

    public void RunMode(bool isRun)
    {
        int randomInt = Random.Range(0, 101);

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
            float randomTime = Random.Range(0.5f, 1f);
            Invoke("RunModeChange", randomTime);
        }

        // ∏ÛΩ∫≈Õ
        if (other.tag == "Monster")
        {
            status = Status.Attack;

            other.gameObject.GetComponent<MonsterInfo>().Damage(attack);

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


    public void UpdateRightMove(float speed)
    {
        transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * speed);
    }

    public void UpdateLeftMove(float speed)
    {
        transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * speed);
    }

}
