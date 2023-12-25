using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Gasimo.Subtitles.Timeline
{
    /// <summary>
    /// a clip instance containing one SubtitleDataEntry on the Timeline
    /// </summary>
#if TIMELINE_PRESENT
    public class SubtitlerPlayableAsset : PlayableAsset
    {

        public SubtitleDataEntry entry;


        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<SubtitlerPlayableBehaviour>.Create(graph);
            var playableBehaviour = playable.GetBehaviour();
            playableBehaviour.entry = entry;


            return playable;
        }
    }
#endif

}
