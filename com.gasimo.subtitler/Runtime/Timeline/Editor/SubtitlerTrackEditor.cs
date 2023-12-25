#if TIMELINE_PRESENT
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEditor.Timeline;

namespace Gasimo.Subtitles.Timeline
{
    /// <summary>
    /// Custom editor for the track. Will be used for custom icon once I find the docs on hows the proper way to implement this.
    /// </summary>
    [CustomTimelineEditor(typeof(SubtitlerTrack))]
    public class SubtitlerTrackEditor : TrackEditor
    {
        
        public static Sprite icon;

        override public TrackDrawOptions GetTrackOptions(TrackAsset track, Object binding)
        {
            var options = base.GetTrackOptions(track, binding);

            // See:  https://github.com/halak/unity-editor-icons
            // options.icon = (Texture2D)icon.texture;
            return options;
        }


    }
}
#endif