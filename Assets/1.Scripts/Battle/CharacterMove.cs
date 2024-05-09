using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public enum Status
    {
        Idle,
        Move,
        Run,
        Attack,
    }

    public Status status;

    public int runPercent;
    private bool isRun = false;
    private float moveSpeed = 2f;
    private float runSpeed = 4f;

    private void Awake()
    {
        status = Status.Move;

        

    }

    private void Start()
    {
        status = Status.Move;

        int randomInt = Random.Range(0, 101);
        if (randomInt > runPercent) isRun = false;
        else isRun = true;
    }

    private void Update()
    {
          
    }



    // Update is called once per frame
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
        }
    }

    public void RunModeChange()
    {
        if (!isRun) return;

        transform.localScale = new Vector3(1, 1, 1);
        status = Status.Run;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("충돌1!!");
        if (other.tag == "RunCollider")
        {
            Debug.Log("충돌!!");
            float randomTime = Random.Range(1f, 2f);
            Invoke("RunModeChange", randomTime);
        }
    }
}
