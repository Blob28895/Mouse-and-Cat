using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField] private HealthSO _healthSO = default;

    private Slider _slider = default;

    void Start()
    {
        _slider = GetComponent<Slider>();
    }

    void Update()
    {
        float sliderValue = (float)_healthSO.GetHealth() / (float)_healthSO.GetStartingHealth();
        _slider.value = sliderValue;
    }
}
