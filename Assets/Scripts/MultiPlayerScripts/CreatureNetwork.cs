using UnityEngine;
using System.Collections;
using Photon;
public class CreatureNetwork : PunBehaviour
{
    private Rigidbody2D rb;
    private Vector2 correctPlayerPos;
    private Quaternion correctPlayerRot;
    private Vector2 currentVelocity;
    private float updateTime = 0;
    // Use this for initialization
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// If it is a remote car, interpolates position and rotation
    /// received from network. 
    /// </summary>
    public void FixedUpdate()
    {
        if (!photonView.isMine)
        {
            Vector2 projectedPosition = this.correctPlayerPos + currentVelocity * (Time.time - updateTime);
            transform.position = Vector2.Lerp(transform.position, projectedPosition, Time.deltaTime * 4);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 4);
        }
    }
    /// <summary>
	/// At each synchronization frame, sends/receives player input, position
	/// and rotation data to/from peers/owner.
	/// </summary>
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //We own this car: send the others our input and transform data
            Vector2 pos = transform.position;

            stream.SendNext(pos);
            stream.SendNext(transform.rotation);
            stream.SendNext(rb.velocity);
        }
        else
        {
            //Remote car, receive data
     
            correctPlayerPos = (Vector2)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();
            currentVelocity = (Vector2)stream.ReceiveNext();
            updateTime = Time.time;
        }
    }
}
