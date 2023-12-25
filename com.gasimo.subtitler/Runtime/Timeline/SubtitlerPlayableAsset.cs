using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Gasimo.Subtitles.Timeline
{
    /// <summary>
    /// a clip instance on timeline
    /// </summary>
    public class SubtitlerPlayableAsset : PlayableAsset
    {

        public SubtitleDataEntry entry;


        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<SubtitlerPlayableBehaviour>.Create(graph);
            var playableBehaviour = playable.GetBehaviour();
            playableBehaviour.entry = entry;


            // Init Subtitler since this is called before awake :(
#if UNITY_EDITOR
            // if (Application.isPlaying)
#endif
                



            return playable;
        }






    }
}
