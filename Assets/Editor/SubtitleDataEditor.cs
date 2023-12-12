using Gasimo.Subtitles;
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
            SerializedProperty subtitlesProperty = serializedObject.FindProperty("Subtitles");


            // Set the binding of the MultiColumnListView to the serialized property
            // listView.Bind(serializedObject);

            // Set the binding path for the MultiColumnListView
            listView.bindingPath = "Subtitles";

            listView.itemsSource = (target as SubtitleData).Subtitles;
            var cols = listView.columns;

            cols["Speaker"].makeCell = () => new PropertyField();
            cols["Speaker"].bindCell = (VisualElement e, int index) =>
            {
                var l = e as PropertyField;
                l.bindingPath = "Subtitles.Array.data[" + index + "].speaker";
                l.Bind(serializedObject);
                l.Q<Label>().style.display = DisplayStyle.None;
            };

            cols["Dialogue"].makeCell = () => new PropertyField();
            cols["Dialogue"].bindCell = (VisualElement e, int index) =>
            {
                var l = e as PropertyField;
                l.bindingPath = "Subtitles.Array.data[" + index + "].dialogue";
                l.Bind(serializedObject);
                l.Q<Label>().style.display = DisplayStyle.None;
            };


            cols["Start"].makeCell = () => new PropertyField();
            cols["Start"].bindCell = (VisualElement e, int index) =>
            {
                var l = e as PropertyField;
                l.bindingPath = "Subtitles.Array.data[" + index + "].waitFor";
                l.Bind(serializedObject);
                l.Q<Label>().style.display = DisplayStyle.None;
            };

            cols["End"].makeCell = () => new PropertyField();
            cols["End"].bindCell = (VisualElement e, int index) =>
            {
                var l = e as PropertyField;
                l.bindingPath = "Subtitles.Array.data[" + index + "].displayFor";
                l.Bind(serializedObject);
                l.Q<Label>().style.display = DisplayStyle.None;
            };

            cols["Audio"].makeCell = () => new PropertyField();
            cols["Audio"].bindCell = (VisualElement e, int index) =>
            {
                var l = e as PropertyField;
                l.bindingPath = "Subtitles.Array.data[" + index + "].audio";
                l.Bind(serializedObject);
                l.Q<Label>().style.display = DisplayStyle.None;
            };

            
            cols["Event"].makeCell = () => new PropertyField();
            cols["Event"].bindCell = (VisualElement e, int index) =>
            {
                var l = e as PropertyField;
                l.bindingPath = "Subtitles.Array.data[" + index + "].subtitleEvent";
                l.Bind(serializedObject);
                l.Q<Label>().style.display = DisplayStyle.None;
            };
            

            listView.Rebuild();


            return root;
        }
    }
}
