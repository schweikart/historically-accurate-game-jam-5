using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;                
    public float edgeDistanceThreshold;     
    public int edgeResolveIterations;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    public Material visibleTargetMaterial;
    public Material notVisibleTargetMaterial;

    [Range(4,360)]
    public int resolution = 360;



    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    

    void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        StartCoroutine("FindTargetsWithDelay", 0.2f);
    }

    void LateUpdate()
    {
        DrawFieldOfView();
        UpdateVisibleTargets();
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }


    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            float dstToTarget = Vector3.Distance(transform.position, target.position);
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (!Physics.Raycast (transform.position, dirToTarget, dstToTarget, obstacleMask))
            {
                visibleTargets.Add(target);
            } 
        }
    }


    void UpdateVisibleTargets()
    {
        foreach (Transform visibleTarget in visibleTargets)
        {
            //call to target
        }
   
    }


    void DrawFieldOfView()
    {
        var viewPoints = buildViewPoints();

        var meshInfo = buildMeshInfo(viewPoints);


        viewMesh.Clear();
        viewMesh.vertices = meshInfo.vertices;
        viewMesh.triangles = meshInfo.triangles;
        viewMesh.RecalculateNormals();
    }

    List<Vector3> buildViewPoints()
    {
        var viewPoints = new List<Vector3>();
        var oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= resolution; i++)
        {
            var newViewCast = ViewCast((i*360)/resolution);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDistanceThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero) viewPoints.Add(edge.pointA);
                    if (edge.pointB != Vector3.zero) viewPoints.Add(edge.pointB);
                }
            }
            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        return viewPoints;
    }

    MeshInfo buildMeshInfo(List<Vector3> viewPoints)
    {
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        return new MeshInfo(vertices, triangles);
    }


    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDistanceThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromGlobalAngle(globalAngle);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        } 
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }



    public Vector3 DirFromGlobalAngle(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }



    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

    public struct MeshInfo
    {
        public Vector3[] vertices;
        public int[] triangles;

        public MeshInfo(Vector3[] _vertices, int[] _triangles)
        {
            vertices = _vertices;
            triangles = _triangles;
        }
    }

}
