using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;
using LitJson;


/// <summary>
/// 读取json文件内容
/// </summary>
public class ReadJson
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="jsonPath"></param>
    public static void Analysis(string jsonStr)
    {
        JsonData data = JsonMapper.ToObject(jsonStr);
        Debug.Log("json的数据元素个数   ----------------------   " + data.Keys.Count);
        RecursionJudge(data);
    }

    /// <summary>
    /// 递归遍历json各个节点内容
    /// </summary>
    /// <param name="data">整个json文件的JsonData对象</param>
    public static void RecursionJudge(JsonData data)
    {
        if ((data.IsArray || data.IsObject))
        {
            foreach (string key in data.Keys)
            {
                if (data[key].IsArray)
                {
                    Debug.Log("数组：  ----------------------  " + key + "    :     " + data[key]);

                    for (int i = 0; i < data[key].Count; ++i)
                    {
                        Debug.Log("数组元素下标：-------------------   " + i);
                        RecursionJudge(data[key][i]);
                    }
                }
                else if (data[key].IsObject)
                {
                    Debug.Log("对象：  ========================    " + key + "      :      " + data[key]);

                    foreach (string key2 in data[key].Keys)
                    {
                        Debug.Log("对象元素键：===================   " + key2);
                        RecursionJudge(data[key][key2]);
                    }
                }
                else
                {
                    Debug.Log("内单个(元素)：    ++++++++++++++++++++++++++  " + key + "     :       " + data[key]);
                }
            }
        }
        else
        {
            Debug.Log("外单个(元素)：    ++++++++++++++++++++++++++++++++  " + data);
        }
    }
}

