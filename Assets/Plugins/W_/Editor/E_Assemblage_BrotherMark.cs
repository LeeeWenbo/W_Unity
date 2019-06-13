/*Name:		 				E_Assemblage_BrotherMark	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-09-30
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(C_Assemblage_BrotherMark))]
public class E_Assemblage_BrotherMark : Editor
{

    SerializedProperty chaiOneEvent;
    SerializedProperty chaiGroupEvent;
    SerializedProperty zhuangOneEvent;
    SerializedProperty zhuangGroupEvent;

    SerializedProperty logStepOnComplete;
    SerializedProperty logGroupOnComplete;

    C_Assemblage_BrotherMark cTarget ;
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("鼠标悬停在参数上，查看参数说明",MessageType.None);


        EditorGUILayout.BeginHorizontal();
        GUI.color = new Color32(255,133,0,255);
        if (GUILayout.Button(new GUIContent("更新数据", "Init()"), GUILayout.Width(70)))
        {
            cTarget.Init();
        }
        GUI.color = new Color32(220, 88, 88, 255);

        if (GUILayout.Button(new GUIContent("清空数据", "RemoveData()"), GUILayout.Width(70)))
        {
            cTarget.RemoveData();
        }
        GUI.color = Color.white;

        EditorGUILayout.EndHorizontal();

        DrawDefaultInspector();


        GUI.color = Color.green;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("拆：", GUILayout.MaxWidth(20));
        if (GUILayout.Button(new GUIContent("单步", "chai_One=true"),GUILayout.Width(70)))
        {
            cTarget.chai_One = true;
        }
        if (GUILayout.Button(new GUIContent("按顺序", "chai_OneKeyAll=true"), GUILayout.Width(70)))
        {
            cTarget.chai_OneKeyAll = true;
        }

  
        if (GUILayout.Button(new GUIContent("一步到位", "chai_OneStepAll=true"), GUILayout.Width(70)))
        {
            cTarget.chai_OneStepAll = true;
        }
        EditorGUILayout.EndHorizontal();

        GUI.color = Color.yellow;
       
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("装：", GUILayout.MaxWidth(20));
        if (GUILayout.Button(new GUIContent("单步",  "zhuang_One=true"),GUILayout.Width(70)))
        {
            cTarget.zhuang_One = true;
        }
        if (GUILayout.Button(new GUIContent("按顺序", "zhuang_OneKeyAll=true"), GUILayout.Width(70)))
        {
            cTarget.zhuang_OneKeyAll = true;
        }
        if (GUILayout.Button(new GUIContent("一步到位", "zhuang_OneStepAll=true"), GUILayout.Width(70)))
        {
            cTarget.zhuang_OneStepAll = true;
        }
        EditorGUILayout.EndHorizontal();

        GUI.color = Color.white;

        //折叠
        showEvent = EditorGUILayout.Foldout(showEvent, "完成回调");
        if (showEvent)
        {
            EditorGUILayout.PropertyField(chaiOneEvent, new GUIContent("拆完一个"), true);
            EditorGUILayout.PropertyField(chaiGroupEvent, new GUIContent("拆完一组"), true);
            EditorGUILayout.PropertyField(zhuangOneEvent, new GUIContent("装完一个"), true);
            EditorGUILayout.PropertyField(zhuangGroupEvent, new GUIContent("装完一组"), true);
        }
 

        showLog = EditorGUILayout.Foldout(showLog,"打印");
        if(showLog)
        {
            EditorGUILayout.PropertyField(logStepOnComplete,new GUIContent("完成每步打印"),true);
            EditorGUILayout.PropertyField(logGroupOnComplete,new GUIContent("完成每组打印"),true);
        }
        showIntroduce = EditorGUILayout.Foldout(showIntroduce,"使用说明");
        if (showIntroduce)
        {
            EditorGUILayout.HelpBox(@"使用说明：
1、为模型创建一个父物体
2、第一组需要移动的物体加上后缀xxx00，第二组为xxx01
3、Ctrl+D复制模型（建议将第一组(1)改为(0)），便于查找修改
4、第n+1组是复制的第n组，不可跳跃复制", MessageType.Info);
            EditorGUILayout.HelpBox(@"注意：
1、统一修改某一组，在Inspector上搜索就可以。
2、需保证每一组层级关系相同！只修改后缀
3、若方法拖拽于此，仅为回调，传参为实参。
4、若做被观察者的参数，请用AddListener
5、建议运行前先手动更新数据", MessageType.Info, true);
        }
        //如果没有这一句，就会刷新，什么属性都无法录入
        serializedObject.ApplyModifiedProperties();

    }
    bool showEvent=false;
    bool showLog = false;
    bool showIntroduce = false;

    private void OnEnable()
    {
        cTarget = (C_Assemblage_BrotherMark)(serializedObject.targetObject);

        chaiOneEvent = serializedObject.FindProperty("Chai_Step_Complete");
        chaiGroupEvent = serializedObject.FindProperty("Chai_Group_Complete");
        zhuangOneEvent = serializedObject.FindProperty("Zhuang_Step_Complete");
        zhuangGroupEvent = serializedObject.FindProperty("Zhuang_Group_Complete");

        logStepOnComplete = serializedObject.FindProperty("logStepOnComplete");
        logGroupOnComplete = serializedObject.FindProperty("logGroupOnComplete");
    }

    public static void ShowList(SerializedProperty list, bool showListSize = true, bool showListLabel = true)
    {
        if (showListLabel)
        {
            EditorGUILayout.PropertyField(list);
            EditorGUI.indentLevel += 1;
        }
        if (!showListLabel || list.isExpanded)
        {
            if (showListSize)
            {
                EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
            }
            for (int i = 0; i < list.arraySize; i++)
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            }
        }
        if (showListLabel)
        {
            EditorGUI.indentLevel -= 1;
        }
    }
}




