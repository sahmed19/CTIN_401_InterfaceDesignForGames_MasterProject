using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SODAVariable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

    public abstract new string ToString();

}
