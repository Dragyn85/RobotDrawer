using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;


public class CoordinateReader : MonoBehaviour
{
    Socket socket;

    [SerializeField] string IP = "127.0.0.1";
    [SerializeField] int portnumber = 1601;
    [SerializeField] Vector2 PaperSizeInMM;
    [SerializeField] float _minDistance = 0.5f;

    private bool connected;
    private bool waitingForResponse;
    Queue<SendData> sendQueue = new Queue<SendData>();
    Action OnConnected;
    Vector2 lastPos = new Vector2(-50, -50);

    private void Update()
    {
        if (!connected)
            return;


        if (waitingForResponse)
        {
            //GetResponsAsync();
        }


        if (Input.GetMouseButtonDown(0))
        {
            float posx = Input.mousePosition.x / Camera.main.pixelWidth;
            float posy = Input.mousePosition.y / Camera.main.pixelHeight;
            SetLastPos();
            Vector2 paperpos = CaluclatePaperPos(posx, posy);

            sendQueue.Enqueue(new SendData(paperpos.x, paperpos.y, true, false));

        }
        if (Input.GetMouseButton(0))
        {
            if (Vector2.Distance(lastPos, Input.mousePosition) > _minDistance)
            {
                float posx = Input.mousePosition.x / Camera.main.pixelWidth;
                float posy = Input.mousePosition.y / Camera.main.pixelHeight;
                SetLastPos();
                Vector2 paperpos = CaluclatePaperPos(posx, posy);

                sendQueue.Enqueue(new SendData(paperpos.x, paperpos.y, false, false));
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            float posx = Input.mousePosition.x / Camera.main.pixelWidth;
            float posy = Input.mousePosition.y / Camera.main.pixelHeight;
            SetLastPos();
            Vector2 paperpos = CaluclatePaperPos(posx, posy);
            sendQueue.Enqueue(new SendData(paperpos.x, paperpos.y, false, true));

        }


        if (!waitingForResponse && sendQueue.Count > 0)
        {
            var data = sendQueue.Dequeue();
            SendPosition(data.posx, data.posy, data.beginDraw, data.endDraw);
            GetResponsAsyncs();
        }
    }

    private void SetLastPos()
    {
        lastPos = Input.mousePosition;
    }

    private void GetRespons()
    {
        byte[] buffer = new byte[1024];
        int bytesRecived = socket.Receive(buffer);
        if (bytesRecived == 1)
        {
            waitingForResponse = false;
        }
    }
    void GetResponsAsyncs()
    {
        var buffer = new byte[4];
        var socketArgs = new SocketAsyncEventArgs();

        socketArgs.SetBuffer(buffer, 0, buffer.Length);

        var startedAsyncronousTask = socket.ReceiveAsync(socketArgs);

        if (startedAsyncronousTask)
        {
            socketArgs.Completed += handleCompletedSend;
        }
        else
        {
            waitingForResponse = false;
        }
    }


    private void handleCompletedSend(object sender, SocketAsyncEventArgs e)
    {
        e.Completed -= handleCompletedSend;
        waitingForResponse = false;
    }

    private Vector2 CaluclatePaperPos(float posx, float posy)
    {
        return new Vector2(posx * PaperSizeInMM.x, posy * PaperSizeInMM.y);
    }

    private void SendPosition(float posx, float posy, bool beginDraw = false, bool endDraw = false, bool debug = false)
    {
        if (!connected && !debug)
            return;
        byte[] bytes = GetBytesToSend(posx, posy, beginDraw, endDraw);
        if (!debug)
        {
            socket.Send(bytes);
            waitingForResponse = true;
        }


    }

    private static byte[] GetBytesToSend(float posx, float posy, bool beginDraw, bool endDraw)
    {
        string x = posx.ToString();
        string y = posy.ToString();
        x = x.Replace(',', '.');
        y = y.Replace(',', '.');

        StringBuilder sb = new StringBuilder();
        sb.Append("[");
        sb.Append(x);
        sb.Append(",");
        sb.Append(y);
        sb.Append(",");
        sb.Append(beginDraw);
        sb.Append(",");
        sb.Append(endDraw);
        sb.Append("]");

        string done = sb.ToString();
        var bytes = Encoding.ASCII.GetBytes(done);
        return bytes;
    }


    [ContextMenu("Connect")]
    public void ConnectToRobot(string ipAdress, string port, Action<ConnectionResults> OnConnectionCallback)
    {
        IP = ipAdress;
        portnumber = int.Parse(port);

        IPHostEntry host = Dns.GetHostEntry("localhost");
        //IPAddress ip = host.AddressList[0];

        IPAddress ip = IPAddress.Parse(IP);
        IPEndPoint iPEndPoint = new IPEndPoint(ip, portnumber);
        socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        StartCoroutine(connectTo(socket, iPEndPoint, OnConnectionCallback));
    }
    IEnumerator connectTo(Socket socket, IPEndPoint iPEndPoint, Action<ConnectionResults> OnCompletedCallback)
    {
        var connection = socket.ConnectAsync(iPEndPoint);
        while (!connection.IsCompleted)
        {
            yield return null;
        }
        if (connection.IsCompletedSuccessfully)
        {
            OnCompletedCallback?.Invoke(new ConnectionResults(true));
            connected = true;
        }
        else
        {
            OnCompletedCallback?.Invoke(new ConnectionResults(false));
        }

    }

    private void CompletedConnection(object sender, SocketAsyncEventArgs e)
    {
        e.Completed -= CompletedConnection;
        OnConnected?.Invoke();
        connected = true;
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


