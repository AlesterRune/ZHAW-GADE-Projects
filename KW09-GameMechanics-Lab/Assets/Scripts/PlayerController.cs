using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody player;

    [SerializeField]
    private Color playerDefaultColor;
    
    [SerializeField]
    private Color playerFlatColor;

    [SerializeField]
    private Color playerSmallColor;

    [SerializeField]
    private Color playerLargeColor;

    [SerializeField]
    private Color playerDeadColor;

    [SerializeField]
    private float maxDirectionalVelocity = 8;

    [SerializeField]
    private AudioSource pickUpSound;
    
    [SerializeField]
    private AudioSource unlockSound;
    
    [SerializeField]
    private AudioSource gameOverSound;
    

    private float _initialMaxDirectionalVelocity;
    
    private bool _shrinking;
    private bool _enlarging;
    private bool _flattening;

    public event Action TargetReached; 
    public event Action<int> PointsUpdated; 
    
    public event Action WallTouched;
    
    private bool AllowFlatten => Points >= 5;
    private bool AllowEnlarge => Points >= 10;
    private bool AllowShrink => Points >= 15;

    public int Points { get; set; }

    private MeshRenderer MeshRenderer { get; set; }

    private void Awake()
    {
        _initialMaxDirectionalVelocity = maxDirectionalVelocity;
    }

    private void Start()
    {
        MeshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Update()
    {
        HandleShrink();
        HandleEnlarge();
        HandleFlatten();
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (player.velocity.y < maxDirectionalVelocity)
                player.AddForce(0, 1 + Random.Range(-0.1f, 0.1f), 0, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (player.velocity.x > -maxDirectionalVelocity)
                player.AddForce(-1 + Random.Range(-0.1f, 0.1f), 0, 0, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (player.velocity.y > -maxDirectionalVelocity)
                player.AddForce(0, -1 + Random.Range(-0.1f, 0.1f), 0, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (player.velocity.x < maxDirectionalVelocity)
                player.AddForce(1 + Random.Range(-0.1f, 0.1f), 0, 0, ForceMode.Impulse);
        }
    }

    private void HandleShrink()
    {
        if (!AllowShrink || _flattening || _enlarging)
            return;
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _shrinking = true;
            maxDirectionalVelocity = 2 * _initialMaxDirectionalVelocity;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            player.transform.localScale = Vector3.Lerp(player.transform.localScale, new Vector3(0.5f, 0.5f, 1), Time.deltaTime);
            player.mass = Mathf.Lerp(player.mass, 0.2f, Time.deltaTime);
            MeshRenderer.material.color = Color.Lerp(MeshRenderer.material.color, playerSmallColor, Time.deltaTime);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _shrinking = false;
            player.transform.localScale = Vector3.one;
            player.mass = 1f;
            maxDirectionalVelocity = _initialMaxDirectionalVelocity;
            player.velocity = new Vector3(Mathf.Clamp(player.velocity.x, -_initialMaxDirectionalVelocity, _initialMaxDirectionalVelocity),
                Mathf.Clamp(player.velocity.y, -_initialMaxDirectionalVelocity, _initialMaxDirectionalVelocity));
            MeshRenderer.material.color = playerDefaultColor;
        }
    }

    private void HandleEnlarge()
    {
        if (!AllowEnlarge || _flattening || _shrinking)
            return;
        
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _enlarging = true;
            maxDirectionalVelocity = 0.75f * _initialMaxDirectionalVelocity;
            player.velocity = new Vector3(Mathf.Clamp(player.velocity.x, -maxDirectionalVelocity, maxDirectionalVelocity),
                Mathf.Clamp(player.velocity.y, -maxDirectionalVelocity, maxDirectionalVelocity));
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            player.transform.localScale = Vector3.Lerp(player.transform.localScale, new Vector3(1.5f, 1.5f, 1), Time.deltaTime);
            player.mass = Mathf.Lerp(player.mass, 2.5f, Time.deltaTime);
            MeshRenderer.material.color = Color.Lerp(MeshRenderer.material.color, playerLargeColor, Time.deltaTime);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _enlarging = false;
            player.transform.localScale = Vector3.one;
            player.mass = 1f;
            maxDirectionalVelocity = _initialMaxDirectionalVelocity;
            MeshRenderer.material.color = playerDefaultColor;
        }
    }
    
    private void HandleFlatten()
    {
        if (!AllowFlatten || _shrinking || _enlarging)
            return;        
        
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            _flattening = true;
        }
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            player.transform.localScale = Vector3.Lerp(player.transform.localScale, new Vector3(4f, 0.25f, 1), Time.deltaTime);
            MeshRenderer.material.color = Color.Lerp(MeshRenderer.material.color, playerFlatColor, Time.deltaTime);
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            _flattening = false;
            player.transform.localScale = Vector3.one;
            MeshRenderer.material.color = playerDefaultColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TargetArea"))
        {
            Points++;
            if (Points <= 15 && Points % 5 == 0)
                unlockSound.Play();
            else
                pickUpSound.Play();
            PointsUpdated?.Invoke(Points);
            TargetReached?.Invoke();
        }
        else
        {
            player.velocity = Vector3.zero;
            MeshRenderer.material.color = playerDeadColor;
            player.useGravity = true;
            GetComponent<BoxCollider>().isTrigger = false;
            gameOverSound.Play();
            StartCoroutine(WaitForGameOverSoundToFinish(() => WallTouched?.Invoke()));
        }
    }

    private IEnumerator WaitForGameOverSoundToFinish(Action whenFinished)
    {
        while (gameOverSound.isPlaying)
        {
            yield return null;
        }

        whenFinished();
    }
}
