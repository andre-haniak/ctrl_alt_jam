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

    private bool isMoving;
    private Vector2 input;

    private Animator animator;
    public LayerMask solidObjectsLayer;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    private void Update() 
    {

        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0;
            {
                
            }

            if (input != Vector2.zero)
            {
                animator.SetFloat("Horizontal", input.x);
                animator.SetFloat("Vertical", input.y);

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;
                
                if (isWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }

        animator.SetBool("isMoving", isMoving);
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }

    private bool isWalkable(Vector2 targetPos)
    {
        if(Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) != null)
        {
            return false;
        } 
        return true;
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
