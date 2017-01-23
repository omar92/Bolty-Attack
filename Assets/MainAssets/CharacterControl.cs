using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {
    public float NormalSpeed=1;
    public float RunningSpeed=2;
    public float staminaTime=1;
    public float restingTime = 5;
    float currentSpeed;
    internal bool isControllable =true;
    private bool allowChangeSpeed = true;
    // Use this for initialization
    void Start () {
         currentSpeed = NormalSpeed;
        this.GetComponent<GhostSprites>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {

      

        RaycastHit r;
        Physics.Raycast(new Ray(this.transform.position,new Vector3(50,0,0)),out r);
        Debug.DrawRay(r.point, this.transform.position);
        
        Vector3 target = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);

        if (isControllable && ((target - transform.position).magnitude>0.1|| (target - transform.position).magnitude < -0.1))
        {
            try
            {
                this.transform.LookAt(target);
                this.transform.Rotate(0, 90, 90);
                GetComponent<Rigidbody2D>().velocity = (new Vector2(target.x - transform.position.x, target.y - transform.position.y) * currentSpeed);
                Debug.Log(transform.GetChild(0).GetComponent<ParticleSystem>());
                transform.GetChild(0).GetComponent<ParticleSystem>().Emit(10);
            }
            catch (Exception)
            {

            }

        }

        if (Input.GetMouseButtonDown(0) && allowChangeSpeed)
        {
            StartSprint();
        }
        if (Input.GetMouseButtonUp (0))
        {
            StopSprint();
        }
    }
    private GhostSprites GhostSpritesinist;
    private void StartSprint()
    {
        currentSpeed = RunningSpeed;
        this.GetComponent<GhostSprites>().enabled = true;

        allowChangeSpeed = false;
        StartCoroutine(RunActionAfterDelay(delegate () {
            StopSprint();
        }, staminaTime));
    }
    private void StopSprint()
    {
        currentSpeed = NormalSpeed;
        this.GetComponent<GhostSprites>().enabled = false;

        
        var trash = GameObject.Find  (this.name+" - GhostSprite");
        while(trash != null)
        {
            trash.name = trash.name + "setToDestroy";
            Destroy(trash);
            trash = GameObject.Find(this.name + " - GhostSprite");
        }

        StartCoroutine(RunActionAfterDelay(delegate () {
            allowChangeSpeed = true; ;
        }, restingTime));
    }
    private IEnumerator RunActionAfterDelay(Action callBack, float delay_s)
    {
        yield return new WaitForSeconds(delay_s);
        callBack();
    }

    private void OnMouseDown()
    {
      //  Debug.Log("mouse down");
    }
}
