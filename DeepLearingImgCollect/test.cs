using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    private Mesh mesh;
    bool isStart = true;

    bool isChange = true;
    // Use this for initialization


    private Vector3[] apexPos;
    void Start()
    {
        mesh = this.GetComponent<MeshFilter>().mesh;
        apexPos = new Vector3[mesh.vertices.Length];
    }

    void Update()
    {
        Vector3[] vectors = mesh.vertices;
        Vector3[] results_x = new Vector3[vectors.Length];
        Vector3[] results_y = new Vector3[vectors.Length];
        Vector3[] results_z = new Vector3[vectors.Length];


        if (isStart)
        {
            for (int i = 0; i < vectors.Length; i++)
            {
                Debug.Log("======================== :" + vectors[i]);
                GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
                g.transform.parent = this.gameObject.transform;
                g.transform.localPosition = vectors[i];
                Debug.Log("world Position :" + g.transform.position);
                g.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            }
            isStart = false;
        }

        /*
        if (isChange)
        {
            //first ， calculate the X-Axis Change
            for (int i = 0; i < vectors.Length; i++)
            {
                results_x[i] = DoTwistX(vectors[i],this.transform.rotation.x);
            }       
            mesh.vertices = results_x;

            //second , the Y-Axis Change
            for (int i = 0; i < vectors.Length; i++)
            {
                results_y[i] = DoTwistY(mesh.vertices[i], this.transform.rotation.y);
            }
            mesh.vertices = results_y;


            //last , calculate the Z-Axis Change
            for (int i = 0; i < vectors.Length; i++)
            {
                results_z[i] = DoTwistY(mesh.vertices[i], this.transform.rotation.z);
            }
            mesh.vertices = results_z;

            //print 
            /*
            for (int i = 0; i < vectors.Length; i++)
            {
                Debug.Log("mesh vectors ========================------------ :" + mesh.vertices[i]);
            }
         

            for (int i =0;i<vectors.Length; i++)
            {
                Vector3 v = GetApexPos(this.gameObject, mesh.vertices[i]);
                GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
                g.transform.position = v;
                g.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                Debug.Log("V --------------:" + v);
            }

            isChange = false;
        }
    */
    }

    Vector3 GetApexPos(GameObject obj, Vector3 meshVertice)
    {
        Vector3 parentObjSize = new Vector3(1, 1, 1);
        Vector3 apexPos = Vector3.zero;
        apexPos.x = obj.transform.position.x + parentObjSize.x * meshVertice.x;
        apexPos.y = obj.transform.position.y + parentObjSize.y * meshVertice.y;
        apexPos.z = obj.transform.position.z + parentObjSize.z * meshVertice.z;
        return apexPos;
    }


    /*
    Vector3 DoTwistX(Vector3 Position, float angle)
    {
        Vector3 result = Vector3.zero;
        float CosAngle = Mathf.Cos(angle);
        float SinAngle = Mathf.Sin(angle);
        result.x = Position.x;
        result.y = Position.y * CosAngle + SinAngle * Position.z;
        result.z = -Position.y * SinAngle + CosAngle * Position.z;
        return result;
    }

    Vector3 DoTwistY(Vector3 Position, float angle)
    {
        Vector3 result = Vector3.zero;
        float CosAngle = Mathf.Cos(angle);
        float SinAngle = Mathf.Sin(angle);
        result.x = Position.x * CosAngle - SinAngle * Position.z;
        result.y = Position.y;
        result.z = Position.x * SinAngle + CosAngle * Position.z;
        return result;
    }


    Vector3 DoTwistZ(Vector3 Position, float angle)
    {
        Vector3 result = Vector3.zero;
        float CosAngle = Mathf.Cos(angle);
        float SinAngle = Mathf.Sin(angle);
        result.x = Position.x * CosAngle + SinAngle * Position.y;
        result.y = -Position.x * SinAngle + CosAngle * Position.y;
        result.z = Position.z;
        return result;
    }
    */
}
