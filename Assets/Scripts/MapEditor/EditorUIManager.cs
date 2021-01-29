using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DontFreeze.MapEditor
{
    public class EditorUIManager : MonoBehaviour
    {
        #region public references
        [Header("UI Object references")]
        public Button saveButton;
        public Button loadButton;
        public Button applyNewMap;

        public InputField nameInputField;
        public InputField newMapWidth;
        public InputField newMapHeight;
        public InputField newMapName;

        public Text notificationText;

        [Header("Other references")]
        public MapManager mapManager;
        #endregion

        private void Update()
        {
            UpdateLoadButton();
            UpdateSaveButton();
            UpdateApplyNewMapButton();
        }

        #region set Button rules
        private void UpdateLoadButton()
        {
            loadButton.interactable = nameInputField.text != "" && mapManager.FileExist(nameInputField.text);
        }
        private void UpdateSaveButton()
        {
            saveButton.interactable = nameInputField.text != "" && mapManager.map != null;
        }
        private void UpdateApplyNewMapButton()
        {
            applyNewMap.interactable = newMapHeight.text != "" && newMapName.text != "" && newMapWidth.text != "";
        }
        #endregion

        #region notification text
        public void WriteNotificationText(string text)
        {
            StopAllCoroutines();
            StartCoroutine(Notify(text));
        }
        private IEnumerator Notify(string text)
        {
            notificationText.text = text;
            notificationText.gameObject.SetActive(true);

            for (float f = 3; f>0; f-= Time.unscaledDeltaTime)
            {
                notificationText.color = new Color(notificationText.color.r, notificationText.color.g, notificationText.color.b, Mathf.Clamp01(f));
                yield return new WaitForEndOfFrame();
            }
            notificationText.gameObject.SetActive(false);
        }
        #endregion
    }
}