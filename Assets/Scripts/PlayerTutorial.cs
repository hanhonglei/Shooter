using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTutorial : MonoBehaviour
{

    public Transform weaponPos = null;
    public GameObject[] weapons;
    public int currWeapon = 0;
    protected Vector3 desPos;

    // Use this for initialization
    void Start()
    {
        if (!weaponPos)
            weaponPos = gameObject.transform;
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i] = (GameObject)Instantiate(weapons[i], weaponPos.position, Quaternion.identity);
            if (gameObject.tag == Tags.enemy)
                weapons[i].SendMessage("SetAsEnemy", true);
            weapons[i].transform.SetParent(weaponPos);
            //weapons[i].SendMessage("SetUsable", i == currWeapon);
            weapons[i].gameObject.SetActive(i == currWeapon);
        }
    }
    public void Fire(Vector3 target)
    {
        if (weapons.Length > 0)
        {
            weapons[currWeapon].SendMessage("Fire", target);

        }
    }

    public void StopFire()
    {
        if (weapons.Length > 0)
        {
            weapons[currWeapon].SendMessage("StopFire");

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter = 0.0f;
            Plane firePlane = new Plane(Vector3.up, weaponPos.position);
            if (firePlane.Raycast(ray, out enter))
            {
                desPos = ray.GetPoint(enter);
            }
            Fire(desPos);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopFire();
        }
        // change weapon
        float f = Input.GetAxis("Mouse ScrollWheel");
        if (f > 0)
            NextWeapon(1);
        else if (f < 0)
            NextWeapon(-1);
    }
    protected void Die()
    {
        gameObject.SetActive(false);
        Application.Quit();
    }
    bool NextWeapon(int next)
    {
        weapons[currWeapon].gameObject.SetActive(false);
        currWeapon = Mathf.Abs(currWeapon + next + weapons.Length) % weapons.Length;

        weapons[currWeapon].gameObject.SetActive(true);
        StopFire(); // stop fire when changing to a new weapon
        return true;
    }
    bool ChangeWeapon(int idx)
    {
        weapons[currWeapon].gameObject.SetActive(false);
        weapons[idx].gameObject.SetActive(true);
        currWeapon = idx;
        //Debug.Log("Changed Weapon");
        return true;
    }
}
