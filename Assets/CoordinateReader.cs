using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;


public class CoordinateReader : MonoBehaviour
{
    
    [SerializeField] Vector2 PaperSizeInMM;
    ITakePositionData positionReciverTarget;

    [SerializeField] float _minDistance = 0.5f;

    private bool connected;
    private bool waitingForResponse;
    Vector2 lastPos = new Vector2(-50, -50);
    
   
    public void SetITakePosition(ITakePositionData takePositionData)
    {
        positionReciverTarget = takePositionData;
    }
    private void Update()
    {
        if(positionReciverTarget == null)
        {
            return;
        }
        if (!positionReciverTarget.ReadyToRecieve)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            float posx = Input.mousePosition.x / Camera.main.pixelWidth;
            float posy = Input.mousePosition.y / Camera.main.pixelHeight;
            SetLastPos();
            Vector2 paperpos = CaluclatePaperPos(posx, posy);

            positionReciverTarget.AddNewPositionData(new SendData(paperpos.x, paperpos.y, true, false));

        }
        if (Input.GetMouseButton(0))
        {
            if (Vector2.Distance(lastPos, Input.mousePosition) > _minDistance)
            {
                float posx = Input.mousePosition.x / Camera.main.pixelWidth;
                float posy = Input.mousePosition.y / Camera.main.pixelHeight;
                SetLastPos();
                Vector2 paperpos = CaluclatePaperPos(posx, posy);

                positionReciverTarget.AddNewPositionData(new SendData(paperpos.x, paperpos.y, false, false));
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            float posx = Input.mousePosition.x / Camera.main.pixelWidth;
            float posy = Input.mousePosition.y / Camera.main.pixelHeight;
            SetLastPos();
            Vector2 paperpos = CaluclatePaperPos(posx, posy);
            positionReciverTarget.AddNewPositionData(new SendData(paperpos.x, paperpos.y, false, true));

        }


        
    }
    private void OnDestroy()
    {
        if(positionReciverTarget != null)
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


