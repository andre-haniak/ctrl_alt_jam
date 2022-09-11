using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GridMovement : MonoBehaviour
{

    // hearth system
    public float count;
    public int maxLife;
    public bool isAlive, inLight;

    public float moveSpeed;

    private float moveProgress, currentMoveSpeed;
    private bool isMoving;
    private Vector2 input;
    private Vector3 targetPos;

    private RoomController roomController;
    public LayerMask solidObjectsLayer;
    public Action moveStop, moveHorizontal, moveVertical, push;

    public List<GameObject> keys = new List<GameObject>();

    private void Start()
    {
        roomController = Camera.main.GetComponent<RoomController>();
        currentMoveSpeed = moveSpeed;
    }

    private void Update() 
    {
        if (inLight)
        {
            count += Time.deltaTime;
            if (maxLife <= count)
            {
                roomController.CallResetRoom();
            }
        }
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            {
                targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (targetPos.x > transform.position.x)
                {
                    transform.GetChild(0).localScale = new Vector3(1, 1, 1);
                }
                else if (targetPos.x < transform.position.x)
                {
                    transform.GetChild(0).localScale = new Vector3(-1, 1, 1);
                }

                if (isWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
                else
                {
                    CheckPush();
                    CheckDoor();
                }
            }
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        if (targetPos.x != transform.position.x)
        {
            if (moveHorizontal != null)
            {
                moveHorizontal();
            }
        }
        else if (targetPos.y != transform.position.y)
        {
            if (moveVertical != null)
            {
                moveVertical();
            }
        }
        moveProgress = 0;
        int i = 0;
        while (Vector3.Distance(targetPos, transform.position) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, moveProgress);
            yield return null;
            moveProgress += Time.deltaTime * currentMoveSpeed;
            i++;
            if (i > 5000)
            {
                break;
            }
        }
        transform.position = targetPos;
        isMoving = false;
        if (moveStop != null)
        {
            moveStop();
        }
    }

    private bool isWalkable(Vector2 targetPos)
    {
        return Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) == null;
    }

    private void CheckPush()
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer).gameObject.layer == 7)
        {
            Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer).GetComponent<Pushable>().CallPush(targetPos + new Vector3(input.x, input.y, 0));
            if (push != null)
            {
                push();
            }
        }
    }

    private void CheckDoor()
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer).gameObject.layer == 12 && !Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer).GetComponent<Door>().locked)
        {
            roomController.CallChangeRoom(Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer).GetComponent<Door>().nextRoom, Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer).GetComponent<Door>().playerNextPosition);
        }
        else if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer).gameObject.layer == 12 && Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer).GetComponent<Door>().locked)
        {
            if (keys.Contains(Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer).GetComponent<Door>().key))
            {
                Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer).GetComponent<Door>().Open();
                keys.Remove(Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer).GetComponent<Door>().key);
                Destroy(Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer).GetComponent<Door>().key);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.tag == "Light")
        {
            inLight = true;
            currentMoveSpeed = moveSpeed / 5;
        }
        if (collision.gameObject.tag == "Key")
        {
            keys.Add(collision.gameObject);
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.enabled = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Light")
        {
            inLight = false;
            currentMoveSpeed = moveSpeed;
            count = 0;
        }

    }
}
