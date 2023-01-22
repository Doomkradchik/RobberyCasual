using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    public override void OnInspectorGUI() 
    {
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();

        if (EditorGUI.EndChangeCheck())
        {
            var grid = (GridManager)target;
            grid.GetComponentsInChildren<Transform>()
                .Where(c => c != grid.transform)
                .ToList()
                .ForEach(c => DestroyImmediate(c.gameObject));

            grid.CreateGrid();
        }
    }
}
