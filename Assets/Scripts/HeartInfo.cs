using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartInfo : MonoBehaviour
{
    public void Fill(){
        FindObjectOfType<GameController>().FillHeartMini();
    }
}
