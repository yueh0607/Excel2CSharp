using AirEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;


public class FilePathAttribute : System.Attribute
{

    public enum LocationType
    {
        AbsPath,
        ProjectFolder,
        AssetsFolder
    }

    public readonly string RelativePath;
    public readonly LocationType Location;

    public string AbsPath
    {
        get
        {
            switch (Location)
            {
                case LocationType.AbsPath:
                    {
                        return RelativePath;
                    }
                case LocationType.ProjectFolder:
                    {
                        return EditorHelper.GetAbsPathToProject(RelativePath);
                    }
                case LocationType.AssetsFolder:
                    {
                        return EditorHelper.GetAbsPathToAsset(RelativePath);
                    }

            }
            throw new System.InvalidOperationException("Unknown Error happend.");
        }
    }
    public FilePathAttribute(string relativePath, LocationType location = LocationType.ProjectFolder)
    {
        if (!Path.HasExtension(relativePath)) Debug.LogError("FilePathAttribute need a path with extension");
        RelativePath = relativePath;
        Location = location;
    }
}




public class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObjectSingleton<T>
{
    public ScriptableObjectSingleton() { }

    private static T instance = null;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                string[] assets = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
                if (assets.Length == 0)
                {
                    T asset = ScriptableObject.CreateInstance<T>();
                    FilePathAttribute filePath = typeof(T).GetCustomAttribute<FilePathAttribute>();
                    if (filePath == null)
                    {
                        Debug.LogError("ScriptableObject singleton must have FilePath attribute");
                        return null;
                    }
                    string dir = Path.GetDirectoryName(filePath.AbsPath);
                    EditorHelper.NotExistCreate(dir);
                    AssetDatabase.CreateAsset(asset, "Assets/" + filePath.RelativePath);
                    instance = asset;
                }
                else if (assets.Length > 1)
                {
                    Debug.LogError("There are multiple persistent data resources of type T, but T is a persistent singleton");
                    foreach (string asset in assets)
                    {
                        Debug.LogWarning(asset);
                    }
                    instance = null;
                }
                else if (assets.Length == 1)
                {
                    instance = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(assets[0]));
                }

            }
            return instance;
        }
    }


}