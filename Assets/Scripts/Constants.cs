using System;
using UnityEngine;
public static class Constants
 {
    public static readonly string GlobalPopulationKey = "GlobalPopulation";
    public static readonly string CoupledPopulationKey = "CoupledPopulationKey";
    public static readonly string HappinessKey = "HappinessKey";
    public static readonly string EndGameKey = "EndGameKey";

    public static readonly int MaxAffinity = 30;
    public static readonly string HideShowPopUp = "hideShowPopUp";
    public static readonly string UnpairedText = "UNPAIRED: "; 
    public static readonly string HideShowHeart = "hideShowHeart";

    //Development constants
    public static readonly string LevelCreatorKey = "LevelCreator";
    public static readonly string LevelsPath = Application.dataPath + "/levels.json";

 }