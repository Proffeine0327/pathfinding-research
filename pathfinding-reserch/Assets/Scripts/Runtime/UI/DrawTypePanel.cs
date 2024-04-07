using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class DrawTypePanel : MonoBehaviour
{
    private PathFinder pathFinder => PathFinder.Instance;

    private Button[] drawTypeButtons;

    private void Start()
    {
        drawTypeButtons = GetComponentsInChildren<Button>();
        
        pathFinder
            .ObserveEveryValueChanged(p => p.DrawType)
            .Subscribe(type => drawTypeButtons.For((b, i) => b.image.color = (i + 1) == (int)type ? Color.yellow : Color.white));

        drawTypeButtons.For((b, i) =>
        {
            b.onClick.AddListener(() =>
                pathFinder.DrawType =
                    pathFinder.DrawType == (PointType)(i + 1) ?
                    PointType.None :
                    (PointType)(i + 1));
        });
    }
}
