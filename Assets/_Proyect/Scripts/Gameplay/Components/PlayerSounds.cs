// path: Assets/_Proyect/Scripts/Gameplay/Player/PlayerSounds.cs
using UnityEngine;
using Project.Core.Events.DTOs;

namespace Project.Gameplay.Player
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerSounds : MonoBehaviour
    {
        [Header("Audio Clips")]
        [SerializeField] private AudioClip takeDamageSound;

        [Header("Events")]
        [SerializeField] private MonoBehaviour eventBusSource;

        private AudioSource _audioSource;
        private IEventBus _bus;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _bus = eventBusSource as IEventBus;
        }

        private void OnEnable()
        {
            _bus?.Subscribe<PlayerTookDamage>(OnPlayerTookDamage);
        }

        private void OnDisable()
        {
            _bus?.Unsubscribe<PlayerTookDamage>(OnPlayerTookDamage);
        }

        private void OnPlayerTookDamage(PlayerTookDamage evt)
        {
            if (takeDamageSound != null)
            {
                _audioSource.PlayOneShot(takeDamageSound);
            }
        }
    }
}
