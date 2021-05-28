using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class GizmoScript : MonoBehaviour
{
    public BoxCollider2D bCol;
    public Color boxColor = Color.white;
    public float arbitraryHeight = .5f;
    private void OnDrawGizmos()
    {
        Gizmos.color = boxColor;

        Gizmos.DrawWireCube(transform.position, new Vector3(bCol.size.x, bCol.size.y, arbitraryHeight));
    }



}

