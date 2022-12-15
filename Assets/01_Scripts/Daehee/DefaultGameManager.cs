using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefaultGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Invoke("SceneVS",10f);
    }

    // Update is called once per frame
    void SceneVS()
    {
        SceneManager.LoadScene("vs");
    }
}
