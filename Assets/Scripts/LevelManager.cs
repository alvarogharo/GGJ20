using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelManager : MonoBehaviour
{
    //Obtiene todos los niveles
    public static List<Level> GetAllLevels(){
        string json = ReadFromJson(Constants.LevelsPath);
        var levels = new List<Level>();
        if(json != ""){
            levels = JsonHelper.FromJson<Level>(json);
        }
        return levels;
    }

    //Obtiene un nivel en concreto
    public static Level GetLevel(int index){
        var levels = GetAllLevels();
        return levels[index];
    }

    //Transforma los niveles en JSON y los guarda
    public static void SaveLevels(List<Level> levels){
        string jsonToWrite = JsonHelper.ToJson(levels, true);
        SaveToJson(Constants.LevelsPath, jsonToWrite);
    }

    //Devuelve el JSON del archivo de niveles
    private static string ReadFromJson(string path){
         if(File.Exists(path)){
             using(StreamReader reader = new StreamReader(path))
             {
                 string json = reader.ReadToEnd();
                 return json;
             }
         }
         return "";
     }

    //Guarda los niveles en formato JSON
     private static void SaveToJson(string path, string json){
         FileStream fileStream = new FileStream(path, FileMode.Create);
         using(StreamWriter writer = new StreamWriter(fileStream)){
             writer.WriteLine(json);
         }
     }
}
