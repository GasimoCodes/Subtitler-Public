using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Gasimo.Subtitles.Timeline
{
    /// <summary>
    /// Behaviour called by clips
    /// </summary>
    public class SubtitlerPlayableBehaviour : PlayableBehaviour
    {
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            base.OnBehaviourPlay(playable, info);
            Subtitler.Instance.DisplaySubtitle("AAA", "AAAA", 10);
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            base.OnBehaviourPlay(playable, info);
            Debug.Log("STOP");
        }
    }
}
