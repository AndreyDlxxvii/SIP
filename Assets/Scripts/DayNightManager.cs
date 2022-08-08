using System;
using UnityEngine;

public class DayNightManager : MonoBehaviour
{
    [SerializeField] private Light _sunLight;
    [SerializeField] private Light _moonLight;
    
    [SerializeField] private AnimationCurve _sunAnimationCurve;
    [SerializeField] private AnimationCurve _moonAnimationCurve;
    [SerializeField] private AnimationCurve _skyBoxCurve;

    [SerializeField] private Material _skyBoxDay;
    [SerializeField] private Material _skyBoxNight;

    [SerializeField] private ParticleSystem _systemStar;
    
    [Range(0,1)]
    [SerializeField] private float _timeOfDay;
    private float _duration = 30f;
    private float _lightSunIntensity;
    private float _lightMoonIntensity;

    private void Start()
    {
        _lightSunIntensity = _sunLight.intensity;
        _lightMoonIntensity = _moonLight.intensity;
    }

    private void Update()
    {
        _timeOfDay += Time.deltaTime / _duration;
        if (_timeOfDay >= 1)
        {
            _timeOfDay -= 1;
        }
        
        RenderSettings.skybox.Lerp(_skyBoxNight, _skyBoxDay, _skyBoxCurve.Evaluate(_timeOfDay));
        if (_skyBoxCurve.Evaluate(_timeOfDay) > 0.1f)
        {
            RenderSettings.sun = _sunLight;
        }
        else
        {
            RenderSettings.sun = _moonLight;
        }
        DynamicGI.UpdateEnvironment();

        var systemStarMain = _systemStar.main;
        systemStarMain.startColor = new Color(1,1,1, 1-_skyBoxCurve.Evaluate(_timeOfDay));
        
        _sunLight.transform.localRotation = Quaternion.Euler(_timeOfDay * 360f, 180f, 0f);
        _moonLight.transform.localRotation = Quaternion.Euler(_timeOfDay * 360f + 180f, 180f, 0f);
        
        _sunLight.intensity = _lightSunIntensity * _sunAnimationCurve.Evaluate(_timeOfDay);
        _moonLight.intensity = _lightMoonIntensity * _moonAnimationCurve.Evaluate(_timeOfDay);
    }
}
