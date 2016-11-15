using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTutorial : MonoBehaviour {
    public GameObject bullet = null;
    public Transform bulletPos = null;
    public float freezeTime = 1f;
    private float lastShootTime = 0f;
    public int num = 10;

    // Use this for initialization
    void Start()
    {
        lastShootTime = freezeTime;
        StopFire();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastShootTime < freezeTime)
        {
            lastShootTime += Time.deltaTime;
        }
    }

    public void Fire(Vector3 target)
    {
        if (lastShootTime < freezeTime)
            return;
        Vector3 dir = target - bulletPos.position;
        Vector3 xyProject = Vector3.ProjectOnPlane(dir, Vector3.up);
        if (num <= 0)
            return;
        Vector3 bulletStartPos = bulletPos.position;
        bulletStartPos.y = 1;
        GameObject g = (GameObject)GameObject.Instantiate(bullet, bulletStartPos, Quaternion.FromToRotation(Vector3.forward, xyProject.normalized));
       num--;
        lastShootTime = 0f;
    }
    public void StopFire()
    {
    }
}
