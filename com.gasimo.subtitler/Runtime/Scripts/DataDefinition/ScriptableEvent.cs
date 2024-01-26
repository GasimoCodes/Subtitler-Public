using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gasimo.Subtitles
{

    /// <summary>
    /// Object which you can bind custom events to. Use it by creating an ScriptableObject for every instance of events you want to use.
    /// To subscribe, use onEventRaised. To raise, use Raise(). Subtitles automatically calls Raise() when you attach instance of this class to a Subtitle line. 
    /// </summary>
    [CreateAssetMenu(fileName = "ScriptableEvent", menuName = "Gasimo/ScriptableEvent")]
    [Serializable]
    public class ScriptableEvent : ScriptableObject
    {
        
        public UnityAction onEventRaised;

        /// <summary>
        /// Invokes events subscribed to onEventRaised.
        /// </summary>
        public void Raise()
        {
            onEventRaised?.Invoke();
        }

    }
}
