using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    static SceneLoader instance;

    public GameObject Content;
    public CanvasGroup Fade;

    WaitForSeconds loadDelay = new WaitForSeconds(0.5f);
    public static string LastScene = string.Empty;
    public static string CurrentScene = string.Empty;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            transform.SetParent(null);
        }

        if(instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(instance.gameObject);

        CurrentScene = SceneManager.GetActiveScene().name;
    }

    public static void LoadScene(string scene)
    {
        instance.StartCoroutine(instance.SceneLoad(scene));
    }

    IEnumerator SceneLoad(string scene)
    {
        Fade.alpha = 0;
        Content.SetActive(true);
        LastScene = SceneManager.GetActiveScene().name;
        CurrentScene = scene;

        yield return StartCoroutine(FadeAnimation(0, 1));
        yield return loadDelay;

        AsyncOperation loading = SceneManager.LoadSceneAsync(scene);
        yield return loading;
        yield return loadDelay;

        yield return StartCoroutine(FadeAnimation(1, 0));

        Content.SetActive(false);

    }

    IEnumerator FadeAnimation(float from, float to)
    {
        float t = 0;

        while(t < 1)
        {
            Fade.alpha = Mathf.Lerp(from, to, t);
            t += Time.deltaTime * 2;
            yield return null;
        }
        Fade.alpha = to;
    }
}
