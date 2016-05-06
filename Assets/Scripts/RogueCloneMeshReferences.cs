using UnityEngine;
using System.Collections;

public class RogueCloneMeshReferences : MonoBehaviour 
{
    [SerializeField] private MeshRenderer[] subMeshes = null;

    public void UpdateCloneColors(Color color)
    {
        for (int i = 0; i < subMeshes.Length; i++)
        {
            subMeshes[i].material.color = color;
        }
        transform.GetChild(0).GetComponent<Light>().color = color;
    }
}
