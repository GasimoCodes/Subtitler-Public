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
        public ExposedReference<Subtitler> SubtitlerInstance;


        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<SubtitlerPlayableBehaviour>.Create(graph);
            var playableBehaviour = playable.GetBehaviour();


            // Init Subtitler since this is called before awake :(
#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
                SubtitlerInstance.Resolve(graph.GetResolver()).Init();


            return playable;
        }

        
    }
}
