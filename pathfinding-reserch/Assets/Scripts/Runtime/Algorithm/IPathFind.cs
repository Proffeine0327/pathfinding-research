using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePathFind : MonoBehaviour
{
    protected Coroutine routine;

    public void FindPathVisualize(Vector2Int start, Vector2Int end)
    {
        routine = StartCoroutine(FindPathRoutine(start, end));
    }

    public void StopPathFind()
    {
        if(routine != null)
            StopCoroutine(routine);
    }

    protected abstract IEnumerator FindPathRoutine(Vector2Int start, Vector2Int end);
}
