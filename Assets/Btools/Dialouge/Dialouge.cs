using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using Btools.TimedEvents;

namespace Btools.DialougeSystem
{
    /// <summary>Dialouge system, that uses delegates</summary>
    public class Dialouge : MonoBehaviour
    {
        private string _text;
        public static float defaultTimeBetweenLetters = .05f;

        private DialougeResponseComponent[] currentResponses;
        [SerializeField] private DialougeResponseComponent responseTemplate;
        [SerializeField] private TextMeshProUGUI textComponent;
        [SerializeField] private RectTransform bottomLeft;
        [SerializeField] private RectTransform bottomRight;

        [SerializeField] private AudioClip BlipSound;

        public static Dialouge Instance { get; private set; }

        private bool _Skip = false;

        public static string Text => Instance._text;
        public string MyText => _text;

        private void Awake()
        {
            Instance = this;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                Skip();
        }

        #region Static
        public static Coroutine Say(string DisplayText) =>
            Instance.SayInstance(DisplayText, defaultTimeBetweenLetters);

        public static Coroutine Say(string DisplayText, float timeBetweenLetters) =>
            Instance.SayInstance(DisplayText, timeBetweenLetters);

        public static Coroutine Append(string thingToAppend) =>
            Instance.AppendInstance(thingToAppend, defaultTimeBetweenLetters);

        public static Coroutine Append(string thingToAppend, float timeBetweenLetters) =>
            Instance.AppendInstance(thingToAppend, timeBetweenLetters);

        public static Coroutine Remove(int letters) =>
            Instance.RemoveInstance(letters, defaultTimeBetweenLetters);

        public static Coroutine Remove(int letters, float timeBetweenLetters) =>
            Instance.RemoveInstance(letters, timeBetweenLetters);

        public static Coroutine Clear() =>
            Instance.ClearInstance(-1);

        public static Coroutine Clear(float timeBetweenLetters) =>
            Instance.ClearInstance(timeBetweenLetters);

        public static void Show() =>
            Instance.ShowInstance();

        public static void Hide() =>
            Instance.HideInstance();

        public static void AddResponses(params Response[] responses) =>
            Instance.AddResponsesInstance(responses);

        public static void ClearResponses() =>
            Instance.ClearResponsesInstance();
			
		public static void Skip() =>
            Instance._Skip = true;
        #endregion

        #region Instance

        public Coroutine SayInstance(string TargetText, float timeBetweenLetters)
        {
            textComponent.text = "";
            return AppendInstance(TargetText, timeBetweenLetters);
        }

        public Coroutine AppendInstance(string thingToAppend, float timeBetweenLetters)
        {
            string currentText = textComponent.text;
            string targetText = currentText + thingToAppend;
            _text = targetText;

            bool Halt = false;
            //Prints it out
            return Timed.RepeatUntil(() =>
            {
                currentText = AddLetter(targetText, currentText, ref Halt);
                if (Halt)
                    currentText = targetText;

                textComponent.text = currentText;
                return currentText == targetText || Halt;
            }, timeBetweenLetters);
        }

        public Coroutine RemoveInstance(int letters, float timeBetweenLetters)
        {
            string currentText = textComponent.text;
            string targetText = currentText.Substring(0, currentText.Length - letters);
            _text = targetText;

            bool Halt = false;
            return Timed.RepeatUntil(() =>
            {
                currentText = currentText.Substring(0, currentText.Length - 1);
                textComponent.text = currentText;
                if (Halt)
                    currentText = targetText;

                Halt = _text != targetText;
                return currentText == targetText || Halt;
            }, timeBetweenLetters);
        }

        public Coroutine ClearInstance(float timeBetweenLetters)
        {
            if (timeBetweenLetters < 0)
            {
                textComponent.text = "";
                _text = "";
                return null;
            }

            return RemoveInstance(textComponent.text.Length, timeBetweenLetters);
        }

        public void ClearResponsesInstance()
        {
            if (currentResponses is null)
                return;

            for (int i = 0; i < currentResponses.Length; i++)
                Destroy(currentResponses[i].gameObject);
        }

        public void AddResponsesInstance(params Response[] responses)
        {
            ClearResponsesInstance();

            float start = bottomLeft.localPosition.x;
            float end = bottomRight.localPosition.x;

            currentResponses = new DialougeResponseComponent[responses.Length];

            /**Makes new responses**/
            for (int i = 0; i < responses.Length; i++)
            {
                var DialougeResponse = Instantiate(responseTemplate.gameObject, transform)
                                .GetComponent<DialougeResponseComponent>();

                //calculate position
                float newX = Mathf.Lerp(start, end, (i + .5f) / responses.Length);

                DialougeResponse.rectTransform.localPosition = new Vector3(newX, bottomLeft.localPosition.y);

                DialougeResponse.text.text = responses[i].text;
                DialougeResponse.button.onClick.AddListener(new UnityEngine.Events.UnityAction(responses[i].action));

                //set size
                DialougeResponse.rectTransform.sizeDelta = new Vector2(DialougeResponse.rectTransform.sizeDelta.x / (responses.Length) * 2,
                    DialougeResponse.rectTransform.sizeDelta.y);

                currentResponses[i] = DialougeResponse;
            }
        }

        public void ShowInstance() =>
            gameObject.SetActive(true);

        public void HideInstance() =>
            gameObject.SetActive(false);
			
		public void SkipInstance() =>
            _Skip = true;

        #endregion

        private string AddLetter(string TargetText, string CurrentText, ref bool Halt)
        {
            if (_Skip)
            {
                Halt = true;
                _Skip = false;
                return CurrentText;
            }

            //Another dialouge was started
            if (_text != TargetText)
            {
                Halt = true;
                return TargetText;
            }

            if (TargetText[CurrentText.Length] == '<')
            {
                string newString = GetStringFromAngleBrackets(CurrentText, CurrentText.Length);
                CurrentText += newString;
                return CurrentText;
            }

            CurrentText += TargetText[CurrentText.Length];

            if (BlipSound)
                AudioSource.PlayClipAtPoint(BlipSound, Camera.main.transform.position, .2f);

            return CurrentText;
        }

        private string GetStringFromAngleBrackets(string s, int start)
        {
            string newString = "";
            for (int i = start; i < s.Length; i++)
            {
                newString += s[i];
                if (s[i] == '>')
                    return newString;
            }
            return "<";
        }
    }
}
