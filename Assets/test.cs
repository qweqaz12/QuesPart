using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIPackage.AddPackage("Assets/Resources/UI/QuesPart");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            UI.QuestionMainPanel.CreatePanel();
            Debug.LogError("123");   
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CommonWindowPanel win = new CommonWindowPanel();
            //win.contentPane = UIPackage.CreateObject("QuesPart", "Windows").asCom;
            win.Show();
            Debug.Log("start");
        }
    }
}
