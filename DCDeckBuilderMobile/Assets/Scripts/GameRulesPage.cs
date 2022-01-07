using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRulesPage : Page
{
    [SerializeField]
    private GameManager GM;
    [SerializeField]
    private GameObject BookindexCounter;
    [SerializeField]
    private GameObject BookPanel;
    [SerializeField]
    private Image BookPageImage;
    [SerializeField]
    private GameObject BtnNext;
    [SerializeField]
    private GameObject BtnPrevious;

    private Book RuleBook;
    private int BookIndex;

    public void OnClickBaseGame()
    {
        BtnPrevious.SetActive(false);
        BtnNext.SetActive(true);
        RuleBook = new BaseGameRuleBook();
        BookIndex = 0;
        BookPageImage.sprite = RuleBook.bookPages[BookIndex];
        BookindexCounter.GetComponent<Text>().text = BookIndex.ToString() + "/" + RuleBook.numOfPages.ToString();
        BookPanel.SetActive(true);
    }

    public void OnClickNext()
    {
        if(BookIndex < RuleBook.numOfPages - 1)
        {
            BookIndex += 1;
            if (BookIndex == RuleBook.numOfPages - 1)
            {
                BtnNext.SetActive(false);
            }            
            BookPageImage.sprite = RuleBook.bookPages[BookIndex];
            BookindexCounter.GetComponent<Text>().text = BookIndex.ToString() + "/" + RuleBook.numOfPages.ToString();
            BtnPrevious.SetActive(true);
        }
    }

    public void OnClickPrevious()
    {
        if (BookIndex >= 0)
        {
            BookIndex -= 1;
            if (BookIndex == 0)
            {
                BtnPrevious.SetActive(false);
            }            
            BookPageImage.sprite = RuleBook.bookPages[BookIndex];
            BookindexCounter.GetComponent<Text>().text = BookIndex.ToString() + "/" + RuleBook.numOfPages.ToString();
            BtnNext.SetActive(true);
        }
    }

    public override void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (BookPanel.activeSelf)
                {
                    OnClickCloseBookPanel();
                }
                else
                {
                    BtnBack();
                }
            }
        }
    }

    public void BtnBack()
    {
        StartCoroutine(IBack());
    }

    IEnumerator IBack()
    {
        yield return new WaitForSeconds(0.2f);
        GM.SwitchPage(this, GameManager.PageNames.MenuPage);
    }

    public void OnClickCloseBookPanel()
    {
        RuleBook = null;
        BookIndex = 0;
        BookindexCounter.GetComponent<Text>().text = "";
        BookPanel.SetActive(false);
    }
}
