using UnityEngine;
using System;

namespace Project.Data
{
    [CreateAssetMenu(fileName = "NewCounter", menuName = "Data/CounterPuzzle")]
    public class CounterSO : ScriptableObject
    {
        [Header("Config")]
        [SerializeField] private int targetValue = 3;
        [SerializeField] private int initialValue = 0;

        [Header("Lectura para el juego")]
        [SerializeField] private int currentValue;
        public event Action OnTargetReached;
        public int CurrentValue => currentValue;
        public int TargetValue => targetValue;

        void OnEnable()
        {
            currentValue = initialValue;
        }
        public void Increment()
        {
            if (targetValue <= currentValue) return;
            currentValue++;
            Debug.Log(currentValue);
            if(currentValue >= targetValue)
            {
                OnTargetReached?.Invoke();
            }
        }
    }

}