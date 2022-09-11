using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialog/DialogObject")]
public class DialogObject : ScriptableObject
{
    [SerializeField] [TextArea] private string[] dialog;

    public string[] Dialog => dialog;
}
