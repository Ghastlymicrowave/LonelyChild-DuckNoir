using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneRandomMat : MonoBehaviour
{
    [SerializeField] Material[] materials;
    void Start(){
        MeshRenderer rend = GetComponent<MeshRenderer>();
        rend.material = materials[Random.Range(0,materials.Length)];
    }
}
