using UnityEngine;


public class TimedObjectDeactivator : MonoBehaviour
{
    [SerializeField] private float m_TimeOut = 1.0f;
    [SerializeField] private bool m_DetachChildren = false;


    private void OnEnable()
    {
        Invoke("DestroyNow", m_TimeOut);
    }


    private void DestroyNow()
    {
      gameObject.SetActive(false);
    }
}


