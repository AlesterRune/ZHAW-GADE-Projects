using UnityEngine;

public class FixedLevelObjectController : MonoBehaviour
{
    private Vector3 _targetScale = new(3, 3, 3);
    private Vector3 _dampVelocity = Vector3.zero;

    private bool _shrink;

    [SerializeField]
    private float rotationSpeed = 360f;

    [SerializeField]
    private float dampSpeed = 100;

    private void Awake()
    {
        rotationSpeed = Random.value switch
        {
            < 0.40f => -Random.Range(180, 360),
            > 0.60f => Random.Range(180, 360),
            _ => 0
        };

        dampSpeed = Random.Range(100f, 200f);
    }
    
    private void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
        var scale = transform.localScale;

        if (scale.magnitude > (_targetScale.magnitude - 0.05f))
            _shrink = true;
        if (scale.magnitude < Vector3.one.magnitude + 0.05f)
            _shrink = false;
        
        if (!_shrink)
            transform.localScale = Vector3.SmoothDamp(scale, _targetScale, ref _dampVelocity, Time.deltaTime * dampSpeed);
        else
            transform.localScale = Vector3.SmoothDamp(scale, Vector3.one, ref _dampVelocity, Time.deltaTime * dampSpeed);
    }
    
    
}
