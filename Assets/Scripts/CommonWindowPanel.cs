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
        Shape shape;
        context = contentPane.GetChild("context_text").asTextField;
        shape = contentPane.GetChild("abililty_shape").asGraph.shape;
        shape.DrawRegularPolygon(6, 2, shape.color, new Color32(0xFF, 0x99, 0x00, 128), shape.color, 0, new float[] { 0.6f, 0.8f, 0.6f, 0.8f, 0.6f, 0.6f });
        context.text = Logic.DataManger.getInstance().mUserScore.ToString();

    }

    override protected void DoHideAnimation()
    {
        this.TweenScale(new Vector2(0.1f, 0.1f), 0.3f).OnComplete(this.HideImmediately);
    }
}
