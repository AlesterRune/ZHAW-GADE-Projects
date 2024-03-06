using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoobleController : MonoBehaviour
{
    [SerializeField]
    private Transform woobleCubeOne;
    
    [SerializeField]
    private Transform woobleCubeTwo;

    private void Update()
    {
        woobleCubeOne.rotation *= Quaternion.Euler(0, 360f * Time.deltaTime, 0);
        woobleCubeTwo.rotation *= Quaternion.Euler(0, -360f * Time.deltaTime, 0);
    }
}
