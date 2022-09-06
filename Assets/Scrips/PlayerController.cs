using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int count;
    public int maxLife;
    // public int currentLife;
    public bool isAlive = true;

    private Rigidbody2D rb;

    private float moveSpeed;
    private float jumpForce;
    private bool isJumping;
    private float moveHorizontal;
    private float moveVertical;

    private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        // currentLife = maxLife;
        moveSpeed = 3f;
        jumpForce = 40f;
        isJumping = false;   
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate() 
    {

        if(isAlive)
        {
            if (moveHorizontal > 0.1f || moveHorizontal < -0.1f)
            {
                rb.AddForce(new Vector2(moveHorizontal * moveSpeed, 0f), ForceMode2D.Impulse);
            }

            if (!isJumping && moveVertical > 0.1f)
            {
                rb.AddForce(new Vector2(0f, moveVertical * jumpForce), ForceMode2D.Impulse);
            }

            if (facingRight == false && moveHorizontal > 0)
            {
                Flip();
            }else if (facingRight == true && moveHorizontal < 0)
            {
                Flip();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.tag == "Platform")
        {
            isJumping = false;
        }

        if(collision.gameObject.tag == "Light")
        {
            count++;
            moveSpeed = moveSpeed - 2f;
            TakeDamage();
            Debug.Log(count);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            isJumping = true;
        }

        if(collision.gameObject.tag == "Light")
        {
            moveSpeed = 3f;
        }

    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public void TakeDamage()
    {
        if (isAlive)
        {    
            // currentLife--;
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
