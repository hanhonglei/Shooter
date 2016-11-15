using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTutorial : MonoBehaviour {

    public Transform weaponPos = null;
    public GameObject[] weapons;
    public int currWeapon = 0;

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
            weapons[i].SendMessage("SetUsable", i == currWeapon);
            weapons[i].gameObject.SetActive(i == currWeapon);
        }
    }
    protected virtual void Die()
    {

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
}
