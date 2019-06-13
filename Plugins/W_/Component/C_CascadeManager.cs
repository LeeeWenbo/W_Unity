/*Name:		 				C_CascadingManager	
 *Description: 				这个拖给Canvas，对某一个画布里所有button监控
 *                          同级别的，显示或者隐藏一个，剩下的都非激活的。如果是二级菜单，。
 *Author:       			lwb
 *Date:         			2019-06-
 *Copyright(C) 2019 by 		company@zhiwyl.com*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_CascadeManager : MonoBehaviour
{

    C_CascadeMenu[] casAll = new C_CascadeMenu[] { };
    List<List<C_CascadeMenu>> casSS;

    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        casSS = U_List.NewListOfList<C_CascadeMenu>(9);
        casAll = transform.GetComponentsInChildren<C_CascadeMenu>(true);
        foreach (C_CascadeMenu cas in casAll)
        {
            switch (cas.level)
            {
                case 1: casSS[0].Add(cas); break;
                case 2: casSS[1].Add(cas); break;
                case 3: casSS[2].Add(cas); break;
                case 4: casSS[3].Add(cas); break;
                case 5: casSS[4].Add(cas); break;
                case 6: casSS[5].Add(cas); break;
                case 7: casSS[6].Add(cas); break;
                case 8: casSS[6].Add(cas); break;
                case 9: casSS[6].Add(cas); break;
            }
        }
    }

    public void ActiveButton(C_CascadeMenu cas)
    {
        int level = cas.level - 1;

        if (!cas.isMetuxOther)
        {
            U_GO.AcitveSwitch(cas.acitveOBJ);
        }
        else
        {
            foreach (C_CascadeMenu ca in casSS[level])
            {
                if (ca != cas)
                {
                    if (ca.isMetux)
                        ca.acitveOBJ.SetActive(false);
                }
                else
                {
                    U_GO.AcitveSwitch(cas.acitveOBJ);
                }
            }
        }
    }
}
