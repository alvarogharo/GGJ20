using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Person : MonoBehaviour
{
    public GameObject Couple;
    private GameObject OverCouple;
    private int Affinity = 0;
    private bool Clicked = false;
    private Animator Animator;
    private int Layers; //layer of people
    private Transform UIMask;
    //Shadows
    private GameObject SingleShadow;
    private GameObject CoupleShadow;
    //AudioSource
    private AudioSource ASource;
    public AudioClip[] AClips;
    void Awake(){
        Couple = null;
        Animator = this.GetComponent<Animator>();
        Layers = LayerMask.GetMask("Person");
        UIMask = this.transform.GetChild(2).GetChild(0);
        SingleShadow = this.transform.GetChild(3).gameObject;
        CoupleShadow = this.transform.GetChild(4).gameObject;
        ASource = this.GetComponent<AudioSource>();
    }

    void Start() {
        SingleShadow.SetActive(true);
        CoupleShadow.SetActive(false);        
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.y);
        SetSingleAttributes();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0)){            
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, Mathf.Infinity, Layers);            
            if (hit) {
                if (hit.transform == this.transform){
                    PlaySFX(AClips[0]);
                    Clicked = true;
                    HideAttributes();
                    HideShadows();
                    Animator.Play("PersonShaking");
                    if(Couple){
                        Couple.GetComponent<Person>().HideAttributes();
                        Couple.GetComponent<Person>().HideShadows();
                    }
                    
                }
            }         
        }
        if(Input.GetMouseButton(0) && Clicked){
            this.transform.position = new Vector3(pos.x, pos.y - 0.4f, pos.y - 0.4f);
        }

        if (Input.GetMouseButtonUp(0) && Clicked){
            Clicked = false;
            CheckCouple();
            CheckOutOfScreen();
        }
    }

    private void CheckCouple(){
        if(!Couple && OverCouple){ //Couple
            if(!OverCouple.GetComponent<Person>().HasCouple()){
                int a = CalculateScore(this.GetComponent<PersonAttributes>().GetAttributes(), OverCouple.GetComponent<PersonAttributes>().GetAttributes());
                Debug.Log("Affinity: "+a);
                DoCouple(OverCouple, true, a);
                OverCouple.GetComponent<Person>().DoCouple(this.gameObject, false, a);
                ShowAttributes();
            }else{
                SingleShadow.SetActive(true);
                Animator.Play("HideAttributes");
                this.transform.position = new Vector3(this.transform.position.x +0.4f, this.transform.position.y-0.5f, this.transform.position.y-0.5f);
            }
        }else if(Couple && OverCouple){ //Cheat
            if(Couple == OverCouple){ //if same just returns to original pos
                this.transform.position = new Vector3(Couple.transform.position.x -0.7f, Couple.transform.position.y, Couple.transform.position.z);
                CoupleShadow.SetActive(true);
                ShowAttributes();
            }else if(!OverCouple.GetComponent<Person>().HasCouple()){ //other person without couple
                Couple.GetComponent<Person>().DoBreakUp();
                int a = CalculateScore(this.GetComponent<PersonAttributes>().GetAttributes(), OverCouple.GetComponent<PersonAttributes>().GetAttributes());
                Debug.Log("Affinity: "+a);
                DoCouple(OverCouple, true, a);
                OverCouple.GetComponent<Person>().DoCouple(this.gameObject, false, a);
                ShowAttributes();
            }else{
                this.transform.position = new Vector3(this.transform.position.x +0.4f, this.transform.position.y-0.5f, this.transform.position.y-0.5f);
                Couple.GetComponent<Person>().DoBreakUp();
                DoBreakUp();
                Animator.Play("HideAttributes");
            }
        }else if(Couple && !OverCouple){ //Divorce
            Couple.GetComponent<Person>().DoBreakUp();
            DoBreakUp();
            Animator.Play("HideAttributes");
        }else{
            Animator.Play("SingleAnimation");
            SingleShadow.SetActive(true);
        }
    }

    private void CheckOutOfScreen(){
        Vector3 pos = this.transform.position;
        if(pos.x < -2.3){ //Limit left
            this.transform.position = new Vector3(-2.3f, pos.y, pos.y);
            if(HasCouple()){
                Couple.transform.position = new Vector3(-1.6f, Couple.transform.position.y, Couple.transform.position.y);
            }
        }
        if(pos.x > 2){
            this.transform.position = new Vector3(1.7f, pos.y, pos.y);
            if(HasCouple()){
                Couple.transform.position = new Vector3(2.4f, Couple.transform.position.y, Couple.transform.position.y);
            }
        }
        if(pos.y > 3.7){
            this.transform.position = new Vector3(pos.x, 3.7f, 3.7f);
            if(HasCouple()){
                Couple.transform.position = new Vector3(Couple.transform.position.x, 3.7f, 3.7f);
            }
        }
        if(pos.y < -3){
            this.transform.position = new Vector3(pos.x, -3f, -3f);
            if(HasCouple()){
                Couple.transform.position = new Vector3(Couple.transform.position.x, -3f, -3f);
            }
        }
        if(this.transform.position != pos){
            PlaySFX(AClips[2]);
        }else{
            PlaySFX(AClips[1]);
        }
    }

    public void DoCouple(GameObject c, bool clicked, int a){
        Couple = c;
        Affinity = a;
        SingleShadow.SetActive(false);            
        if(clicked){
            this.transform.position = new Vector3(Couple.transform.position.x -0.7f, Couple.transform.position.y, Couple.transform.position.z);
            CoupleShadow.SetActive(true);
        }
        //Do Animation
    }

    public void DoBreakUp(){
        SingleShadow.SetActive(true);
        CoupleShadow.SetActive(false);
        Couple = null;
        Affinity = 0;
    }

    
    public bool HasCouple(){
        return Couple != null;
    }

    public int GetAffinity(){ return Affinity;}
    public GameObject GetCouple(){return Couple;}

    private int CalculateScore(int[] attributes1, int[] attributes2){
        int result = 0;
        for(int i = 0; i < 3; i++) {
            if(attributes1[i] == attributes2[i]){
                //Maximum value
                result = 10;
            }else{
                result += Mathf.Abs(attributes1[i] + attributes2[i]);
            }
        }
        return result;
    }

    public void ShowAttributes(){
        int[] attr1 = this.GetComponent<PersonAttributes>().GetAttributes();
        int[] attr2 = Couple.GetComponent<PersonAttributes>().GetAttributes();        

        UIMask.GetChild(0).GetComponent<Image>().sprite = attr1[0] < 0 ? Resources.Load<Sprite>("Sprites/GatoPareja") : Resources.Load<Sprite>("Sprites/PerroPareja");
        UIMask.GetChild(1).GetComponent<Image>().sprite = attr1[1] < 0 ? Resources.Load<Sprite>("Sprites/HamburguesaPareja")  : Resources.Load<Sprite>("Sprites/PizzaPareja");
        UIMask.GetChild(2).GetComponent<Image>().sprite = attr1[2] < 0 ? Resources.Load<Sprite>("Sprites/MontañaPareja") : Resources.Load<Sprite>("Sprites/PlayaPareja");
        UIMask.GetChild(3).GetComponent<Image>().sprite = attr2[0] < 0 ? Resources.Load<Sprite>("Sprites/GatoPareja") : Resources.Load<Sprite>("Sprites/PerroPareja");
        UIMask.GetChild(4).GetComponent<Image>().sprite = attr2[1] < 0 ? Resources.Load<Sprite>("Sprites/HamburguesaPareja")  : Resources.Load<Sprite>("Sprites/PizzaPareja");
        UIMask.GetChild(5).GetComponent<Image>().sprite = attr2[2] < 0 ? Resources.Load<Sprite>("Sprites/MontañaPareja") : Resources.Load<Sprite>("Sprites/PlayaPareja");
        if(Affinity < 10){
             UIMask.GetChild(6).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/ARO shit");
        }
        else if(Affinity >= 10 && Affinity < 20){
             UIMask.GetChild(6).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/ARO okey");
        }
        else if(Affinity >= 20 && Affinity <= 30){
             UIMask.GetChild(6).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/ARO besties");
        }              
        UIMask.GetChild(7).GetComponent<Slider>().value = Affinity;
        Animator.Play("ShowAttributes");
    }

    private void SetSingleAttributes(){
        int[] attr = this.GetComponent<PersonAttributes>().GetAttributes();
        if(attr != null){
            UIMask.GetChild(8).GetComponent<Image>().sprite = attr[0] < 0 ? Resources.Load<Sprite>("Sprites/GatoSolo") : Resources.Load<Sprite>("Sprites/PerroSolo");
            UIMask.GetChild(9).GetComponent<Image>().sprite = attr[1] < 0 ? Resources.Load<Sprite>("Sprites/HamburguesaSolo")  : Resources.Load<Sprite>("Sprites/PizzaSolo");
            UIMask.GetChild(10).GetComponent<Image>().sprite = attr[2] < 0 ? Resources.Load<Sprite>("Sprites/MontañaSolo") : Resources.Load<Sprite>("Sprites/PlayaSolo");
        }else{
            Debug.LogError("No se han obtenido los atributos");
        }
    }

    private void PlaySFX(AudioClip clip){
        ASource.clip = clip;
        ASource.Play();
    }

    public void PlayAffinity(){
        if(Affinity < 10){
            ASource.clip = AClips[5];
        }
        else if(Affinity >= 10 && Affinity < 20){
            ASource.clip = AClips[4];
        }
        else if(Affinity >= 20 && Affinity <= 30){
            ASource.clip = AClips[3];
        }
        ASource.Play();
    }

    public void HideAttributes(){
        Animator.Play("HideAttributes");
    }
    
    public void HideShadows(){
        SingleShadow.SetActive(false);
        CoupleShadow.SetActive(false);
    }

    //********************
    //COLLIISION
    //********************
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other && other.transform.tag != "Decor" && other.transform.gameObject.layer != 2 && Clicked){
            OverCouple = other.transform.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.gameObject == OverCouple){
            OverCouple = null;
        }
    }
}
