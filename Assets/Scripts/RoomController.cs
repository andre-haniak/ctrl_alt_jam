using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public int room = 0;
    public GameObject player;
    public Vector3 lastPlayerPos;

    private void Start()
    {
        lastPlayerPos = player.transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ResetRoom());
        }
    }

    public void CallChangeRoom(int nextRoom, Vector3 playerPos)
    {
        StartCoroutine(ChangeRoom(nextRoom, playerPos));
    }

    IEnumerator ResetRoom()
    {
        yield return new WaitForSeconds(1);
        player.transform.position = lastPlayerPos;
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
        player.transform.position = playerPos;
        lastPlayerPos = playerPos;
    }
}
