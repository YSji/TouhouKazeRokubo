using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class ObjectPool:Singleton<ObjectPool> 
{
    public Dictionary<string, Stack<GameObject>> poolDic = new Dictionary<string, Stack<GameObject>>();
    public GameObject GetGObject(string name)
    {
        GameObject gObj = null;
        if (poolDic.ContainsKey(name) && poolDic[name].Count > 0)
        {
            gObj = poolDic[name].Pop();
            gObj.GetComponent<IResetable>().Reset();
            gObj.SetActive(true);
        }
        else
        {
            gObj = GameObject.Instantiate(Resources.Load<GameObject>(name));
            gObj.name = name;
        }
        return gObj;
    }
    public void ReCircleObj(string name,GameObject gObj)
    {
        gObj.SetActive(false);
        
        if (poolDic.ContainsKey(name))
        {
            poolDic[name].Push(gObj);
        }
        else
        {
            Stack<GameObject> newStack = new Stack<GameObject>();
            newStack.Push(gObj);
            poolDic.Add(name,newStack);

        }
    }
}
