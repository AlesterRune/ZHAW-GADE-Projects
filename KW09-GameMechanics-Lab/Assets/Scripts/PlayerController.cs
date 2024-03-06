using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody player;

    [SerializeField]
    private Material playerAliveMaterial;

    [SerializeField]
    private Material playerDeadMaterial;

    [SerializeField]
    private Material playerWinMaterial;

    [SerializeField]
    private float maxDirectionalVelocity = 8;

    private int _points;

    public event Action TargetReached; 
    public event Action<int> PointsUpdated; 
    
    public event Action WallTouched;

    public int Points
    {
        get => _points;
        set => _points = value;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W down");
            if (player.velocity.y < maxDirectionalVelocity) 
                player.AddForce(0, 1 + Random.Range(-0.1f, 0.1f), 0, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A down");
            if (player.velocity.x > -maxDirectionalVelocity) 
                player.AddForce(-1 + Random.Range(-0.1f, 0.1f), 0, 0, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("S down");
            if (player.velocity.y > -maxDirectionalVelocity) 
                player.AddForce(0, -1 + Random.Range(-0.1f, 0.1f), 0, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("D down");
            if (player.velocity.x < maxDirectionalVelocity) 
                player.AddForce(1 + Random.Range(-0.1f, 0.1f), 0, 0, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        Debug.Log(other.tag);
        if (other.CompareTag("TargetArea"))
        {
            GetComponentInChildren<MeshRenderer>().material = playerWinMaterial;
            Points++;
            PointsUpdated?.Invoke(Points);
            TargetReached?.Invoke();
        }
        else
        {
            GetComponentInChildren<MeshRenderer>().material = playerDeadMaterial;
            WallTouched?.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("TargetArea"))
        {
            GetComponentInChildren<MeshRenderer>().material = playerWinMaterial;
        }
        else
        {
            GetComponentInChildren<MeshRenderer>().material = playerDeadMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other);
        GetComponentInChildren<MeshRenderer>().material = playerAliveMaterial;
    }
}
