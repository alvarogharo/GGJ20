using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public Button newDayBtn;
    private Animator newDayAnmtr;

    void Awake()
    {
        newDayAnmtr = GetComponent<Animator>();
        newDayBtn.enabled = false;
    }

    public void NewDayShown(bool show) {
        newDayAnmtr.SetBool("isActive", show);
    }

    public void setEnabled(int isEnabled) {
        // int argument because you can't use boolean at Animation Events
        if (isEnabled == 0) {
            newDayBtn.enabled = false;
        } else if (isEnabled == 1) {
            newDayBtn.enabled = true;
        }
    }
}
