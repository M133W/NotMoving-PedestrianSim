using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    private bool isFading = false;
    public GameObject ChoiceYes;
    public GameObject ChoiceNon;
    public int ChoiceMade; // 2: No choice, 1: Yes, 0: No
    private GameObject car; // 添加一个 GameObject 类型的变量来保存找到的车辆
    private float blackoutStartTime;
    public float reactionTime;
    private MarkerEvent markerEvent;
    private GameManager gameManager;
    private CarMovement carMovement;
    private processControl processcontrol;

    void Start()
    {
        ChoiceMade = 2; // 设置默认值
        reactionTime=0;
        markerEvent=FindObjectOfType<MarkerEvent>();
        carMovement=FindObjectOfType<CarMovement>();
    }

    // 新的方法，用于在需要时触发黑屏
    public void TriggerScreenFader()
    {
        gameManager=FindObjectOfType<GameManager>();
        markerEvent.RecordBlackOutStart();
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.enabled = true;
            Transform canvasTransform = canvas.transform;

            Transform questionTransform = canvasTransform.Find("Question");
            if (questionTransform != null)
            {
                questionTransform.gameObject.SetActive(true);
            }

            Transform ouiTransform = canvasTransform.Find("Oui");
            if (ouiTransform != null)
            {
                ouiTransform.gameObject.SetActive(true);
            }

            Transform nonTransform = canvasTransform.Find("Non");
            if (nonTransform != null)
            {
                nonTransform.gameObject.SetActive(true);
            }
        }

        if (!isFading)
        {
            isFading = true;
            blackoutStartTime = Time.realtimeSinceStartup; // 记录黑屏开始的时间
            car = GameObject.FindGameObjectWithTag("car"); // 保存找到的车辆
            if (car != null)
            {
                AudioSource audioSource = car.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.Stop();
                }
            }
            
            Time.timeScale = 0f; // 暂停游戏
        }
    }

    // // 新的方法，用于在需要时解除黑屏
    // public void EndScreenFader()
    // {
    //     // markerEvent.RecordBlackOutEnd();
    //     Canvas canvas = GetComponent<Canvas>();
    //     if (canvas != null)
    //     {
    //         canvas.enabled = false;
    //     }

    //     isFading = false;
    //     Time.timeScale = 1f;
    //     // if(gameManager.ConditionTransition){
    //     // gameManager.nbTry++;
    //     // }
    //     // ChoiceMade = 2;
    // }
    public void EndScreenFader()
    {
        // markerEvent.RecordBlackOutEnd();
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            Transform canvasTransform = canvas.transform;

            // Transform questionTransform = canvasTransform.Find("Question");
            // if (questionTransform != null)
            // {
            //     questionTransform.gameObject.SetActive(false);
            // }

            Transform ouiTransform = canvasTransform.Find("Oui");
            if (ouiTransform != null)
            {
                ouiTransform.gameObject.SetActive(false);
            }

            Transform nonTransform = canvasTransform.Find("Non");
            if (nonTransform != null)
            {
                nonTransform.gameObject.SetActive(false);
            }

            // Show the WhiteCross object
            // Transform whiteCrossTransform = canvasTransform.Find("WhiteCross");
            // if (whiteCrossTransform != null)
            // {
            //     whiteCrossTransform.gameObject.SetActive(true);
            // }
        }
        
        GameObject blackOutObject = GameObject.FindGameObjectWithTag("BlackOut");

        if (blackOutObject != null)
        {
            // Get the Text component from the GameObject
            Text blackOutText = blackOutObject.GetComponent<Text>();

            // Check if Text component exists
            if (blackOutText != null)
            {
                // Update the text content
                blackOutText.text = "+";
            }
        }

        StartCoroutine(DisableCanvasAfterDelay(3f));
                // 在这里重新激活AudioSource
    }

    private IEnumerator DisableCanvasAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.enabled = false;
        }
        GameObject blackOutObject = GameObject.FindGameObjectWithTag("BlackOut");

        if (blackOutObject != null)
        {
            // Get the Text component from the GameObject
            Text blackOutText = blackOutObject.GetComponent<Text>();

            // Check if Text component exists
            if (blackOutText != null)
            {
                // Update the text content
                blackOutText.text = "La voiture vous laisserait-elle passer ?";
            }
        }
        isFading = false;
        Time.timeScale = 1f;
        // if(gameManager.ConditionTransition){
        gameManager.nbTry++;
        gameManager.tryStartTime = Time.realtimeSinceStartup;//Time.time;
        if(gameManager.ConditionTransition==true){
            gameManager.nbTry=1;
        }
        carMovement.ResetCar();
        markerEvent.RecordRestartTry();
        if (car != null)
        {
            AudioSource audioSource = car.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
        // }
        // ChoiceMade = 2;
    }

    public void ChoiceOptionYes()
    {
        ChoiceMade = 1;
        reactionTime = Time.realtimeSinceStartup - blackoutStartTime; // 计算反应时间
        EndScreenFader();
        Debug.Log($"You clicked Yes. Reaction Time: {reactionTime} seconds");    }

    public void ChoiceOptionNon()
    {
        ChoiceMade = 0;
        reactionTime = Time.realtimeSinceStartup - blackoutStartTime; // 计算反应时间
        EndScreenFader();
        Debug.Log($"You clicked No. Reaction Time: {reactionTime} seconds");
    }

}
