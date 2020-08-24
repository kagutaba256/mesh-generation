using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GitHub.Unity {
    [InitializeOnLoad]
    public class UnityAPIWrapper : ScriptableSingleton<UnityAPIWrapper> {
        static UnityAPIWrapper () { }
    }
}