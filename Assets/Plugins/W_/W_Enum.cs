/*Name:		 				W_Enum	
 *Description: 				所有大类枚举，都在这里。
 *Author:       			李文博 
 *Date:         			2018-09-
 *Copyright(C) 2018 by 		智网易联*/

namespace W_Enum
    {
    public enum WE_TransformRange { Self, SelfSon, SelfChildren, SelfBrother, Son, Children, Brother }
    public enum WE_LeftRight { None, Left, Right, Both }

    /// <summary>
    /// 包含或者排除
    /// </summary>
    public enum IncludeOrExcept { include, except }
    public enum W_Axis { none, localX, localY, localZ, worldX, worldY, worldZ }
    public enum W_UpdateMode { FixedUpdate, Update, LateUpdate }
    public enum MainButton { 左键, 右键, 中键,无}

    public enum ManYouMode { 禁止移动,步行,飞行,路径}
}


