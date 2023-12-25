using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;


namespace Gasimo.Subtitles
{
    /// <summary>
    /// Subtitler Manager. Contains all display logic and cc functions.
    /// </summary>
    public class Subtitler : MonoSingleton<Subtitler>
    {
        /// <summary>
        /// Target audioListener, used for audio occlusion calculation.
        /// </summary>
        public AudioListener player;

        /// <summary>
        /// Pool of TextMeshProUGUI Instances. Initializes automatically.
        /// </summary>
        public ObjectPool<TextMeshProUGUI> subtitlePool;

        /// <summary>
        /// UI Panel GameObject where Subtitles will spawn
        /// </summary>
        public Transform displayPanel;

        /// <summary>
        /// Default Speaker Color
        /// </summary>
        public Color speakerHighlight = Color.white;

        /// <summary>
        /// Should background panel be visible?
        /// </summary>
        public bool enableBackgroundPanel = true;

        /// <summary>
        /// Centers text. Text is aligned left by default.
        /// </summary>
        public bool centeredText = false;

        /// <summary>
        /// Size of subtitle text. Default is 24.
        /// </summary>
        public int subtitleSize = 24;


        [HideInInspector]
        /// <summary>
        /// Max amount of lines visible at once. Unused right now. 
        /// </summary>
        public int subtitlePoolSize = 10;


        // Privates
        int shownLines = 0;
        Image displayBackground;
        Dictionary<int, bool> activeSubtitleList = new Dictionary<int, bool>();
        object subtitleLock = new System.Object();
        bool isReady = false;

        #region init


        protected override void Awake()
        {
            // Set singleton instance
            base.Awake();
            Init();
        }

        private void Start()
        {
            if (player == null)
                Debug.LogError("Player object not assigned, range-limited subtitles will fail.");
        }

        /// <summary>
        /// Used internaly to force init before awake
        /// </summary>
        public void Init()
        {
            if (!isReady)
            {
                initSettings();
                initSubtitlePool();
                isReady = true;
            }
        }

        private void initSettings()
        {
            Addressables.InitializeAsync();
            Addressables.LoadAssetAsync<GameObject>("Gasimo/Subtitle");

            displayBackground = displayPanel.GetComponent<Image>();
            displayBackground.enabled = false;

            if (centeredText)
                displayPanel.GetComponent<VerticalLayoutGroup>().childAlignment = TextAnchor.LowerCenter;

            if (player == null)
                player = FindAnyObjectByType<AudioListener>();
        }

        private void initSubtitlePool()
        {

            // This is in case we already ran this
            if (subtitlePool != null)
            {
                return;
            }

            // Define pool methods
            subtitlePool = new ObjectPool<TextMeshProUGUI>(
            createFunc: () =>
            {
                var op = Addressables.LoadAssetAsync<GameObject>("Gasimo/Subtitle");
                GameObject go = Instantiate<GameObject>(op.WaitForCompletion(), displayPanel);
                TextMeshProUGUI text = go.GetComponent<TextMeshProUGUI>();
                text.fontSize = subtitleSize;
                return text;

            },
            actionOnGet: (obj) =>
            {
                obj.transform.SetAsLastSibling();
                obj.text = "";
                obj.gameObject.SetActive(true);
            },
            actionOnRelease: (obj) =>
            {
                obj.text = "";
                obj.gameObject.SetActive(false);
            },
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: false,
            defaultCapacity: 5,
            maxSize: 10);
        }

        #endregion init

        /// <summary>
        /// Plays a given subtitle track 
        /// </summary>
        /// <param name="sD">Subtitle Data file</param>
        /// <param name="aS">AudioSource to playOneShot through</param>
        /// <returns>Cancellation ID of the subtitle instance</returns>
        public int PlaySubtitleSequence(SubtitleSequenceData sD, AudioSource aS)
        {
            int id = 0;

            lock (subtitleLock)
            {
                if (activeSubtitleList.Count != 0)
                {
                    id = activeSubtitleList.Max(kvp => kvp.Key) + 1;
                }
                activeSubtitleList.Add(id, true);
            }

            // Debug.Log("Playing " + id);

            _ = playSubtitleFile(sD, aS, id);
            return id;
        }


        /// <summary>
        /// Plays a given subtitle entry 
        /// </summary>
        /// <param name="sD">Subtitle Data file</param>
        /// <param name="aS">AudioSource to playOneShot through</param>
        /// <returns>Cancellation ID of the subtitle instance</returns>
        public void PlaySubtitleEntry(SubtitleDataEntry sD, AudioSource aS)
        {
            Init();
            playSubtitleEntry(sD, aS);
        }



        /// <summary>
        /// Immediately kills an active subtitle loop based on ID
        /// </summary>
        /// <param name="id">Subtitle Session ID to be killed</param>
        public void killSubtitleById(int id)
        {
            lock (subtitleLock)
            {
                Debug.Log("Removing " + id);
                if (activeSubtitleList.ContainsKey(id))
                {
                    activeSubtitleList[id] = false;
                }
            }
        }

