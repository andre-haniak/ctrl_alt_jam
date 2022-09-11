using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogUI : MonoBehaviour
{
    [SerializeField] private GameObject DialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private DialogObject testDialog;
    
    private TypeWriteEffect typeWriteEffect;

    private void Start() 
    {
        typeWriteEffect = GetComponent<TypeWriteEffect>();
        CloseDialogBox();
        ShowDialog(testDialog);
    }

    public void ShowDialog(DialogObject dialogObject)
    {
        DialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialog(dialogObject));
    }

    private IEnumerator StepThroughDialog(DialogObject dialogObject)
    {
        yield return new WaitForSeconds(1);

        foreach(string dialog in dialogObject.Dialog)
        {
            yield return typeWriteEffect.Run(dialog, textLabel);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        CloseDialogBox();
    }

    private void CloseDialogBox()
    {
        DialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}
