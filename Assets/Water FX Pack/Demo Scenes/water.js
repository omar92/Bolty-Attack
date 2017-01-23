#pragma strict

var speed = 1.0;
var Height = 10.0;
private var baseHeight : Vector3[];
var useOriginal : boolean = false;
function Start () {

}



//




function Update () {
    var mesh : Mesh = GetComponent(MeshFilter).mesh;
 
    if (baseHeight == null)
        baseHeight = mesh.vertices;
 
    //
 
    var vertices = new Vector3[baseHeight.Length];
    for (var i=0;i<vertices.Length;i++)
    {
        var vertex = baseHeight[i];
  
        if (useOriginal) {
            vertex.y += Mathf.Sin(Time.time * speed+ baseHeight[i].x + baseHeight[i].y + baseHeight[i].z) * Height;
        } else {
            vertex.y += Mathf.Sin(Time.time * speed+ baseHeight[i].x + baseHeight[i].y) * (Height*.5) + Mathf.Sin(Time.time * speed+ baseHeight[i].z + baseHeight[i].y) * (Height*.5);
        }
  
        vertices[i] = vertex;
    }
    mesh.vertices = vertices;
    mesh.RecalculateNormals();
 
    gameObject.Destroy(GetComponent(MeshCollider));
 
    var collider : MeshCollider = GetComponent(MeshCollider);
    if (collider == null) {
        collider = gameObject.AddComponent(MeshCollider);
        collider.isTrigger = true;
    }
 
}