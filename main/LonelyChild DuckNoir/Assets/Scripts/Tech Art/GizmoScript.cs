using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class GizmoScript : MonoBehaviour
{
    //This puts a wirecube and an icon on your gameobjects!
    public BoxCollider2D bCol;
    public Color boxColor = Color.white;
    public float arbitraryHeight = .5f;
    //public
    public enum IconType { Camera, Ghost, Polygon, None };
    public IconType iconType;
    public bool showCube = false;
    private void OnDrawGizmos()
    {
        if (bCol == null && showCube)
        {
            bCol = GetComponent<BoxCollider2D>();
        }
        Gizmos.color = boxColor;
        if (iconType != IconType.None)
        {
            string iconLocation = "..\\Editor\\Iconography\\" + iconType.ToString() + ".png";
            Gizmos.DrawIcon(transform.position, iconLocation, true);
        }
        if (showCube)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(bCol.size.x, bCol.size.y, arbitraryHeight));
        }
    }



}

