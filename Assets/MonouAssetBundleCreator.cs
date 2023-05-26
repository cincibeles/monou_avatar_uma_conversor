#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System.IO;

public class MonouAssetBundleCreator : MonoBehaviour
{
    [MenuItem("Assets/Monou Build Asset Bundle")]
    static void BuildBundles(){

        List<BundleB> file2Bundle = new List<BundleB>();

        // *** RACE
        DirectoryInfo info = new DirectoryInfo("Assets/Monou Avatar/race");
        var directoriesInfo = info.GetDirectories();
        foreach(DirectoryInfo directory in directoriesInfo){
            var directoryFileInfo = directory.GetFiles();
            foreach(FileInfo file in directoryFileInfo)
                if(file.Name.Substring(file.Name.Length - 5) != ".meta")
                    file2Bundle.Add(new BundleB("Assets/Monou Avatar/race/" + directory.Name + "/" + file.Name, "race"));
        }
        var fileInfo = info.GetFiles();
        foreach(FileInfo file in fileInfo)
            if(file.Name.Substring(file.Name.Length - 5) != ".meta")
                file2Bundle.Add(new BundleB("Assets/Monou Avatar/race/" + file.Name, "race"));

        // *** CLOTHES 
        info = new DirectoryInfo("Assets/Monou Avatar/clothes");
        directoriesInfo = info.GetDirectories();
        foreach(DirectoryInfo directory in directoriesInfo){
            var directoryFileInfo = directory.GetFiles();
            foreach(FileInfo file in directoryFileInfo)
                if(file.Name.Substring(file.Name.Length - 5) != ".meta")
                    file2Bundle.Add(new BundleB("Assets/Monou Avatar/clothes/" + directory.Name + "/" + file.Name, directory.Name));
        }

        // *** ANIMATIONS 
        info = new DirectoryInfo("Assets/Monou Avatar/animations");
        directoriesInfo = info.GetDirectories();
        foreach(DirectoryInfo directory in directoriesInfo){
            var directoryFileInfo = directory.GetFiles();
            foreach(FileInfo file in directoryFileInfo)
                if(file.Name.Substring(file.Name.Length - 5) != ".meta")
                    file2Bundle.Add(new BundleB("Assets/Monou Avatar/animations/" + directory.Name + "/" + file.Name, directory.Name));
        }

        int idx = -1;
        string bundleName = "";
        List<string> fileNames = new List<string>();
        AssetBundleBuild[] buildMap = new AssetBundleBuild[file2Bundle.Count];
        foreach(BundleB f2b in file2Bundle){
            if(bundleName != f2b.group){
                bundleName = f2b.group;
                if(idx>=0){
                    string[] data = new string[fileNames.Count];
                    int c=0; foreach(string n in fileNames){
                        data[c++] = n;
                        print(">>>>" + n);
                    }
                    buildMap[idx].assetNames = data;
                }
                idx++;
                fileNames = new List<string>();
                buildMap[idx].assetBundleName = "monou_" + f2b.group;
                print(">>monou_" + f2b.group);
            }
            fileNames.Add(f2b.file);
        }
        if(idx>=0){
            string[] data = new string[fileNames.Count];
            int c=0; foreach(string n in fileNames){
                data[c++] = n;
                print(">>>>" + n);
            }
            buildMap[idx].assetNames = data;
        }

        AssetBundleBuild[] theFinalBuildMap = new AssetBundleBuild[idx+1];
        for(int i=0; i<=idx; i++){
            theFinalBuildMap[i].assetBundleName = buildMap[i].assetBundleName;
            theFinalBuildMap[i].assetNames = buildMap[i].assetNames;
        }
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles", theFinalBuildMap, BuildAssetBundleOptions.None, BuildTarget.WebGL);
    }
}

public class BundleB {
    public string file;
    public string group;
    public BundleB(string f, string g){
        file = f;
        group = g;
    }
}

#endif