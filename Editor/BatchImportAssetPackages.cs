using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UI;

using System.IO;
using System;

public static class BatchImportAssetPackages
{
    public static void ImportAllPackagesInFolder(string folderPath)
    {
        try
        {
            folderPath = folderPath.Replace("\\", "/") + "/";
            string installedFolder = Path.Join(folderPath, "Installed");
            CreateDirectoryIfNotExists(installedFolder);
            string[] files = Directory.GetFiles(folderPath, "*.unitypackage", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                AssetDatabase.ImportPackage(file, false);
                MoveFileToFolder(file, installedFolder);
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log("Error: " + ex.Message);
        }
    }

    private static void MoveFileToFolder(string file, string installedFolder)
    {
        string fileName = Path.GetFileName(file);
        string newFilePath = Path.Join(installedFolder, fileName);
        File.Move(file, newFilePath);
    }

    private static void CreateDirectoryIfNotExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}

// Original: https://gist.github.com/leestuartx/9e1bc376fd5cda583d098ecfc309120b
public class BatchImportAssetPackagesWizard : ScriptableWizard
{
    [SerializeField] private string _packagePath = "./PackagesBatch";

    [MenuItem("Tools/Sensen/Import Packages Batch")]
    private static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Create Menu", typeof(BatchImportAssetPackagesWizard));
    }

    [MenuItem("Tools/Sensen/Resolve Package Manager")]
    private static void ForcePackageManagerResolve()
    {
        UnityEditor.PackageManager.Client.Resolve();
        AssetDatabase.Refresh();
    }

    private void OnWizardCreate()
    {
        BatchImportAssetPackages.ImportAllPackagesInFolder(_packagePath);
    }
}
