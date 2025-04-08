using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class UIAnchorSetter : EditorWindow
    {
        [MenuItem("Tools/Set UI Image Anchor to Center %_#w")]
        public static void ShowWindow()
        {
            GetWindow<UIAnchorSetter>("UI Anchor Setter");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Set Anchor to Center"))
            {
                SetAnchorToCenter();
            }
        }

        private void SetAnchorToCenter()
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                RectTransform rectTransform = obj.GetComponent<RectTransform>();
                if (rectTransform != null && obj.GetComponent<Image>() != null)
                {
                    Undo.RecordObject(rectTransform, "Set Anchor to Center");
                    RectTransform parentRect = rectTransform.parent as RectTransform;
                    if (parentRect != null)
                    {
                        Vector2 anchorPosition = new Vector2(
                            (rectTransform.anchoredPosition.x +
                             rectTransform.rect.width * (rectTransform.pivot.x - 0.5f)) / parentRect.rect.width +
                            rectTransform.anchorMin.x,
                            (rectTransform.anchoredPosition.y +
                             rectTransform.rect.height * (rectTransform.pivot.y - 0.5f)) / parentRect.rect.height +
                            rectTransform.anchorMin.y
                        );

                        Vector2 offset = rectTransform.anchoredPosition;
                        rectTransform.anchorMin = anchorPosition;
                        rectTransform.anchorMax = anchorPosition;
                      //  rectTransform.anchoredPosition = offset;
                    }
                }
            }
        }
    }
}