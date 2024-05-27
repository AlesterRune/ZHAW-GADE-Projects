using UnityEngine;

namespace CyberspaceOlympics
{
    public class FanController : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        protected void PlayCheer()
        {
            _animator.SetTrigger("Critical");
        }
    }
}