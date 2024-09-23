using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Gasimo.Subtitles.Editor
{
    [ScriptedImporter(1, "srt")]
    public class SRTImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var subtitleData = ImportSRT(ctx.assetPath);
            ctx.AddObjectToAsset("subtitles", subtitleData);
            ctx.SetMainObject(subtitleData);
        }

        public static SubtitleSequenceData ImportSRT(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            List<SubtitleDataEntry> subtitles = new List<SubtitleDataEntry>();
            SubtitleDataEntry currentEntry = null;
            string dialogueBuffer = "";
            
            float lastEndTime = 0f;
            float lastStartTime = 0f;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                if (string.IsNullOrEmpty(line))
                {
                    if (currentEntry != null)
                    {
#if !SUBTITLER_LOCALIZATION
                        currentEntry.dialogue = dialogueBuffer.Trim();
#endif
                        subtitles.Add(currentEntry);
                        currentEntry = null;
                        dialogueBuffer = "";
                    }
                }
                else if (int.TryParse(line, out _))
                {
                    // This is a subtitle number, skip it
                    continue;
                }
                else if (Regex.IsMatch(line, @"\d{2}:\d{2}:\d{2},\d{3} --> \d{2}:\d{2}:\d{2},\d{3}"))
                {
                    currentEntry = new SubtitleDataEntry();
                    string[] times = line.Split(new string[] { " --> " }, System.StringSplitOptions.None);
                    float startTime = ParseTimeToSeconds(times[0]);
                    float endTime = ParseTimeToSeconds(times[1]);

                    // Calculate waitFor and displayFor
                    currentEntry.waitFor = startTime - lastStartTime;
                    currentEntry.displayFor = endTime - startTime;


                    lastEndTime = endTime;
                    lastStartTime = startTime;
                }
                else
                {
                    dialogueBuffer += line + "\n";
                }
            }

            // Add the last subtitle if exists
            if (currentEntry != null)
            {
#if !SUBTITLER_LOCALIZATION
                currentEntry.dialogue = dialogueBuffer.Trim();
#endif
                subtitles.Add(currentEntry);
            }

            SubtitleSequenceData sequenceData = ScriptableObject.CreateInstance<SubtitleSequenceData>();
            sequenceData.Subtitles = subtitles.ToArray();

            return sequenceData;
        }

        private static float ParseTimeToSeconds(string timeString)
        {
            TimeSpan timeSpan;
            if (TimeSpan.TryParseExact(timeString, @"hh\:mm\:ss\,fff", CultureInfo.InvariantCulture, out timeSpan))
            {
                return (float)timeSpan.TotalSeconds;
            }
            return 0f;
        }
    }

    [CustomEditor(typeof(SRTImporter))]
    public class SRTImporterEditor : ScriptedImporterEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Create Subtitler Sequence Asset"))
            {
                string srtPath = AssetDatabase.GetAssetPath(target);
                string assetPath = Path.ChangeExtension(srtPath, "asset");

                if (File.Exists(assetPath))
                {
                    if (!EditorUtility.DisplayDialog("Asset Already Exists",
                        "A Subtitler Sequence Asset already exists at this location. Do you want to overwrite it?",
                        "Yes", "No"))
                    {
                        return;
                    }
                }

                var subtitleData = SRTImporter.ImportSRT(srtPath);
                if (subtitleData != null)
                {
                    AssetDatabase.CreateAsset(subtitleData, assetPath);
                    AssetDatabase.SaveAssets();
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = subtitleData;
                    EditorUtility.DisplayDialog("Success", "Subtitler Sequence Asset created at: " + assetPath, "OK");
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Failed to create Subtitler Sequence Asset.", "OK");
                }
            }
        }
    }
}