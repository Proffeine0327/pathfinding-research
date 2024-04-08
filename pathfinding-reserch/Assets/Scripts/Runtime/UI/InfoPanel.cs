using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    private Map map => Map.Instance;

    [SerializeField] private TextMeshProUGUI startText;
    [SerializeField] private TextMeshProUGUI endText;

    private void Start()
    {
        map
            .ObserveEveryValueChanged(p => p.StartCoord)
            .Subscribe(x => startText.text = $"Start: {x}");
            
        map
            .ObserveEveryValueChanged(p => p.EndCoord)
            .Subscribe(x => endText.text = $"End: {x}");
    }
}
