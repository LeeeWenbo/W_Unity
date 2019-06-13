/*Name:		 				U_Cursor	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2019-05-
 *Copyright(C) 2019 by 		智网易联*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class U_MouseCursor
{
    public static void ChangeMouseCursor(Sprite sprite)
    {
        Texture2D texture2D = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, sprite.texture.format, false);
        texture2D.SetPixels(sprite.texture.GetPixels((int)sprite.rect.xMin, (int)sprite.rect.yMin, (int)sprite.rect.width, (int)sprite.rect.height));
        Cursor.SetCursor(texture2D, Vector2.zero, CursorMode.Auto);
    }
    public static void ChangeMouseCursor(Texture2D sprite)
    {
        Cursor.SetCursor(sprite, Vector2.zero, CursorMode.Auto);
    }

    public static void RecoverDefaultCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

    }

}
