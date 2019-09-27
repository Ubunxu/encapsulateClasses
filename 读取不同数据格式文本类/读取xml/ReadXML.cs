using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

/// <summary>
/// 读取xml文件内容
/// </summary>
public class ReadXML
{
    /// <summary>
    /// 读取xml文件的所有节点内容
    /// </summary>
    /// <param name="xmlStr">xml文本字符串形式</param>
    public static void ReadXmlByStr(string xmlStr)
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlStr);
        XmlElement root = xmlDocument.DocumentElement;
        Debug.Log("根节点：" + root.Name + "  ------  节点内容：" + root.InnerText);
        FindAllAttributes(root);
        if (root.FirstChild.NodeType == XmlNodeType.Element)
        {
            FindAllNodeByXmlNode(root);
        }
        
    }
    /// <summary>
    /// 读取xml文件的所有节点内容
    /// </summary>
    /// <param name="filepath">xml文本的地址(本地和远程的都可以)</param>
    public static void ReadXmlByPath(string filepath)
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(filepath);
        XmlElement root = xmlDocument.DocumentElement;
        Debug.Log("根节点：" + root.Name + "  ------  节点内容：" + root.InnerText);
        FindAllAttributes(root);
        if (root.FirstChild.NodeType == XmlNodeType.Element)
        {
            FindAllNodeByXmlNode(root);
        }
    }
    /// <summary>
    /// 递归遍历xml的节点，及其属性
    /// </summary>
    /// <param name="nodeList"></param>
    private static void FindAllNodeByList(XmlNodeList nodeList)
    {
        foreach (XmlNode node in nodeList)
        {            
            //前面使用haschildnodes进行判断只要是为了，在执行node.FirstChild排除，空子节点的情况。
            //  排除空子节点情况             排除没有标签子节点情况。
            if (!node.HasChildNodes || !(node.FirstChild.NodeType == XmlNodeType.Element))
            {
                Debug.Log("节点名： " + node.Name + "  -----  节点内容：" + node.InnerText);
                FindAllAttributes(node);
            }   
            else
            {   
                Debug.Log("节点名：" + node.Name  + "   --------   子节点个数：" + node.ChildNodes.Count);
                FindAllAttributes(node);
                FindAllNodeByList(node.ChildNodes);               
            }
        }

    }
    /// <summary>
    /// 根据xmlnode对象获取所有节点
    /// </summary>
    /// <param name="node"></param>
    private static void FindAllNodeByXmlNode(XmlNode node)
    {
        FindAllNodeByList(node.ChildNodes);
    }
    
    /// <summary>
    /// 获取节点属性
    /// </summary>
    /// <param name="node"></param>
    private static void FindAllAttributes(XmlNode node)
    {
        XmlAttributeCollection attributes = node.Attributes;
        if (attributes != null)
        {
            foreach (XmlAttribute attribute in attributes)
            {
                Debug.Log("属性名：" + attribute.Name + "    ------   属性值： " + attribute.Value);
            }
        }
    }

}

