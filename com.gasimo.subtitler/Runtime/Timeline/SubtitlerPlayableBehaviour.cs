using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

#if TIMELINE_PRESENT

namespace Gasimo.Subtitles.Timeline
{
    /// <summary>
    /// Behaviour called by clips. This plays a single SubtitleDataEntry at the beginning of the clip.
    /// </summary>
    public class SubtitlerPlayableBehaviour : PlayableBehaviour
    {

        public SubtitleDataEntry entry;


        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            base.OnBehaviourPlay(playable, info);
            if (Application.isPlaying)
                Subtitler.Instance.PlaySubtitleEntry(entry, null);

        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            base.OnBehaviourPlay(playable, info);
        }
    }
}
#endif