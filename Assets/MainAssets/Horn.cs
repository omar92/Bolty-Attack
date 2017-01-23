using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horn : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        gameObject.name = "Horn";
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.name == "Horn")
        {
            Debug.Log("Collided with horn");
            Rigidbody2D Parent = transform.parent.GetComponent<Rigidbody2D>();
            Parent.GetComponent<CharacterControl>().isControllable = false;
            Parent.AddForce(new Vector2(Parent.transform.up.x, Parent.transform.up.y) * -10);
            StartCoroutine(ResetParentIsControllable(Parent));
        }
    }

    private IEnumerator ResetParentIsControllable(Rigidbody2D Parent)
    {
        yield return new WaitForSeconds(.25f);
        Parent.GetComponent<CharacterControl>().isControllable = true;
    }
}
