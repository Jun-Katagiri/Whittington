using UnityEditor;
using UnityEngine;

public static class FixToiletAudioSources
{
    [MenuItem("Tools/Fix Toilet Audio Sources")]
    static void Fix()
    {
        var controllers = Object.FindObjectsByType<ToiletFlushController>(FindObjectsSortMode.None);

        if (controllers.Length == 0)
        {
            // Try selected prefab in Prefab Mode
            var stage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            if (stage != null)
                controllers = stage.prefabContentsRoot.GetComponentsInChildren<ToiletFlushController>(true);
        }

        if (controllers.Length == 0)
        {
            Debug.LogWarning("[FixToiletAudioSources] No ToiletFlushControllers found. Open the prefab first, then run this.");
            return;
        }

        int fixedCount = 0;
        int alreadyOk = 0;

        foreach (var ctrl in controllers)
        {
            var so = new SerializedObject(ctrl);
            var prop = so.FindProperty("soundEffect");
            var current = prop.objectReferenceValue as AudioSource;

            // Already correctly assigned
            if (current != null && current.transform.IsChildOf(ctrl.transform))
            {
                alreadyOk++;
                continue;
            }

            // Find the AudioSource that belongs to this toilet's own hierarchy
            var ownSource = ctrl.GetComponentInChildren<AudioSource>(true);
            if (ownSource == null)
            {
                Debug.LogWarning($"[FixToiletAudioSources] '{ctrl.name}' has no child AudioSource — skipping.", ctrl);
                continue;
            }

            prop.objectReferenceValue = ownSource;
            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(ctrl);
            fixedCount++;
            Debug.Log($"[FixToiletAudioSources] Fixed '{ctrl.name}': reassigned to '{ownSource.name}'.", ctrl);
        }

        Debug.Log($"[FixToiletAudioSources] Done — {fixedCount} fixed, {alreadyOk} already correct.");
    }
}
