using UnityEngine;
using System.Collections;

public class SendMessage2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void Hit(Vector3 p)
    {
        Debug.Log("Hit me" + gameObject.name+p);
    }
}
