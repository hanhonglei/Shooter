using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                hit.transform.gameObject.SendMessage("Hit", Input.mousePosition);
                //hit.transform.gameObject.SendMessage("Hit");
            }

        }
    }
}
