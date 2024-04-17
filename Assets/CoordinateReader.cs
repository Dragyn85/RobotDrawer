using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEditor;
using UnityEngine;


public class CoordinateReader : MonoBehaviour
{
    [SerializeField] Vector2 PaperSizeInMM;
    ITakePositionData positionReciverTarget;
    [SerializeField] RectTransform positionTransform;
    [SerializeField] MousePositionWithinPanel mousePositionWithinPanel;

    [SerializeField] float _minDistance = 0.5f;

    private bool ready;

    Vector2 lastPos = new Vector2(-50, -50);
    private Camera mainCamera;

    public void SetITakePosition(ITakePositionData takePositionData)
    {
        positionReciverTarget = takePositionData;
        StartCoroutine(AwaitMouseUp());
    }

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private IEnumerator AwaitMouseUp()
    {
        while (!Input.GetMouseButtonUp(0))
        {
            yield return null;
        }

        yield return null;
        ready = true;
    }

    private void Update()
    {
        if (!ready && positionReciverTarget == null)
        {
            return;
        }

        

        if (Input.GetMouseButtonDown(0))
        {
            var paperpos = GetPaperPositionFromMousePositionInPanel();
            SetLastPos();
            positionReciverTarget.AddNewPositionData(new SendData(paperpos.x, paperpos.y, true, false));
        }

        if (Input.GetMouseButton(0))
        { 
            var distance = Vector2.Distance(lastPos, Input.mousePosition);
            if (distance > _minDistance)
            {
                Vector2 paperpos = GetPaperPositionFromMousePositionInPanel();
                SetLastPos();

                positionReciverTarget.AddNewPositionData(new SendData(paperpos.x, paperpos.y, false, false));
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            SetLastPos();
            Vector2 paperpos = GetPaperPositionFromMousePositionInPanel();

            positionReciverTarget.AddNewPositionData(new SendData(paperpos.x, paperpos.y, false, true));
        }
        
    }

    private Vector2 GetPaperPositionFromMousePositionInPanel()
    {
        var normalizedPosWithindDrawRect = mousePositionWithinPanel.GetNormalizedPosition(Input.mousePosition);

        float posx = Mathf.Clamp(normalizedPosWithindDrawRect.x, 0, 1);
        float posy = Mathf.Clamp(normalizedPosWithindDrawRect.y, 0, 1);
        Vector2 paperpos = CaluclatePaperPos(posx, posy);
        return paperpos;
    }

    private void OnDestroy()
    {
        if (positionReciverTarget != null)
            positionReciverTarget.EndProces();
    }

    private void SetLastPos()
    {
        lastPos = Input.mousePosition;
    }

    private Vector2 CaluclatePaperPos(float posx, float posy)
    {
        return new Vector2(posx * PaperSizeInMM.x, posy * PaperSizeInMM.y);
    }
}

public struct SendData
{
    public float posx;
    public float posy;
    public bool beginDraw;
    public bool endDraw;

    public SendData(float x, float y, bool begin, bool end)
    {
        posx = x;
        posy = y;
        beginDraw = begin;
        endDraw = end;
    }
}