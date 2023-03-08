using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private string[] _animations;

    [SerializeField] private float _activeDuration = 5f;

    [SerializeField] private float _castRadius = 1f;
    [SerializeField] private LayerMask _layers;

    [SerializeField] private AudioClip[] _kickClips;
    [SerializeField] private AudioSource _audioSource;
    
    [SerializeField] private float _kickForceMin = 10f;
    [SerializeField] private float _kickForceMax = 30f;
    
    [System.Serializable]
    private class GoalBounds
    {
        public float xMin;
        public float xMax;
        public float yMin;
        public float yMax;
        public float z;
    }

    [SerializeField] private GoalBounds _goalBounds;

    private RaycastHit _hit;
    
    // Start is called before the first frame update
    private void Start()
    {
        if (!_animator) _animator = GetComponent<Animator>();
        if (!_audioSource) _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (!_animator) _animator = GetComponent<Animator>();
        if (!_audioSource) _audioSource = GetComponent<AudioSource>();
        RandomKick();
        StartCoroutine(DisableAfterTime());
    }

    // Called from animation event
    public void KickBall()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _castRadius, _layers);
        foreach (var collider in colliders)
        {
            Debug.Log(collider.gameObject);
            KickSfx();
            // ball physics here
            BallPhysics ballPhysics = collider.GetComponent<BallPhysics>();
            ApplyBallPhysics(ballPhysics, collider.transform);
        }
    }

    private void ApplyBallPhysics(BallPhysics ballPhysics, Transform ballTransform)
    {
        var xPos = Random.Range(_goalBounds.xMin, _goalBounds.xMax);
        var yPos = Random.Range(_goalBounds.yMin, _goalBounds.yMax);
        
        Vector3 finalPos = new Vector3(xPos,yPos, _goalBounds.z);
        Vector3 dir = finalPos - ballTransform.position;

        var force = Random.Range(_kickForceMin, _kickForceMax);
        ballPhysics.SetValues(dir,force);
    }

    private void KickSfx()
    {
        if(_audioSource || _kickClips.Length == 0) return;
        var selected = Random.Range(0, _kickClips.Length);
        _audioSource.PlayOneShot(_kickClips[selected]);
    }

    private void RandomKick()
    {
        var selected = Random.Range(0, _animations.Length);
        _animator.SetTrigger(_animations[0]);
    }

    private IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(_activeDuration);
        gameObject.SetActive(false);
    }
}

