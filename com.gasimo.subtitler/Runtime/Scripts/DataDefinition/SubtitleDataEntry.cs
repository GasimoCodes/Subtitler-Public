using System;
using UnityEngine;

namespace Gasimo.Subtitles
{
    /// <summary>
    /// Contains data for 1 line of dialogue.
    /// </summary>
    [Serializable]
    public class SubtitleDataEntry
    {
        /// <summary>
        /// Written Closed-Captions
        /// </summary>
        public string dialogue;

        /// <summary>
        /// Display name of speaker/object which plays the dialogue. Leave empty for none (useful for sounds).
        /// </summary>
        [SerializeField]
        public string speaker;

        /// <summary>
        /// AudioClip which will play simultaneously with the subtitle line. Can be left empty to not play sound.
        /// </summary>
        public AudioClip audio;
        
        /// <summary>
        /// Programmable event which gets ivoked when this subtitle gets played. 
        /// Useful to trigger mechanics exactly when player hears a certain line.
        /// </summary>
        public ScriptableEvent subtitleEvent;

        /// <summary>
        /// Delay between last Subtitle line and this one. Leave 0 to play at same time as previous line.
        /// </summary>
        public float waitFor = 1f;

        /// <summary>
        /// How long will this subtitle be displayed for.
        /// </summary>
        public float displayFor = 5;


        public float start;  // Add these properties
        public float end;    // Add these properties


    }
}