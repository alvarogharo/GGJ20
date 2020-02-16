using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonAttributes : MonoBehaviour
{
    // Start is called before the first frame update
    private readonly int UpperRange = 5;
    private readonly int LowerRange = -5;

    public int CatDogs;
    public int HamburgerPizza;
    public int MountainBeach;

    private  int[] Attributes;

    // Start is called before the first frame update
    void Awake()
    {
        CatDogs = Random.Range(LowerRange, UpperRange + 1);
        CatDogs =  CatDogs == 0 ? CatDogs + 1: CatDogs;
        HamburgerPizza = Random.Range(LowerRange, UpperRange + 1);
        HamburgerPizza =  HamburgerPizza == 0 ? HamburgerPizza - 1: HamburgerPizza;
        MountainBeach = Random.Range(LowerRange, UpperRange + 1);
        MountainBeach =  MountainBeach == 0 ? MountainBeach - 1: MountainBeach;
        

        Attributes = new int[] {CatDogs, HamburgerPizza, MountainBeach};
    }

    public int[] GetAttributes(){
        return Attributes;
    }
}
