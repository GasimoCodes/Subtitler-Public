using FMODUnity;
using Gasimo.Subtitles;
using GrassFlow;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Gasimo.Subtitles
{
#if UNITY_2023_3_OR_NEWER
    [CustomEditor(typeof(SubtitleSequenceData))]
    public class SubtitleDataEditor : Editor
    {
        [SerializeField]
        public VisualTreeAsset m_VisualTreeAsset = default;

        private const float TimelineHeight = 500;
        private const float WaveformHeight = 50f;
        private const float EntryHeight = 50f;
        private const float TimelineWidth = 1000f; // Adjust this value as needed

        private ScrollView timelineScrollView;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            VisualElement uiFromUXML = m_VisualTreeAsset.CloneTree();
            root.Add(uiFromUXML);

            var listView = uiFromUXML.Q<MultiColumnListView>();
            var serializedObject = new SerializedObject(target);
            listView.Rebuild();

            // Add scrollable timeline view
            timelineScrollView = new ScrollView(ScrollViewMode.Horizontal);
            timelineScrollView.style.height = TimelineHeight;
            timelineScrollView.style.marginTop = 20;
            timelineScrollView.style.marginBottom = 20;
            root.Add(timelineScrollView);

            var timelineView = CreateTimelineView();
            timelineScrollView.Add(timelineView);

            // Register callback for changes in the list view
            RegisterChangeCallbacks(listView);

            return root;
        }

        /// <summary>
        /// When any value in the list view changes, refresh the timeline view.
        /// </summary>
        /// <param name="listView"></param>
        private void RegisterChangeCallbacks(MultiColumnListView listView)
        {
            listView.RegisterCallback<ChangeEvent<string>>(_ => RefreshTimeline());
            listView.RegisterCallback<ChangeEvent<AudioClip>>(_ => RefreshTimeline());
            listView.RegisterCallback<ChangeEvent<ScriptableEvent>>(_ => RefreshTimeline());
            listView.RegisterCallback<ChangeEvent<float>>(_ => RefreshTimeline());


        }

        private VisualElement CreateTimelineView()
        {
            var timelineContainer = new VisualElement();
            // Width = 100%
            timelineContainer.style.width = new StyleLength(new Length(100, LengthUnit.Percent));

            // auto resize to match content height
            timelineContainer.style.height = TimelineHeight;
            
            

            timelineContainer.style.backgroundColor = Color.gray;

            var subtitleData = (SubtitleSequenceData)target;
            float totalDuration = CalculateTotalDuration(subtitleData);

            // Add time markers
            //var timeMarkers = CreateTimeMarkers(totalDuration);
            //timelineContainer.Add(timeMarkers);

            // Add waveform preview
            var waveformPreview = CreateWaveformPreview(subtitleData, totalDuration);
            timelineContainer.Add(waveformPreview);

            // Add timeline entries
            for (int i = 0; i < subtitleData.Subtitles.Length; i++)
            {
                var entry = subtitleData.Subtitles[i];
                var entryElement = CreateTimelineEntry(entry, totalDuration, i);
                timelineContainer.Add(entryElement);
            }


            return timelineContainer;
        }

        private VisualElement CreateTimelineEntry(SubtitleDataEntry entry, float totalDuration, int index)
        {
            var entryElement = new VisualElement();
            entryElement.style.position = Position.Relative;
            entryElement.style.height = EntryHeight;
            entryElement.style.left = new StyleLength(new Length((entry.start / totalDuration) * 100, LengthUnit.Percent));
            entryElement.style.width = new StyleLength(new Length(((entry.end - entry.start) / totalDuration) * 100, LengthUnit.Percent));
            entryElement.style.backgroundColor = new Color(0.4f, 0.4f, 0.8f, 0.8f);

            var label = new Label($"{entry.speaker}: {entry.dialogue}");
            label.style.fontSize = 10;
            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            label.style.color = Color.white;
            label.style.overflow = Overflow.Hidden;
            label.style.textOverflow = TextOverflow.Ellipsis;
            entryElement.Add(label);

            return entryElement;
        }

        private VisualElement CreateTimeMarkers(float totalDuration)
        {
            var markerContainer = new VisualElement();
            markerContainer.style.position = Position.Absolute;
            markerContainer.style.top = TimelineHeight - 0;
            markerContainer.style.left = 0;
            markerContainer.style.right = 0;
            markerContainer.style.height = 20;

            int markerCount = Mathf.CeilToInt(totalDuration / 5f);
            for (int i = 0; i <= markerCount; i++)
            {
                float time = i * 5f;
                float position = (time / totalDuration) * 100;

                var marker = new VisualElement();
                marker.style.position = Position.Absolute;
                marker.style.left = new StyleLength(new Length(position, LengthUnit.Percent));
                marker.style.width = 1;
                marker.style.height = 10;
                marker.style.backgroundColor = Color.white;

                var label = new Label(time.ToString("0.0") + "s");
                label.style.position = Position.Absolute;
                label.style.left = new StyleLength(new Length(position, LengthUnit.Percent));
                label.style.fontSize = 8;
                label.style.color = Color.white;

                markerContainer.Add(marker);
                markerContainer.Add(label);
            }

            return markerContainer;
        }

        private float CalculateTotalDuration(SubtitleSequenceData data)
        {
            float totalDuration = 0f;
            foreach (var entry in data.Subtitles)
            {
                totalDuration += entry.waitFor + entry.displayFor;
                entry.start = totalDuration - entry.displayFor;
                entry.end = totalDuration;
            }
            return totalDuration;
        }

        private void RefreshTimeline()
        {
            if (timelineScrollView != null)
            {
                timelineScrollView.Clear();
                var newTimelineView = CreateTimelineView();
                timelineScrollView.Add(newTimelineView);
            }
        }


        private VisualElement CreateWaveformPreview(SubtitleSequenceData data, float totalDuration)
        {
            var waveformContainer = new VisualElement();
            waveformContainer.style.height = WaveformHeight;
            waveformContainer.style.backgroundColor = new Color(0.8f, 0.3f, 0.3f);

            var renderTexture = new RenderTexture((int)TimelineWidth, (int)WaveformHeight, 0);
            renderTexture.Create();

            var waveformTexture = new Texture2D((int)TimelineWidth, (int)WaveformHeight, TextureFormat.RGBA32, false);

            RenderTexture.active = renderTexture;
            GL.Clear(true, true, Color.clear);
            GL.PushMatrix();
            GL.LoadPixelMatrix(0, TimelineWidth, WaveformHeight, 0);

            foreach (var entry in data.Subtitles)
            {
                float startPosition = (entry.start / totalDuration) * TimelineWidth;
                float entryWidth = ((entry.end - entry.start) / totalDuration) * TimelineWidth;

                if (entry.audio != null)
                {
                    float[] samples = new float[entry.audio.samples * entry.audio.channels];
                    entry.audio.GetData(samples, 0);

                    int stepSize = Mathf.CeilToInt((float)samples.Length / entryWidth);

                    for (int x = 0; x < entryWidth; x++)
                    {
                        float max = 0f;
                        for (int i = 0; i < stepSize && (x * stepSize + i < samples.Length); i++)
                        {
                            float sample = Mathf.Abs(samples[x * stepSize + i]);
                            if (sample > max) max = sample;
                        }

                        float height = max * WaveformHeight * 0.75f;
                        GL.Begin(GL.LINES);
                        GL.Color(new Color(0.7f, 0.7f, 1f));
                        GL.Vertex3(startPosition + x, WaveformHeight / 2 - height / 2, 0);
                        GL.Vertex3(startPosition + x, WaveformHeight / 2 + height / 2, 0);
                        GL.End();
                    }
                }
            }

            GL.PopMatrix();

            waveformTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            waveformTexture.Apply();

            RenderTexture.active = null;

            var waveformImage = new Image();
            waveformImage.image = waveformTexture;
            waveformImage.style.width = TimelineWidth;
            waveformImage.style.height = WaveformHeight;
            waveformContainer.Add(waveformImage);

            renderTexture.Release();

            return waveformContainer;
        }





    }
#endif
}