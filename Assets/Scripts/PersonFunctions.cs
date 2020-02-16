using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonFunctions
{
    // Start is called before the first frame update
    private const int UPPER_RANGE = 5;
    private const int LOWER_RANGE = -5;
    public const int MAX_AFFINITY_SCORE = 30;

    public static int[] GenerateAttributes(){
        int catDogs = Random.Range(LOWER_RANGE, UPPER_RANGE + 1);
        catDogs =  catDogs == 0 ? catDogs + 1: catDogs;
        int hamburguerPizza = Random.Range(LOWER_RANGE, UPPER_RANGE + 1);
        hamburguerPizza =  hamburguerPizza == 0 ? hamburguerPizza - 1: hamburguerPizza;
        int mountainBeach = Random.Range(LOWER_RANGE, UPPER_RANGE + 1);
        mountainBeach =  mountainBeach == 0 ? mountainBeach - 1: mountainBeach;
        

        return new int[] {catDogs, hamburguerPizza, mountainBeach};
    }

    public static int CalculateAffinity(int[] attr1, int[] attr2){
        int result = 0;
        for(int i = 0; i < attr1.Length; i++) {
            if(attr1[i] == attr2[i]){
                //Maximum value
                result = 10;
            }else{
                result += Mathf.Abs(attr1[i] + attr2[i]);
            }
        }
        return result;
    }

}
