using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gasimo.Subtitles
{
    /// <summary>
    /// Component representing one subtitle source in the world. 
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class SubtitleContainer : MonoBehaviour
    {
        /// <summary>
        /// Play automatically on awake?
        /// </summary>
        public bool autoPlay = true;

        /// <summary>
        /// Subtitle Data to play
        /// </summary>
        public SubtitleData subtitleData;


        // Start is called before the first frame update
        void Start()
        {
            if(autoPlay) { 
            Play();
            }
        }

        /// <summary>
        /// Plays the attached Subtitle Data
        /// </summary>
        public void Play()
        {
            Subtitler.Instance.playSubtitle(subtitleData, this.GetComponent<AudioSource>());
        }
    }
}