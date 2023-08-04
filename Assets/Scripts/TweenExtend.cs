using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KTween
{
    public static class TweenExtend
    {
        public static TweenKey DoMove(this Transform self, Vector3 endPos, float duration, EaseType ease = EaseType.Linear)
        {
            return Tween.To(() => self.localPosition, (value) => { self.localPosition = value; }, endPos, duration, ease);
        }

        public static TweenKey DoMove(this Transform self, Vector3 endPos, float duration, AnimationCurve curve)
        {
            return Tween.To(() => self.localPosition, (value) => { self.localPosition = value; }, endPos, duration, curve);
        }

        public static TweenKey DoScale(this Transform self, Vector3 endPos, float duration, EaseType ease = EaseType.Linear)
        {
            return Tween.To(() => self.localScale, (value) => { self.localScale = value; }, endPos, duration, ease);
        }
    }
}
