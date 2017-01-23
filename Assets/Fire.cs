using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {
    bool aim;
    bool shoot;
    public GameObject Projectile;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            aim = true;
            transform.parent.gameObject.GetComponent<CharacterControl>().isControllable = false;
        }
        if(aim == true)
        {
            this.transform.LookAt(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
            this.transform.Rotate(0, 90, 90);
            GameObject child = transform.GetChild(0).gameObject;
            Debug.Log(child);
            child.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            aim = false;
            transform.parent.gameObject.GetComponent<CharacterControl>().isControllable = true;
            GameObject child = transform.GetChild(0).gameObject;
            Debug.Log(child);
            child.SetActive(false);
            GameObject fired = GameObject.Instantiate(Projectile);
            fired.transform.position = this.transform.GetChild(0).transform.position ;
            fired.transform.rotation = this.transform.rotation;

        }
    }
    
}
