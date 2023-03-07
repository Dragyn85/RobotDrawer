using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ValidateConnectionSetting : MonoBehaviour
{
    public Action ValidationChanged;
    public abstract bool IsValid();
}


