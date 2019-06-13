using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class C_Line : MonoBehaviour
{
    LineRenderer line;
    public Transform startTran;
    public Transform endTran;
    public float width = 0.001f;
    public Color startColor = Color.blue;
    public Color endColor = Color.black;
    public bool drawLine = false;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        DrawLine();
    }




    void Init()
    {
        line = GetComponent<LineRenderer>();
        if (startTran == null)
        {
            startTran = transform;
        }
        line.positionCount = 2;
        line.startWidth = width;
        line.endWidth = width;
        line.startColor = startColor;
        line.endColor = endColor;
    }

    private void DrawLine()
    {
        if (drawLine)
        {
            Vector3[] poisS = new Vector3[] { startTran.position, endTran.position };
            line.SetPositions(poisS);
        }
    }
}
