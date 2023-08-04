using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KTween
{
    public static class Tween
    {
        private static int tweenKey = 0;
        private static Dictionary<TweenKey, TweenItem<Vector3>> tweenItems = new Dictionary<TweenKey, TweenItem<Vector3>>();
        private static List<TweenKey> removeList = new List<TweenKey>();
        private static TweenHelper _tweenHelper;
        //private static TweenHelper tweenHelper => _tweenHelper ??= new GameObject().AddComponent<TweenHelper>();

        static Tween()
        {
            _tweenHelper = new GameObject("Tween").AddComponent<TweenHelper>();
            _tweenHelper.RegisterUpdateAction(Update);
            UnityEngine.Object.DontDestroyOnLoad(_tweenHelper.gameObject);
        }

        public static TweenKey To(Func<Vector3> getStartValue, Action<Vector3> setValue, Vector3 endValue, float duration, EaseType ease = EaseType.Linear)
        {
            TweenKey key = new TweenKey(tweenKey++);

            Func<float, float> calculateEase;
            switch (ease)
            {
                case EaseType.Linear:
                    calculateEase = Linear;
                    break;
                case EaseType.EaseInSine:
                    calculateEase = EaseInSine;
                    break;
                case EaseType.EaseOutSine:
                    calculateEase = EaseOutSine;
                    break;
                case EaseType.EaseInOutSine:
                    calculateEase = EaseInOutSine;
                    break;
                case EaseType.EaseInBack:
                    calculateEase = EaseInBack;
                    break;
                case EaseType.EaseOutBack:
                    calculateEase = EaseOutBack;
                    break;
                case EaseType.EaseInOutBack:
                    calculateEase = EaseInOutBack;
                    break;
                default:
                    calculateEase = Linear;
                    break;
            }

            Vector3 startValue = getStartValue();
            tweenItems.Add(key, new TweenItem<Vector3>()
            {
                startValue = startValue,
                endValue = endValue,
                curValue = startValue,
                duration = duration,
                curTime = Time.deltaTime,
                calculateEase = calculateEase,
                setAction = setValue
            });
            return key;
        }

        public static TweenKey To(Func<Vector3> getStartValue, Action<Vector3> setValue, Vector3 endValue, float duration, AnimationCurve animationCurve)
        {
            TweenKey key = new TweenKey(tweenKey++);

            Func<float, float> calculateEase = (input) => animationCurve.Evaluate(input);

            Vector3 startValue = getStartValue();
            tweenItems.Add(key, new TweenItem<Vector3>()
            {
                startValue = startValue,
                endValue = endValue,
                curValue = startValue,
                duration = duration,
                curTime = Time.deltaTime,
                calculateEase = calculateEase,
                setAction = setValue
            });
            return key;
        }

        public static void Kill(TweenKey key)
        {
            tweenItems.Remove(key);
        }

        public static void Update()
        {
            removeList.Clear();
            foreach (var (key, item) in tweenItems)
            {
                item.curTime += Time.deltaTime;
                if (item.curTime > item.duration)
                {
                    item.curValue = item.endValue;
                    removeList.Add(key);
                }
                else
                {
                    float easeValue = item.calculateEase(item.curTime / item.duration);
                    item.curValue.x = CalculateValue(item.startValue.x, item.endValue.x, easeValue);
                    item.curValue.y = CalculateValue(item.startValue.y, item.endValue.y, easeValue);
                    item.curValue.z = CalculateValue(item.startValue.z, item.endValue.z, easeValue);
                }
                try
                {
                    item.setAction(item.curValue);
                }
                catch (Exception e)
                {
                    removeList.Add(key);
                    Debug.LogWarning("Tween Error: " + e);
                }
            }
            for (int i=0; i<removeList.Count; i++)
            {
                tweenItems.Remove(removeList[i]);
            }
        }

        public static float CalculateValue(float start, float end, float k)
        {
            return start + (end - start) * k;
        }

        public static float Linear(float input)
        {
            return input;
        }

        public static float EaseInSine(float input)
        {
            return 1 - Mathf.Cos(input * Mathf.PI / 2);
        }

        public static float EaseOutSine(float input)
        {
            return Mathf.Sin(input * Mathf.PI / 2);
        }

        public static float EaseInOutSine(float input)
        {
            return -(Mathf.Cos(Mathf.PI * input) - 1) / 2;
        }

        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;
        const float c3 = c1 + 1;

        public static float EaseInBack(float input)
        {
            return c3 * input * input * input - c1 * input * input;
        }

        public static float EaseOutBack(float input)
        {
            return 1 + c3 * Mathf.Pow(input - 1, 3) + c1 * Mathf.Pow(input - 1, 2);
        }

        public static float EaseInOutBack(float input)
        {
            return input < 0.5
              ? (Mathf.Pow(2 * input, 2) * ((c2 + 1) * 2 * input - c2)) / 2
              : (Mathf.Pow(2 * input - 2, 2) * ((c2 + 1) * (input * 2 - 2) + c2) + 2) / 2;
        }
    }

    public enum EaseType
    {
        Linear = 0,
        EaseInSine,
        EaseOutSine,
        EaseInOutSine,
        EaseInBack,
        EaseOutBack,
        EaseInOutBack
    }

    public class TweenItem<T>
    {
        public T startValue;
        public T endValue;
        public T curValue;
        public float duration;
        public float curTime;
        public Func<float, float> calculateEase;
        public Action<T> setAction;
    }

    public struct TweenKey : IEquatable<TweenKey>
    {
        int key;

        public TweenKey(int key)
        {
            this.key = key;
        }

        public void Kill()
        {
            Tween.Kill(this);
        }

        public bool Equals(TweenKey other)
        {
            return this.key == other.key;
        }

        public override int GetHashCode()
        {
            return key;
        }
    }
}

