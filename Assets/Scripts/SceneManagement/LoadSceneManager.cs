using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager loadSceneManager;
    //　シーン移動に関するデータファイル
    [SerializeField]
    private SceneMovementData sceneMovementData = null;
    //　フェードプレハブ
    [SerializeField]
    private GameObject fadePrefab = null;
    //　フェードインスタンス
    private GameObject fadeInstance;
    //　フェードの画像
    private Image fadeImage;
    [SerializeField]
    private float fadeSpeed = 5f;

    private void Awake()
    {
        // LoadSceneMangerは常に一つだけにする
        if (loadSceneManager == null)
        {
            loadSceneManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //　次のシーンを呼び出す
    public void GoToNextScene(SceneMovementData.SceneType scene)
    {
        sceneMovementData.SetSceneType(scene);
        StartCoroutine(FadeAndLoadScene(scene));
    }
    //　フェードをした後にシーン読み込み
    IEnumerator FadeAndLoadScene(SceneMovementData.SceneType scene)
    {
        if (scene != SceneMovementData.SceneType.WorldMapToBattle)
        {
            //　フェードUIのインスタンス化
            fadeInstance = Instantiate<GameObject>(fadePrefab);
            fadeImage = fadeInstance.GetComponentInChildren<Image>();
            //　フェードアウト処理
            yield return StartCoroutine(Fade(1f));
        }
        //else
        //{
        //    yield return StartCoroutine(FadeWorldMapToBattle(0.1f));
        //}

        //　シーンの読み込み
        if (scene == SceneMovementData.SceneType.FirstVillage)
        {
            yield return StartCoroutine(LoadScene("Village"));
        }
        else if (scene == SceneMovementData.SceneType.FirstVillageToWorldMap)
        {
            yield return StartCoroutine(LoadScene("WorldMap"));
        }
        else if (scene == SceneMovementData.SceneType.WorldMapToBattle)
        {
            yield return StartCoroutine(LoadScene("Battle"));
        }
        else if (scene == SceneMovementData.SceneType.BattleToWorldMap)
        {
            yield return StartCoroutine(LoadScene("WorldMap"));
        }
        else if (scene == SceneMovementData.SceneType.StartGame)
        {
            yield return StartCoroutine(LoadScene("Village"));
        }

        //　フェードUIのインスタンス化
        fadeInstance = Instantiate<GameObject>(fadePrefab);
        fadeImage = fadeInstance.GetComponentInChildren<Image>();
        fadeImage.color = new Color(0f, 0f, 0f, 1f);

        //　フェードイン処理
        yield return StartCoroutine(Fade(0f));

        Destroy(fadeInstance);
    }
    //　フェード処理
    IEnumerator Fade(float alpha)
    {
        var fadeImageAlpha = fadeImage.color.a;

        while (Mathf.Abs(fadeImageAlpha - alpha) > 0.01f)
        {
            fadeImageAlpha = Mathf.Lerp(fadeImageAlpha, alpha, fadeSpeed * Time.deltaTime);
            fadeImage.color = new Color(0f, 0f, 0f, fadeImageAlpha);
            yield return null;
        }
    }
    //　実際にシーンを読み込む処理
    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        while (!async.isDone)
        {
            yield return null;
        }
    }
}