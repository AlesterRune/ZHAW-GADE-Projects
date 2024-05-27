using System.Collections.Generic;
using UnityEngine;

namespace CyberspaceOlympics
{
    public class ActionPointsContainerController : MonoBehaviour
    {
        [SerializeField]
        private ActionPointUiController actionPointUiPrefab;

        private readonly List<ActionPointUiController> _actionPoints = new();
        
        private void Start()
        {
            var actionPointsProvider = FindObjectOfType<PlayerController>();
            foreach (var actionPoint in actionPointsProvider.ActionPoints)
            {
                var actionPointUi = Instantiate(actionPointUiPrefab, transform);
                actionPointUi.SetSource(actionPoint);
                _actionPoints.Add(actionPointUi);
            }
        }

        private void OnDestroy()
        {
            _actionPoints.ForEach(Destroy);
        }
    }
}
