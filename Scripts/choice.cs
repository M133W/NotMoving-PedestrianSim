using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class choice : MonoBehaviour
{
    public GameObject question;
    public GameObject ChoiceYes;
    public GameObject ChoiceNon;
    public int ChoiceMade;
    // Start is called before the first frame update
    public void ChoiceOptionYes(){
        question.GetComponent<Text>().text = "You chose Yes";
        ChoiceMade=1;
    }
    public void ChoiceOptionNon(){
        question.GetComponent<Text>().text = "You chose No";
        ChoiceMade=0;
    }
    // Update is called once per frame
    void Update()
    {
        if(ChoiceMade<2){
            ChoiceYes.SetActive(false);
            ChoiceNon.SetActive(false);
        }
    }
}
