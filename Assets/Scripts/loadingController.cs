using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class loadingController : MonoBehaviour
{

    [SerializeField] private Slider loadingScreen;
    [SerializeField] private TMP_Text loadingText;

    [SerializeField] private float minimumLoadingTime = 1.2f; // Minimum time the loading screen should be displayed

    private static string tagetScene;

    public static void LoadScene(string sceneName)
    {
        tagetScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    void Start()
    {
        if(string.IsNullOrEmpty(tagetScene))
        {
            tagetScene = "StartGame"; // Default scene if none is set
        }
        StartCoroutine(LoadTargetScene());
    }

     private IEnumerator LoadTargetScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(tagetScene);
        operation.allowSceneActivation = false;

        float timer = 0f;
        while(operation.progress < 0.9f || timer < minimumLoadingTime)
        {
            timer += Time.deltaTime;
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);
            float fakeProgress = Mathf.Clamp01(timer / minimumLoadingTime);
            float displayedProgress = Mathf.Min(realProgress, fakeProgress);   

            if(loadingScreen != null)
            {
                loadingScreen.value = displayedProgress;
            }
            if(loadingText != null)
            {
                loadingText.text =$"{Mathf.RoundToInt(displayedProgress * 100)}%";
            }

            yield return null;
        }
        if(loadingScreen != null)
        {
            loadingScreen.value = 1f;
        }

        if(loadingText != null)
        {
            loadingText.text = "100%";
        }

        yield return new WaitForSeconds(0.2f);
        operation.allowSceneActivation = true;
    }   



}
