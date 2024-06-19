using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingBar;
    public string sceneToLoad;

    public void Start(){
        StartCoroutine(LoadAsynchronously(sceneToLoad));
        loadingBar.SetActive(true);
        loadingBar.GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
    }

    IEnumerator LoadAsynchronously(string sceneName){
        yield return new WaitForSeconds(3f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while(!operation.isDone){
            float progress = Mathf.Clamp01(operation.progress / .9f);
            
            loadingBar.GetComponent<UnityEngine.UI.Image>().fillAmount = progress;

            yield return null;
        }
    }

}
