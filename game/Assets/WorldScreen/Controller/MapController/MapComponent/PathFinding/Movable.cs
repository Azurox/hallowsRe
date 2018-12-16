using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour {
    private const float Speed = 8f;

    List<Vector2> path;

    public void TakePath(Vector2 startPosition, List<Vector2> path, Action<int, int> callback = null, Action endCallBack = null)
    {
        if (path == null) return;

        if (startPosition.Equals(path[0]))
        {
            path.RemoveAt(0);
        }

        if(this.path == null)
        {
            this.path = path;
            StartCoroutine(Move(callback, endCallBack));
        } else {
            this.path = path;
        }
    }

    private IEnumerator Move(Action<int, int> callback, Action endCallBack)
    {

        if (path == null || path.Count == 0)
        {
            path = null;
            endCallBack?.Invoke();
            yield return null;
        } else
        {
            var currentMove = path[0];

            float i = 0.0f;
            float rate = 1.0f * Speed;
            var startPos = gameObject.transform.position;
            var endPos = new Vector3(currentMove.x, gameObject.transform.position.y, currentMove.y);
            path.RemoveAt(0);

            if (Mathf.Abs(startPos.x - endPos.x) + Mathf.Abs(startPos.z - endPos.z) > 1)
            {
                rate = 1.0f * (Speed / 1.5f);
            }

            while (i < 1.0)
            {
                i += Time.deltaTime * rate;
                gameObject.transform.position = Vector3.Lerp(startPos, endPos, i);
                yield return null;
            }

            callback?.Invoke((int)endPos.x , (int)endPos.z);
            StartCoroutine(Move(callback, endCallBack));
        }
    }
}
