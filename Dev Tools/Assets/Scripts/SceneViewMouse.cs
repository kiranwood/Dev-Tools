using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[InitializeOnLoad]
public class SceneViewMouse
{
    public static Vector3 mousePos = Vector3.zero;
    public static SceneView viewInstance;
    public static Ray mouseRay;

    static SceneViewMouse()
    {
        SceneView.beforeSceneGui += OnBeforeSceneGui;
    }

    private static void OnBeforeSceneGui(SceneView view)
    {
        mousePos = Event.current.mousePosition;
        viewInstance = view;

        mouseRay = HandleUtility.GUIPointToWorldRay(mousePos);
        
    }
}