        /// <summary>
        /// Internal method for playing Subtitles
        /// </summary>
        /// <param name="sD">File to play</param>
        /// <param name="aS">AudioSource to play this with</param>
        /// <returns>UniTaskVoid Handle</returns>
        private async UniTaskVoid playSubtitleFile(SubtitleSequenceData sD, AudioSource aS, int id)
        {

            if (sD != null)
                foreach (SubtitleDataEntry sE in sD.Subtitles)
                {

                    await UniTask.Delay((int)(sE.waitFor * 1000f));

                    // Check if we werent cancelled
                    lock (subtitleLock)
                    {
                        if (activeSubtitleList[id] == false)
                        {
                            activeSubtitleList.Remove(id);
                            return;
                        }
                    }

                    playSubtitleEntry(sE, aS);

                }
        }



        private void playSubtitleEntry(SubtitleDataEntry sE, AudioSource aS)
        {
            bool isRangeLimited = false;

            // If we have audioSource, volume checks for occlusion
            if (aS != null)
            {
                isRangeLimited = (aS.spatialBlend == 1);

                // Play audio
                if (sE.audio != null)
                    aS.PlayOneShot(sE.audio);

                // If the audioSource is really, really silent, or straight up disabled, do not show subtitle
                if (aS == null || aS.volume <= 0.05f || aS.enabled == false)
                {
                    return;
                }
            }


            // Trigger programmed events
            if (sE.subtitleEvent != null)
            {
                sE.subtitleEvent.Raise();
            }


            // Display dialogue
            if (sE.dialogue != "")
            {

                // Sound aint null AND (If we are (range-limited, out of range AND not a 2D source) OR IF (the audioSource is not playing AND there was an valid AudioClip))
                if ( aS != null && ((isRangeLimited && !checkAudioDistance(aS.maxDistance, aS) && aS.spatialBlend != 0) || (aS.isPlaying == false && sE.audio != null)))
                {
                    return;
                }

                if (enableBackgroundPanel)
                {
                    displayPanel.GetComponent<Image>().DOKill();
                    displayPanel.GetComponent<Image>().enabled = true;
                    displayPanel.GetComponent<Image>().DOFade(0.4f, 0.1f);
                }

                _ = DisplaySubtitle(sE.dialogue, sE.speaker, sE.displayFor);
            }

        }

        /// <summary>
        /// Method to display a single line of subtitles. Use this when you need to play subtitles using custom logic and want to skip all the additional features.
        /// </summary>
        /// <param name="message">Message (Dialogue) line to show</param>
        /// <param name="speaker">Name of object which said the line (Leave empty string for none)</param>
        /// <param name="displayFor">How long should this line be displayed for?</param>
        /// <returns></returns>
        private async UniTaskVoid DisplaySubtitle(string message, string speaker, float displayFor)
        {

            /*
#if UNITY_EDITOR
            if (!Application.isPlaying)
                // Show debug thru editor window
                return;
#endif
            */



            TextMeshProUGUI subtitle = subtitlePool.Get();
            shownLines++;

            CheckHideSubtitlePanel();

            // Display text
            if (speaker != "")
                subtitle.text += $"<color=#{ColorUtility.ToHtmlStringRGB(speakerHighlight)}><b>{speaker}</b></color>: ";
            subtitle.text += message;
            subtitle.alpha = 255;
            subtitle.GetComponent<ContentSizeFitter>().enabled = true;

            LayoutRebuilder.ForceRebuildLayoutImmediate(displayPanel.GetComponent<RectTransform>());


            // Neat animation intro
            subtitle.transform.DOScaleY(0.5f, 0);
            subtitle.transform.DOScaleY(1, 0.2f);
           


            await UniTask.Delay(100);
            subtitle.DOFade(1, 0.1f);


            // Display for specified amount of time
            await UniTask.Delay((int)(displayFor * 1000f));


            // Fadeout, then make small
            subtitle.DOFade(0.0f, 0.3f);
            // await UniTask.Delay(0.3f);

            // LayoutRebuilder.ForceRebuildLayoutImmediate(displayPanel.GetComponent<RectTransform>());
            subtitle.GetComponent<ContentSizeFitter>().enabled = false;
            subtitle.transform.localScale = new Vector3(1, 1, 1);
            await subtitle.transform.DOScaleY(0, 0.5f).AsyncWaitForCompletion();


            shownLines--;

            subtitlePool.Release(subtitle);
            CheckHideSubtitlePanel();

        }


        #region utils

        /// <summary>
        /// Checks whether the Subtitle Panel shouldnt be hidden. Hides it if no subtitles are currently on display.
        /// </summary>
        /// <returns></returns>
        public void CheckHideSubtitlePanel()
        {
            if (enableBackgroundPanel == false) return;

            if (shownLines == 0)
            {
                displayBackground.enabled = false;
            }
            else
            {
                displayBackground.enabled = true;
            }
        }

        /// <summary>
        /// Checks whether the distance between player and the audioSource is within the AudioSource maxRange
        /// (e.g. if the player is in the AudioSources range)
        /// </summary>
        /// <param name="range">Max range from audiosource</param>
        /// <param name="audioObj">AudioSource to check</param>
        /// <returns></returns>
        private bool checkAudioDistance(float range, AudioSource audioObj)
        {
            if (Vector3.Distance(player.transform.position, audioObj.transform.position) <= range)
            {
                return true;
            }

            return false;
        }

        #endregion utils

    }
}
