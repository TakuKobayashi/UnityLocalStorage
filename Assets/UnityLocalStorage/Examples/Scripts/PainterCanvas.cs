using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityLocalStorage;

public class PainterCanvas : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] Image painterImage;
    [SerializeField] Text messageText;
    [SerializeField] bool showLocalStorageLog = true;
    [SerializeField] string encrypedPassword = "aqdai6*--8~7Tex(R*|pVARVI0OJeOF>";
    [SerializeField] string saveFileName = "localstorageeample";

    const string SaveLocalStorageKey = "drawPointsKey";

    Texture2D texture;
    Vector3 beforeMousePos;

    Color bgColor = Color.white;
    Color lineColor = Color.black;

    List<List<Vector3>> drawPointsList = new List<List<Vector3>>();
    List<Vector3> currentDrawPoints = new List<Vector3>();

    void Awake()
    {
        LocalStorage.LogEnabled = showLocalStorageLog;
        LocalStorage.EncyptPassword = encrypedPassword;
        LocalStorage.SaveFileName = saveFileName;
        LocalStorage.Setup();
    }

    void Start()
    {
        ReloadFromLocalStorage();
    }

    private void DrawMessageText()
    {
        if(drawPointsList.Count > 0 || currentDrawPoints.Count > 0)
        {
            messageText.gameObject.SetActive(false);
        }
        else
        {
            messageText.gameObject.SetActive(true);
        }
    }

    private void ClearDrawPainter()
    {
        var rt = painterImage.GetComponent<RectTransform>();
        var width = (int)rt.rect.width;
        var height = (int)rt.rect.height;
        texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        painterImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

        Color32[] texColors = Enumerable.Repeat<Color32>(bgColor, width * height).ToArray();
        texture.SetPixels32(texColors);
        texture.Apply();
    }

    private void DrawCurrentPainter()
    {
        foreach (List<Vector3> pathPoints in drawPointsList)
        {
            for(int i = 0;i < pathPoints.Count - 1; ++i)
            {
                Vector3 point = pathPoints[i];
                Vector3 nextPoint = pathPoints[i + 1];
                LineTo(point, nextPoint, lineColor);
            }
        }
        texture.Apply();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beforeMousePos = GetPosition(eventData);
        currentDrawPoints.Add(beforeMousePos);
        DrawMessageText();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 v = GetPosition(eventData);
        LineTo(beforeMousePos, v, lineColor);
        beforeMousePos = v;
        currentDrawPoints.Add(v);
        texture.Apply();
        DrawMessageText();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 v = GetPosition(eventData);
        LineTo(beforeMousePos, v, lineColor);
        texture.Apply();
        currentDrawPoints.Add(v);
        drawPointsList.Add(currentDrawPoints);
        LocalStorage.SetValue(SaveLocalStorageKey, drawPointsList);
        currentDrawPoints = new List<Vector3>();
        DrawMessageText();
    }

    public Vector3 GetPosition(PointerEventData dat)
    {
        var rect1 = GetComponent<RectTransform>();
        var pos1 = dat.position;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rect1, pos1,
            null, out Vector2 localCursor))
            return localCursor;

        int xpos = (int)(localCursor.x);
        int ypos = (int)(localCursor.y);

        if (xpos < 0) xpos = xpos + (int)rect1.rect.width / 2;
        else xpos += (int)rect1.rect.width / 2;

        if (ypos > 0) ypos = ypos + (int)rect1.rect.height / 2;
        else ypos += (int)rect1.rect.height / 2;
        return new Vector3(xpos, ypos, 0);
    }

    private void LineTo(Vector3 start, Vector3 end, Color color)
    {
        float x = start.x, y = start.y;
        // color of pixels
        Color[] wcolor = { color };

        if (Mathf.Abs(start.x - end.x) > Mathf.Abs(start.y - end.y))
        {
            float dy = Math.Abs(end.x - start.x) < float.Epsilon ? 0 : (end.y - start.y) / (end.x - start.x);
            float dx = start.x < end.x ? 1 : -1;
            //draw line loop
            while (x > 0 && x < texture.width && y > 0 && y < texture.height)
            {
                try
                {
                    texture.SetPixels((int)x, (int)y, 1, 1, wcolor);
                    x += dx;
                    y += dx * dy;
                    if (start.x < end.x && x > end.x ||
                        start.x > end.x && x < end.x)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    break;
                }
            }
        }
        else if (Mathf.Abs(start.x - end.x) < Mathf.Abs(start.y - end.y))
        {
            float dx = Math.Abs(start.y - end.y) < float.Epsilon ? 0 : (end.x - start.x) / (end.y - start.y);
            float dy = start.y < end.y ? 1 : -1;
            while (x > 0 && x < texture.width && y > 0 && y < texture.height)
            {
                try
                {
                    texture.SetPixels((int)x, (int)y, 1, 1, wcolor);
                    x += dx * dy;
                    y += dy;
                    if (start.y < end.y && y > end.y ||
                        start.y > end.y && y < end.y)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    break;
                }
            }
        }
    }

    public void ReloadFromLocalStorage()
    {
        LocalStorage.Reload();
        drawPointsList = LocalStorage.GetGenericObject<List<List<Vector3>>>(SaveLocalStorageKey, new List<List<Vector3>>());
        ClearDrawPainter();
        DrawCurrentPainter();
        DrawMessageText();
    }

    public void SaveLocalStorage()
    {
        LocalStorage.Save();
    }

    public void Clear()
    {
        drawPointsList.Clear();
        currentDrawPoints.Clear();
        LocalStorage.SetValue(SaveLocalStorageKey, drawPointsList);
        ClearDrawPainter();
        DrawMessageText();
    }
}
