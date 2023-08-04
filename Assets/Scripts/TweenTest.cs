using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KTween;

public class TweenTest : MonoBehaviour
{
    TweenKey tweenKey;
    public AnimationCurve easeCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

    void Start()
    {
        tweenKey = transform.DoMove(new Vector3(5, 5, 0), 4, EaseType.EaseInBack);
        //transform.DoScale(new Vector3(4, 4, 4), 8);
        //StartCoroutine(TestKill());
        //tweenKey = transform.DoMove(new Vector3(5, 5, 0), 4, easeCurve);
    }

    IEnumerator TestKill()
    {
        yield return new WaitForSeconds(2);
        tweenKey.Kill();
    }
}
