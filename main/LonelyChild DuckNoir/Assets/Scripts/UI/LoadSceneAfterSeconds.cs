using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAfterSeconds : MonoBehaviour
{
    public float timeTill = 2f;
    public string toLoad = "MainMenu";
    void Start()
    {
        //Start the coroutine we define below named ExampleCoroutine.
        StartCoroutine(ExampleCoroutine());
    }

    IEnumerator ExampleCoroutine()
    {

        yield return new WaitForSeconds(timeTill);
        SceneManager.LoadScene(toLoad);
    }
}
