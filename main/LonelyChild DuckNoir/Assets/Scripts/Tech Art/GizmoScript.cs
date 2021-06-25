using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class GizmoScript : MonoBehaviour
{
    //This puts a wirecube and an icon on your gameobjects!
    [SerializeField] Collider bCol;
    [SerializeField] Color color = Color.white;
    [SerializeField] float arbitraryHeight = .5f;
    //public
    [SerializeField] enum IconType { Camera, Ghost, Polygon, None };
    [SerializeField] IconType iconType;
    [SerializeField] bool show = false;
    private void OnDrawGizmos()
    {
        
        Gizmos.color = color;
        if (iconType != IconType.None)
        {
            string iconLocation = "..\\Editor\\Iconography\\" + iconType.ToString() + ".png";
            Gizmos.DrawIcon(transform.position, iconLocation, true);
        }
        if (show)
        {
            if (bCol.GetType() == typeof(BoxCollider)){
                BoxCollider box = (BoxCollider)bCol;
                Gizmos.DrawCube(box.center,box.size);
            }else if (bCol.GetType() == typeof(SphereCollider)){
                SphereCollider sphere = (SphereCollider)bCol;
                Gizmos.DrawSphere(sphere.center,sphere.radius);
            }
        }
    }



}

