using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class PrefabsReplacer : MonoBehaviour
{
    public enum Type
    {
        ReplaceDestroy,
        Replace
    }
    [SerializeField] private GameObject prefab;
    [SerializeField] private Type type;
    
    //EDITOR SCRIPT 
#if UNITY_EDITOR
    
    public void Replace()
    {
        var childs = new List<Transform>();
        
        foreach (Transform child in transform)
        {
            childs.Add(child);
        }

        foreach (var child in childs)
        {
            var item = PrefabUtility.InstantiatePrefab(prefab, child.parent) as GameObject;
            item.transform.position = child.transform.position;
            item.transform.rotation = child.transform.rotation;
            item.transform.localScale = child.transform.localScale;
            EditorUtility.SetDirty(item);
        }

        if (type == Type.ReplaceDestroy)
        {
            foreach (var child in childs)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
#endif
}
