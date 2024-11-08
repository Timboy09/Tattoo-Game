using System;
using System.Collections.Generic;
using System.Linq;
using FreeDraw;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecognitionManager : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private Drawable _drawable;
    [SerializeField] private TextMeshProUGUI _recognitionResult;
    [SerializeField] private Button _templateModeButton;
    [SerializeField] private Button _recognitionModeButton;
    [SerializeField] private Button _reviewTemplates;
    [SerializeField] private TMP_InputField _templateName;
    [SerializeField] private TemplateReviewPanel _templateReviewPanel;
    [SerializeField] private RecognitionPanel _recognitionPanel;

    private GestureTemplates _templates => GestureTemplates.Get();
    private static readonly DollarOneRecognizer _dollarOneRecognizer = new DollarOneRecognizer();
    private static readonly DollarPRecognizer _dollarPRecognizer = new DollarPRecognizer();
    private IRecognizer _currentRecognizer = _dollarPRecognizer;
    private RecognizerState _state = RecognizerState.RECOGNITION;

    public enum RecognizerState
    {
        TEMPLATE,
        RECOGNITION,
        TEMPLATE_REVIEW
    }

    [Serializable]
    public struct GestureTemplate
    {
        public string Name;
        public DollarPoint[] Points;

        public GestureTemplate(string templateName, DollarPoint[] preparePoints)
        {
            Name = templateName;
            Points = preparePoints;
        }
    }

    private string TemplateName => _templateName.text;


    private void Start()
    {
        gameManager = GameManager.instance;

        _drawable.OnDrawFinished += OnDrawFinished;
        if (_templateModeButton != null) 
        {
            _templateModeButton.onClick.AddListener(() => SetupState(RecognizerState.TEMPLATE));
        }
        if (_recognitionModeButton != null)
        {
            _recognitionModeButton.onClick.AddListener(() => SetupState(RecognizerState.RECOGNITION));
        }
        if (_reviewTemplates != null)
        {
            _reviewTemplates.onClick.AddListener(() => SetupState(RecognizerState.TEMPLATE_REVIEW));
        }
        if (_recognitionPanel != null)
        {
            _recognitionPanel.Initialize(SwitchRecognitionAlgorithm);
        }

        SetupState(_state);
    }

    private void SwitchRecognitionAlgorithm(int algorithm)
    {
        if (algorithm == 0)
        {
            _currentRecognizer = _dollarOneRecognizer;
        }

        if (algorithm == 1)
        {
            _currentRecognizer = _dollarPRecognizer;
        }
    }

    private void SetupState(RecognizerState state)
    {
        _state = state;
        if (_templateModeButton != null)
        {
            _templateModeButton.image.color = _state == RecognizerState.TEMPLATE ? Color.green : Color.white;
        }
        if (_recognitionModeButton != null)
        {
            _recognitionModeButton.image.color = _state == RecognizerState.RECOGNITION ? Color.green : Color.white;
        }
        if (_reviewTemplates != null)
        {
            _reviewTemplates.image.color = _state == RecognizerState.TEMPLATE_REVIEW ? Color.green : Color.white;
        }
        if (_templateName != null)
        {
            _templateName.gameObject.SetActive(_state == RecognizerState.TEMPLATE);
        }
        if(_recognitionResult != null)
        {
            _recognitionResult.gameObject.SetActive(_state == RecognizerState.RECOGNITION);
        }

        _drawable.gameObject.SetActive(state != RecognizerState.TEMPLATE_REVIEW);
        if (_templateReviewPanel != null)
        {
            _templateReviewPanel.SetVisibility(state == RecognizerState.TEMPLATE_REVIEW);
        }
        if (_recognitionPanel != null)
        {
            _recognitionPanel.SetVisibility(state == RecognizerState.RECOGNITION);
        }
    }

    private void OnDrawFinished(DollarPoint[] points)
    {
        if (_state == RecognizerState.TEMPLATE)
        {
            GestureTemplate preparedTemplate =
                new GestureTemplate(TemplateName, _currentRecognizer.Normalize(points, 64));
            _templates.RawTemplates.Add(new GestureTemplate(TemplateName, points));
            _templates.ProceedTemplates.Add(preparedTemplate);
        }
        else
        {
            //  (string, float) result = _dollarOneRecognizer.DoRecognition(points, 64, _templates.GetTemplates());
            (string, float) result = _currentRecognizer.DoRecognition(points, 64,
                _templates.RawTemplates);
            string resultText = "";
            if (_currentRecognizer is DollarOneRecognizer)
            {
                resultText = $"Recognized: {result.Item1}, Score: {result.Item2}";
            }
            else if (_currentRecognizer is DollarPRecognizer)
            {
                resultText = $"Recognized: {result.Item1}, Distance: {result.Item2}";
            }

            //recognitionResult.text = resultText;
            Debug.Log(resultText);
            CreateTattoo(result.Item1, result.Item2);
        }
    }

    private void CreateTattoo(string _shapeData, float _accuracy)
    {
        Shapes targetShape = Shapes.MIN;
        bool shapeDetected = false;

        if (gameManager.tattooShapes.Count >= 1)
        {
            targetShape = gameManager.tattooShapes.Find(x => x == (Shapes)Enum.Parse(typeof(Shapes), _shapeData));
            if(_accuracy > 0.7f && _accuracy < 2f)
            {
                shapeDetected = true;
                gameManager.tattooShapes.Remove(targetShape);
            }
            else
            {
                shapeDetected = false;
            }
        }

        if (shapeDetected)
        {
            gameManager.DrawTattooShapes(targetShape);
        }
        else
        {
            //Issue Warning
            Debug.Log(_shapeData.ToString() + " " + _accuracy);
            gameManager.uiManager.TriggerTattooWarning();
        }

        if(gameManager.tattooShapes.Count == 0)
        {
            Debug.Log("Tattoo Complete");
            StartCoroutine(gameManager.EndTattooGame());
        }
    }

    private void OnApplicationQuit()
    {
        _templates.Save();
    }
}