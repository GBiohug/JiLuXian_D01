using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �洢����UI����Ϣ���������ֺ�·��
/// </summary>
public class UIType
{
    /// <summary>
    /// UI����
    /// </summary>
   public string Name {  get;private set; }
    /// <summary>
    /// UI·��
    /// </summary>
    public string Path { get; private set; }

    public UIType(string path)
    {
        Path = path;
        Name = path.Substring(path.LastIndexOf('/')+1);
    }

}
