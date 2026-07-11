using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultBall : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;

    public void ChangeNameText(string text)
    {
        nameText.text = text;
    }
}
