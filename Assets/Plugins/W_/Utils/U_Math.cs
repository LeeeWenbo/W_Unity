using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_Math : MonoBehaviour
{
    //输入一个vector2，返回他对应的角度，以正x轴为0度，正y为90度
    public static float Vector2ToEulerY(Vector2 vector2)
    {
        return Vector2ToEulerY(vector2.x, vector2.y);
    }
    public static float Vector2ToEulerY(float x, float y)
    {
        float value = 0;
        float length = Mathf.Sqrt(x * x + y * y);
        if (y == 0)
        {
            if (x >= 0)
                value = 0;
            else
                value = Mathf.PI/2;
        }
        else if (y > 0)
        {
            if (x == 0)
                value = Mathf.PI/2;
            else if (x > 0)
                value = Mathf.Asin(y / length);
            else if (x < 0)
                value = Mathf.PI - Mathf.Asin(y / length);
        }
        else
        {
            if (x == 0)
                value = Mathf.PI*1.5f;
            else if (x < 0)
                value = Mathf.PI + Mathf.Asin(-y / length);
            else if (x > 0)
                value = Mathf.PI * 2 - Mathf.Asin(-y / length);
        }
        value *= Mathf.Rad2Deg;
        return value;
    }


    /// 返回一个数在两个数之间，位于什么位置。
    public static float GetRatio(float target, float min, float max)
    {
        float ratio;
        if (max < min)
        {
            Debug.LogWarning("输入的大数反而小于小数!");
            return 0;
        }
        else if (target < min)
        {
            ratio = 0;
        }
        else if (target > max)
        {
            ratio = 1;
        }
        else
        {
            ratio = (target - min) / (max - min);
        }
        return ratio;
    }
}
