using UnityEngine;
using System.Collections;
using UnityEditor;

public class PlayerControl : Actor
{
    void Turning()
    {
#if !MOBILE_INPUT
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, 100.0f, LayerMask.GetMask("Floor")))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

            // Set the player's rotation to this new rotation.
            GetComponent<Rigidbody>().MoveRotation(newRotatation);
        }
#else

            Vector3 turnDir = new Vector3(CrossPlatformInputManager.GetAxisRaw("Mouse X") , 0f , CrossPlatformInputManager.GetAxisRaw("Mouse Y"));

            if (turnDir != Vector3.zero)
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = (transform.position + turnDir) - transform.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                playerRigidbody.MoveRotation(newRotatation);
            }
#endif
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!m_Animator.GetBool("Shooting"))
                m_Animator.SetBool("Shooting", true);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter = 0.0f;
            Plane firePlane = new Plane(Vector3.up, weaponPos.position);
            if (firePlane.Raycast(ray, out enter))
            {
                desPos = ray.GetPoint(enter);
            }
            Fire(desPos);
            m_Animator.SetLayerWeight(1, 1.0f);
        }
        else
        {
            m_Animator.SetBool("Shooting", false);
            m_Animator.SetLayerWeight(1, 0.0f);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopFire();
        }
        // 更换武器
        float f = Input.GetAxis("Mouse ScrollWheel");
        if (f > 0)
            NextWeapon(1);
        else if (f < 0)
            NextWeapon(-1);
        Turning();
    }
    protected override void Die()
    {
        gameObject.SetActive(false);
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
	Application.Quit();
#endif
    }
    bool NextWeapon(int next)
    {
        weapons[currWeapon].gameObject.SetActive(false);
        currWeapon = Mathf.Abs(currWeapon + next + weapons.Length) % weapons.Length;
        if (weapons[currWeapon].GetComponent<Weapon>().usable)
        {
            weapons[currWeapon].gameObject.SetActive(true);
            StopFire(); // 交换武器以后，新武器是不射击状态
            return true;
        }
        else
            return NextWeapon(next);
    }
    bool ChangeWeapon(int idx)
    {
        weapons[currWeapon].gameObject.SetActive(false);
        weapons[idx].gameObject.SetActive(true);
        currWeapon = idx;
        //Debug.Log("Changed Weapon");
        return true;
    }
    public void PickupItem(GameObject item)
    {
        Item it = item.GetComponent<Item>();
        if (!it || it.tag != Tags.item)
            return;

        switch (item.GetComponent<Item>().itemType)
        {
            case ItemType.Weapon:
                PickupWeapon(item.GetComponent<Weapon>());
                break;
            case ItemType.Bullet:
                PickupBullet(item.GetComponent<Bullet>());
                break;
            case ItemType.Health:
                health += it.num;
                break;
            default:
                break;
        }
    }
    private void PickupWeapon(Weapon w)
    {
        int idx = 0;
        foreach (GameObject g in weapons)
        {
            if (g.GetComponent<Weapon>().type == w.type)
            {
                g.GetComponent<Weapon>().PickUp(w);
                g.GetComponent<Weapon>().usable = true;
                ChangeWeapon(idx);
                return;
            }
            idx++;
        }
        //Debug.Log("Weapon type error!");
    }
    private void PickupBullet(Bullet b)
    {
        int idx = 0;
        foreach (GameObject g in weapons)
        {
            if (g.GetComponent<Weapon>().type == b.bulletType)
            {
                g.GetComponent<Weapon>().PickUp(b);
                return;
            }
            idx++;
        }
    }
}
