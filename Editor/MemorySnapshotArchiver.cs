// **************************************************************** //
//
//   Copyright (c) RimuruDev. All rights reserved.
//   Contact me: 
//          - Gmail:    rimuru.dev@gmail.com
//          - LinkedIn: https://www.linkedin.com/in/rimuru/
//          - GitHub:   https://github.com/RimuruDev
//
// **************************************************************** //

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RimuruDev
{
    public sealed class MemorySnapshotArchiver : EditorWindow
    {
        private string sourceDir;
        private string destinationDir;

        [MenuItem("RimuruDev Tools/Archive Memory Snapshots")]
        public static void ShowWindow() =>
            GetWindow<MemorySnapshotArchiver>("Memory Snapshot Archiver");

        private void OnEnable()
        {
            sourceDir = EditorPrefs.GetString("MemorySnapshotArchiver_SourceDir", string.Empty);
            destinationDir = EditorPrefs.GetString("MemorySnapshotArchiver_DestinationDir", string.Empty);
        }

        private void OnGUI()
        {
            GUILayout.Label("Настройки архивации снимков памяти", EditorStyles.boldLabel);

            if (string.IsNullOrEmpty(sourceDir))
                sourceDir = "Выберите папку исходных снимков";

            if (string.IsNullOrEmpty(destinationDir))
                destinationDir = "Выберите папку назначения";

            sourceDir = EditorGUILayout.TextField("Папка исходных снимков", sourceDir);
            destinationDir = EditorGUILayout.TextField("Папка назначения", destinationDir);

            if (GUILayout.Button("Выбрать папку исходных снимков"))
            {
                var selectedSourceDir =
                    EditorUtility.OpenFolderPanel("Выбрать папку исходных снимков", sourceDir, string.Empty);

                if (!string.IsNullOrEmpty(selectedSourceDir))
                    sourceDir = selectedSourceDir;
            }

            if (GUILayout.Button("Выбрать папку назначения"))
            {
                var selectedDestinationDir =
                    EditorUtility.OpenFolderPanel("Выбрать папку назначения", destinationDir, string.Empty);

                if (!string.IsNullOrEmpty(selectedDestinationDir))
                    destinationDir = selectedDestinationDir;
            }

            if (GUILayout.Button("Сохранить пути"))
            {
                if (!string.IsNullOrEmpty(sourceDir) && sourceDir != "Выберите папку исходных снимков")
                    EditorPrefs.SetString("MemorySnapshotArchiver_SourceDir", sourceDir);

                if (!string.IsNullOrEmpty(destinationDir) && destinationDir != "Выберите папку назначения")
                    EditorPrefs.SetString("MemorySnapshotArchiver_DestinationDir", destinationDir);

                Debug.Log("<color=yellow>Пути сохранены.</color>");
            }

            if (GUILayout.Button("Архивировать снимки"))
            {
                if (!Directory.Exists(sourceDir) || !Directory.Exists(destinationDir))
                    Debug.LogError("<color=red>Ошибка: Оба пути должны быть выбраны и существовать.</color>");
                else
                    ArchiveSnapshots();
            }
        }

        private void ArchiveSnapshots()
        {
            if (!Directory.Exists(destinationDir))
                Directory.CreateDirectory(destinationDir);

            var snapshotFiles = Directory.GetFiles(sourceDir, "*.snap");
            var snapshotImages = Directory.GetFiles(sourceDir, "*.png");

            foreach (var file in snapshotFiles)
            {
                var fileName = Path.GetFileName(file);
                var destFile = Path.Combine(destinationDir, fileName);

                File.Move(file, destFile);

                Debug.Log($"<color=yellow>Archived {fileName} to {destinationDir}</color>");
            }

            foreach (var file in snapshotImages)
            {
                var fileName = Path.GetFileName(file);
                var destFile = Path.Combine(destinationDir, fileName);

                File.Move(file, destFile);

                Debug.Log($"<color=yellow>Archived {fileName} to {destinationDir}.</color>");
            }

            AssetDatabase.Refresh();

            Debug.Log("<color=yellow>Все снимки архивированы.</color>");
        }
    }
}
#endif