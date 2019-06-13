/*Name:		 				U_AssetBundles	
 *Description: 				打包AssetBundle，Editor方法
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/

using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;

public static class U_AssetBundles
{
    //从文件读取
    public static AssetBundle Load_AB_File(string path, string abName, bool inAssets = true)
    {
        AssetBundle ab;
        if (!inAssets)
        {
            ab = AssetBundle.LoadFromFile(path + "/" + abName);
        }
        else
        {
            ab = AssetBundle.LoadFromFile("Assets/" + path + "/" + abName);
        }
        return ab;
    }
    public static IEnumerator<AssetBundleCreateRequest> Load_AB_FileAsync(string path, string abName, bool inAssets = true)
    {
        AssetBundleCreateRequest request;
        if (!inAssets)
        {
            request = AssetBundle.LoadFromFileAsync(path + "/" + abName);
        }
        else
        {
            request = AssetBundle.LoadFromFileAsync(U_Path.assets + path + "/" + abName);
        }
        yield return request;
       // return request.assetBundle
    }

    //从内存读取 ？？参数不对，这是模拟内存，到时候应该收到的是byte数组
    public static AssetBundle Load_AB_Memory(string path, string assetBundleName, bool inAssets = true)
    {
        AssetBundle ab;
        if (inAssets)
        {
            ab = AssetBundle.LoadFromMemory(File.ReadAllBytes("Assets/" + path + "/" + assetBundleName));
        }
        else
        {
            ab = AssetBundle.LoadFromMemory(File.ReadAllBytes(path + "/" + assetBundleName));
        }
        return ab;
    }
    public static IEnumerator Load_AB_MemoryAsync(string path, string assetBundleName, bool inAssets = true)
    {
        //创建一个请求
        AssetBundleCreateRequest request;
        if (inAssets)
        {
            request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes("Assets/" + path + "/" + assetBundleName));
        }
        else
        {
            request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(path + "/" + assetBundleName));
        }
        yield return request;//上面的就一直执行，直到返回一个request之后，执行下边那句，还想深入，迭代器。
        AssetBundle ab = request.assetBundle;
    }


    ////WWW读取     旧方式
    ////协程这样返回值！带yield和泛型就好！
    //public static IEnumerator Load_AB_WWW(string path,string name,int version=1, bool isFile=false)
    //{
    //    //AssetBundle abs;
    //    AssetBundle abs;
    //    WWW www;
    //    //Caching 可以从服务器或者本地 第一次会放到本地，第二次直接在Caching读取。如果用的第一个压缩，会自动开启一个协程解压
    //    //直到ready里才能继续执行
    //    while (!Caching.ready)
    //    {
    //        yield return null;
    //    }
    //    if(isFile)
    //    {
    //        //如果使用的是本地协议，前边需要加上"file://"，第二个参数是版本号，跟上次的一样，就不在更新
    //        www = WWW.LoadFromCacheOrDownload(U_Path.file+ path + name, version);
    //    }
    //    else
    //    {
    //        www = WWW.LoadFromCacheOrDownload(path + name, version);
    //    }
    //    yield return www;
    //    //www即使有错误他也不会有异常，所以要自己判断下。所以为空或者空字符串，则说明没问题
    //    if (!string.IsNullOrEmpty(www.error))
    //    {
    //        Debug.Log(www.error);
    //        yield break;
    //    }
    //    abs = www.assetBundle;
    //}

    //UnityWebRequest 已经替代了WWW，之前WWW能实现的，他都能实现，
    public static IEnumerator Load_AB_Web(string path)
    {
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(path);
        yield return request.SendWebRequest();
        //这样也行
        //AssetBundle abs = DownloadHandlerAssetBundle.GetContent(request);
        AssetBundle abs = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
    }


    //加载依赖包
    public static void Load_ABRelyOn(string path, string name, bool inAssets = true)
    {
        AssetBundle manifestAB;
        if (inAssets)
            //这个是要获取的包的目录包，里边保存了其中各个包的依赖关系
            manifestAB = AssetBundle.LoadFromFile(Application.dataPath + "/" + path + "/" + path);
        else
            manifestAB = AssetBundle.LoadFromFile(path + "/" + path);

        //这个包的manifest文件，后边是固定的
        AssetBundleManifest manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        //得到一个包的依赖包[]
        string[] strs = manifest.GetAllDependencies(name);
        foreach (string str in strs)
        {
            if (inAssets)
                //所有依赖的，都加载一下
                AssetBundle.LoadFromFile(Application.dataPath + "/" + path + "/" + str);
            else
                AssetBundle.LoadFromFile(path + "/" + str);
        }
    }

    public static AssetBundle Load_AB_File_ContainRelyOn(string path, string abName, bool inAssets = true)
    {
        AssetBundle ab;
        if (!inAssets)
        {
            ab = AssetBundle.LoadFromFile(path + "/" + abName);
        }
        else
        {
            ab = AssetBundle.LoadFromFile("Assets/" + path + "/" + abName);
        }
        Load_ABRelyOn(path,abName,inAssets);
        return ab;
    }



    //从已经获取的AB包里获取物体
    public static GameObject Load_ABObj_AB(AssetBundle ab, string assetName)
    {
        GameObject go = ab.LoadAsset<GameObject>(assetName);
        return go;
    }

    //加载AssetBundle里边的某个GameObject对象
    public static GameObject Load_ABObj_File(string path, string assetBundleName, string assetName, bool inAssets = true)
    {
        GameObject go;
        AssetBundle ab = Load_AB_File(path, assetBundleName, inAssets);
        go = Load_ABObj_AB(ab, assetName);
        return go;
    }
}
