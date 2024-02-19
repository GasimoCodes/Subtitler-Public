using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gasimo.Subtitles
{
    public interface ISubtitleEntry
    {

        public AudioClip getAudio();
        public string getDialogue();
        public string getSpeaker();
        public float getWaitFor();
        public float getDisplayFor();

        public ScriptableEvent getSubtitleEvent();

        
    }
}
