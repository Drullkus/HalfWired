using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MissileController : MonoBehaviour
{
    [SerializeField] private FirstPersonController _firstPersonController;
    [SerializeField] private float steerRate = 120;
    [SerializeField] private float speed = 20;
    [SerializeField] private float lifetime = 6.5f;
    [SerializeField] private float guidanceDelay = 0.5f;

    private Rigidbody _rb;
    private Vector3 LaserPoint => _firstPersonController.GetLaserPoint();
    private float _launchTime;

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

    private void OnEnable()
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        _launchTime = Time.time;
        StartCoroutine(ScheduleExpire(lifetime));
    }

    private IEnumerator ScheduleExpire(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        Vector3 heading = transform.forward;
        
        if (Time.time - _launchTime > guidanceDelay)
            heading = Vector3.RotateTowards(heading, LaserPoint - _rb.position, steerRate * Mathf.Deg2Rad * Time.fixedDeltaTime, 0f);

        _rb.MoveRotation(Quaternion.LookRotation(heading));
        _rb.linearVelocity = heading * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_firstPersonController.CanHit(collision.gameObject)) return;
        
        gameObject.SetActive(false);
        
        // TODO boom
    }
    
}
