using System;
using UnityEngine;
public class BallPhysics : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _forceAmount = 10f;
    [SerializeField] private float _torqueAmount = 10f;
    [SerializeField] private Vector3 _direction;

    [SerializeField] private string[] _hitTags;
    [SerializeField] private string _playerTag;
    
    private bool _scoreAwarded = false;

    private bool _startPhysics = false;

    private bool _disableScript = false;
    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        ResetValues();
    }

    private void ResetValues()
    {
        _scoreAwarded = false;
        _startPhysics = false;
        _disableScript = false;
        _rigidbody.ResetInertiaTensor();
        _rigidbody.ResetCenterOfMass();
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(_disableScript) return;

        if (_startPhysics)
        {
            _rigidbody.AddForce(_direction * _forceAmount, ForceMode.Impulse);
            _rigidbody.AddTorque(Vector3.up * _torqueAmount);
        }
    }

    public void SetValues(Vector3 dir, float force)
    {
        _direction = dir;
        _forceAmount = force;
        _startPhysics = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(_playerTag))
        {
            if(_scoreAwarded) return;
            _rigidbody.AddForce(-transform.forward * _forceAmount, ForceMode.Impulse);

            var hitScore = other.gameObject.GetComponent<HitScore>();
            if(!hitScore) return;
            _startPhysics = false;
            _disableScript = true;
            if (!GameScores.Instance)
            {
                Debug.LogWarning("Scores will not be recorded because there is no GameScore Script in the Scene");
                
                return;
            }
            GameScores.Instance.AddScore(hitScore.GetScorePoint());
            _scoreAwarded = true;
            return;
        }
        
        // Detect hit - set _startPhysics = false and stop moving to _direction
        foreach (var tag in _hitTags)
        {
            if (!other.gameObject.CompareTag(tag)) continue;
            _startPhysics = false;
            _disableScript = true;
        }
    }
}

