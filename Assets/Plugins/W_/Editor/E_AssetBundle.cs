/*Name:		 				E_AssetBundles	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/

using UnityEditor;
public class E_AssetBundles
{
    [MenuItem("W_/AssetBundle/BuildAll")]
    public static void Buid()
    {
        BuildAllAssetBundle("abs");
    }
    public static void BuildAllAssetBundle(string directory, bool inAssets=true)
    {
        U_IO.CheckAndSetDirectory(directory, inAssets);
       if(!inAssets)
        {
            BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        }
       else
        {
            BuildPipeline.BuildAssetBundles("Assets/"+ directory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        }

    }
	

}
