using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Adds a menu:  Apocalyptic Cafe > Create Prototype Scene
// One click creates + saves the scene and adds it to Build Settings (needed for the R-restart key).
public static class CreatePrototypeScene
{
    [MenuItem("Apocalyptic Cafe/Create Prototype Scene")]
    public static void Create()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        GameObject game = new GameObject("Game");
        game.AddComponent<GameBootstrap>();   // also adds GameManager + Spawner automatically

        Directory.CreateDirectory("Assets/Scenes");
        string path = "Assets/Scenes/Prototype.unity";
        EditorSceneManager.SaveScene(scene, path);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(path, true) };

        EditorUtility.DisplayDialog("Apocalyptic Cafe", "Scene created: " + path + "\n\nPress the Play button to test the prototype.", "OK");
    }
}
