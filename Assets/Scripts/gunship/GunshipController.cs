using UnityEngine;

public class GunshipController : MonoBehaviour
{
    
    [SerializeField] private float orbitRadius = 40f;
    [SerializeField] private float cruiseSpeed = 15f;
    [SerializeField] private float evadeSpeed = 40f;
    [SerializeField] private float angularAccel = 40f;
    [SerializeField] private float heightAbovePlayer = 8f;
    [SerializeField] private float heightWobble = 6f;
    [SerializeField] private float wobbleFrequency = 0.3f;
    
    [SerializeField] private Transform player;

    private float _angle, _angularVel, _direction = 1f, _seed;
    
    private void Start()
    {
        _seed = Random.Range(0f, int.MaxValue);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        bool evade = false; // FIXME set true once missile is in flight
        
        float targetVel = (evade ? evadeSpeed : cruiseSpeed) * _direction;
        
        _angularVel = Mathf.MoveTowards(_angularVel, targetVel, angularAccel * Time.deltaTime);
        _angle += _angularVel * Time.deltaTime;
        
        float wobble = (Mathf.PerlinNoise(_seed, Time.time * wobbleFrequency) - 0.5f) * 2f * heightWobble;
        float radians = _angle * Mathf.Deg2Rad;
        transform.position = new Vector3(Mathf.Cos(radians) * orbitRadius, player.position.y + heightAbovePlayer + wobble, Mathf.Sin(radians) * orbitRadius);
        transform.rotation = Quaternion.LookRotation(player.position - transform.position);
    }
}
