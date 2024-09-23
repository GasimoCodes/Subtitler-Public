using System;
using UnityEngine;

#if SUBTITLER_LOCALIZATION
using UnityEngine.Localization;
#endif

namespace Gasimo.Subtitles
{
    /// <summary>
    /// Contains data for 1 line of dialogue.
    /// </summary>
    [Serializable]
    public class SubtitleDataEntry : ISubtitleEntry
    {

#if !SUBTITLER_LOCALIZATION
        
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

#else
        [SerializeField]
        public LocalizedString dialogue;
        [SerializeField]
        public LocalizedString speaker;
        [SerializeField]
        public LocalizedAudioClip audio;
#endif

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


#if !SUBTITLER_LOCALIZATION
        public AudioClip getAudio()
        {
            return audio;
        }

        public string getDialogue()
        {
            return dialogue;
        }

        public string getSpeaker()
        {
            return speaker;
        }
#else
        public AudioClip getAudio()
        {
            if (audio != null && audio.IsEmpty != true)
            {
                return audio.LoadAsset();
            }
            else
            {
                return null;
            }

        }

        public string getDialogue()
        {
            if (dialogue != null && dialogue.IsEmpty != true)
            {
                return dialogue.GetLocalizedString();
            }
            else
            {
                return "";
            }
        }

        public string getSpeaker()
        {
            if(speaker != null && speaker.IsEmpty != true)
            {
                return speaker.GetLocalizedString();
            }
            else
            {
                return "";
            }
        }
#endif

        public float getDisplayFor()
        {
            return displayFor;
        }


        public ScriptableEvent getSubtitleEvent()
        {
            return subtitleEvent;
        }

        public float getWaitFor()
        {
            return waitFor;
        }
    }
}