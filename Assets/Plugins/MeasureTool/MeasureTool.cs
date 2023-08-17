using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.Overlays;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace Plugins.MeasureTool
{
    [EditorTool("Measure Tool")]
    class MeasureTool : EditorTool, IDrawSelectedHandles
    {
        [Overlay(defaultDisplay = true)]
        class MeasureToolOverlay : Overlay, ITransientOverlay
        {
            
            private readonly MeasureTool _tool;

            public MeasureToolOverlay(MeasureTool tool)
            {
                _tool = tool;
                collapsedIcon = (Texture2D)tool.toolbarIcon.image;
            }

            public override VisualElement CreatePanelContent()
            {
                var root = new VisualElement();
                
                root.Add(new Label("Hold Alt to create new points")
                {
                    style =
                    {
                        unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold),
                        marginBottom = new StyleLength(10)
                    }
                });
                

                root.Add(new Label("Path Length: " + _tool.CalculateDistance())
                {
                    style = { marginBottom = new StyleLength(10)}
                });
                root.Add(new Button(() => _tool.ClearPoint()) {text = "Clear path"});
                return root;
            }
            
            // Use the visible property to hide or show this instance from within the class.
            public bool visible => true;
        }

        private void ClearPoint()
        {
            points.Clear();
            overlay.displayed = false;
        }

        public float CalculateDistance()
        {
            float dist = 0;
            for (int i = 0; i < points.Count-1; i++)
            {
                dist += Vector3.Distance(points[i], points[i + 1]);
            }

            return dist;
        }

        private List<Vector3> points = new List<Vector3>();
        MeasureToolOverlay overlay;

        public override GUIContent toolbarIcon
        {
            get { return new GUIContent(AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Plugins/MeasureTool/masureIcon.png")); }
        }

        [Shortcut("Measure Tool", typeof(SceneView), KeyCode.M)]
        static void PlatformToolShortcut()
        {
            ToolManager.SetActiveTool<MeasureTool>();
        }
        public override void OnActivated()
        {
            SceneView.lastActiveSceneView.ShowNotification(new GUIContent("Entering Measure Tool"), .1f);
            SceneView.AddOverlayToActiveView(overlay = new MeasureToolOverlay(this));
        }
        public override void OnWillBeDeactivated()
        {
            points.Clear();
            SceneView.lastActiveSceneView.ShowNotification(new GUIContent("Exiting Measure Tool"), .1f);
            SceneView.RemoveOverlayFromActiveView(overlay);
        }

        public override void OnToolGUI(EditorWindow window)
        {
            if (!(window is SceneView sceneView))
                return;

            
            if (Event.current.type == EventType.MouseDown)
            {
                if (Event.current.button == 0)
                {
                    if (Event.current.alt)
                    {
                        AddPoint();
                    }
                }
            }

            OnDrawHandles();
            overlay.displayed = true;
        }

        private void AddPoint()
        {
            Event e = Event.current;
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider != null)
                {
                    points.Add(hit.point);
                    overlay.displayed = false;
                }
            }
        }

        public void OnDrawHandles()
        {
            for (int i = 0; i < points.Count-1; i++)
            {
                DrawLine(points[i], points[i+1], Color.red);
            }
            bool temp = GUI.changed;
            bool haveChanges = false;
            GUI.changed = false;
            for (int i = 0; i < points.Count; i++)
            {
                points[i] = Handles.PositionHandle(points[i], Quaternion.Euler(SceneView.lastActiveSceneView.rotation.eulerAngles));

                if (GUI.changed)
                {
                    var hit = GetRayFromMouse();
                    if (hit.collider != null)
                    {
                        points[i] = hit.point;
                        overlay.displayed = false;
                    }

                    haveChanges = true;
                    GUI.changed = false;
                }
            }
            GUI.changed |= temp;


            if (!haveChanges)
            {
                if (Event.current.alt)
                {
                    var hit = GetRayFromMouse();
                    if (hit.collider != null)
                    {
                        if (points.Count != 0)
                        {
                            DrawLine(points[points.Count - 1], hit.point, Color.green);
                        }
                        else
                        {
                            Handles.color = Color.green;
                            Handles.SphereHandleCap(999, hit.point, Quaternion.LookRotation(hit.normal, Vector3.up), 0.1f, EventType.Repaint);
                        }
                    }
                }
            }
        }

        private void DrawLine(Vector3 pos1, Vector3 pos2, Color color)
        {
            Handles.color = color;
            Handles.DrawLine(pos1, pos2, 6f);

            Handles.color = Color.white;
            Handles.DrawDottedLine(pos1, pos2, 1);
        }

        private RaycastHit GetRayFromMouse()
        {
            Event e = Event.current;
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore))
            {
                return hit;
            }

            return new RaycastHit();
        }
    }
}