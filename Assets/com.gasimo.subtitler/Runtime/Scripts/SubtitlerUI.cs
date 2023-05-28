using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;
using Cysharp.Threading.Tasks.CompilerServices;
using Cysharp.Threading.Tasks.Triggers;

namespace Gasimo.Subtitles
{
    public class SubtitlerUI : MonoBehaviour
    {
        public AudioListener player;
        public ObjectPool<TextMeshProUGUI> subtitlePool;
        public Transform displayPanel;
        public Color speakerHighlight = Color.white;
        public bool enableBackgroundPanel = true;
        public bool centeredText = false;
        public int subtitleSize = 24;

        // Privates
        int shownLines = 0;
        Image displayBackground;


        private void Awake()
        {
            Subtitler.instance = this;
            Addressables.InitializeAsync();
            Addressables.LoadAssetAsync<GameObject>("Gasimo/Subtitle");

            displayBackground = displayPanel.GetComponent<Image>();
            displayBackground.enabled = false;

            if (centeredText)
                displayPanel.GetComponent<VerticalLayoutGroup>().childAlignment = TextAnchor.LowerCenter;


            /*
            if(player == null)
                player = FindAnyObjectByType<AudioListener>();
            */

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
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 5,
            maxSize: 20);

            // Foolish way to precache
            // subtitlePool.Release(subtitlePool.Get());

        }

        private void Start()
        {
            if (player == null)
                Debug.LogError("Player object not assigned, range-limited subtitles will fail.");
        }


        /// <summary>
        /// Plays a given subtitle track 
        /// </summary>
        /// <param name="sD"></param>
        /// <param name="aS"></param>
        public void playSubtitle(SubtitleData sD, AudioSource aS)
        {
            playSubtitleFile(sD, aS);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sD"></param>
        /// <param name="aS"></param>
        /// <returns></returns>
        private async UniTaskVoid playSubtitleFile(SubtitleData sD, AudioSource aS)
        {
            bool isRangeLimited = (aS.spatialBlend == 1);

            if (sD != null)
                foreach (SubtitleDataEntry sE in sD.Subtitles)
                {
                    await UniTask.Delay((int)(sE.waitFor * 1000f));
                    
                    if (enableBackgroundPanel)
                    {
                        displayPanel.GetComponent<Image>().DOKill();
                        displayPanel.GetComponent<Image>().enabled = true;
                        displayPanel.GetComponent<Image>().DOFade(0.4f, 0.1f);
                    }

                    // Play audio
                    if (sE.audio != null)
                        aS.PlayOneShot(sE.audio);

                    // Display dialogue
                    if (sE.dialogue != "")
                    {
                        // If we are range-limited and out of range, skip
                        if (isRangeLimited && !checkAudioDistance(aS.maxDistance, aS))
                        {
                            continue;
                        }

                        DisplaySubtitle(sE.dialogue, sE.speaker, sE.displayFor);
                    }

                }
        }

        /// <summary>
        /// Checks whether the distance between player and the audioSource is within a range
        /// </summary>
        /// <param name="range"></param>
        /// <param name="audioObj"></param>
        /// <returns></returns>
        private bool checkAudioDistance(float range, AudioSource audioObj)
        {
            if (Vector3.Distance(player.transform.position, audioObj.transform.position) <= range)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Method to display a subtitle text
        /// </summary>
        /// <param name="message"></param>
        /// <param name="speaker"></param>
        /// <param name="displayFor"></param>
        /// <returns></returns>
        public async UniTaskVoid DisplaySubtitle(string message, string speaker, int displayFor)
        {
            TextMeshProUGUI subtitle = subtitlePool.Get();
            shownLines++;

            // Display text
            if (speaker != "")
                subtitle.text += $"<color=#{ColorUtility.ToHtmlStringRGB(speakerHighlight)}><b>{speaker}</b></color>: ";
            subtitle.text += message;
            subtitle.alpha = 255;
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(displayPanel.GetComponent<RectTransform>());

            // Neat animation intro
            subtitle.transform.DOScaleY(0.5f, 0);
            subtitle.transform.DOScaleY(1, 0.2f);
            await UniTask.Delay(100);
            subtitle.DOFade(1, 0.1f);


            // Display for specified amount of time
            await UniTask.Delay((int)(displayFor * 1000f));


            // Fadeout
            await subtitle.DOFade(0, 1).AsyncWaitForCompletion();


            shownLines--;

            subtitlePool.Release(subtitle);
            CheckHideSubtitlePanel();

        }


        public async UniTaskVoid CheckHideSubtitlePanel()
        {
            if (shownLines == 0)
            {
                displayBackground.enabled = false;
            }
        }


    }
}