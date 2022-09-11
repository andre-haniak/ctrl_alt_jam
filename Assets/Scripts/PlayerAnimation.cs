using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private GridMovement player;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = transform.GetComponentInParent<GridMovement>();
        player.moveStop += MoveStop;
        player.moveHorizontal += MoveHorizontal;
        player.moveVertical += MoveVertical;
        player.push += Push;
    }

    public void MoveStop()
    {
        animator.Play("Idle");
    }

    public void MoveHorizontal()
    {
        animator.Play("DashHorizontal");
    }

    public void MoveVertical()
    {
        animator.Play("DashVertical");
    }

    public void Push()
    {
        animator.Play("Push");
    }
}
