using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class RemoveMissingScripts : MonoBehaviour
{
    [MenuItem("Tools/Cleanup/Remove Missing Scripts In Scene")]
    static void RemoveMissingScriptsInScene()
    {
        int count = 0;
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject go in allObjects)
        {
            // Record undo for the object
            Undo.RegisterCompleteObjectUndo(go, "Remove Missing Scripts");
            int removed = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
            if (removed > 0)
                count += removed;
        }

        Debug.Log($"Removed {count} missing script(s) from GameObjects in scene.");
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}
