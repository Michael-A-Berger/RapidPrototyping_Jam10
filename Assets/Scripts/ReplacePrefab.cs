using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ReplacePrefab : MonoBehaviour
{
    private string LoaclPath = "Assets/Prefabs/PlatformD";
    public GameObject Object;
    private void OnDrawGizmos() {
        Object prefab = PrefabUtility.CreatePrefab(LoaclPath, )
    }
}
