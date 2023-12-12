using Gasimo.Subtitles;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.IO;

namespace gasimo.subtitles
{

    [CustomPropertyDrawer(typeof(SubtitleDataEntry))]
    public class SubtitleEntryDrawer : PropertyDrawer
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;


        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {

            // Each editor window contains a root VisualElement object
            VisualElement root = new VisualElement();

            Foldout fold = new Foldout();

            fold.text = property.displayName;
            root.Add(fold);

            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/PropertyDrawers/SubtitleEntryDrawer.uxml");
            fold.Add(visualTree.CloneTree());

            // Validation
            FloatField intField = root.Q<FloatField>("FieldStart");
            intField.RegisterValueChangedCallback(x => { intField.value = Mathf.Abs(x.newValue); });

            return root;
        }



    }
}