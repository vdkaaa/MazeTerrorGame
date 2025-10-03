using UnityEngine;
using System.Collections;

namespace Project.Gameplay.Inspect
{
    public class ScreamerController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup screamerGroup; // UI full-screen with image
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip screamerSfx;
        [SerializeField] private float duration = 1.2f;
        [SerializeField] private bool lockPlayer = true;
        [SerializeField] private CanvasGroup group;
        private bool _active = false;

        public void TriggerScreamer()
        {
            if (_active) return;
            StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            _active = true;
            if (lockPlayer)
            {
                var p = FindFirstObjectByType<Project.Gameplay.Player.PlayerMovement>();
                if (p) p.enabled = false;
            }
            if (audioSource && screamerSfx) audioSource.PlayOneShot(screamerSfx);
            if (screamerGroup)
            {
                HideHUD();
                screamerGroup.alpha = 1f;
                screamerGroup.blocksRaycasts = true;
            }
            yield return new WaitForSeconds(duration);
            if (screamerGroup)
            {
                // fade out
                float t = 0f;
                float fade = 0.4f;
                while (t < fade) { t += Time.deltaTime; screamerGroup.alpha = Mathf.Lerp(1, 0, t / fade); yield return null; }
                screamerGroup.alpha = 0f; screamerGroup.blocksRaycasts = false;
            }
            if (lockPlayer)
            {
                var p = FindFirstObjectByType<Project.Gameplay.Player.PlayerMovement>();
                if (p) p.enabled = true;
            }
            _active = false;
            ShowHUD();
        }



        public void HideHUD()
        {
            group.alpha = 0f;
            group.interactable = false;
            group.blocksRaycasts = false;
        }

        public void ShowHUD()
        {
            group.alpha = 1f;
            group.interactable = true;
            group.blocksRaycasts = true;
        }
    }



}
