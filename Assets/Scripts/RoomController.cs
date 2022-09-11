using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public int room = 0;
    public GameObject player;
    public Vector3 lastPlayerPos;
    public Animator transition;

    private void Start()
    {
        lastPlayerPos = player.transform.position;
        transition = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transition.Play("Transition");
            StartCoroutine(ResetRoom());
        }
    }

    public void CallChangeRoom(int nextRoom, Vector3 playerPos)
    {
        transition.Play("Transition");
        StartCoroutine(ChangeRoom(nextRoom, playerPos));
    }

    IEnumerator ResetRoom()
    {
        yield return new WaitForSeconds(1);
        player.transform.position = lastPlayerPos;
        for (int i = 0; i < player.GetComponent<GridMovement>().keys.Count; i++)
        {
            player.GetComponent<GridMovement>().keys[i].GetComponent<SpriteRenderer>().enabled = true;
            player.GetComponent<GridMovement>().keys[i].GetComponent<CircleCollider2D>().enabled = true;
            player.GetComponent<GridMovement>().keys.Remove(player.GetComponent<GridMovement>().keys[i]);
        }
    }

    IEnumerator ChangeRoom(int nextRoom, Vector3 playerPos)
    {
        yield return new WaitForSeconds(1);
        room = nextRoom;
        if (nextRoom == 0)
        {
            transform.position = new Vector3(0, 0, -10);
        }
        if (nextRoom == 1)
        {
            transform.position = new Vector3(25, 0, -10);
        }
        if (nextRoom == 2)
        {
            transform.position = new Vector3(50, 0, -10);
        }
        player.transform.position = playerPos;
        lastPlayerPos = playerPos;
    }
}
