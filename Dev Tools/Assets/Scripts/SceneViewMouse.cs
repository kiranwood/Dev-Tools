using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class SceneViewMouse
{
    public static SceneView viewInstance; // Current scene view opened
    public static Ray mouseRay;

    // Creates events for detecting input
    public delegate void LeftMouseDown();
    public static event LeftMouseDown OnLeftMouseDown;

    public delegate void EscButtonPress();
    public static event EscButtonPress OnEscButtonPress;

    static SceneViewMouse()
    {
        SceneView.beforeSceneGui += OnBeforeSceneGui;
    }

    // Runs when OnGUI is called, Sets variables from scene view
    private static void OnBeforeSceneGui(SceneView view)
    {
        viewInstance = view;

        // Gets the ray of the mouse position
        mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            // Invokes event for left click
            OnLeftMouseDown?.Invoke();
        }
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
        {
            // Invokes event for esc key
            OnEscButtonPress?.Invoke();
        }
    }
}
