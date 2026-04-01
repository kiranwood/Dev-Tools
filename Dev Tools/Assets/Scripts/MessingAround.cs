using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class MessingAround : EditorWindow
{
    static GameObject myGameObject;
    static LayerMask layer;
    static bool spawnPrefab = false;
    static GameObject currentGameObject = null;

    [MenuItem("Tools/Idk")]
    private static void Testing()
    {
        EditorWindow.GetWindow(typeof(MessingAround));
        
    }

    void OnGUI()
    {
        SceneView testing = SceneView.lastActiveSceneView;
        
        myGameObject = (GameObject)EditorGUILayout.ObjectField("Example!!", myGameObject, typeof(GameObject), false);

        layer = EditorGUILayout.LayerField("Attach Layer", layer);
    }

    private void Update()
    {
        //Debug.Log(SceneViewMouse.mousePos);
        
        if (!currentGameObject) return;

        RaycastHit mouseHit;

        if (Physics.Raycast(SceneViewMouse.mouseRay, out mouseHit, Mathf.Infinity))
        {
            Debug.Log("worked :)");
            currentGameObject.transform.position = mouseHit.point;
        }
        Debug.DrawRay(SceneViewMouse.mouseRay.origin, SceneViewMouse.mouseRay.direction * 100f, Color.aliceBlue, 1f);
        
    }

    [MenuItem("Tools/Spawn :)")]
    private static void No()
    {
        currentGameObject = PrefabUtility.InstantiatePrefab(myGameObject) as GameObject;
        Debug.Log(currentGameObject.name);
    }

    [MenuItem("Tools/stopy :(")]
    private static void StopTest()
    {
        currentGameObject = null;
    }
}
