using Gasimo.Subtitles;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.IO;

namespace Gasimo.Subtitles.Editor
{
    #if UNITY_2023_3_OR_NEWER
    /*
    [CustomPropertyDrawer(typeof(SubtitleDataEntry))]
    public class SubtitleEntryDrawer : PropertyDrawer
    {

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



    }*/
#endif
}