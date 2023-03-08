using System;
using UnityEngine;

public class BallSfx : MonoBehaviour
{

    [SerializeField] private AudioSource _ballAudio;
    [System.Serializable]
    private class HitSfx
    {
        public string tag;
        public AudioClip sfx;
    }
    [SerializeField] private HitSfx[] _hitSfxes;
    
    // Start is called before the first frame update
    private void Start()
    {
        if (!_ballAudio)
        {
            _ballAudio = GetComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        foreach (var hitsfx in _hitSfxes)
        {
            if (hitsfx.tag != other.gameObject.tag) continue;
            if(!_ballAudio)
            {
                Debug.LogWarning("Add Audio Source to ball to play sounds on collisions");
                return;
            }
            _ballAudio.PlayOneShot(hitsfx.sfx);
        }
    }
}

