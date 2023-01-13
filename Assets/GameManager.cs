using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using g3;



public class GameManager : MonoBehaviour
{


    [SerializeField] GameObject subjectGO, targetGO;

    DMesh3 sub_dmesh, tar_dmesh;

    DMeshAABBTree3 targetTree;

    void Start()
    {
        Mesh subMesh = subjectGO.GetComponent<MeshFilter>().mesh;
        Mesh tarMesh = targetGO.GetComponent<MeshFilter>().mesh;

        // next, we have to convert these UNITY meshes to g3 meshes

        sub_dmesh = g3UnityUtils.UnityMeshToDMesh(subMesh);
        tar_dmesh = g3UnityUtils.UnityMeshToDMesh(tarMesh);

        targetTree = new DMeshAABBTree3 (tar_dmesh, true);


        InvokeRepeating(nameof(RunICP), 1f, .1f);
    }

    void RunICP ()
    {
        MeshICP myIcp = new MeshICP(sub_dmesh, targetTree);
        myIcp.MaxIterations = 1;


        myIcp.Solve();

        myIcp.UpdateVertices(sub_dmesh);


        GameObject GO = new GameObject(); // only a transform at the center
        subjectGO.transform.parent.parent = GO.transform;

        GO.transform.localPosition += (Vector3) myIcp.Translation;
        GO.transform.rotation *= (Quaternion) myIcp.Rotation;

        subjectGO.transform.parent.parent = null;
        Destroy(GO);


    }
}
