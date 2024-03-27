using UnityEngine;

public class processControl : MonoBehaviour
{
    private GameManager gameManager;
    private ScreenFader screenFader;

    void Start()
    {
        gameManager=FindObjectOfType<GameManager>();
        screenFader=FindObjectOfType<ScreenFader>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SessionOver();
        }
        if (gameManager.availableConditions.Count==0 && gameManager.nbTry==gameManager.maxNbTry && screenFader.ChoiceMade!=2)
        {
            UnityEngine.Debug.Log("<color=green>Session OVER !</color>");
            SessionOver();
        }
    }

    public void SessionOver()
    {
        UnityEngine.Debug.Log("Session Over");

#if UNITY_EDITOR
        // 在编辑器中播放时，停止播放
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 在构建后的应用中，关闭应用
        Application.Quit();
#endif
    }
    
}
