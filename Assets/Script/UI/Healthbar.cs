using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] Health healthComponent;
    Slider healthbarSlider;

    private void Awake()
    {
        healthbarSlider = GetComponent<Slider>();
        if (!healthbarSlider || !healthComponent) { gameObject.SetActive(false); }
        healthComponent.HealthUpdate += UpdateSlider;
    }

    public void UpdateSlider(int value, int maxValue)
    {
        float slider_value = (float) value / maxValue;
        healthbarSlider.value = slider_value;
    }
}
