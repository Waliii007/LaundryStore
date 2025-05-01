using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class NotificationHandler : MonoBehaviour
    {
        [Header("UI References")] public RectTransform notificationPanel;
        public Text notificationText;
        public float displayDuration = 2f;
        public float slideDuration = 0.3f;

        public Vector2 hiddenPosition;
        public Vector2 visiblePosition;
        private Queue<string> messageQueue = new Queue<string>();
        private bool isShowing = false;

        private void Awake()
        {
            visiblePosition = notificationPanel.anchoredPosition;
            hiddenPosition = new Vector2(visiblePosition.x, visiblePosition.y + 200); // slide from top
            notificationPanel.anchoredPosition = hiddenPosition;
        }

        public void ShowNotification()
        {
            ShowNotification("Mubeen");
        }
        public void ShowNotification(string message)
        {
            messageQueue.Enqueue(message);
            if (!isShowing)
                StartCoroutine(ProcessQueue());
        }

        private IEnumerator ProcessQueue()
        {
            isShowing = true;

            while (messageQueue.Count > 0)
            {
                string message = messageQueue.Dequeue();
                notificationText.text = message;

                // Slide in
                yield return notificationPanel.DOAnchorPos(visiblePosition, slideDuration).SetEase(Ease.OutBack)
                    .WaitForCompletion();

                yield return new WaitForSeconds(displayDuration);

                // Slide out
                yield return notificationPanel.DOAnchorPos(hiddenPosition, slideDuration).SetEase(Ease.InBack)
                    .WaitForCompletion();
            }

            isShowing = false;
        }
    }
}