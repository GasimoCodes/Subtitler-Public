using Gasimo.Subtitles;
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
            
            var subtitlesProperty = serializedObject.FindProperty("Subtitles");

            // Debug.Log(subtitlesProperty.arraySize);
            

            // Set the binding of the MultiColumnListView to the serialized property
            listView.Bind(serializedObject);

            // Set the binding path for the MultiColumnListView
            listView.bindingPath = "Subtitles";

            // Make sure to call the DataReloaded callback to refresh the UI
            listView.RefreshItems();

            Debug.Log(listView.columns.Count);

            return root;
        }
    }
}
