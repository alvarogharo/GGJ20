using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoveTester : MonoBehaviour
{
    [Header("P1")]
    public Image P1attr1;
    public Image P1attr2;
    public Image P1attr3;
    public Text P1Text1;
    public Text P1Text2;
    public Text P1Text3;

    [Header("P2")]
    public Image P2attr1;
    public Image P2attr2;
    public Image P2attr3;
    public Text P2Text1;
    public Text P2Text2;
    public Text P2Text3;

    [Header("Common")]
    public Text NumCoupleText;
    public Text MeanText;
    public Text AffinityText;

    private int _numCouple;
    private float _meanPercentage;
    private float _sumAffinity;
    // Start is called before the first frame update
    void Start()
    {
        ResetCounter();
    }

    public void ResetCounter(){
        _numCouple = 0;
        _meanPercentage = 0f;
        _sumAffinity = 0f;

        P1Text1.text = "0";
        P1Text2.text = "0";
        P1Text3.text = "0";

        P2Text1.text = "0";
        P2Text2.text = "0";
        P2Text3.text = "0";

        NumCoupleText.text = _numCouple.ToString();
        MeanText.text = _meanPercentage.ToString();
    }

    public void GenerateCouple(){
        int[] a1 = PersonFunctions.GenerateAttributes(); 
        P1attr1.sprite = a1[0] < 0 ? Resources.Load<Sprite>("Sprites/GatoSolo") : Resources.Load<Sprite>("Sprites/PerroSolo");
        P1attr2.sprite = a1[1] < 0 ? Resources.Load<Sprite>("Sprites/HamburguesaSolo")  : Resources.Load<Sprite>("Sprites/PizzaSolo");
        P1attr3.sprite = a1[2] < 0 ? Resources.Load<Sprite>("Sprites/MontañaSolo") : Resources.Load<Sprite>("Sprites/PlayaSolo");
        P1Text1.text = Mathf.Abs(a1[0]).ToString();
        P1Text2.text = Mathf.Abs(a1[1]).ToString();
        P1Text3.text = Mathf.Abs(a1[2]).ToString();        

        int[] a2 = PersonFunctions.GenerateAttributes(); 
        P2attr1.sprite = a2[0] < 0 ? Resources.Load<Sprite>("Sprites/GatoSolo") : Resources.Load<Sprite>("Sprites/PerroSolo");
        P2attr2.sprite = a2[1] < 0 ? Resources.Load<Sprite>("Sprites/HamburguesaSolo")  : Resources.Load<Sprite>("Sprites/PizzaSolo");
        P2attr3.sprite = a2[2] < 0 ? Resources.Load<Sprite>("Sprites/MontañaSolo") : Resources.Load<Sprite>("Sprites/PlayaSolo");
        P2Text1.text = Mathf.Abs(a2[0]).ToString();
        P2Text2.text = Mathf.Abs(a2[1]).ToString();
        P2Text3.text = Mathf.Abs(a2[2]).ToString();

        int affinity = PersonFunctions.CalculateAffinity(a1, a2);
        _numCouple++;
        float percentage = (affinity / (float)PersonFunctions.MAX_AFFINITY_SCORE) * 100;
        _sumAffinity += percentage;
        _meanPercentage = _sumAffinity / _numCouple;

        AffinityText.text = percentage.ToString("F2");
        NumCoupleText.text = _numCouple.ToString();
        MeanText.text = _meanPercentage.ToString("F2");
    }

    public void GenerateNCouples(int n){
        for(var i = 0; i < n; i++){
            GenerateCouple();
        }
    }

}
