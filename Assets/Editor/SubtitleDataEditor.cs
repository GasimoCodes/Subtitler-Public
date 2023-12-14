using Gasimo.Subtitles;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace gasimo.subtitles
{
    [CustomEditor(typeof(SubtitleData))]
    public class SubtitleDataEditor : Editor
    {
        [SerializeField]
        public VisualTreeAsset m_VisualTreeAsset = default;

        public override VisualElement CreateInspectorGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = new VisualElement();

            // Instantiate UXML
            VisualElement uiFromUXML = m_VisualTreeAsset.CloneTree();
            root.Add(uiFromUXML);

            var listView = uiFromUXML.Q<MultiColumnListView>();
            var serializedObject = new SerializedObject(target);

            var btn = root.Q<Button>("unity-list-view__add-button");
            var btnRem = root.Q<Button>("unity-list-view__remove-button");

            btn.clicked += () => AddNewItem((target as SubtitleData), listView);
            btnRem.clicked += () => RemoveItem((target as SubtitleData), listView);


            // Set the binding path for the MultiColumnListView
            listView.bindingPath = "Subtitles";
            // listView.fixedItemHeight = 40;
            listView.itemsSource = (target as SubtitleData).Subtitles;
            var cols = listView.columns;


            foreach (string key in new string[]{ "Speaker", "Dialogue", "Start", "End", "Audio", "Event"} ) {

                cols[key].makeCell = () => new PropertyField();
                cols[key].unbindCell = (VisualElement e, int index) => e.Unbind();
            }


            cols["Speaker"].bindCell = (VisualElement e, int index) =>
            {
                var l = e as PropertyField;
                l.bindingPath = "Subtitles.Array.data[" + index + "].speaker";
                l.Bind(serializedObject);

            };

            
            cols["Dialogue"].bindCell = (VisualElement e, int index) =>
            {
                var l = e as PropertyField;
                l.bindingPath = "Subtitles.Array.data[" + index + "].dialogue";
                l.Bind(serializedObject);

            };
            
            cols["Start"].bindCell = (VisualElement e, int index) =>
            {
                var l = e as PropertyField;
                l.bindingPath = "Subtitles.Array.data[" + index + "].waitFor";
                l.Bind(serializedObject);

            };

            
            cols["End"].bindCell = (VisualElement e, int index) =>
            {
                var l = e as PropertyField;
                l.bindingPath = "Subtitles.Array.data[" + index + "].displayFor";
                l.Bind(serializedObject);

            };

            
            cols["Audio"].bindCell = (VisualElement e, int index) =>
            {
                var l = e as PropertyField;
                l.bindingPath = "Subtitles.Array.data[" + index + "].audio";
                l.Bind(serializedObject);

            };

            cols["Event"].bindCell = (VisualElement e, int index) =>
            {
                var l = e as PropertyField;
                l.bindingPath = "Subtitles.Array.data[" + index + "].subtitleEvent";
                l.Bind(serializedObject);

            };


            

            listView.Rebuild();

            return root;
        }

        private void AddNewItem(SubtitleData array, MultiColumnListView view)
        {
            Debug.Log("Clicked");

            SubtitleDataEntry[] temp = new SubtitleDataEntry[array.Subtitles.Length + 1];
            temp[temp.Length - 1] = new SubtitleDataEntry() { speaker = "", dialogue = "Sample Dialogue" };
            array.Subtitles.CopyTo(temp, 0);
            array.Subtitles = temp;
            // view.Rebuild();
        }

        private void RemoveItem(SubtitleData array, MultiColumnListView view)
        {

            List<SubtitleDataEntry> temp = new List <SubtitleDataEntry>();
            temp.AddRange(array.Subtitles);

            int selected = view.selectedIndex;

            if (selected == -1)
                selected = (temp.Count - 1);

            temp.RemoveAt(selected);

            array.Subtitles = temp.ToArray();

        }



    }
}
