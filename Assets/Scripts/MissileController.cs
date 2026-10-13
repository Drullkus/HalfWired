using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MissileController : MonoBehaviour
{
    [SerializeField] private FirstPersonController _firstPersonController;
    [SerializeField] private float steerRate = 100;
    [SerializeField] private float speed = 40;

    private Rigidbody _rb;
    private Vector3 LaserPoint => _firstPersonController.GetLaserPoint();

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (_firstPersonController == null)
        {
            _firstPersonController = GameObject.FindWithTag("Player").GetComponent<FirstPersonController>();
        }
    }

    private void FixedUpdate()
    {
        Vector3 heading = transform.forward;
        heading = Vector3.RotateTowards(heading, LaserPoint - _rb.position, steerRate * Mathf.Deg2Rad, 0f);
        _rb.MoveRotation(Quaternion.LookRotation(heading));
        _rb.linearVelocity = heading * speed;
    }
    
}
