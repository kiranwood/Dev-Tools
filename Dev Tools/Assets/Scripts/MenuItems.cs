/*using Codice.CM.Common.Tree;
using System.Linq;
using UnityEditor;
using UnityEngine;



//This is a new structure, specificlly for debbug tools 

public class MenuItems //This works as a group, where they all end at the same window 

{

    [MenuItem("Tools/Demos/HelloWorld")] //This is the path itself 

    static void HelloWorld() //The action to do here 

    {

        Debug.Log("Helloworld");

    }





    [MenuItem("Tools/Demos/LogTime")]

    static void LogTime()

    {

        Debug.Log(Application.unityVersion);

        Debug.Log(System.DateTime.Now.ToString());

    }



    [MenuItem("Tools/Demos/Dialogues")]

    static void ShowDialog()

    {

        bool confirmed = EditorUtility.DisplayDialog(

            "Title",

            "Do you have a concussion?",

            "Yes, why not?",

            "What is a concussion?"

            );



        if (confirmed)

        {

            Debug.Log("Stuff happens");

        }

        else

        {

            Debug.Log("Stuff disappear");

        }

    }



    //For selection item 



    [MenuItem("Tools/Demos/RenameFirstSelection")]

    static void RenameFirstSelection()

    {

        Selection.activeGameObject.name = "Super Cole";

        Debug.Log("Object renamed");

    }



    //with validation by boolean (Which it will block the origianl tool if this one blocks it) 

    [MenuItem("Tools/Demos/RenameFirstSelection", isValidateFunction: true)]

    static bool RenameFirstSelectionValidate()

    {

        return Selection.activeGameObject != null;

    }



    //Here an example with multiple selection objects 

    [MenuItem("Tools/Demos/logSelectedObjects")]

    static void LogAllSelection()

    {

        if (Selection.gameObjects.Length == 0)

        {

            Debug.Log("Select SOMETHING");

            return;

        }



        Debug.Log($"You have selected {Selection.gameObjects.Length} number of object(s)");



        foreach (GameObject go in Selection.gameObjects)

        {

            Debug.Log(go.name);

        }

    }



    [MenuItem("GameObject/Demos/AddEmptyChild")]

    static void AddEmptyChild()

    {

        if (Selection.activeGameObject != null)

        {

            //The child itself 

            GameObject child = new GameObject("NewChild");



            //Register to undo the action when needed 

            Undo.RegisterCreatedObjectUndo(child, "Add Empty Child");



            //Child with selected object and the transform 

            child.transform.SetParent(Selection.activeGameObject.transform, worldPositionStays: false);

            child.transform.localPosition = Vector3.zero;



            //To end the selection with the child 

            Selection.activeGameObject = child;

        }

        else

        {

            Debug.LogWarning("Select a gameObject first");

        }

    }

}

EditorWindow - WindowTools

using UnityEditor; 

using UnityEngine; 

using UnityEngine.UIElements; 

  

//In this case we are going to inherit from the windows to apply their rules to it 

  

//When testing always close the window to avoid any glitch 

public class EditorWindows : EditorWindow

{



    enum Shapes

    {

        Triange = 0,

        Square,

        Parallelogram

    }



    string myTextField = "Write something here... if you dare";

    int myIntField = 5;

    Vector3 myVector = Vector3.zero;

    bool myToogle = false;

    float mySlider = 0.5f;

    Vector2 scrollPos;

    Shapes myShapes = 0;

    int myDropdownIndex = 0;

    Color myColor = Color.aliceBlue;

    GameObject myGameObject = null;

    bool myFoldOut = false;





    //Creates the window 

    [MenuItem("Tools/Windows/BasicWindow")]

    public static void ShowWindow()

    {

        //This to avoid multiple same window open 

        EditorWindows win = GetWindow<EditorWindows>("Basic Window");

        win.minSize = new Vector2(320f, 500f);

    }



    //This is a event from editor window which works as a update in monobehaivour 

    //This renders as it see's it, like in the Android Studio Layout order 

    private void OnGUI()

    {

        //start the scroll 

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);



        //titles 

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Controls", EditorStyles.boldLabel);

        EditorGUILayout.Space();



        //show primitives and update them 

        myTextField = EditorGUILayout.TextField("String", myTextField);

        myIntField = EditorGUILayout.IntField("Number", myIntField);

        myVector = EditorGUILayout.Vector3Field("Vector", myVector);



        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Toogles and sliders", EditorStyles.boldLabel);

        EditorGUILayout.Space();



        //Checks and sliders 

        myToogle = EditorGUILayout.Toggle("Toogle", myToogle);

        mySlider = EditorGUILayout.Slider("Slider", mySlider, 0f, 1f);



        //Popups 

        myShapes = (Shapes)EditorGUILayout.EnumPopup("Shape", myShapes);

        string[] actions = { "Punch", "Push", "Pull" };

        myDropdownIndex = EditorGUILayout.Popup("Actions", myDropdownIndex, actions);

        EditorGUILayout.LabelField($"ActionsSelected: {actions[myDropdownIndex]}", EditorStyles.boldLabel);



        //Color selection 

        myColor = EditorGUILayout.ColorField("Color", myColor);



        //GameObject with condition 

        myGameObject = (GameObject)EditorGUILayout.ObjectField("GameObject", myGameObject, typeof(GameObject), allowSceneObjects: true);



        //For the foldout 

        myFoldOut = EditorGUILayout.Foldout(myFoldOut, "More Settings", toggleOnLabelClick: true);



        EditorGUILayout.Space();



        if (myFoldOut)

        {

            EditorGUI.indentLevel++;

            EditorGUILayout.LabelField("Settings 1", "Value 1");

            EditorGUILayout.LabelField("Settings 2", "Value 2");

            EditorGUI.indentLevel--;

        }



        EditorGUILayout.Space();



        //Help boxes 



        EditorGUILayout.HelpBox("I have a concussion :)", MessageType.Info); //Information 

        EditorGUILayout.HelpBox("I could have a concussion :)", MessageType.Warning); //Warning 

        EditorGUILayout.HelpBox("I definitiuvelly have a concussion :)", MessageType.Error); //Error 



        //For the buttons 



        EditorGUILayout.Space();



        if (GUILayout.Button("Click me", GUILayout.Width(80)))

        {

            Debug.Log("Button clicked!");

        }



        EditorGUILayout.Space();

        //For horizontal 

        EditorGUILayout.BeginHorizontal();

        {

            if (GUILayout.Button("4", GUILayout.Width(80)))

            {

                Debug.Log("4 Button clicked!");

            }

            if (GUILayout.Button("5", GUILayout.Width(80)))

            {

                Debug.Log("5 Button clicked!");

            }

            if (GUILayout.Button("a", GUILayout.Width(80)))

            {

                Debug.Log("a Button clicked!");

            }

        }

        EditorGUILayout.EndHorizontal();



        EditorGUILayout.Space();

        //Disabled Groups 

        EditorGUI.BeginDisabledGroup(!myToogle);

        {

            if (GUILayout.Button("Coolbutton"))

            {

                Debug.Log("Coolbutton Button clicked!");

            }

        }

        EditorGUI.EndDisabledGroup();



        EditorGUILayout.Space();



        //progress bar 

        EditorGUILayout.LabelField("Progress bar", EditorStyles.boldLabel);



        Rect progressRect = GUILayoutUtility.GetRect(10f, 20f, GUILayout.ExpandHeight(true));

        EditorGUI.ProgressBar(progressRect, mySlider, $"The value is arround {mySlider}");



        //End the scroll 

        EditorGUILayout.EndScrollView();

    }

}



Batch creator

using UnityEngine; 

using UnityEditor; 

using System.Linq; 

using Codice.Client.Common.GameUI; 

  

public class BatchRenamer : EditorWindow

{

    private string prefix = "Thing";

    private string suffix = "VFX";

    private int startNumber = 1;

    private int padDigits = 2;

    private char separator = '_';

    private bool sortAlphabetically = true;



    private Vector2 scrollPos;

    private bool previewMode = true;



    [MenuItem("Tools/AdvancedTools/Batch Rename...")]

    public static void ShowWindow()

    {

        BatchRenamer window = GetWindow<BatchRenamer>("Batch Renamer");

        window.minSize = new Vector2(300f, 350f);

    }



    private void OnGUI()

    {



        EditorGUILayout.LabelField("Batch Rename", EditorStyles.boldLabel);



        EditorGUILayout.Space();



        prefix = EditorGUILayout.TextField("Prefix", prefix);



        EditorGUILayout.Space();



        suffix = EditorGUILayout.TextField("Suffix", suffix);



        EditorGUILayout.Space();



        startNumber = EditorGUILayout.IntField("Start Number", startNumber);



        EditorGUILayout.Space();



        padDigits = EditorGUILayout.IntSlider("PadDigits", padDigits, 0, 5);



        EditorGUILayout.Space();



        string separatorString = EditorGUILayout.TextField("PadDigits", separator.ToString());



        if (separatorString.Length > 0) { separator = separatorString[0]; }

        else { separator = '_'; }



        EditorGUILayout.Space();



        sortAlphabetically = EditorGUILayout.Toggle("Sort", sortAlphabetically);



        EditorGUILayout.Space();



        previewMode = EditorGUILayout.Toggle("Preview Mode", previewMode);



        EditorGUILayout.Space();

        EditorGUILayout.Space();

        EditorGUILayout.Space();



        string GetName(int index)

        {

            string number = (startNumber + index).ToString().PadLeft(padDigits, '0');

            string middle = $"{separator}{number}";

            string end = suffix.Length > 0 ? $"{separator}{suffix}" : "";



            return $"{prefix}{middle}{end}";

        }



        GameObject[] selected = Selection.gameObjects;



        if (sortAlphabetically) selected = selected.OrderBy(go => go.name).ToArray();



        int count = selected.Length;



        //Constant element style 

        GUIStyle countStyle = new GUIStyle(EditorStyles.label);

        countStyle.normal.textColor = count == 0 ? Color.red : Color.green;

        EditorGUILayout.LabelField($"{count} Objects selected", countStyle);



        if (previewMode && count > 0)

        {

            EditorGUILayout.LabelField("Preview (First 10)", EditorStyles.boldLabel);



            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(140f));

            {

                int previewCount = Mathf.Min(count, 10);

                for (int i = 0; i < previewCount; i++)

                {

                    EditorGUILayout.BeginHorizontal();

                    {

                        GUIStyle old = new GUIStyle(EditorStyles.label);

                        old.normal.textColor = Color.gray;

                        EditorGUILayout.LabelField(selected[i].name, old);



                        GUIStyle newStyle = new GUIStyle(EditorStyles.label);

                        newStyle.normal.textColor = Color.green;

                        EditorGUILayout.LabelField(GetName(i), newStyle);



                    }

                    EditorGUILayout.EndHorizontal();

                }

            }

            EditorGUILayout.EndScrollView();

        }





        EditorGUILayout.Space();



        EditorGUI.BeginDisabledGroup(count == 0);

        {

            GUI.backgroundColor = count > 0 ? new Color(0.4f, 0.6f, 0.4f) : Color.wheat;

            //Button 

            if (GUILayout.Button($"Rename {count} Object", GUILayout.Height(35)))

            {

                DoRename(selected, GetName);

            }



        }

        EditorGUI.EndDisabledGroup();



    }



    private void DoRename(GameObject[] objects, System.Func<int, string> getNameFun)

    {

        if (objects.Length == 0) return;



        Undo.SetCurrentGroupName("Bacth Rename"); //create the undo action gruop 

        int undoGroup = Undo.GetCurrentGroup(); //adjust it 



        for (int i = 0; i < objects.Length; i++)

        {

            Undo.RecordObject(objects[i], "Batch Rename"); // Record the undo action 



            objects[i].name = getNameFun(i);



            EditorUtility.SetDirty(objects[i]); //This is the system to create a warning before it 

        }



        Undo.CollapseUndoOperations(undoGroup); //Confirm the undo 



        Debug.Log($"Rename {objects.Length} objects");

    }



    //It basically adapts the window UI to not refresh everything each time 

    private void OnSelectionChange()

    {

        Repaint();

    }

}*/