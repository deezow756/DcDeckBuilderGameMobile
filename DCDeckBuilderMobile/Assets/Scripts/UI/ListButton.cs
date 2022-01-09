using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListButton : MonoBehaviour
{
    private string buttonName;
    private Page page;

    public void SetUpButton(Page _page, string _buttonName)
    {
        buttonName = _buttonName;
        this.gameObject.GetComponentInChildren<Text>().text = buttonName;

        page = _page;
    }

    public void Clicked()
    {
        if (page != null)
            page.ButtonCallBack(buttonName);
    }
}
