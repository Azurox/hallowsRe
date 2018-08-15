using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour {
    private const float Speed = 8f;
    private bool isMoving = false;
    private List<Vector2> path;
    private List<Vector2> newPath;

	public void TakePath(List<Vector2> path)
    {
        path.RemoveAt(0);
        if (!isMoving)
        {
            this.path = path;
            StartCoroutine(Move());
        } else
        {
            newPath = path;
        }

    }

    private IEnumerator Move()
    {
        isMoving = true;

        float i = 0.0f;
        float rate = 1.0f * Speed;
        var startPos = gameObject.transform.position;
        var endPos = new Vector3(path[0].x, gameObject.transform.position.y, path[0].y);

        if(Mathf.Abs(startPos.x - endPos.x) + Mathf.Abs(startPos.z - endPos.z) > 1)
        {
            rate = 1.0f * (Speed / 1.5f);
        }

        while (i < 1.0)
        {
            i += Time.deltaTime * rate;
            gameObject.transform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }

        path.RemoveAt(0);

        if(newPath != null)
        {
            path = newPath;
        }

        if (path.Count > 0)
        {
            StartCoroutine(Move());
        } else
        {
            isMoving = false;
            newPath = null;
        }

    }

}
