using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    public Vector3 startPos;
    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public int room;
    private float moveProgress;
    private RoomController roomController;

    private void Start()
    {
        startPos = transform.position;
        roomController = Camera.main.GetComponent<RoomController>();
        roomController.resetRoom += CallResetPosition;
    }


    private void CallResetPosition()
    {
        if (roomController.room == room)
        {
            transform.position = startPos;
        }
    }

    public void CallPush(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) == null)
        {
            StartCoroutine(GoToPosition(targetPos));
        }
    }

    IEnumerator GoToPosition(Vector3 targetPos)
    {
        moveProgress = 0;
        int i = 0;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, moveProgress);
            yield return null;
            moveProgress += Time.deltaTime * moveSpeed;
            i++;
            if (i > 5000)
            {
                break;
            }
        }
        transform.position = targetPos;
    }
}
