using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeLoadingScreen : MonoBehaviour {


    [SerializeField] private float loadingTime = 4f;
    [SerializeField] private string sceneName = "";

    [SerializeField] private bool autoStart = false;

    private void Start() {
        if(autoStart){
            StartFakeLoading();
        }
    }

    public void StartFakeLoading(){
        StartCoroutine(Wait());
    }
    
    private IEnumerator Wait(){
        yield return new WaitForSeconds(loadingTime);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

}
