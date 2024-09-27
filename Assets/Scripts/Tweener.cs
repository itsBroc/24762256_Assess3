using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    private List<Tween> activeTweens = new List<Tween>();

    void Update()
    {
        for (int i = 0; i < activeTweens.Count; i++)
        {
            Tween tween = activeTweens[i];
            if (tween != null)
            {
                float timeElapsed = Time.time - tween.StartTime;
                float t = timeElapsed / tween.Duration;

                tween.Target.position = Vector3.Lerp(tween.StartPos, tween.EndPos, t);

                if (t >= 1.0f)
                {
                    tween.Target.position = tween.EndPos;
                    activeTweens.RemoveAt(i);
                }
            }
        }
    }

    public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
        if (TweenExists(targetObject))
        {
            return false;
        }
        else
        {
            activeTweens.Add(new Tween(targetObject, startPos, endPos, Time.time, duration));
            return true;
        }
    }

    public bool TweenExists(Transform target)
    {
        foreach (Tween tween in activeTweens)
        {
            if (tween.Target == target) return true;
        }
        return false;
    }

    public bool IsTweenComplete()
    {
        return activeTweens.Count == 0;
    }
}