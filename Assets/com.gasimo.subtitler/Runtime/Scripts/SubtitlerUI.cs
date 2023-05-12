using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;

namespace Gasimo.Subtitles
{
    public class SubtitlerUI : MonoBehaviour
    {
        public AudioListener player;
        public ObjectPool<TextMeshProUGUI> subtitlePool;
        public Transform displayPanel;
        int shownLines = 0;

        private void Awake()
        {
            Subtitler.instance = this;
            Addressables.InitializeAsync();
            Addressables.LoadAssetAsync<GameObject>("Gasimo/Subtitle");

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
                return go.GetComponent<TextMeshProUGUI>();

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
            // Debug.Log(displayPanel.transform.GetChild(0).gameObject);

            displayPanel.GetComponent<Image>().enabled = false;

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



        private async UniTaskVoid playSubtitleFile(SubtitleData sD, AudioSource aS)
        {
            bool isRangeLimited = (aS.spatialBlend == 1);

            if(sD != null)
            foreach (SubtitleDataEntry sE in sD.Subtitles)
            {
                await UniTask.Delay((int)(sE.waitFor * 1000f));

                displayPanel.GetComponent<Image>().enabled = true;
                displayPanel.GetComponent<Image>().DOFade(0.4f, 0.1f);

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


            await UniTask.Delay((int)(4 * 1000f));

            if (shownLines == 0)
            {
                displayPanel.GetComponent<Image>().DOFade(0, 1);
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

            if (speaker != "")
                subtitle.text += $"<b>{speaker}</b>: ";
            subtitle.text += message;

            subtitle.alpha = 255;
            LayoutRebuilder.ForceRebuildLayoutImmediate(displayPanel.GetComponent<RectTransform>());

            // Neat animation intro
            subtitle.transform.DOScaleY(0.5f, 0);
            subtitle.transform.DOScaleY(1, 0.2f);
            await UniTask.Delay(100);
            subtitle.DOFade(1, 0.1f);

            // Keep display
            await UniTask.Delay((int)(displayFor * 1000f));

            // Exit
            subtitle.DOFade(0, 1);
            await UniTask.Delay(1000);

            subtitle.text = "";
            subtitlePool.Release(subtitle);
            shownLines--;

        }


    }
}