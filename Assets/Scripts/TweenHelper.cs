using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KTween
{
    public class TweenHelper : MonoBehaviour
    {
        private event Action updateActions;

        public void RegisterUpdateAction(Action updateAction)
        {
            updateActions += updateAction;
        }

        public void UnRegisterUpdateAction(Action updateAction)
        {
            updateActions -= updateAction;
        }

        void Update()
        {
            updateActions?.Invoke();
        }
    }
}
