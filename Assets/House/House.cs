using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public enum StateHouse
{
    IsActive,
    NotActive,
    Neytral
}
public class House : Touch, IPointerClickHandler, IPointerDownHandler
{
    public Vector2Int sides;
    [HideInInspector] public ColorsObjects colorsObjects;
    [SerializeField] private Color clickColor;
    public int[,,] posOnMap;
    private void OnEnable()
    {
        colorsObjects = new ColorsObjects();
        ReturnOrInitColor();
    }
    private void Update()
    {
        if(stateHouse == StateHouse.IsActive && Input.touchCount != 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            endMove = true;
            stateHouse = StateHouse.NotActive;
            TakeObjects._house = null;
            ReturnOrInitColor();
        }
    }
    #region Colors
    public void ReturnOrInitColor()
    {
        colorsObjects.ReturnOrInitColor(this.transform);
    }
    public void InitColor()
    {
        colorsObjects.InitColor(clickColor);
    }
    #endregion
    #region Pointers
    public void OnPointerClick(PointerEventData eventData)
    {
        TakeObjects._house = this; //через те що дом буде активним при тому коли його беруть
        if (stateHouse == StateHouse.NotActive)
        {
            startMove = true;
            stateHouse = StateHouse.IsActive;
            colorsObjects.InitColor(clickColor);
        }
        if (stateHouse == StateHouse.Neytral)
        {
            stateHouse = StateHouse.IsActive;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(stateHouse == StateHouse.IsActive)
        {
            stateHouse = StateHouse.Neytral;
        }
    }
    #endregion
}


public class ColorsObjects : MonoBehaviour
{
    private List<Color> listStartColor;
    public ColorsObjects()
    {
        listStartColor = new List<Color>();
    }
    public void ReturnOrInitColor(Transform trnsf)
    {
        InitializeOrReturnColor(trnsf, Color.white);
    }
    public void InitColor(Color _clickColor)
    {
        InitializeOrReturnColor(TakeObjects._house.transform, _clickColor);
    }
    private void InitializeOrReturnColor(Transform trnsf, Color _color)
    {
        bool emptyList = false;
        int countChild = 0;
        if (listStartColor.Count == 0)
        {
            emptyList = true;
        }
        Init(trnsf, _color);
        void Init(Transform _trnsf, Color _color_)
        {
            if (_trnsf.childCount != 0)
            {
                for (int i = 0; i < _trnsf.childCount; i++)
                {
                    if (emptyList)
                    {
                        listStartColor.Add(_trnsf.GetChild(i).GetComponent<Renderer>().material.color);
                    }
                    else
                    {
                        if (_color_ == Color.white)
                        {
                            if (listStartColor[countChild] != null)
                            {
                                _trnsf.GetChild(i).GetComponent<Renderer>().material.color = listStartColor[countChild];
                            }
                        }
                        else
                        {
                            _trnsf.GetChild(i).GetComponent<Renderer>().material.color = _color_;
                        }
                    }
                    countChild++;
                    Init(_trnsf.GetChild(i), _color_);
                }
            }
        }
    }
}