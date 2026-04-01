using System;
using UnityEditor;
using UnityEngine;

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
        
        EditorApplication.update += DoSomething;

        //SceneView.duringSceneGui += Hello;
    }

    void OnGUI()
    {
        Vector3 mousePosition = Event.current.mousePosition;
        Debug.Log(mousePosition);

        SceneView testing = SceneView.lastActiveSceneView;
        
        myGameObject = (GameObject)EditorGUILayout.ObjectField("Example!!", myGameObject, typeof(GameObject), false);
        //wantsMouseMove = true;

        layer = EditorGUILayout.LayerField("Attach Layer", layer);
    }

    private void Hello(SceneView view)
    {
        Debug.Log("Mouse Position");
        if (!currentGameObject) return;

        /*Vector3 mousePosition = Event.current.mousePosition;
        mousePosition.y = view.camera.pixelHeight - mousePosition.y;
        Debug.Log("mousePosition");*/
        ;
        /*Ray mouseRay = HandleUtility.GUIPointToWorldRay(mousePosition);
        RaycastHit mouseHit;
        //currentGameObject.transform.position = mousePosition;

        if (Physics.Raycast(mouseRay, out mouseHit, Mathf.Infinity, layer))
        {
            Debug.Log("hi?");
            //currentGameObject.transform.position = mouseHit.point;
        }
        Debug.Log("u no hit? :(");*/
    }

    private void OnInspectorUpdate()
    {

        /*Vector3 mousePosition = Event.current.mousePosition;
        Debug.Log("Mouse Position");*/
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
        EditorApplication.update -= DoSomething;
        currentGameObject = null;
    }

    private static void DoSomething()
    {
        if (!currentGameObject) return;


        //Debug.Log("F u work");

        //Vector3 mousePosition = Event.current.mousePosition;

        /*Ray mouseRay = HandleUtility.GUIPointToWorldRay(Mouse.current.position.ReadValue());
        RaycastHit mouseHit;

        if (Physics.Raycast(mouseRay, out mouseHit, Mathf.Infinity, layer))
        {
            Debug.Log("hi?");
            currentGameObject.transform.position = mouseHit.point;
        }
        Debug.Log("u no hit? :(");*/
    }
}
