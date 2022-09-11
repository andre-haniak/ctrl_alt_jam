using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;

    public GameObject key;
    public bool locked;
    public int nextRoom;
    public Vector3 playerNextPosition;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    [ContextMenu(itemName:"Open")]
    public void Open()
    {
        //animar depois
        //animator.SetTrigger(name:"Open");
        locked = false;
    }
}
