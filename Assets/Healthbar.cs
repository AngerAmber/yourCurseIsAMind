using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(int health)
    {
        slider.value = health;
        slider.maxValue = health;
    }
    public void SetHealth(int health) 
    {  
        slider.value = health;
    }
}
