using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridLayoutResizer : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] float coefficient;
    [SerializeField] bool isExpandHorizontal;

    [SerializeField] private bool waitForLoad;
    [SerializeField] [Range(0,5)] private float waitDuration;

    void Start()
    {
        if (waitForLoad)
        {
            StartCoroutine(WaitCo());
        }
        else
        {
            Resize();
        }
    }
    public void Resize()
    {
        GridLayoutGroup _gridLayout = GetComponent<GridLayoutGroup>();
        RectTransform _rectTransform = GetComponent<RectTransform>();

        if (isExpandHorizontal)
        {
            Vector2 _oldSize = _gridLayout.cellSize;
            float _height = _rectTransform.rect.height;
            float _newY = _height * coefficient;
            float _newX = _oldSize.x * _newY / _oldSize.y;
            _gridLayout.cellSize = new Vector2(_newX, _newY);
        }
        else
        {
            Vector2 _oldSize = _gridLayout.cellSize;
            float _width = _rectTransform.rect.width;
            float _newX = _width * coefficient;
            float _newY = _oldSize.y * _newX / _oldSize.x;
            _gridLayout.cellSize = new Vector2(_newX, _newY);
        }
    }
    IEnumerator WaitCo()
    {
        yield return new WaitForSeconds(waitDuration);
        Resize();
    }
}
