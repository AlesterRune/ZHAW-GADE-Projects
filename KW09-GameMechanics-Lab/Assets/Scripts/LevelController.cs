using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private Transform targetArea;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private Transform fixedLevelObjectPrefab;

    [SerializeField]
    private Transform fixedLevelObjectContainer;

    private (int X, int Y) _playerStart;
    private (int X, int Y) _targetAreaPosition;
    private List<(Transform Transform, WorldPosition WorldPosition)> _obstacles;
    private Vector3 _dampVelocity;

    private static bool IsLeft(int x)
    {
        return x <= 25;
    }

    private void Awake()
    {
        _obstacles = new List<(Transform Transform, WorldPosition WorldPosition)>();
        _playerStart = (X: Random.Range(5, 10), Y: Random.Range(3, 12));
        player.transform.position = ToUnityPosition(_playerStart.X, _playerStart.Y, 0.001f);
    }
    
    private void Start()
    {
        SetTargetAreaPosition();
        SpawnObstacles();
        playerController.TargetReached += OnPlayerTargetReached;
        playerController.PointsUpdated += OnPlayerPointsUpdated;
        playerController.WallTouched += OnPlayerWallTouched;
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void OnPlayerPointsUpdated(int points)
    {
        if (points % 5 == 0)
        {
            var x = Random.Range(10, 40);
            var y = Random.Range(0,15);
            var obstacle = Instantiate(fixedLevelObjectPrefab, ToUnityPosition(Random.Range(10, 40), -10),
                Quaternion.Euler(0, Random.Range(0f, 360f), 0), fixedLevelObjectContainer);
            _obstacles.Add((obstacle, new WorldPosition { X = x, Y = y }));
        }
    }

    private void OnPlayerWallTouched()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        foreach (var obstacle in _obstacles)
        {
            var currentPosition = obstacle.Transform.position;
            var targetPosition = ToUnityPosition(obstacle.WorldPosition.X, obstacle.WorldPosition.Y);
            if ((currentPosition - targetPosition).magnitude > 0.01f)
            {
                obstacle.Transform.position = Vector3.Slerp(currentPosition, targetPosition, Time.deltaTime * 0.2f);
            }
        }
    }

    private void SpawnObstacles()
    {
        for (var x = 10; x <= 40; x++)
        {
            for (var y = 0; y < 15; y++)
            {
                if (Random.value <= 0.95f)
                    continue;
                var obstacle = Instantiate(fixedLevelObjectPrefab, ToUnityPosition(x, y),
                    Quaternion.Euler(0, Random.Range(0f, 360f), 0), fixedLevelObjectContainer);
                _obstacles.Add((obstacle, new WorldPosition { X = x, Y = y }));
                x += 4;
                if (x > 40)
                    break;
            }
        }
    }

    private void OnPlayerTargetReached()
    {
        SetTargetAreaPosition();
        foreach (var obstacle in _obstacles)
        {
            obstacle.WorldPosition.X += obstacle.WorldPosition.X > 25 ? -Random.Range(5, 15) : Random.Range(5, 15);
            obstacle.WorldPosition.Y += obstacle.WorldPosition.Y > 7.5 ? -Random.Range(4, 8) : Random.Range(4, 8);
        }
    }

    private void SetTargetAreaPosition()
    {
        _targetAreaPosition = (X: 0, Y: 0);
        Debug.Log(ToLevelCoordinate(player.transform.position));
        if (IsLeft(ToLevelCoordinate(player.transform.position).X))
        {
            _targetAreaPosition.X += Random.Range(40, 45);
        }
        else
        {
            _targetAreaPosition.X += Random.Range(5, 10);
        }

        _targetAreaPosition.Y += Random.Range(3, 12);
        targetArea.position = ToUnityPosition(_targetAreaPosition.X, _targetAreaPosition.Y);
    }

    private (int X, int Y) ToLevelCoordinate(Vector3 position)
    {
        return (Mathf.FloorToInt(position.x) + 25, Mathf.FloorToInt(position.y) + 5);
    }
    
    private Vector3 ToUnityPosition(float x, float y, float z = 0)
    {
        return new Vector3(x-25, y-5, z);
    }

    private record WorldPosition
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
