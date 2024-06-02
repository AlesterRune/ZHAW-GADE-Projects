using UnityEngine;
using UnityEngine.UI;

namespace CyberspaceOlympics
{
    [RequireComponent(typeof(AudioSource))]
    public class ButtonWithSoundFeedback : MonoBehaviour
    {
        private void Start()
        {
            var audioSource = GetComponent<AudioSource>();
            var self = GetComponent<Button>();
            self.onClick.AddListener(() => audioSource.Play());
        }
    }
}