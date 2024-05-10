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
    public bool isRun = false;

    private float moveSpeed = 2f;
    private float runSpeed = 4f;

    public Vector3 stopPosition = Vector3.zero;

    private void Awake()
    {
        status = Status.Move;
    }
    private void Start()
    {
        
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
        }
    }

    public void RunMode()
    {
        int randomInt = Random.Range(0, 101);

        isRun = randomInt > runPercent ? false : true;

    }

    public void RunModeChange()
    {
        if (!isRun) return;

        transform.localScale = new Vector3(1, 1, 1);
        status = Status.Run;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");

        if (other.tag == "RunCollider")
        {
            Debug.Log("OnTriggerEnter Ãæµ¹!!");
            float randomTime = Random.Range(0.5f, 1f);
            Invoke("RunModeChange", randomTime);
        }
    }
}
