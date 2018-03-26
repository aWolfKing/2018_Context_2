using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSceneChange : MonoBehaviour {

    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private string animationName = "Play";
    [SerializeField] private string sceneName = "";

    private void Start() {
        try {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }
        catch{ }
        StartCoroutine(Wait());    
    }

    private IEnumerator Wait(){
        yield return new WaitForSeconds(1f);

        try {
            audioSource.Play();
            animator.Play(animationName);
        }
        catch{
            #if UNITY_EDITOR
            UnityEditor.EditorGUIUtility.PingObject(gameObject);
            Debug.LogWarning("Please assign the variables");
            #endif
        }

        if (audioSource != null && audioSource.clip != null) {
            yield return new WaitForSeconds(audioSource.clip.length);
        }

        yield return new WaitForSeconds(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

    }

}
