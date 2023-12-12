using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gasimo.Subtitles
{
    [CreateAssetMenu(fileName = "SubtitleFile", menuName = "Gasimo/SubtitleFile")]
    [Serializable]
    public class SubtitleData : ScriptableObject
    {
        [SerializeField]
        public SubtitleDataEntry[] Subtitles;

    }

}