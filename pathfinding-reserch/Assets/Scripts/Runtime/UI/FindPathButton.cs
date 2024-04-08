using UnityEngine;
using UnityEngine.UI;

public class FindPathButton : MonoBehaviour
{
    private PathFinder pathFinder => PathFinder.Instance;
    private Map map => Map.Instance;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => pathFinder.FindPath());
    }

    private void Update()
    {
        button.interactable = map.StartCoord != Vector2Int.one * -1 && map.EndCoord != Vector2Int.one * -1;
    }
}