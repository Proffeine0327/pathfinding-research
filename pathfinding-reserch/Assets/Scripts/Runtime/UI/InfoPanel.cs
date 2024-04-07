using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    private PathFinder pathFinder => PathFinder.Instance;

    [SerializeField] private TextMeshProUGUI startText;
    [SerializeField] private TextMeshProUGUI endText;
    [SerializeField] private TextMeshProUGUI lastFindTimeText;
    [SerializeField] private TextMeshProUGUI distText;

    private void Start()
    {
        pathFinder
            .ObserveEveryValueChanged(p => p.StartCoordinate)
            .Subscribe(x => startText.text = $"Start: {x}");
            
        pathFinder
            .ObserveEveryValueChanged(p => p.EndCoordinate)
            .Subscribe(x => endText.text = $"End: {x}");
            
        pathFinder
            .ObserveEveryValueChanged(p => p.FindTime)
            .Subscribe(x => lastFindTimeText.text = $"Time: {x}");
            
        pathFinder
            .ObserveEveryValueChanged(p => p.Distance)
            .Subscribe(x => distText.text = $"Dist: {x}");
    }
}
