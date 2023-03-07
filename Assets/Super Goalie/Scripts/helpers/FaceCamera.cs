using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform _camTrans;
		
    // Use this for initialization
    private void Start ()
    {
        if (Camera.main != null) _camTrans = Camera.main.transform;
    }
	
    // Update is called once per frame
    private void Update () {
        if(!_camTrans) return;
        this.transform.LookAt(_camTrans.transform.position);
        this.transform.Rotate(new Vector3(0, 180, 0));
    }
}
