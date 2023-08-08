using UnityEditor;
using UnityEngine;

namespace Misc.Editor
{
    [CustomEditor(typeof(PrefabsReplacer))]
    public class PrefabsReplacerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Replace"))
            {
                (target as PrefabsReplacer).Replace();
            }
        }
    }
}
