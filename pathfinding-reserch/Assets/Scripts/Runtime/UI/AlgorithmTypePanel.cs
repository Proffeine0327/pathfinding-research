using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class AlgorithmTypePanel : MonoBehaviour
{
    private PathFinder pathFinder => PathFinder.Instance;

    private Button[] algorithmTypeButtons;

    private void Start()
    {
        algorithmTypeButtons = GetComponentsInChildren<Button>();

        pathFinder
            .ObserveEveryValueChanged(p => p.PathFindType)
            .Subscribe(x => algorithmTypeButtons.For((b, i) => b.image.color = x == (PathFindType)i ? Color.yellow : Color.white));

        algorithmTypeButtons.For((b, i) => b.onClick.AddListener(() => pathFinder.PathFindType = (PathFindType)i));
    }
}
