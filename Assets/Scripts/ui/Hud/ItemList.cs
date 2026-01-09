using UnityEngine;
using UnityEngine.UI;

namespace ui.Hud
{
    public class ItemList : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private GameObject itemPanel;
        [SerializeField] private Image itemIconPrefab;
        [SerializeField] private Vector2 startOffset = new Vector2(30f, -30f);
        [SerializeField] private Vector2 spacing = new Vector2(55f, -55f);
        [SerializeField] private int itemsPerRow = 4;
        [SerializeField] private GameObject newItemHintPrefab;
        private int _fixedInventorySize = 8;
        private int _selectedSlot = 0;
        private RectTransform _newItemHintRect;
        private CanvasGroup _newItemHintCanvas;
        private Vector2 _newItemHintStartPos;
        private Coroutine _newItemHintRoutine;

        private void OnEnable()
        {
            RefreshIcons();
        }

        private void Awake()
        {
            if (newItemHintPrefab != null)
            {
                _newItemHintRect = newItemHintPrefab.GetComponent<RectTransform>();
                if (_newItemHintRect != null)
                {
                    _newItemHintStartPos = _newItemHintRect.anchoredPosition;
                }

                _newItemHintCanvas = newItemHintPrefab.GetComponent<CanvasGroup>();
                if (_newItemHintCanvas == null)
                {
                    _newItemHintCanvas = newItemHintPrefab.AddComponent<CanvasGroup>();
                }
            }
        }

        // Expose the player's current inventory to the UI
        public ItemData[] GetItems()
        {
            return player != null ? player.GetItems() : new ItemData[_fixedInventorySize];
        }

        // Rebuilds the icon list on the panel
        public void RefreshIcons()
        {
            if (itemPanel == null || itemIconPrefab == null || player == null)
                return;

            var templateTransform = itemIconPrefab.transform;
            RemoveOldIcons(templateTransform);

            ItemData[] currentItems = player.GetItems();
            if (currentItems == null)
                return;

            for (int i = 0; i < currentItems.Length; i++)
            {
                ItemData item = currentItems[i];
                Image iconInstance = Instantiate(itemIconPrefab, itemPanel.transform);
                iconInstance.sprite = item != null ? item.icon : null;

                RectTransform rt = iconInstance.rectTransform;
                int col = i % itemsPerRow;
                int row = i / itemsPerRow;
                rt.anchoredPosition = startOffset + new Vector2(col * spacing.x, row * spacing.y);

                if (item == null)
                    iconInstance.color = new Color(1f, 1f, 1f, 0);

                if (i == _selectedSlot)
                {
                    iconInstance.transform.Find("Border").gameObject.SetActive(true);
                }

                iconInstance.gameObject.SetActive(true);
            }


            templateTransform.gameObject.SetActive(false);
        }

        private void RemoveOldIcons(Transform templateTransform)
        {
            foreach (Transform child in itemPanel.transform)
            {
                if (child.CompareTag("Background"))
                {
                    continue;
                }
                if (templateTransform != null && child == templateTransform)
                {
                    child.gameObject.SetActive(false);
                    continue;
                }
                Destroy(child.gameObject);
            }
        }

        public void UpdateSelectedSlot(int selectedSlot)
        {
            _selectedSlot = selectedSlot;
            RefreshIcons();
        }

        public void ShowNewItemHint()
        {
            if (newItemHintPrefab == null)
            {
                return;
            }

            if (_newItemHintRoutine != null)
            {
                StopCoroutine(_newItemHintRoutine);
            }

            if (_newItemHintRect == null)
            {
                _newItemHintRect = newItemHintPrefab.GetComponent<RectTransform>();
                if (_newItemHintRect != null)
                {
                    _newItemHintStartPos = _newItemHintRect.anchoredPosition;
                }
            }

            if (_newItemHintCanvas == null)
            {
                _newItemHintCanvas = newItemHintPrefab.GetComponent<CanvasGroup>();
                if (_newItemHintCanvas == null)
                {
                    _newItemHintCanvas = newItemHintPrefab.AddComponent<CanvasGroup>();
                }
            }

            _newItemHintRoutine = StartCoroutine(PlayNewItemHint());
        }

        private System.Collections.IEnumerator PlayNewItemHint()
        {
            const float hintMoveDistance = 200f;
            const float hintMoveDuration = 0.5f;
            const float hintHoldDuration = 10f;
            const float hintFadeDuration = 0.5f;

            newItemHintPrefab.SetActive(true);
            if (_newItemHintRect != null)
            {
                _newItemHintRect.anchoredPosition = _newItemHintStartPos;
            }
            if (_newItemHintCanvas != null)
            {
                _newItemHintCanvas.alpha = 1f;
            }

            if (_newItemHintRect != null && hintMoveDuration > 0f)
            {
                Vector2 startPos = _newItemHintStartPos;
                Vector2 endPos = startPos + new Vector2(0f, hintMoveDistance);
                float elapsed = 0f;
                while (elapsed < hintMoveDuration)
                {
                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / hintMoveDuration);
                    _newItemHintRect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
                    yield return null;
                }
            }

            if (hintHoldDuration > 0f)
            {
                yield return new WaitForSeconds(hintHoldDuration);
            }

            if (_newItemHintCanvas != null && hintFadeDuration > 0f)
            {
                float elapsed = 0f;
                while (elapsed < hintFadeDuration)
                {
                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / hintFadeDuration);
                    _newItemHintCanvas.alpha = Mathf.Lerp(1f, 0f, t);
                    yield return null;
                }
            }

            newItemHintPrefab.SetActive(false);
        }
    }
}
