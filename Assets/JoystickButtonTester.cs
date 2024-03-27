using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickButtonTester : MonoBehaviour
{
    private Joystick joystick;
    private ScreenFader screenFader;
    void Start()
    {
        joystick = Joystick.current;
        screenFader=FindObjectOfType<ScreenFader>();
    }
    void Update()
    {

        if (joystick != null)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton3))
            {
                Debug.Log("RNosedDown is pressed.");
                screenFader.ChoiceOptionYes();
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton2))
            {
                Debug.Log("LNosedDown is pressed.");
                screenFader.ChoiceOptionNon();
            }
        } 
    }
}
