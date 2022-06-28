using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.TerrainTools;


[ExecuteAlways]
public class Component_BezierCurve : MonoBehaviour, IMiniGolf
{
    //ControlPoints
    [SerializeField]
    private Transform PS;
    [SerializeField]
    private Transform PE;
    [SerializeField]
    private Transform HS;
    [SerializeField]
    private Transform HE;





    private Vector3[] Dots;

    [Range(0, 1)]
    public float Step;
    [Range(0, 100)]
    public float StreamStr;

    [SerializeField]
    private LineRenderer LineRenderer;

    bool bReady = false;


    #region MeshGen
    [Range(0.2f, 5.0f)]
    public float StreamWidth;
    [Range(0.2f, 5.0f)]
    public float StreamThickness;

    [SerializeField]
    MeshFilter meshFilter;
    [SerializeField]
    MeshRenderer meshRenderer;
    [SerializeField]
    Mesh mesh;
    [SerializeField]
    MeshCollider Mcollider;

    #endregion


    private void Awake()
    {
        UpdateLine();
        CreateStreamMesh();
        UpdateMesh();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        UpdateLine();
        UpdateMesh();
    }


    private Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, t);
    }

    private Vector3 CubicLerp(Vector3 a,Vector3 b,Vector3 c,Vector3 d,float t) 
    {
        Vector3 ab_bc = QuadraticLerp(a,b,c,t);
        Vector3 bc_cd = QuadraticLerp(b,c,d,t); 

        return Vector3.Lerp(ab_bc, bc_cd, t);
    }

    private Vector3 GetClosestDotNormal(Vector3 Normal) 
    {
        if (!bReady)
        {
            return new Vector3(0,0,0);
        }

        float[] DistanceField = new float[Dots.Length];

        int DFindex = 0;

        int ClosestIndex;

        // GetDistanceField
        foreach (var dot in Dots) 
        {
            DistanceField[DFindex] = Vector3.Distance(dot, Normal);
         
            DFindex++; 
        }

        // ResetIteration
        DFindex=0;

        // FindClosestDot Index
        foreach (var dot in Dots)
        {
            if (Mathf.Min(DistanceField) == Vector3.Distance(dot, Normal))
            {
                break;
            }
            DFindex++;
        }
        // GetClosestIndex
        ClosestIndex = DFindex;

        // SetMin value to posinfinity and reset iteration
        //DistanceField[ClosestIndex] = float.PositiveInfinity;
        //DFindex = 0;

        //// Find SecondClosestDot
        //foreach (var dot in Dots)
        //{
        //    if (DistanceField[DFindex] == Vector3.Distance(dot, Normal))
        //    {
        //        break;
        //    }
        //    DFindex++;
        //}
        //// Get SecondClosestIndex
        //SecondClosestIndex = DFindex;


        //Debug.LogWarning("BALL IS DRAGGED" + (Dots[Mathf.Min(ClosestIndex, SecondClosestIndex)] - Dots[Mathf.Max(ClosestIndex, SecondClosestIndex)]).normalized.ToString());


        if (ClosestIndex == Dots.Length - 1)
        {
            Debug.LogWarning(((Dots[ClosestIndex - 1] - Dots[ClosestIndex]).normalized).ToString());
            return (Dots[ClosestIndex - 1] - Dots[ClosestIndex]).normalized;
        }

        Debug.LogWarning(((Dots[ClosestIndex] - Dots[ClosestIndex + 1]).normalized).ToString());


        return (Dots[ClosestIndex] - Dots[ClosestIndex+1]).normalized;


        //return Dots[Mathf.Min(ClosestIndex, SecondClosestIndex)] - Dots[Mathf.Max(ClosestIndex, SecondClosestIndex)].normalized;
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IMiniGolf MiniGolf))
        {
           MiniGolf.BallDragged( GetClosestDotNormal(collision.transform.position), StreamStr);

        }

    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.TryGetComponent(out IMiniGolf MiniGolf))
        {
            MiniGolf.BallDragged(GetClosestDotNormal(collision.transform.position), StreamStr);

        }
    }



    private Vector3 GetTangentAtPos(int Dot) 
    {
        if (Dot<Dots.Length-1 && Dot >= 0 )
        {
            return (Dots[Dot] - Dots[Dot+1]).normalized;
        }

        return new Vector3(0,0,0);

    }


    void UpdateLine() 
    {
        Dots = new Vector3[(int)(1 / Step) + 2];

        //Debug.LogError(Dots.Length.ToString());

        float j = 0;
        for (int i = 0; i < Dots.Length; i++)
        {
            if (j > 1)
            {
                j = 1;
            }

            Dots[i] = CubicLerp(PS.position, HS.position, HE.position, PE.position, j);
            //Debug.LogError(j.ToString());
            j += Step;

        }


        LineRenderer.positionCount = Dots.Length;
        for (int i = 0; i < Dots.Length; i++)
        {
            LineRenderer.SetPosition(i, Dots[i]);
            
        }
        bReady = true;
    }


    /// <summary>
    /// DEPRECATED
    /// </summary>
    void CreateStreamMesh()
    {
        #region MeshRegion
        /*
 * 

Vector3[] verts = new Vector3[(Dots.Length+2) * 8];
Vector2[] uvs = new Vector2[verts.Length];
Vector3[] normals = new Vector3[verts.Length];

int numTris = 2 * ((Dots.Length+2) - 1);
int[] roadTriangles = new int[numTris * 3];
int[] underRoadTriangles = new int[numTris * 3];
int[] sideOfRoadTriangles = new int[numTris * 2 * 3];

int vertIndex = 0;
int triIndex = 0;

// Vertices for the top of the road are layed out:
// 0  1
// 8  9
// and so on... So the triangle map 0,8,1 for example, defines a triangle from top left to bottom left to bottom right.
int[] triangleMap = { 0, 8, 1, 1, 8, 9 };
int[] sidesTriangleMap = { 4, 6, 14, 12, 4, 14, 5, 15, 7, 13, 15, 5 };


for (int i = 0; i < Dots.Length; i++)
{

    Vector3 localRight = Vector3.Cross(-transform.up, GetTangentAtPos(i));

    // Find position to left and right of current path vertex
    Vector3 vertSideA = Dots[i] - localRight * Mathf.Abs(StreamWidth);
    Vector3 vertSideB = Dots[i] + localRight * Mathf.Abs(StreamWidth);

    // Add top of road vertices
    verts[vertIndex + 0] = vertSideA;
    verts[vertIndex + 1] = vertSideB;
    // Add bottom of road vertices
    verts[vertIndex + 2] = vertSideA - -transform.up * StreamThickness;
    verts[vertIndex + 3] = vertSideB - -transform.up * StreamThickness;

    // Duplicate vertices to get flat shading for sides of road
    verts[vertIndex + 4] = verts[vertIndex + 0];
    verts[vertIndex + 5] = verts[vertIndex + 1];
    verts[vertIndex + 6] = verts[vertIndex + 2];
    verts[vertIndex + 7] = verts[vertIndex + 3];

    // Set uv on y axis to path time (0 at start of path, up to 1 at end of path)
    uvs[vertIndex + 0] = new Vector2(0, i / Dots.Length);
    uvs[vertIndex + 1] = new Vector2(1, i / Dots.Length);

    // Top of road normals
    normals[vertIndex + 0] = -transform.up;
    normals[vertIndex + 1] = -transform.up;
    // Bottom of road normals
    normals[vertIndex + 2] = transform.up;
    normals[vertIndex + 3] = transform.up;
    // Sides of road normals
    normals[vertIndex + 4] = -localRight;
    normals[vertIndex + 5] = localRight;
    normals[vertIndex + 6] = -localRight;
    normals[vertIndex + 7] = localRight;

    // Set triangle indices
    if (i < Dots.Length - 1)
    {
        for (int j = 0; j < triangleMap.Length; j++)
        {
            roadTriangles[triIndex + j] = (vertIndex + triangleMap[j]) % verts.Length;
            // reverse triangle map for under road so that triangles wind the other way and are visible from underneath
            underRoadTriangles[triIndex + j] = (vertIndex + triangleMap[triangleMap.Length - 1 - j] + 2) % verts.Length;
        }
        for (int j = 0; j < sidesTriangleMap.Length; j++)
        {
            sideOfRoadTriangles[triIndex * 2 + j] = (vertIndex + sidesTriangleMap[j]) % verts.Length;
        }

    }

    vertIndex += 8;
    triIndex += 6;
}

mesh.Clear();
mesh.vertices = verts;
mesh.uv = uvs;
mesh.normals = normals;
mesh.subMeshCount = 3;
mesh.SetTriangles(roadTriangles, 0);
mesh.SetTriangles(underRoadTriangles, 1);
mesh.SetTriangles(sideOfRoadTriangles, 2);
mesh.RecalculateBounds();

meshFilter.sharedMesh = mesh;

 */
        #endregion


        mesh = new Mesh();

        mesh.RecalculateBounds();
        meshFilter.sharedMesh = mesh;
        Mcollider.sharedMesh = mesh;

    }

    /// <summary>
    /// DEPRECATED
    /// </summary>
    private void UpdateMesh()
    {
        if (mesh==null)
        {
            return;
        }

        mesh.Clear();

        LineRenderer.BakeMesh(mesh);
        mesh.Optimize();
        mesh.RecalculateNormals();

        mesh.RecalculateBounds();
        meshFilter.sharedMesh = mesh;

        
        Mcollider.sharedMesh = mesh;


    }

}
