using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PrefabSpawnerTool : EditorWindow
{
    static GameObject prefabToSpawn;
    static GameObject currentGameObject = null; // Current game object selected in screen

    // Variables to alter scale
    static Vector3 gameObjectScale;
    static bool randomizeScale = false;
    static Vector3 minGameObjectScale;
    static Vector3 maxGameObjectScale;

    // Variables to alter rotation
    static bool editRotation;
    static bool randomRotaton = false;
    static Vector3 gameObjectRotation = Vector3.zero;
    static Vector3 minGameObjectRotation = Vector3.zero;
    static Vector3 maxGameObjectRotation = Vector3.zero;

    // Variables to offset position
    static bool offsetPosition = false;
    static Vector3 positionOffsetVector = Vector3.zero;

    static bool IsGameObjectColliderActive;

    [MenuItem("Tools/Prefab Spawner")]
    private static void PrefabSpawner()
    {
        EditorWindow.GetWindow(typeof(PrefabSpawnerTool));

        // Adds listeners
        SceneViewMouse.OnLeftMouseDown += PlaceGameObject;
        SceneViewMouse.OnEscButtonPress += ToggleEditMode;

        // Creates gameobject if applicable
        ResetObjectVariables();
        CreateGameObject();
    }

    void OnGUI()
    {
        EditorGUILayout.Space(25);
        EditorGUILayout.HelpBox("Set a prefab to spawn to enter edit mode.\nPress esc to toggle on/off edit mode.", MessageType.Info);
        EditorGUILayout.Space(15);

        EditorGUI.BeginChangeCheck();

        prefabToSpawn = (GameObject)EditorGUILayout.ObjectField("Prefab to spawn", prefabToSpawn, typeof(GameObject), false);

        // Detects change of gameobject to spawn
        if (EditorGUI.EndChangeCheck())
        {
            RemoveCurrentGameObject();

            // Spawns if a prefab has been chosen
            if (prefabToSpawn != null)
            {
                
                ResetObjectVariables();
                CreateGameObject();
            }
        }

        // Only shows object variables if a gameobject has been selected
        if (prefabToSpawn != null)
        {
            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Object Variables", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            EditorGUI.BeginChangeCheck();

            // Inputs for scale
            randomizeScale = EditorGUILayout.Toggle("Randomize Scale", randomizeScale);
            if (!randomizeScale)
            {
                gameObjectScale = EditorGUILayout.Vector3Field("Scale", gameObjectScale);
            }
            else // Randomizes scale
            {
                minGameObjectScale = EditorGUILayout.Vector3Field("Min", minGameObjectScale);
                maxGameObjectScale = EditorGUILayout.Vector3Field("Max", maxGameObjectScale);
                
            }

            EditorGUILayout.Space(15);

            // Inputs for rotation
            editRotation = EditorGUILayout.Toggle("Edit Rotation", editRotation);
            if (editRotation)
            {
                randomRotaton = EditorGUILayout.Toggle("Randomize Rotation", randomRotaton);

                if (!randomRotaton)
                {
                    gameObjectRotation = EditorGUILayout.Vector3Field("Rotation", gameObjectRotation);
                }
                else // Randomize the rotation
                {
                    minGameObjectRotation = EditorGUILayout.Vector3Field("Min", minGameObjectRotation);
                    maxGameObjectRotation = EditorGUILayout.Vector3Field("Max", maxGameObjectRotation);
                }
            }

            EditorGUILayout.Space(15);
            EditorGUILayout.HelpBox("Lets you change the offset of the gameobject from mouse location\nUse this to spawn gameobjects in the air or offset pivot", MessageType.Info);
            EditorGUILayout.Space(5);

            // Input for offset position
            offsetPosition = EditorGUILayout.Toggle("Offset Pivot", offsetPosition);
            if (offsetPosition)
            {
                positionOffsetVector = EditorGUILayout.Vector3Field("Pivot Offset", positionOffsetVector);
            }

            // Recreates/Creates a gameobject if any value has been altered
            if (EditorGUI.EndChangeCheck())
            {
                CreateGameObject();
            }
        }

        // Toggle edit mode on off if escape is hit
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
        {
            ToggleEditMode();
        }
    }

    // Removes current game object or spawns it
    private static void ToggleEditMode()
    {
        if (currentGameObject != null) RemoveCurrentGameObject();
        else CreateGameObject();
    }

    void OnDestroy()
    {
        RemoveCurrentGameObject();

        // Removes listeners
        SceneViewMouse.OnLeftMouseDown -= PlaceGameObject;
        SceneViewMouse.OnEscButtonPress -= ToggleEditMode;
    }

    private void Update()
    {
        if (!currentGameObject) return;

        // Removes selection for objects
        Selection.objects = null;

        // Raycast to move gameobject
        RaycastHit mouseHit;
        if (Physics.Raycast(SceneViewMouse.mouseRay, out mouseHit, Mathf.Infinity))
        {
            Vector3 newPosition = mouseHit.point + positionOffsetVector; // Applies offset if exists
            currentGameObject.transform.position = newPosition;
        }
    }

    // Resets all variables for prefab
    private static void ResetObjectVariables()
    {
        if (prefabToSpawn == null) return; // No variable selected

        gameObjectScale = prefabToSpawn.transform.localScale;
        minGameObjectScale = prefabToSpawn.transform.localScale;
        gameObjectRotation = Vector3.zero;
        minGameObjectRotation = Vector3.zero;
        positionOffsetVector = Vector3.zero;
    }

    // Creates a gameobject from prefab
    private static void CreateGameObject()
    {
        if (prefabToSpawn == null) return; // No prefab selected to spawn

        // Removes current object and creates new one
        RemoveCurrentGameObject();
        currentGameObject = PrefabUtility.InstantiatePrefab(prefabToSpawn) as GameObject;

        // Sets gameobject transform
        currentGameObject.transform.localScale = gameObjectScale;
        if (randomizeScale) // Randomizes scale
        {
            Vector3 newScale = new Vector3(  Random.Range(minGameObjectScale.x, maxGameObjectScale.x),
                                                Random.Range(minGameObjectScale.y, maxGameObjectScale.y),
                                                Random.Range(minGameObjectScale.z, maxGameObjectScale.z));
            currentGameObject.transform.localScale = newScale;
        }

        // Sets gameobject rotation
        if (editRotation)
        {
            currentGameObject.transform.rotation = Quaternion.Euler(gameObjectRotation);

            if (randomRotaton) // Randomizes rotation
            {
                Vector3 newRotation = new Vector3(Random.Range(minGameObjectRotation.x, maxGameObjectRotation.x),
                                                Random.Range(minGameObjectRotation.y, maxGameObjectRotation.y),
                                                Random.Range(minGameObjectRotation.z, maxGameObjectRotation.z));
                currentGameObject.transform.rotation = Quaternion.Euler(newRotation);
            }
        }

        // Disables collider
        if (currentGameObject.TryGetComponent(out Collider collider))
        {
            IsGameObjectColliderActive = collider.enabled;
            collider.enabled = false;
        }
    }

    // Places the gameobject at location
    private static void PlaceGameObject()
    {
        if (currentGameObject == null) return;

        // Returns collider to previous enabled state
        if (currentGameObject.TryGetComponent(out Collider collider))
        {
            collider.enabled = IsGameObjectColliderActive;
        }

        // Creates a new gameobject
        currentGameObject = null;
        CreateGameObject();
    }

    // Removes the current gameobject being projected
    private static void RemoveCurrentGameObject()
    {
        if (currentGameObject != null) UnityEngine.Object.DestroyImmediate(currentGameObject);
        currentGameObject = null;
    }
}
