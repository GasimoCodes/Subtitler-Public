using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace Gasimo.Subtitles
{
    [RequireComponent(typeof(UIDocument))]
    public class Subtitler : MonoSingleton<Subtitler>
    {
        [Header("Appearance")]
        [SerializeField] private Color speakerHighlight = Color.white;
        [SerializeField] private bool enableBackgroundPanel = true;
        [SerializeField] private TextAnchor subtitlerAlign = TextAnchor.MiddleLeft;
        [SerializeField] private int subtitleSize = 24;
        [SerializeField] private int subtitlePoolSize = 10;

        [Header("References")]
        [SerializeField] private AudioListener player;

        private UIDocument uiDocument;
        private VisualElement displayPanel;
        private ObjectPool<Label> subtitlePool;
        private Dictionary<int, CancellationTokenSource> activeSubtitles = new Dictionary<int, CancellationTokenSource>();
        private int activeSubtitleCount = 0;
        private int nextSubtitleId = 0;
        private bool isInitialized = false;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        private void Start()
        {
            if (player == null)
            {
                Debug.LogWarning("Player object not assigned, range-limited subtitles will fail. Attempting to find AudioListener.");
                player = FindObjectOfType<AudioListener>();
                if (player == null)
                {
                    Debug.LogError("Failed to find AudioListener. Range-limited subtitles will not work.");
                }
            }
        }

        private void Initialize()
        {
            if (isInitialized) return;

            uiDocument = GetComponent<UIDocument>();
            InitializeDisplayPanel();
            InitializeSubtitlePool();
            isInitialized = true;
        }

        private void InitializeDisplayPanel()
        {
            displayPanel = uiDocument.rootVisualElement.Q<VisualElement>("SubtitlePanel");
            if (displayPanel == null)
            {
                Debug.LogError("SubtitlePanel not found in UIDocument. Please ensure it exists in your UI layout.");
                return;
            }

            displayPanel.usageHints = UsageHints.GroupTransform;
            displayPanel.style.visibility = Visibility.Hidden;
            displayPanel.Clear();
        }

        private void InitializeSubtitlePool()
        {
            subtitlePool = new ObjectPool<Label>(
                createFunc: CreateSubtitleLabel,
                actionOnGet: PrepareSubtitleLabel,
                actionOnRelease: ReleaseSubtitleLabel,
                actionOnDestroy: DestroySubtitleLabel,
                collectionCheck: false,
                defaultCapacity: 5,
                maxSize: subtitlePoolSize
            );
        }

        #region PoolFunctions

        private Label CreateSubtitleLabel()
        {
            var label = new Label
            {
                enableRichText = true,
                usageHints = UsageHints.DynamicTransform
            };
            label.AddToClassList("Label_Hide");
            label.style.fontSize = subtitleSize;
            label.style.flexWrap = Wrap.Wrap;
            label.style.unityTextAlign = new StyleEnum<TextAnchor>(subtitlerAlign);
            return label;
        }

        private void PrepareSubtitleLabel(Label label)
        {
            displayPanel.Add(label);
            label.text = string.Empty;
            label.style.visibility = Visibility.Visible;
        }

        private void ReleaseSubtitleLabel(Label label)
        {
            displayPanel.Remove(label);
            label.text = string.Empty;
            label.style.visibility = Visibility.Hidden;
        }

        private void DestroySubtitleLabel(Label label)
        {
            label.RemoveFromHierarchy();
        }

        #endregion

        /// <summary>
        /// Plays a sequence of SubtitleEntries on a given AudioSource
        /// </summary>
        /// <param name="sequenceData">Sequence to be played</param>
        /// <param name="audioSource">AudioSource to play through</param>
        /// <returns>Id of the session instance</returns>
        public int PlaySubtitleSequence(SubtitleSequenceData sequenceData, AudioSource audioSource)
        {
            Initialize();
            int id = GetNextSubtitleId();
            var cts = new CancellationTokenSource();
            activeSubtitles[id] = cts;
            _ = PlaySubtitleSequenceAsync(sequenceData, audioSource, id, cts.Token);
            return id;
        }

        /// <summary>
        /// Plays a single line of subtitles on a given AudioSource
        /// </summary>
        /// <param name="entry">Entry containing the subtitle data</param>
        /// <param name="audioSource">AudioSource to play through</param>
        /// <returns>Id of the session instance</returns>
        public int PlaySubtitleEntry(ISubtitleEntry entry, AudioSource audioSource)
        {
            Initialize();
            int id = GetNextSubtitleId();
            var cts = new CancellationTokenSource();
            activeSubtitles[id] = cts;
            _ = PlaySubtitleEntryAsync(entry, audioSource, cts.Token);
            return id;
        }

        /// <summary>
        /// Removes and hides a Subtitle session immediately.
        /// </summary>
        /// <param name="id">Id of the session to hide</param>
        public void RemoveSubtitle(int id)
        {
            if (activeSubtitles.TryGetValue(id, out var cts))
            {
                cts.Cancel();
                cts.Dispose();
                activeSubtitles.Remove(id);
            }
        }

        /// <summary>
        /// Removes and hides a active Subtitle Session with the oldest id.
        /// </summary>
        public void RemoveOldest()
        {
            if (activeSubtitles.Count > 0)
            {
                int oldestSubtitleId = activeSubtitles.Keys.Min();
                RemoveSubtitle(oldestSubtitleId);
            }
        }

        private async Task PlaySubtitleSequenceAsync(SubtitleSequenceData sequenceData, AudioSource audioSource, int id, CancellationToken cancellationToken)
        {
            try
            {
                foreach (var entry in sequenceData.Subtitles)
                {
                    await Task.Delay(TimeSpan.FromSeconds(entry.waitFor), cancellationToken);
                    _ = PlaySubtitleEntryAsync(entry, audioSource, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                // Subtitle sequence was cancelled, clean up if necessary
            }
            finally
            {
                activeSubtitles.Remove(id);
            }
        }

        private async Task PlaySubtitleEntryAsync(ISubtitleEntry entry, AudioSource audioSource, CancellationToken cancellationToken)
        {
            if (!ShouldDisplaySubtitle(entry, audioSource)) return;

            if (audioSource != null && entry.getAudio() != null)
            {
                audioSource.PlayOneShot(entry.getAudio());
            }

            entry.getSubtitleEvent()?.Raise();

            if (!string.IsNullOrEmpty(entry.getDialogue()))
            {
                await DisplaySubtitleAsync(entry.getDialogue(), entry.getSpeaker(), entry.getDisplayFor(), cancellationToken);
            }
        }

        private async Task DisplaySubtitleAsync(string message, string speaker, float displayDuration, CancellationToken cancellationToken)
        {
            var subtitle = subtitlePool.Get();
            activeSubtitleCount++;
            UpdateSubtitlePanelVisibility();

            try
            {
                SetSubtitleText(subtitle, speaker, message);
                await AnimateSubtitleEntrance(subtitle, cancellationToken);
                await Task.Delay(TimeSpan.FromSeconds(displayDuration), cancellationToken);
            }
            finally
            {
                await AnimateSubtitleExit(subtitle, CancellationToken.None);

                activeSubtitleCount--;
                subtitlePool.Release(subtitle);
                UpdateSubtitlePanelVisibility();
            }
        }

        private void SetSubtitleText(Label subtitle, string speaker, string message)
        {
            subtitle.text = string.IsNullOrEmpty(speaker)
                ? message
                : $"<color=#{ColorUtility.ToHtmlStringRGB(speakerHighlight)}><b>{speaker}</b></color>: {message}";
        }

        #region animations

        private async Task AnimateSubtitleEntrance(Label subtitle, CancellationToken cancellationToken)
        {
            subtitle.style.maxHeight = 0;
            subtitle.experimental.animation
                .Start(0, 999, 100, (element, value) => element.style.maxHeight = value)
                .Ease(Easing.InSine);

            await Task.Delay(50, cancellationToken);
            subtitle.RemoveFromClassList("Label_Hide");
            await Task.Delay(50, cancellationToken);
        }

        private async Task AnimateSubtitleExit(Label subtitle, CancellationToken cancellationToken)
        {
            subtitle.AddToClassList("Label_Hide");

            subtitle.experimental.animation
                .Start(1000, 0, 100, (element, value) => element.style.maxHeight = value)
                .Ease(Easing.Linear);

            await Task.Delay(100, cancellationToken);
        }

        private void UpdateSubtitlePanelVisibility()
        {
            if (!enableBackgroundPanel) return;

            if (activeSubtitleCount == 0)
            {
                displayPanel.AddToClassList("SubtitlePanel_Hide");
                displayPanel.style.visibility = Visibility.Hidden;
            }
            else
            {
                displayPanel.RemoveFromClassList("SubtitlePanel_Hide");
                displayPanel.style.visibility = Visibility.Visible;
            }
        }

        #endregion

        private int GetNextSubtitleId()
        {
            return Interlocked.Increment(ref nextSubtitleId);
        }

        #region utils

        private bool ShouldDisplaySubtitle(ISubtitleEntry entry, AudioSource audioSource)
        {
            if (audioSource == null) return true;
            if (audioSource.volume <= 0.05f || !audioSource.enabled) return false;
            if (audioSource.spatialBlend == 1 && !IsWithinAudioRange(audioSource)) return false;
            return true;
        }

        private bool IsWithinAudioRange(AudioSource audioSource)
        {
            return Vector3.Distance(player.transform.position, audioSource.transform.position) <= audioSource.maxDistance;
        }

        #endregion
    }
}
