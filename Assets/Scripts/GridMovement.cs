using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridMovement : MonoBehaviour
{

    // hearth system
    public int count;
    public int maxLife;
    public bool isAlive = true;

    public float moveSpeed;

    private float moveProgress;
    private bool isMoving;
    private Vector2 input;
    private Vector3 targetPos;

    private Animator animator;
    private RoomController roomController;
    public LayerMask solidObjectsLayer;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        roomController = Camera.main.GetComponent<RoomController>();
    }

    private void Update() 
    {

        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            {
                animator.SetFloat("Horizontal", input.x);
                animator.SetFloat("Vertical", input.y);

                targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;
                
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

        animator.SetBool("isMoving", isMoving);
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        moveProgress = 0;
        int i = 0;
        while (Vector3.Distance(targetPos, transform.position) > 0.05f)
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
        isMoving = false;
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
        }
    }

    private void CheckDoor()
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer).gameObject.layer == 12)
        {
            roomController.CallChangeRoom(Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer).GetComponent<Door>().nextRoom, Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer).GetComponent<Door>().playerNextPosition);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.tag == "Light")
        {
            count++;
            moveSpeed = moveSpeed - 1.5f;
            TakeDamage();
            Debug.Log(count);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Light")
        {
            moveSpeed = 3f;
        }

    }

    public void TakeDamage()
    {
        if (isAlive)
        {    
            if (maxLife <= count)
            {
                isAlive = false;
                Invoke("Restart", 2f);
                Debug.Log("Game Over!");
            }
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
