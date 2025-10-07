using UnityEngine;
using Project.Core.Events.DTOs;

namespace Project.Core.Services
{
    public class TimeManager : MonoBehaviour, ITimeService
    {
        [SerializeField] private MonoBehaviour eventBusSource; // EventBus
        private IEventBus _bus;

        private float _elapsed;
        public float DeltaTime => Time.deltaTime;
        public float TimeSinceStart => _elapsed;

        private void Awake()
        {
            _bus = eventBusSource as IEventBus;
        }

        private float _accum;
        private int _sec, _min;

        private void Update()
        {
            _elapsed += Time.deltaTime;
            _accum += Time.deltaTime;

            if (_accum >= 1f)
            {
                _accum -= 1f;
                _sec++;
                if (_sec >= 60) { _sec = 0; _min++; }
                _bus?.Publish(new TimeTick(_min, _sec));
            }
        }
    }
}
