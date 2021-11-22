using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GraphController : MonoBehaviour
{
    public static GraphController instance;

    public RectTransform graphContainer;
    public Sprite dotSprite;
    public RectTransform labelTemplateX;
    public RectTransform labelTemplateY;
    public RectTransform dashTemplateX;
    public RectTransform dashTemplateY;
    private List<GameObject> gameObjectList;
    public GameObject tooltipGameObject;

    private void Awake()
    {
        gameObjectList = new List<GameObject>();
        instance = this;
        //List<float> valueList = new List<float>
        //(){
        //    5,98,56,-45,30,22,17,15,-13,17,25,37,-40,36,33
        //};
        //// ShowGraph(valueList);
        ////ShowGraph(valueList, -1, (int _i) => "" + (_i + 1), (float _f) => "" + Mathf.RoundToInt(_f));
        //ECGInfo info = ECGInfo.CreateFromJSON();
        //List<float> lst = info.data.OfType<float>().ToList(); // this isn't going to be fast.
        //ShowGraph(lst, info.features, -1, (int _i) => "" + (_i + 1), (float _f) => "" + _f.ToString("0.00"));
    }

    public void ShowGraph(List<float> valueList, Features features, int maxVisibleValueAmount = -1, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {
        if (getAxisLabelX == null)
        {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        if (getAxisLabelY == null)
        {
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }

        if (maxVisibleValueAmount <= 0)
        {
            maxVisibleValueAmount = valueList.Count;
        }

        foreach (GameObject gameObject in gameObjectList)
        {
            Destroy(gameObject);
        }
        gameObjectList.Clear();

        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;

        float yMaximum = float.MinValue;
        float yMinimum = float.MaxValue;

        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
        {
            float value = valueList[i];
            if (value > yMaximum)
            {
                yMaximum = value;
            }
            if (value < yMinimum)
            {
                yMinimum = value;
            }
        }

        float yDifference = yMaximum - yMinimum;
        if (yDifference <= 0)
        {
            yDifference = 5f;
        }
        yMaximum = yMaximum + (yDifference * 0.2f);
        yMinimum = yMinimum - (yDifference * 0.2f);

        //yMinimum = 0f; // Start the graph at zero

        float xSize = graphWidth / (maxVisibleValueAmount + 1);

        int xIndex = 0;

        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-10f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
            gameObjectList.Add(labelY.gameObject);

            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(0f, normalizedValue * graphHeight);
            gameObjectList.Add(dashY.gameObject);
        }


        Dictionary<int, Vector2> indexToNormPos = new Dictionary<int, Vector2>();
        GameObject lastDotGameObject = null;
        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
        {
            float xPosition = xSize + xIndex * xSize;
            float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;
            GameObject dotGameObject = CreateDot(new Vector2(xPosition, yPosition));
            indexToNormPos[i] = new Vector2(xPosition, yPosition);
            gameObjectList.Add(dotGameObject);
            if (lastDotGameObject != null)
            {
                GameObject dotConnectionGameObject = CreateDotConnection(lastDotGameObject.GetComponent<RectTransform>().anchoredPosition, dotGameObject.GetComponent<RectTransform>().anchoredPosition);
                gameObjectList.Add(dotConnectionGameObject);
            }
            lastDotGameObject = dotGameObject;







            xIndex++;
        }



        List<int> pFeatures = features.p.OfType<int>().ToList();
        List<int> qFeatures = features.q.OfType<int>().ToList();
        List<int> rFeatures = features.r.OfType<int>().ToList();
        List<int> sFeatures = features.s.OfType<int>().ToList();
        List<int> tFeatures = features.t.OfType<int>().ToList();

        for (int i = 0; i < pFeatures.Count; i++)
        {
            int currentIndex = pFeatures[i];
            Vector2 pos = indexToNormPos[currentIndex];
            RectTransform dashX = Instantiate(dashTemplateX);
            RectTransform labelX = Instantiate(labelTemplateX);
            GameObject featureDot = CreatePFeatureDot(pos, indexToNormPos, currentIndex, dashX, labelX);
            featureDot.GetComponent<FeatureDot>().currentIndex = currentIndex;
            gameObjectList.Add(featureDot.gameObject);


            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(pos.x, -7f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);
            gameObjectList.Add(labelX.gameObject);


            labelX.GetComponent<Text>().text = "P";

            EventTrigger myEventTrigger = featureDot.GetComponent<EventTrigger>();
            myEventTrigger.AddListener(EventTriggerType.PointerEnter, delegate
            {
                string tooltipText = getAxisLabelY(valueList[featureDot.GetComponent<FeatureDot>().currentIndex]);
                tooltipText = "P: " + getAxisLabelY(valueList[featureDot.GetComponent<FeatureDot>().currentIndex]);
                ShowTooltip(tooltipText, featureDot.GetComponent<RectTransform>().anchoredPosition);
            });
            myEventTrigger.AddListener(EventTriggerType.PointerExit, delegate
            {
                HideToolTip();
            });


            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(pos.x, 0f);
            gameObjectList.Add(dashX.gameObject);
            featureDot.transform.SetAsLastSibling();
        }
        for (int i = 0; i < qFeatures.Count; i++)
        {
            int currentIndex = qFeatures[i];
            Vector2 pos = indexToNormPos[currentIndex];
            RectTransform dashX = Instantiate(dashTemplateX);
            RectTransform labelX = Instantiate(labelTemplateX);
            GameObject featureDot = CreateQFeatureDot(pos, indexToNormPos, currentIndex, dashX, labelX);
            featureDot.GetComponent<FeatureDot>().currentIndex = currentIndex;
            gameObjectList.Add(featureDot.gameObject);

            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(pos.x, -7f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);
            gameObjectList.Add(labelX.gameObject);


            labelX.GetComponent<Text>().text = "Q";

            EventTrigger myEventTrigger = featureDot.GetComponent<EventTrigger>();
            myEventTrigger.AddListener(EventTriggerType.PointerEnter, delegate
            {
                string tooltipText = getAxisLabelY(valueList[featureDot.GetComponent<FeatureDot>().currentIndex]);
                tooltipText = "Q: " + getAxisLabelY(valueList[featureDot.GetComponent<FeatureDot>().currentIndex]);
                ShowTooltip(tooltipText, featureDot.GetComponent<RectTransform>().anchoredPosition);
            });
            myEventTrigger.AddListener(EventTriggerType.PointerExit, delegate
            {
                HideToolTip();
            });

            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(pos.x, 0f);
            gameObjectList.Add(dashX.gameObject);
            featureDot.transform.SetAsLastSibling();
        }
        for (int i = 0; i < rFeatures.Count; i++)
        {
            int currentIndex = rFeatures[i];
            Vector2 pos = indexToNormPos[currentIndex];
            RectTransform dashX = Instantiate(dashTemplateX);
            RectTransform labelX = Instantiate(labelTemplateX);
            GameObject featureDot = CreateRFeatureDot(pos, indexToNormPos, currentIndex, dashX, labelX);
            featureDot.GetComponent<FeatureDot>().currentIndex = currentIndex;
            gameObjectList.Add(featureDot.gameObject);

            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(pos.x, -7f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);
            gameObjectList.Add(labelX.gameObject);


            labelX.GetComponent<Text>().text = "R";

            EventTrigger myEventTrigger = featureDot.GetComponent<EventTrigger>();
            myEventTrigger.AddListener(EventTriggerType.PointerEnter, delegate
            {
                string tooltipText = getAxisLabelY(valueList[featureDot.GetComponent<FeatureDot>().currentIndex]);
                tooltipText = "R: " + getAxisLabelY(valueList[featureDot.GetComponent<FeatureDot>().currentIndex]);
                ShowTooltip(tooltipText, featureDot.GetComponent<RectTransform>().anchoredPosition);
            });
            myEventTrigger.AddListener(EventTriggerType.PointerExit, delegate
            {
                HideToolTip();
            });

            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(pos.x, 0f);
            gameObjectList.Add(dashX.gameObject);
            featureDot.transform.SetAsLastSibling();
        }
        for (int i = 0; i < sFeatures.Count; i++)
        {
            int currentIndex = sFeatures[i];
            Vector2 pos = indexToNormPos[currentIndex];
            RectTransform dashX = Instantiate(dashTemplateX);
            RectTransform labelX = Instantiate(labelTemplateX);
            GameObject featureDot = CreateSFeatureDot(pos, indexToNormPos, currentIndex, dashX, labelX);
            featureDot.GetComponent<FeatureDot>().currentIndex = currentIndex;
            gameObjectList.Add(featureDot.gameObject);

            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(pos.x, -7f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);
            gameObjectList.Add(labelX.gameObject);


            labelX.GetComponent<Text>().text = "S";

            EventTrigger myEventTrigger = featureDot.GetComponent<EventTrigger>();
            myEventTrigger.AddListener(EventTriggerType.PointerEnter, delegate
            {
                string tooltipText = getAxisLabelY(valueList[featureDot.GetComponent<FeatureDot>().currentIndex]);
                tooltipText = "S: " + getAxisLabelY(valueList[featureDot.GetComponent<FeatureDot>().currentIndex]);
                ShowTooltip(tooltipText, featureDot.GetComponent<RectTransform>().anchoredPosition);
            });
            myEventTrigger.AddListener(EventTriggerType.PointerExit, delegate
            {
                HideToolTip();
            });

            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(pos.x, 0f);
            gameObjectList.Add(dashX.gameObject);
            featureDot.transform.SetAsLastSibling();
        }
        for (int i = 0; i < tFeatures.Count; i++)
        {
            int currentIndex = tFeatures[i];
            Vector2 pos = indexToNormPos[currentIndex];
            RectTransform dashX = Instantiate(dashTemplateX);
            RectTransform labelX = Instantiate(labelTemplateX);
            GameObject featureDot = CreateTFeatureDot(pos, indexToNormPos, currentIndex, dashX, labelX);
            featureDot.GetComponent<FeatureDot>().currentIndex = currentIndex;
            gameObjectList.Add(featureDot.gameObject);

            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(pos.x, -7f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);
            gameObjectList.Add(labelX.gameObject);


            labelX.GetComponent<Text>().text = "T";

            EventTrigger myEventTrigger = featureDot.GetComponent<EventTrigger>();
            myEventTrigger.AddListener(EventTriggerType.PointerEnter, delegate
            {
                string tooltipText = getAxisLabelY(valueList[featureDot.GetComponent<FeatureDot>().currentIndex]);
                tooltipText = "T: " + getAxisLabelY(valueList[featureDot.GetComponent<FeatureDot>().currentIndex]);
                ShowTooltip(tooltipText, featureDot.GetComponent<RectTransform>().anchoredPosition);
            });
            myEventTrigger.AddListener(EventTriggerType.PointerExit, delegate
            {
                HideToolTip();
            });

            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(pos.x, 0f);
            gameObjectList.Add(dashX.gameObject);
            featureDot.transform.SetAsLastSibling();
        }
    }

    private GameObject CreateDot(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("dot", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = dotSprite;
        gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
        gameObject.AddComponent<EventTrigger>();
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(0, 0);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private bool pressing = false;
    private IEnumerator handleMouseClick(GameObject go, Vector3 mousePos, int initIndex, Action<int> increase)
    {
        yield return new WaitUntil(delegate
        {
            Vector3 currentPos = Input.mousePosition;
            if (currentPos.x > mousePos.x)
            {

                increase?.Invoke(++initIndex);

            }
            if (currentPos.x < mousePos.x)
            {

                increase?.Invoke(--initIndex);

            }
            mousePos = currentPos;
            return Input.GetMouseButtonUp(0);
        });



    }


    private GameObject CreatePFeatureDot(Vector2 anchoredPosition, Dictionary<int, Vector2> indexToPos, int initIndex, RectTransform dashX, RectTransform labelX)
    {
        GameObject featureDot = new GameObject("Feature Dot", typeof(Image));
        featureDot.AddComponent<FeatureDot>();
        featureDot.transform.SetParent(graphContainer, false);
        featureDot.GetComponent<Image>().color = new Color(0 / 255f, 100 / 255f, 255 / 255f, 1);

        featureDot.AddComponent<EventTrigger>();
        featureDot.GetComponent<Image>().sprite = dotSprite;
        RectTransform rectTransform = featureDot.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(5, 5);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        EventTrigger myEventTrigger = featureDot.GetComponent<EventTrigger>();
        myEventTrigger.AddListener(EventTriggerType.PointerDown, delegate
        {
            if (CurrentUserModel.instance.userInfo.role.ToUpper() == Config.PROFESSIONAL_NAME)
            {
                Debug.Log("Kabast");
                pressing = true;
                StartCoroutine(handleMouseClick(featureDot, Input.mousePosition, initIndex, (newIndex) =>
                    {
                        rectTransform.anchoredPosition = indexToPos[newIndex];
                        List<int> test = ECGView.instance.features.p.OfType<int>().ToList();
                        int indexToReplace = test.IndexOf(initIndex);
                        ECGView.instance.features.p[indexToReplace] = newIndex;
                        initIndex = newIndex;
                        featureDot.GetComponent<FeatureDot>().currentIndex = newIndex;
                        rectTransform.transform.SetAsLastSibling();
                        dashX.anchoredPosition = new Vector2(indexToPos[newIndex].x, 0f);
                        labelX.anchoredPosition = new Vector2(indexToPos[newIndex].x, -7f);

                    }));
            }
        });
        myEventTrigger.AddListener(EventTriggerType.PointerUp, delegate
        {
            if (CurrentUserModel.instance.userInfo.role.ToUpper() == Config.PROFESSIONAL_NAME)
            {
                StopCoroutine(handleMouseClick(featureDot, Input.mousePosition, initIndex, (newIndex) =>
                {
                    rectTransform.anchoredPosition = indexToPos[newIndex];
                }));
            }
        });

        return featureDot;
    }

    private GameObject CreateQFeatureDot(Vector2 anchoredPosition, Dictionary<int, Vector2> indexToPos, int initIndex, RectTransform dashX, RectTransform labelX)
    {
        GameObject featureDot = new GameObject("Feature Dot", typeof(Image));
        featureDot.AddComponent<FeatureDot>();
        featureDot.transform.SetParent(graphContainer, false);
        featureDot.GetComponent<Image>().color = new Color(0 / 255f, 100 / 255f, 255 / 255f, 1);

        featureDot.AddComponent<EventTrigger>();
        featureDot.GetComponent<Image>().sprite = dotSprite;
        RectTransform rectTransform = featureDot.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(5, 5);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        EventTrigger myEventTrigger = featureDot.GetComponent<EventTrigger>();
        myEventTrigger.AddListener(EventTriggerType.PointerDown, delegate
        {
            if (CurrentUserModel.instance.userInfo.role.ToUpper() == Config.PROFESSIONAL_NAME)
            {
                Debug.Log("Kabast");
                pressing = true;
                StartCoroutine(handleMouseClick(featureDot, Input.mousePosition, initIndex, (newIndex) =>
                {
                    rectTransform.anchoredPosition = indexToPos[newIndex];
                    List<int> test = ECGView.instance.features.q.OfType<int>().ToList();
                    int indexToReplace = test.IndexOf(initIndex);
                    ECGView.instance.features.q[indexToReplace] = newIndex;
                    initIndex = newIndex;
                    featureDot.GetComponent<FeatureDot>().currentIndex = newIndex;
                    rectTransform.transform.SetAsLastSibling();
                    dashX.anchoredPosition = new Vector2(indexToPos[newIndex].x, 0f);
                    labelX.anchoredPosition = new Vector2(indexToPos[newIndex].x, -7f);

                }));
            }
        });
        myEventTrigger.AddListener(EventTriggerType.PointerUp, delegate
        {
            if (CurrentUserModel.instance.userInfo.role.ToUpper() == Config.PROFESSIONAL_NAME)
            {
                StopCoroutine(handleMouseClick(featureDot, Input.mousePosition, initIndex, (newIndex) =>
                {
                    rectTransform.anchoredPosition = indexToPos[newIndex];
                }));
            }
        });

        return featureDot;
    }
    private GameObject CreateRFeatureDot(Vector2 anchoredPosition, Dictionary<int, Vector2> indexToPos, int initIndex, RectTransform dashX, RectTransform labelX)
    {
        GameObject featureDot = new GameObject("Feature Dot", typeof(Image));
        featureDot.AddComponent<FeatureDot>();
        featureDot.transform.SetParent(graphContainer, false);
        featureDot.GetComponent<Image>().color = new Color(0 / 255f, 100 / 255f, 255 / 255f, 1);

        featureDot.AddComponent<EventTrigger>();
        featureDot.GetComponent<Image>().sprite = dotSprite;
        RectTransform rectTransform = featureDot.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(5, 5);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        EventTrigger myEventTrigger = featureDot.GetComponent<EventTrigger>();
        myEventTrigger.AddListener(EventTriggerType.PointerDown, delegate
        {
            if (CurrentUserModel.instance.userInfo.role.ToUpper() == Config.PROFESSIONAL_NAME)
            {
                Debug.Log("Kabast");
                pressing = true;
                StartCoroutine(handleMouseClick(featureDot, Input.mousePosition, initIndex, (newIndex) =>
                {
                    rectTransform.anchoredPosition = indexToPos[newIndex];
                    List<int> test = ECGView.instance.features.r.OfType<int>().ToList();
                    int indexToReplace = test.IndexOf(initIndex);
                    ECGView.instance.features.r[indexToReplace] = newIndex;
                    initIndex = newIndex;
                    featureDot.GetComponent<FeatureDot>().currentIndex = newIndex;
                    rectTransform.transform.SetAsLastSibling();
                    dashX.anchoredPosition = new Vector2(indexToPos[newIndex].x, 0f);
                    labelX.anchoredPosition = new Vector2(indexToPos[newIndex].x, -7f);

                }));
            }
        });
        myEventTrigger.AddListener(EventTriggerType.PointerUp, delegate
        {
            if (CurrentUserModel.instance.userInfo.role.ToUpper() == Config.PROFESSIONAL_NAME)
            {
                StopCoroutine(handleMouseClick(featureDot, Input.mousePosition, initIndex, (newIndex) =>
                {
                    rectTransform.anchoredPosition = indexToPos[newIndex];
                }));
            }
        });

        return featureDot;
    }
    private GameObject CreateSFeatureDot(Vector2 anchoredPosition, Dictionary<int, Vector2> indexToPos, int initIndex, RectTransform dashX, RectTransform labelX)
    {
        GameObject featureDot = new GameObject("Feature Dot", typeof(Image));
        featureDot.AddComponent<FeatureDot>();
        featureDot.transform.SetParent(graphContainer, false);
        featureDot.GetComponent<Image>().color = new Color(0 / 255f, 100 / 255f, 255 / 255f, 1);

        featureDot.AddComponent<EventTrigger>();
        featureDot.GetComponent<Image>().sprite = dotSprite;
        RectTransform rectTransform = featureDot.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(5, 5);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        EventTrigger myEventTrigger = featureDot.GetComponent<EventTrigger>();
        myEventTrigger.AddListener(EventTriggerType.PointerDown, delegate
        {
            if (CurrentUserModel.instance.userInfo.role.ToUpper() == Config.PROFESSIONAL_NAME)
            {
                Debug.Log("Kabast");
                pressing = true;
                StartCoroutine(handleMouseClick(featureDot, Input.mousePosition, initIndex, (newIndex) =>
                {
                    rectTransform.anchoredPosition = indexToPos[newIndex];
                    List<int> test = ECGView.instance.features.s.OfType<int>().ToList();
                    int indexToReplace = test.IndexOf(initIndex);
                    ECGView.instance.features.s[indexToReplace] = newIndex;
                    initIndex = newIndex;
                    featureDot.GetComponent<FeatureDot>().currentIndex = newIndex;
                    rectTransform.transform.SetAsLastSibling();
                    dashX.anchoredPosition = new Vector2(indexToPos[newIndex].x, 0f);
                    labelX.anchoredPosition = new Vector2(indexToPos[newIndex].x, -7f);

                }));
            }
        });
        myEventTrigger.AddListener(EventTriggerType.PointerUp, delegate
        {
            if (CurrentUserModel.instance.userInfo.role.ToUpper() == Config.PROFESSIONAL_NAME)
            {
                StopCoroutine(handleMouseClick(featureDot, Input.mousePosition, initIndex, (newIndex) =>
                {
                    rectTransform.anchoredPosition = indexToPos[newIndex];
                }));
            }
        });

        return featureDot;
    }
    private GameObject CreateTFeatureDot(Vector2 anchoredPosition, Dictionary<int, Vector2> indexToPos, int initIndex, RectTransform dashX, RectTransform labelX)
    {
        GameObject featureDot = new GameObject("Feature Dot", typeof(Image));
        featureDot.AddComponent<FeatureDot>();
        featureDot.transform.SetParent(graphContainer, false);
        featureDot.GetComponent<Image>().color = new Color(0 / 255f, 100 / 255f, 255 / 255f, 1);

        featureDot.AddComponent<EventTrigger>();
        featureDot.GetComponent<Image>().sprite = dotSprite;
        RectTransform rectTransform = featureDot.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(5, 5);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        EventTrigger myEventTrigger = featureDot.GetComponent<EventTrigger>();
        myEventTrigger.AddListener(EventTriggerType.PointerDown, delegate
        {
            if (CurrentUserModel.instance.userInfo.role.ToUpper() == Config.PROFESSIONAL_NAME)
            {
                Debug.Log("Kabast");
                pressing = true;
                StartCoroutine(handleMouseClick(featureDot, Input.mousePosition, initIndex, (newIndex) =>
                {
                    rectTransform.anchoredPosition = indexToPos[newIndex];
                    List<int> test = ECGView.instance.features.t.OfType<int>().ToList();
                    int indexToReplace = test.IndexOf(initIndex);
                    ECGView.instance.features.t[indexToReplace] = newIndex;
                    initIndex = newIndex;
                    featureDot.GetComponent<FeatureDot>().currentIndex = newIndex;
                    rectTransform.transform.SetAsLastSibling();
                    dashX.anchoredPosition = new Vector2(indexToPos[newIndex].x, 0f);
                    labelX.anchoredPosition = new Vector2(indexToPos[newIndex].x, -7f);

                }));
            }
        });
        myEventTrigger.AddListener(EventTriggerType.PointerUp, delegate
        {
            if (CurrentUserModel.instance.userInfo.role.ToUpper() == Config.PROFESSIONAL_NAME)
            {
                StopCoroutine(handleMouseClick(featureDot, Input.mousePosition, initIndex, (newIndex) =>
                {
                    rectTransform.anchoredPosition = indexToPos[newIndex];
                }));
            }
        });

        return featureDot;
    }

    private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(0, 0, 0, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 1);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rectTransform.localEulerAngles = new Vector3(0, 0, angle);
        return gameObject;
    }

    private void ShowTooltip(string tooltipText, Vector2 anchoredPos)
    {
        tooltipGameObject.SetActive(true);
        tooltipGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
        Text tooltipUIText = tooltipGameObject.transform.Find("text").GetComponent<Text>();
        tooltipUIText.text = tooltipText;
        float textPadding = 4f;
        Vector2 backgroundSize = new Vector2(
        2f * textPadding + tooltipUIText.preferredWidth,
        2f * textPadding + tooltipUIText.preferredHeight);
        tooltipGameObject.transform.Find("background").GetComponent<RectTransform>().sizeDelta = backgroundSize;
        tooltipGameObject.transform.SetAsLastSibling();
    }
    private void HideToolTip()
    {
        tooltipGameObject.SetActive(false);
    }

}
public static class ExtensionMethods
{
    public static void AddListener(this EventTrigger trigger, EventTriggerType eventType, System.Action<PointerEventData> listener)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener(data => listener.Invoke((PointerEventData)data));
        trigger.triggers.Add(entry);
    }
}
