#if TIMELINE_PRESENT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


namespace Gasimo.Subtitles.Timeline
{
    /// <summary>
    /// Controls creating a track in Timeline
    /// </summary>
    [TrackColor(0.945f, 0.349f, 0.165f)]
    [TrackClipType(typeof(SubtitlerPlayableAsset))]
    [TrackBindingType(typeof(AudioSource))]
    public class SubtitlerTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<SubtitlerTrackMixer>.Create(graph, inputCount);
        }
    }
}
#endif