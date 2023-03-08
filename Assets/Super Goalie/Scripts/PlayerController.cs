using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject _arrow;
        [SerializeField] private GameObject _ball;
        [SerializeField] private float _waitTime = 10f;
        [SerializeField] private float _arrowRotationSpeed = 5f;
        
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _audioClip;

        private int _currentSelected = 0;
        
        [System.Serializable]
        private class ActivePlayers
        {
            public Transform player;
            public Transform ballPos;
            public Transform playerPos;
        }
        [SerializeField] private ActivePlayers[] _activePlayers;
        
        // Start is called before the first frame update
        private void Start()
        {
            PoolSystem.Instance.InitPool(_ball, 15);
            StartCoroutine(SelectPlayer());
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.IsEnded())
            {
                foreach (var activePlayer in _activePlayers)
                {
                    activePlayer.player.gameObject.SetActive(false);
                }
                return;
            }
            HandleArrowRotation(_activePlayers[_currentSelected].playerPos.transform);
        }

        private void HandleArrowRotation(Transform playerTransform)
        {
            if(!GameManager.Instance.HasStarted()) return;
            
            var dir = playerTransform.position - _arrow.transform.position;
            var lookRot = Quaternion.LookRotation(dir);
            lookRot.x = 0;
            lookRot.z = 0;
            _arrow.transform.rotation = Quaternion.Lerp(_arrow.transform.rotation,lookRot, Time.deltaTime * _arrowRotationSpeed);
        }

        private IEnumerator SelectPlayer()
        {
            while (!GameManager.Instance.IsEnded())
            {
                yield return new WaitForSeconds(GameManager.Instance.NextKickTime());
                var selected = Random.Range(0, _activePlayers.Length);
                while (selected == _currentSelected)
                {
                    selected = Random.Range(0, _activePlayers.Length);
                }

                _currentSelected = selected;
                _activePlayers[selected].player.transform.position = _activePlayers[selected].playerPos.transform.position;
                _activePlayers[selected].player.transform.rotation = _activePlayers[selected].playerPos.transform.rotation;
                _activePlayers[selected].player.gameObject.SetActive(true);
                var ball = PoolSystem.Instance.GetInstance<GameObject>(_ball);
                ball.transform.position = _activePlayers[selected].ballPos.position;
                _audioSource.PlayOneShot(_audioClip);
                //_activePlayers[selected].player = _activePlayers[selected].playerPos;
            }
        }
    }