using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class LevelCreatorFunctions : EditorWindow
{
    string _levelIndex = "0";
    private int _levelstate = 0; //0 para Nuevo, 1 para cargado. 
    private string _statusText = "-";
    private int _currentlevel;
    [MenuItem("Window/Repairs/Level Creator")]
    public static void ShowWindow(){
        GetWindow<LevelCreatorFunctions>("Level Creator");
    }

    private void OnGUI()
     {
         if(SceneManager.GetActiveScene().name == "LevelCreator"){
            //Botón para crear nuevo nivel
            if (GUILayout.Button("New Level"))
            {
                if(EditorUtility.DisplayDialog("Aviso de guardado", "Si continua se perderá el progreso del nivel actual. ¿Desea continuar?", "Si", "No")){
                    CreateEmptyLevel();
                }
            }
            //Botón para cargar niveles
            if (GUILayout.Button("Load Level"))
            {
                if(EditorUtility.DisplayDialog("Aviso de guardado", "Si continua se perderá el progreso del nivel actual. ¿Desea continuar?", "Si", "No")){
                    LoadLevel();
                }
            }
            //El índice del nivel que se va a cargar
            _levelIndex = EditorGUILayout.TextField("Level To Load:", _levelIndex);

            //Botón de guardar el nivel
            if (GUILayout.Button("Save Level"))
            {
                SaveLevel();
            }

            //Botón para cargar niveles
            if (GUILayout.Button("Delete level"))
            {
                if(EditorUtility.DisplayDialog("Aviso de borrado", "Esta acción es irreversible y se borrrará el nivel por completo. ¿Desea continuar?", "Si", "No")){
                    DeleteLevel();
                }
            }
            //Texto para estatus
            EditorGUILayout.LabelField("State: ", _statusText);
         }else{
             EditorGUILayout.LabelField("Esta ventana no sirve para esta escena.");
             EditorGUILayout.LabelField("Abre \"LevelCreator\" para hacer uso de esta ventana.");
         }
     }
 
    //Función que coge el objeto padre del nivel y lo vacía eliminando a todos los hijos.
     private void CreateEmptyLevel()
     {
         //Se busca el objeto padre
         GameObject atrezzo = GameObject.FindWithTag(Constants.LevelCreatorKey);
         //Se destruyen todos los hijos
         if(atrezzo.transform.childCount > 0){
            for(var i = atrezzo.transform.childCount-1; i >= 0; i--){
                DestroyImmediate(atrezzo.transform.GetChild(i).gameObject);
            }
         }
        //Se marca el estado a nuevo.
         _levelstate = 0;
         _statusText = "New Level.";
         Debug.Log("Created new level");
     }

    //Lee el JSON y carga los objetos en las posiciones
     private void LoadLevel()
     {
         //Se vacía el nivel
         CreateEmptyLevel();
         //Se obtiene el nivel a partir del indice
         int.TryParse(_levelIndex, out _currentlevel);
         var level = LevelManager.GetLevel(_currentlevel);
         if(level != null){         
            //Se instancian los elementos como hijos del padre
            GameObject atrezzo = GameObject.FindWithTag(Constants.LevelCreatorKey);
            for (var i  = 0; i < level.items.Count; i++){
                LevelElement item = level.items[i];
                string prefabPath = "Prefabs/"+item.prefabName;
                GameObject instance = Instantiate(Resources.Load(prefabPath, typeof(GameObject)), atrezzo.transform) as GameObject;
                instance.transform.position = new Vector3(item.positionx, item.positiony, 0f);
            }
            //Se actualiza el estado
            _levelstate = 1;
            _statusText = "Loaded level "+_currentlevel;
            Debug.Log("Loaded level "+ _currentlevel);
         }else{
            Debug.LogError("There's not level with that index. Try again.");
         }
     }

    //Guarda los datos de los elementos de atrezzo en el JSON
     private void SaveLevel(){
        var levels = LevelManager.GetAllLevels();

         GameObject atrezzo = GameObject.FindWithTag(Constants.LevelCreatorKey);
         List<LevelElement> items = new List<LevelElement>();
        //Se recorre la lista de hijos y se crea un nuevo objeto del tipo Level
         if(atrezzo.transform.childCount > 0){
            for(var i = atrezzo.transform.childCount-1; i >= 0; i--){
                //Se eliminan las palabras (1) o (Clone) de los nombres de los objetos para acceder correctamente al prefab
                string name = atrezzo.transform.GetChild(i).gameObject.name.Split('(')[0];
                if(name.EndsWith(" ")){
                    name = name.Remove(name.Length-1);
                }
                items.Add(new LevelElement(name, atrezzo.transform.GetChild(i).position.x, atrezzo.transform.GetChild(i).position.y));
            }
         }
         Level l = new Level(items);
         //Si es un nivel nuevo se añade a la lista y se guardan todos.
         //Si es uno cargado se sustituye y se guardan todos de nuevo.
         if(_levelstate == 0){
             levels.Add(l);
             LevelManager.SaveLevels(levels);
             _currentlevel = levels.Count -1;
             _levelstate = 1;
         }else{
             levels[_currentlevel] = l;
             LevelManager.SaveLevels(levels);
         }
         Debug.Log("Level saved");
     }  

     //Borra el nivel
     private void DeleteLevel(){
         //Si es un nuevo nivel no guardado aun simplemente se reinicia el creador de niveles
         if(_levelstate == 1){
             //Si es un nivel ya guardado se elimina del array y se reinicia el creador de niveles
             var levels = LevelManager.GetAllLevels();
             levels.RemoveAt(_currentlevel);
             LevelManager.SaveLevels(levels);
         }
         CreateEmptyLevel();
         Debug.Log("Level deleted");
     }   
}

[Serializable]
public class LevelElement{
    public string prefabName;
    public float positionx;
    public float positiony;

    public LevelElement(string prefabName, float positionx, float positiony){
        this.prefabName = prefabName;
        this.positionx = positionx;
        this.positiony = positiony;
    }
}

[Serializable]
public class Level{
    public List<LevelElement> items;
    public Level(List<LevelElement> items){
        this.items = items;
    }

    public Level(){
        this.items = new List<LevelElement>();
    }
}