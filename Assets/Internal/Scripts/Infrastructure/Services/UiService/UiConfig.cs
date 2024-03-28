using System;
using System.Collections.Generic;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.UiService
{
    [CreateAssetMenu(fileName = "UiConfig", menuName = "Configs/UiConfig")]
    public class UiConfig : ScriptableObject
    {
        [field: SerializeField] public List<WindowConfig> AllWindowConfigs { get; private set; }
        [field: SerializeField] public GameObject UIRootPrefab { get; private set; }
    }

    [Serializable]
    public class WindowConfig
    {
        [field: SerializeField] public UIWindowType Type { get; private set; }
        [field: SerializeField] public GameObject ViewPrefab { get; private set; }
    }
}