using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaitTimeInput : MonoBehaviour
{
    private PathFinder pathFinder => PathFinder.Instance;

    private TMP_InputField inputField;
    
    private void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onSubmit.AddListener(f => pathFinder.WaitTime = float.Parse(f));
        pathFinder.WaitTime = float.Parse(inputField.text);
    }
}
