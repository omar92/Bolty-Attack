using UnityEngine;
using System.Collections;

public class TestMeshMovement : MonoBehaviour
{

    public float speed = 0.2f;
    public float waveSize = 0.1f;

    Vector3[] m_BaseHeight;
    Mesh m_WaterPlane;
    float prevTime = 0f;

    void Start()
    {
        m_WaterPlane = GetComponent<MeshFilter>().mesh;
    }

    void Update()
    {
        Debug.Log(Time.time-prevTime);
        if  (Time.time - prevTime > speed)
        {
            CalculateWaves();
            prevTime = Time.time;
        }
    }

    void CalculateWaves()
    {

        if (m_BaseHeight == null)
            m_BaseHeight = m_WaterPlane.vertices;

        Vector3[] WaterVertices = new Vector3[m_BaseHeight.Length];
        for (int i = 0; i < WaterVertices.Length; i++)
        {
            Vector3 Vertex = m_BaseHeight[i];
            Vertex.y+= Random.Range(-waveSize, waveSize);
            WaterVertices[i] = Vertex;
        }
        m_WaterPlane.vertices = WaterVertices;
        m_WaterPlane.RecalculateNormals();
    }
}