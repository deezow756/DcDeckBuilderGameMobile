using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameRuleBook : Book
{
    public BaseGameRuleBook()
    {
        numOfPages = 16;
        bookPages = new Sprite[numOfPages];
        //Load a Sprite (Assets/Resources/Sprites/sprite01.png)
        bookPages[0] = Resources.Load<Sprite>("RuleBooks/BaseGame/BaseGameRuleBookCover");
        bookPages[1] = Resources.Load<Sprite>("RuleBooks/BaseGame/BaseGameRuleBookPage1");
        bookPages[2] = Resources.Load<Sprite>("RuleBooks/BaseGame/BaseGameRuleBookPage2");
        bookPages[3] = Resources.Load<Sprite>("RuleBooks/BaseGame/BaseGameRuleBookPage3");
        bookPages[4] = Resources.Load<Sprite>("RuleBooks/BaseGame/BaseGameRuleBookPage4");
        bookPages[5] = Resources.Load<Sprite>("RuleBooks/BaseGame/BaseGameRuleBookPage5");
        bookPages[6] = Resources.Load<Sprite>("RuleBooks/BaseGame/BaseGameRuleBookPage6");
        bookPages[7] = Resources.Load<Sprite>("RuleBooks/BaseGame/BaseGameRuleBookPage7");
        bookPages[8] = Resources.Load<Sprite>("RuleBooks/BaseGame/BaseGameRuleBookPage8");
        bookPages[9] = Resources.Load<Sprite>("RuleBooks/BaseGame/BaseGameRuleBookPage9");
        bookPages[10] = Resources.Load<Sprite>("RuleBooks/BaseGame/BaseGameRuleBookPage10");
        bookPages[11] = Resources.Load<Sprite>("RuleBooks/BaseGame/BaseGameRuleBookPage11");
        bookPages[12] = Resources.Load<Sprite>("RuleBooks/BaseGame/BaseGameRuleBookPage12");
        bookPages[13] = Resources.Load<Sprite>("RuleBooks/BaseGame/BaseGameRuleBookPage13");
        bookPages[14] = Resources.Load<Sprite>("RuleBooks/BaseGame/BaseGameRuleBookCredits");
        bookPages[15] = Resources.Load<Sprite>("RuleBooks/BaseGame/BaseGameRuleBookBack");
    }
}
