using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gasimo.Subtitles
{

    [RequireComponent(typeof(AudioSource))]
    public class SubtitleContainer : MonoBehaviour
    {
        public bool autoPlay = true;
        public SubtitleData subtitleData;


        // Start is called before the first frame update
        void Start()
        {
            if(autoPlay) { 
            Play();
            }
        }

        public void Play()
        {
            Subtitler.instance.playSubtitle(subtitleData, this.GetComponent<AudioSource>());
        }
    }
}