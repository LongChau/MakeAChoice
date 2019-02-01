using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Cube))]
public class CubeInspector : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Grab all cubes"))
        {
            Cube cube = target as Cube;
            //cube.cells = new Cell[cube.transform.childCount];
            //for (int i = 0; i < cube.cells.Length; i++)
            //{
            //    cube.cells[i] = cube.transform.GetChild(i).GetComponent<Cell>();
            //}
            cube.cells = cube.transform.GetComponentsInChildren<Cell>();
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(cube);
        }
    }
}
