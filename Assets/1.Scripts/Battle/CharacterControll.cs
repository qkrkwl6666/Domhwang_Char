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
    }

    Animator animator;

    public int attack;

    public Status status;

    public int runPercent;
    public bool isRun = false;

    private float moveSpeed = 2f;
    private float runSpeed = 4f;

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
                transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * moveSpeed);
                break;
            case Status.Run:
                transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * runSpeed);
                break;
            case Status.Attack:

                break;

            case Status.Back:
                var distance = Vector3.Distance(StopPosition , transform.position);
                if (distance < 1)
                {
                    ChangeStatus(Status.Idle);
                    animator.SetBool("Move", false);
                    animator.SetBool("Idle", true);
                    Flip(true);
                }
                transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * moveSpeed);

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

    public void RunMode()
    {
        int randomInt = Random.Range(0, 101);

        isRun = randomInt > runPercent ? false : true;
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

        AnimationRun();
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
            Logger.Log("Monster!!");
        }
    }

    public void AnimationRun()
    {
        animator.SetBool("Attack", false);
        animator.SetBool("Idle", false);

        animator.SetBool("Move", true);

    }

}
