using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        StartCoroutine(CameraMoveRoutine());
    }

    private void Update()
    {
        cam.orthographicSize -= Input.mouseScrollDelta.y;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 5, int.MaxValue);

        
    }

    private IEnumerator CameraMoveRoutine()
    {
        Vector3 startDrag = new();
        while(true)
        {
            if(Input.GetMouseButtonDown(2))
                startDrag = cam.ScreenToWorldPoint(Input.mousePosition);
            
            if(Input.GetMouseButton(2))
            {
                var diff = cam.ScreenToWorldPoint(Input.mousePosition) - startDrag;
                transform.position -= diff;
            }
            yield return null;
        }
    }
}
