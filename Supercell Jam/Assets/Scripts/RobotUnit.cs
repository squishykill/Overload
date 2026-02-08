using UnityEngine;
using System.Collections;

public class RobotUnit : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3.0f;
    public float hitDistance = 0.3f;
    public float startMoveDelay = 1f;

    [Header("Animation")]
    public Animator animator;

    SpriteRenderer sprite;
    GameDirector director;
    Transform target;
    bool canMove = false;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Init(GameDirector directorRef, float effectiveMoveSpeed)
    {
        director = directorRef;
        target = directorRef.alienTarget;
        moveSpeed = effectiveMoveSpeed;

        HandleFacing();

        SetIdle();
        StartCoroutine(StartMovingAfterDelay());
    }

    void HandleFacing()
    {
        if (!sprite || !target) return;

        // If spawned to the right of the alien, flip
        sprite.flipX = transform.position.x > target.position.x;
    }

    IEnumerator StartMovingAfterDelay()
    {
        yield return new WaitForSeconds(startMoveDelay);
        canMove = true;
        SetWalking();
    }

    void Update()
    {
        if (!canMove || !target) return;

        Vector3 dir = target.position - transform.position;
        if (dir.magnitude <= hitDistance)
        {
            Sacrifice();
            return;
        }

        transform.position += dir.normalized * moveSpeed * Time.deltaTime;
    }

    void Sacrifice()
    {
        if (director)
            director.OnRobotSacrificed();

        Destroy(gameObject);
    }

    void SetIdle()
    {
        if (animator) animator.Play("Idle");
    }

    void SetWalking()
    {
        if (animator) animator.Play("Walk");
    }
}
