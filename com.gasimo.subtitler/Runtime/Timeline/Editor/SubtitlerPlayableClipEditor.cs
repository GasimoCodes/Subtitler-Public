#if TIMELINE_PRESENT
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;


namespace Gasimo.Subtitles.Timeline
{

    [CustomTimelineEditor(typeof(SubtitlerPlayableAsset))]
    public class SubtitlerPlayableClipEditor : ClipEditor
    {

        // Called when a clip value, it's attached PlayableAsset, or an animation curve on a template is changed from the TimelineEditor.
        // This is used to keep the displayName of the clip matching the text of the PlayableAsset.
        public override void OnClipChanged(TimelineClip clip)
        {
            var textPlayableasset = clip.asset as SubtitlerPlayableAsset;

            textPlayableasset.entry.displayFor = (float)clip.duration;

            if (textPlayableasset != null && !string.IsNullOrEmpty(textPlayableasset.entry.dialogue))
                clip.displayName = textPlayableasset.entry.speaker + ": " + textPlayableasset.entry.dialogue;
        }

    }
}
#endif