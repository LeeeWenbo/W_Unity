/*Name:		 				BDZ_Db_Editor	
 *Description: 				
 *Author:       			lwb
 *Date:         			2019-06-
 *Copyright(C) 2019 by 		company@zhiwyl.com*/
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DcDBComponent))]
[CanEditMultipleObjects]
public class DcDb_Editor : Editor
{
    private DcDBComponent dbC;
    private readonly Texture _textureLogo = null;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.LabelField("Add：", GUILayout.MaxWidth(200));
        EditorGUILayout.BeginHorizontal();

        GUI.color = Color.yellow;
        if (GUILayout.Button(new GUIContent("添加本身"), GUILayout.Width(70)))
        {
            dbC.AddThis();
            EditorUtility.SetDirty(target);
            return;
        }
        if (GUILayout.Button(new GUIContent("添加子"), GUILayout.Width(70)))
        {
            dbC.AddSon();
            EditorUtility.SetDirty(target);
            return;
        }
        if (GUILayout.Button(new GUIContent("添加子孙,含自己"), GUILayout.Width(100)))
        {
            dbC.AddChildren();
            EditorUtility.SetDirty(target);
            return;
        }

        EditorGUILayout.EndHorizontal();
        GUI.color = Color.white;

        EditorGUILayout.LabelField("Debug：", GUILayout.MaxWidth(200));
        EditorGUILayout.BeginHorizontal();
        GUI.color = Color.green;
        if (GUILayout.Button(new GUIContent("打印本身"), GUILayout.Width(70)))
        {
            dbC.DebugThis();
            EditorUtility.SetDirty(target);
            return;
        }
        if (GUILayout.Button(new GUIContent("打印子"), GUILayout.Width(70)))
        {
            dbC.DebugSon();
            EditorUtility.SetDirty(target);
            return;
        }
        if (GUILayout.Button(new GUIContent("打印子孙,含自己"), GUILayout.Width(100)))
        {
            dbC.DebugChildren();
            EditorUtility.SetDirty(target);
            return;
        }

        EditorGUILayout.EndHorizontal();
        GUI.color = Color.white;


        EditorGUILayout.LabelField("Remove：", GUILayout.MaxWidth(200));
        EditorGUILayout.BeginHorizontal();

        GUI.color = Color.gray;
        if (GUILayout.Button(new GUIContent("清除本身"), GUILayout.Width(70)))
        {
            dbC.DelectThis();
            EditorUtility.SetDirty(target);
            return;
        }
        if (GUILayout.Button(new GUIContent("清除子"), GUILayout.Width(70)))
        {
            dbC.DelectSon();
            EditorUtility.SetDirty(target);
            return;
        }
        if (GUILayout.Button(new GUIContent("清除子孙,含自己"), GUILayout.Width(100)))
        {
            dbC.DelectChildren();
            EditorUtility.SetDirty(target);
            return;
        }

        EditorGUILayout.EndHorizontal();
        GUI.color = Color.white;

        careful = EditorGUILayout.Foldout(careful, "CAREFUL");
        if (careful)
        {
            GUI.color = Color.red;
            if (GUILayout.Button("清空整张表"))
            {
                dbC.DELECT_ALL();
                EditorUtility.SetDirty(target);
                return;
            }
            GUI.color = Color.white;

        }
    }


    public bool careful=false;
    private void OnEnable()
    {
        dbC = (DcDBComponent)target;
    }
}
