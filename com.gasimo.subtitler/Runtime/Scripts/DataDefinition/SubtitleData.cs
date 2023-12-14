using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gasimo.Subtitles
{
    /// <summary>
    /// Contains data holding a single Closed Caption sequence. For individual lines, see SubtitleDataEntry.
    /// </summary>
    [CreateAssetMenu(fileName = "SubtitleFile", menuName = "Gasimo/SubtitleFile")]
    [Serializable]
    public class SubtitleData : ScriptableObject
    {
        /// <summary>
        /// Sequence of Subtitles
        /// </summary>
        [SerializeField]
        public SubtitleDataEntry[] Subtitles;

    }

}