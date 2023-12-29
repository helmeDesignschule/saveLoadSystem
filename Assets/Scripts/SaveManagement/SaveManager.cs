
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// The Save Manager is responsible for turning serialized objects into json files and
/// inversely, json files back into serializable objects.
/// </summary>
public static class SaveManager
{
    //this gives us the folder path we want to save the files to
    private static string SaveFolderPath => Path.Combine(Application.persistentDataPath, "Savegames");
    
    //this takes the folder path and adds the file name to it.
    private static string GetFilePath(string fileName) => Path.Combine(SaveFolderPath, $"{fileName}.json");
    
    /// <summary>
    /// We call this to save the actual data to a json file.
    /// </summary>
    /// <param name="fileName">The name under which we want to save the file</param>
    /// <param name="data">The actual object that we want to save</param>
    /// <typeparam name="T">"The type of serialized object we want to be saved"</typeparam>
    /// <returns>If saving works, returns true. If anything fails, this returns false</returns>
    public static bool TrySaveData<T>(string fileName, T data)
    {
        var path = GetFilePath(fileName);

        //if a try block encounters an error, instead of canceling the execution of the code,
        //it goes to the catch-block
        try 
        {
            if(!Directory.Exists(SaveFolderPath))
            {	
                //if the folder structure does not exist yet, create it
                Directory.CreateDirectory(SaveFolderPath);
            }
            
            if (File.Exists(path))
            {
                //if the file already exists, we delete it, so that we can create it anew
                File.Delete(path);
            }
            //we create the new file
            using FileStream stream = File.Create(path);
            stream.Close();

            //then we fill the file with the created json text
            string jsonConvertedData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(path, jsonConvertedData);
            return true;
        }
        catch (Exception e)
        {
            //if anything goes wrong, we give out an error and return false
            Debug.LogError($"Data cannot be saved due to: {e.Message} {e.StackTrace}");
            return false;
        }
    }

    /// <summary>
    /// We call this to load a json file and turn it back into an object we can work with.
    /// </summary>
    /// <param name="fileName">the filename we attempt to load</param>
    /// <param name="data">the data we will give out once the file is loaded and converted</param>
    /// <typeparam name="T">The type of object we want to turn the json file into</typeparam>
    /// <returns>If the file can be loaded, we return true, otherwise we return false</returns>
    public static bool TryLoadData<T>(string fileName, out T data)
    {
        var path = GetFilePath(fileName);
        //we have to create a default data, so that in case the loading goes wrong, we can give out anything.
        //without this, c# would not compile.
        data = default;

        if (!File.Exists(path))
        {
            //if the file we try to load does not exist, we throw a warning and return false.
            Debug.LogWarning($"File cannot be loaded at \"{path}\".");
            return false;
        }

        try
        {
            //we then read the file and convert it into the object type we try to load
            data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return true;
        }
        catch (Exception e)
        {
            //if anything with the loading goes wrong, we throw an error
            Debug.LogError($"Data cannot be loaded due to: {e.Message} {e.StackTrace}");
            return false;
        }
    }

    /// <summary>
    /// We don't use this in this example, but with this we can get all file names that exist.
    /// We could use this to allow to save and load a number of files, not just one.
    /// </summary>
    /// <returns>An array of strings representing all save file names.</returns>
    public static string[] GetAllSaveFileNames()
    {
        var info = new DirectoryInfo(SaveFolderPath);
        var fileInfo = info.GetFiles();

        List<string> fileNames = new List<string>();
        foreach (var file in fileInfo)
        {
            if (!file.Name.EndsWith(".json"))
                continue;
            fileNames.Add(file.Name.Replace(".json", ""));
        }

        return fileNames.ToArray();
    }
}
