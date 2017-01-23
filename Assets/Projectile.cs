using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed;
    public float secondsTodestroy;
	// Use this for initialization
	void Start () {
        StartCoroutine(DestroyMe());

        GetComponent<Rigidbody2D>().velocity = speed * transform.up;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator DestroyMe()
    {
        yield return new WaitForSeconds(secondsTodestroy);
        GameObject.Destroy(this.gameObject);
    }
}
