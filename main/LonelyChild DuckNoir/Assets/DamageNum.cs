using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNum : MonoBehaviour
{
    [SerializeField] TextMeshPro guiText;
    [SerializeField] float xSpdMax;
    [SerializeField] float xSpdMin;
    float xSpd;
    [SerializeField] float yGravity;
    [SerializeField] float ySpdMax;
    [SerializeField] float ySpdMin;
    float ySpd;
    [SerializeField] float lifetime;
    [SerializeField] float initalScale;
    [SerializeField] float bigScale;
    [SerializeField] float bigScaleTime;
    float time = 0f;
    Vector3 initalPos;
    void Start(){
        initalPos = transform.position;
        xSpd = Random.Range(xSpdMin,xSpdMax);
        ySpd = Random.Range(ySpdMin,ySpdMax);
        if (Random.Range(0f,1f)>0.5f){
            xSpd = -xSpd;
        }
    }

    public void Text(string text){
        guiText.text = text;
    }

    void Update(){
        time += Time.deltaTime;
        Vector3 newPos = initalPos;
        newPos.x += time * xSpd;
        newPos.y += ySpd * time + .5f * yGravity * Mathf.Pow(time,2);
        float scale = 1f ;
        if (time < bigScaleTime){
            scale = Mathf.Lerp(initalScale,bigScale,time/bigScaleTime);
        }else{
            scale = Mathf.Lerp(bigScale,0f,(time-bigScaleTime)/(lifetime-bigScaleTime));
        }
        transform.position = newPos;
        transform.localScale = new Vector3(1f,1f,1f) * scale;
        if (time > lifetime){
            Destroy(gameObject);
        }
    }
}
