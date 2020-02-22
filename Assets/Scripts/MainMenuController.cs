using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public Animator ResetPopUpAnimator;
    public Animator ContactPopUpAnimator;
    public Animator PopUpBackdropAnimator;

    public Text PopulationCounter;
    public Text Unpaired;
    public Slider Heart;
    public int GlobalPopulation;
    public int CoupledPopulation = 0;
    public int MaxHapiness;
    public int HappinessPercentage;
    public int Happiness = 0;
    public int NumberOfLevels;

    //Initial Heart Animation
    private float HeartSliderValue;
    private bool AnimateHeart = false;
    private float HeartValueDelta;
    private float AnimationDuration = 1f;

    void Start()
    {
        PlayerPrefs.SetInt(Constants.GlobalPopulationKey, GlobalPopulation);
        CoupledPopulation = CheckSetOrGet(Constants.CoupledPopulationKey, CoupledPopulation);
        Happiness = CheckSetOrGet(Constants.HappinessKey, Happiness);
        int nonCoupledPopulation = GlobalPopulation - CoupledPopulation;
        Unpaired.text = Constants.UnpairedText + nonCoupledPopulation;
        MaxHapiness = (GlobalPopulation * Constants.MaxAffinity) / 2;
        HappinessPercentage = (Happiness * 100) / MaxHapiness;
        PopulationCounter.text = "0%";
        Heart.value = 0;
        StartCoroutine("FillHeartSlider");
    }

    void Update(){
        if(AnimateHeart){
            HeartSliderValue += Time.deltaTime * HeartValueDelta;
            int IntValue = Mathf.RoundToInt(HeartSliderValue);
            PopulationCounter.text = IntValue + "%";
            Heart.value = IntValue;
        }
    }

    IEnumerator FillHeartSlider(){
        HeartSliderValue = 0f;
        HeartValueDelta = HappinessPercentage / AnimationDuration;
        AnimateHeart = true;
        yield return new WaitForSeconds(AnimationDuration);
        AnimateHeart = false;
        PopulationCounter.text = HappinessPercentage + "%";
        Heart.value = HappinessPercentage;
    }

    public void Play() {
        int rand =  Random.Range(1, NumberOfLevels + 1);
        FindObjectOfType<SoundtrackDDOL>().PlayClick();
        SceneManager.LoadScene("Level" + rand);
    }

    public void ShowHideResetPopUp() {
        FindObjectOfType<SoundtrackDDOL>().PlayClick();
        ResetPopUpAnimator.SetTrigger(Constants.HideShowPopUp);
        PopUpBackdropAnimator.SetTrigger(Constants.HideShowPopUp);

    }

    public void ShowHideContactPopUp() {
        FindObjectOfType<SoundtrackDDOL>().PlayClick();
        ContactPopUpAnimator.SetTrigger(Constants.HideShowPopUp);
        PopUpBackdropAnimator.SetTrigger(Constants.HideShowPopUp);
    }

    public void ResetPopulation() {
        int globalPopulation = PlayerPrefs.GetInt(Constants.GlobalPopulationKey);
        PlayerPrefs.SetInt(Constants.GlobalPopulationKey, globalPopulation);
        PlayerPrefs.SetInt(Constants.HappinessKey, 0);
        PlayerPrefs.SetInt(Constants.CoupledPopulationKey, 0);
        PlayerPrefs.SetInt(Constants.EndGameKey, 0);
        Unpaired.text = Constants.UnpairedText + globalPopulation;
        Happiness = 0;
        CoupledPopulation = 0;
        MaxHapiness = (globalPopulation * Constants.MaxAffinity) / 2;
        HappinessPercentage = (Happiness * 100) / MaxHapiness;
        PopulationCounter.text = HappinessPercentage + "%";
        Heart.value = HappinessPercentage;
        ShowHideResetPopUp();
    }

    public int CheckSetOrGet(string key, int value) {
        int result;
        if (!PlayerPrefs.HasKey(key)){
            PlayerPrefs.SetInt(key, value);
            result = value;
        } else {
            result = PlayerPrefs.GetInt(key);
        }
        return result;
    }
}
