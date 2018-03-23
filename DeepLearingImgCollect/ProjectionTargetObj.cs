using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionTargetObj : MonoBehaviour {

    private Mesh mesh;
    Vector3[] vectors;
    public Camera cam;
    // Use this for initialization
    private List<Vector3> allApexPos = new List<Vector3>();
    private List<Vector3> allProApexPos = new List<Vector3>();


    private Vector3 minZPos;
    //private Vector3[] apexPos;

    private bool getMin = true;
    bool start = true;


    public static  float objProjectHeight;
    public static  float objProjectWidth;

    public static Vector3 centerPosAfterProj;


    float maxx;
    float maxy ;
    float minx ;
    float miny ;

    Quaternion F;
    Quaternion Fp;

    public GameObject pppp;
    public GameObject target;

    void Start()
    {
        mesh = target.GetComponent<MeshFilter>().mesh;
        vectors = mesh.vertices;
        //apexPos = new Vector3[mesh.vertices.Length];
        cam = cam.GetComponent<Camera>();

        // angle of camera = heading of car
        Vector3 CameraDirection;
        CameraDirection = cam.ScreenToWorldPoint(
            new Vector3(Screen.width / 2, Screen.height / 2, cam.nearClipPlane)
            );
        print("CameraDirection  " + CameraDirection);
        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        g.transform.localScale = new Vector3(.1f, .1f, .1f);
        g.transform.position = 10*CameraDirection;

        Vector3 newZ = new Vector3(0, 0, 1);

        F = Quaternion.FromToRotation(CameraDirection, newZ);
        Fp = Quaternion.Inverse(F);

        print("ppp in original space: " + pppp.transform.position);
        //Vector3 Object = new Vector3(34, 87, 9);
        Vector3 NewObject = F * pppp.transform.position;
        print("ppp in new space: " + NewObject);
        NewObject = Quaternion.Inverse( F) * NewObject;
        print("ppp back to space: " + NewObject);

        for (int i = 0; i < vectors.Length; i++)
        {          
            Vector3 v = this.transform.TransformPoint(vectors[i]);
            allApexPos.Add(F * v);          
        }
        minZPos = FindMinZ(allApexPos);
        for (int i = 0; i < allApexPos.Count; i++)
        {
            Vector3 v = ProXYZ(allApexPos[i], minZPos);
            allProApexPos.Add(v);
        }
         maxx = MaxX(allProApexPos);
         maxy = MaxY(allProApexPos);
         minx = MinX(allProApexPos);
         miny = MinY(allProApexPos);

        objProjectHeight = maxy - miny;
        objProjectWidth = maxx - minx;

        centerPosAfterProj = Fp* (new Vector3((maxx + minx) / 2, (maxy + miny) / 2, minZPos.z));

        Debug.Log("the center Pos After Projection :" + centerPosAfterProj);

        //center = ProXYZ(this.transform.position, minZPos);
    }
    public static Vector3 center;


    void Update()
    {

        if (start)
        {
            Vector3 maxx = MAXX(allProApexPos);
            Debug.Log("max x Position :+++++++++++++++++++++" + maxx);

            
            Vector3 maxy = MAXY(allProApexPos);
            Debug.Log("max y Position :+++++++++++++++++++++" + maxy);
            Vector3 minx = MINX(allProApexPos);
            Vector3 miny = MINY(allProApexPos);




            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            g.transform.localScale = new Vector3(.1f, 12f, .1f);
            g.transform.rotation = Fp * g.transform.rotation;
            g.transform.position = Fp * maxx;
            

            GameObject g2 = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            g2.transform.localScale = new Vector3(.1f, 12f, .1f);
            g2.transform.rotation = Fp * g2.transform.rotation;
            g2.transform.position = Fp * minx;

            GameObject g1 = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            g1.transform.localScale = new Vector3(12, 0.1f, 0.1f);
            g1.transform.rotation = Fp * g1.transform.rotation;
            g1.transform.position = Fp * maxy;

            GameObject g3 = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            g3.transform.localScale = new Vector3(12, 0.1f, 0.1f);
            g3.transform.rotation = Fp * g3.transform.rotation;
            g3.transform.position = Fp * miny;


            /*
            for (int i = 0; i < vectors.Length; i++)
            {
                Vector3 v = ProXYZ(allApexPos[i], minZPos);
                GameObject g5 = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                g5.transform.localScale = new Vector3(.1f, .1f, .1f);
                g5.transform.position = v;
            }
            */
            Debug.Log("max x :" + maxx + " max y :0" + maxy + "min x:" + minx + "MIN y: " + miny);
            Debug.Log("Height ---:" + objProjectHeight + "Width ======:" + objProjectWidth);

            start = false;
        }



        /*
       for (int i = 0; i < vectors.Length; i++)
          {              
             // Debug.Log("the Apex World Position----------- :"+this.transform.TransformPoint(vectors[i]));
               Vector3 v = this.transform.TransformPoint(vectors[i]);
               allApexPos.Add(v);
             // Ray ray = cam.ScreenPointToRay(v);
             //  Debug.DrawLine(ray.origin, v * 20, Color.red);
           }

       //get the min Z.
       if (allApexPos.Count > 0)
       {
           if (getMin)
           {
               minZPos = FindMinZ(allApexPos);              
               getMin =false;
           }
       }

       if (start)
       {
           for (int i =0; i < allApexPos.Count; i++)
           {
               Vector3 v = ProXYZ(allApexPos[i], minZPos);
               allProApexPos.Add(v);
           }
           float maxx = MaxX(allProApexPos);
           float maxy = MaxY(allProApexPos);
           float minx = MinX(allProApexPos);
           float miny = MinY(allProApexPos);

           objProjectHeight = maxy - miny;
           objProjectWidth = maxx - minx;
           Debug.Log("max x :" + maxx + " max y :0" + maxy + "min x:" + minx + "MIN y: " + miny);
           for (int i = 0; i < vectors.Length; i++)
           {
               Vector3 v = ProXYZ(allApexPos[i], minZPos);
              // GameObject g = GameObject.CreatePrimitive(PrimitiveType.Capsule);
              // g.transform.localScale = new Vector3(.1f, .1f, .1f);
             //  g.transform.position = v;
              // Debug.Log("the Pos projection after Calculate :-------" + v);
           }
           start = false;
       }
         */   
    }



    public Vector3 ProXYZ(Vector3 apex , Vector3 apexMinZ)
    {
        float scaleZ = apexMinZ.z / apex.z ;
        Vector3 proApex = new Vector3();
        proApex.x = apex.x * scaleZ;
        proApex.y = apex.y * scaleZ;
        proApex.z = apexMinZ.z;
        return proApex;
    }

    public Vector3  FindMinZ(List<Vector3> apexPos)
    {
        Vector3 minZ = apexPos[0];
        for (int i = 1; i < apexPos.Count;)
        {
            if (apexPos[i].z < minZ.z)
            {
                minZ = apexPos[i];              
            }
            i++;
        }
        return minZ;
    }


    //---test
    public Vector3 MAXX(List<Vector3> posApexPos)
    {
        Vector3 max_x = posApexPos[0];
        for (int i = 1; i < posApexPos.Count;)
        {
            if (posApexPos[i].x > max_x.x)
            {
                max_x = posApexPos[i];
            }
            i++;
        }
        return max_x;
    }

    public Vector3 MAXY(List<Vector3> posApexPos)
    {
        Vector3 max_Y = posApexPos[0];
        for (int i = 1; i < posApexPos.Count;)
        {
            if (posApexPos[i].y > max_Y.y)
            {
                max_Y = posApexPos[i];
            }
            i++;
        }
        return max_Y;
    }

    public Vector3 MINX(List<Vector3> posApexPos)
    {
        Vector3  min_x = posApexPos[0];
        for (int i = 1; i < posApexPos.Count;)
        {
            if (posApexPos[i].x <= min_x.x)
            {
                min_x = posApexPos[i];
            }
            i++;
        }
        return min_x;
    }

    public Vector3 MINY(List<Vector3> posApexPos)
    {
        Vector3 min_y = posApexPos[0];
        for (int i = 1; i < posApexPos.Count;)
        {
            if (posApexPos[i].y <= min_y.y)
            {
                min_y = posApexPos[i];
            }
            i++;
        }
        return min_y;
    }

    //=================================
    public float MaxX(List<Vector3> posApexPos)
    {
        float max_x = posApexPos[0].x;
        for (int i = 1; i < posApexPos.Count; )
        {
            if (posApexPos[i].x > max_x)
            {
                max_x = posApexPos[i].x;
            }
            i++;
        }
        return max_x;
    }

    public float MinX(List<Vector3> posApexPos)
    {
        float min_x = posApexPos[0].x;
        for (int i = 1; i < posApexPos.Count; )
        {
            if (posApexPos[i].x <= min_x)
            {
                min_x = posApexPos[i].x;
            }
            i++;
        }
        return min_x;
    }


    public float MaxY(List<Vector3> posApexPos)
    {
        float max_y = posApexPos[0].y;
        for (int i = 1; i < posApexPos.Count; )
        {
            if (posApexPos[i].y > max_y)
            {
                max_y = posApexPos[i].y;
            }
            i++;
        }
        return max_y;
    }

    public float MinY(List<Vector3> posApexPos)
    {
        float min_y = posApexPos[0].y;
        for (int i = 1; i < posApexPos.Count; i++)
        {
            if (posApexPos[i].y <= min_y)
            {
                min_y = posApexPos[i].y;
            }
            i++;
        }
        return min_y;
    }

}


