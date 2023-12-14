using gasimo.subtitles;
using System;
using UnityEngine;

namespace Gasimo.Subtitles
{
    [Serializable]
    public class SubtitleDataEntry
    {

        
        public string dialogue;

        [SerializeField]
        public string speaker;
        public AudioClip audio;
        public ScriptableEvent subtitleEvent;

        public float waitFor = 1f;
        public float displayFor = 5;

        public float start;  // Add these properties
        public float end;    // Add these properties


    }
}