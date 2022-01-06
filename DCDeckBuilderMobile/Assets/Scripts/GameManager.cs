using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum PageNames { StartPage, MenuPage}
    public Dictionary<PageNames, Page> Pages = new Dictionary<PageNames, Page>();
    [SerializeField]
    private Page StartPage;
    [SerializeField]
    private Page MenuPage;


    // Start is called before the first frame update
    void Start()
    {
        Pages.Add(PageNames.StartPage, StartPage);
        Pages.Add(PageNames.MenuPage, MenuPage);
    }

    public void SwitchPage(Page prev, PageNames next)
    {
        prev.gameObject.SetActive(false);
        Pages[next].gameObject.SetActive(true);
    }


}
