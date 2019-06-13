using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class CommonWindowPanel : Window
{
    public CommonWindowPanel()
    {
    }
    protected override void OnInit()
    {
        this.contentPane = UIPackage.CreateObject("QuesPart", "Windows").asCom;

        this.modal = true;
        this.Center();
    }

    override protected void OnShown()
    {
        GTextField context;
        context = contentPane.GetChild("context_text").asTextField;
        context.text = Logic.DataManger.getInstance().mUserScore.ToString();
    }

    override protected void DoHideAnimation()
    {
        this.TweenScale(new Vector2(0.1f, 0.1f), 0.3f).OnComplete(this.HideImmediately);
    }
}
