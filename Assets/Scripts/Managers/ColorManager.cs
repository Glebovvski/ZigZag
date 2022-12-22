using System;
using System.Collections;
using UnityEngine;

namespace Managers
{
    public class ColorManager : MonoBehaviour
    {
        [SerializeField] private Material _material;

        [SerializeField] private int _intervalBetweenColorChange;

        private float currentTime = 0;
        private bool changindColor = false;
        private float _colorChangeDuration = 10f;

        public event Action OnColorChange;

        private void Start()
        {
            ResetTimer();
        }

        private void ResetTimer()
        {
            currentTime = 0;
            changindColor = false;
        }

        private void Update()
        {
            if (changindColor)
                return;

            currentTime += Time.deltaTime;
            if (currentTime > _intervalBetweenColorChange)
            {
                StartCoroutine(ChangeColor(UnityEngine.Random.ColorHSV()));
            }
        }

        IEnumerator ChangeColor(Color color)
        {
            changindColor = true;
            float tick = 0;
            while (tick <= _colorChangeDuration)
            {
                tick += Time.deltaTime;
                _material.color = Color.Lerp(_material.color, color, tick / _colorChangeDuration);
                yield return new WaitForEndOfFrame();
            }
            _material.color = color;
            ResetTimer();
        }
    }
}
