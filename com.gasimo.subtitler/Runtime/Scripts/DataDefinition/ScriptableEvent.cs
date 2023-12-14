using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gasimo.Subtitles
{
    [CreateAssetMenu(fileName = "ScriptableEvent", menuName = "Gasimo/ScriptableEvent")]
    [Serializable]
    public class ScriptableEvent : ScriptableObject
    {
        public delegate void ScriptableEventHandler();
        public event ScriptableEventHandler onEventRaised;

        public void Raise()
        {
            onEventRaised.Invoke();
        }

    }
}
