using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TexturePainter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public RawImage rawImage;
    public float fadeDuration = 5.0f; // time in seconds to fade back to white

    private Texture2D texture;
    private bool isPainting = false;
    private float[,] timer;

    private Vector2 lastPosition = Vector2.zero;

    MousePositionWithinPanel mousePositionWithinPanel;

    private void Start()
    {
        mousePositionWithinPanel = GetComponent<MousePositionWithinPanel>();

        texture = new Texture2D((int) (420 * 1.414f), 420); // adjust size as needed
        rawImage.texture = texture;
        ResetTexture();
        timer = new float[texture.width, texture.height];
    }

    private void Update()
    {
        if (isPainting)
        {
            Paint(Input.mousePosition);
        }

        FadePixelsBack();
        texture.Apply();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Paint(eventData.position);
        isPainting = true;
    }

    void Paint(Vector2 screenPosition)
    {
        var normalizePosition = mousePositionWithinPanel.GetNormalizedPosition(screenPosition);
        Vector2 localCursor;
        if (normalizePosition.x >= 0 && normalizePosition.x <= 1 && normalizePosition.y >= 0 &&
            normalizePosition.y <= 1)
        {
            localCursor = new Vector2(
                normalizePosition.x * rawImage.rectTransform.rect.width - rawImage.rectTransform.rect.width / 2,
                normalizePosition.y * rawImage.rectTransform.rect.height - rawImage.rectTransform.rect.height / 2);
        }
        else
        {
            return;
        }

        // Convert localCursor to be relative to the top left of the texture
        float width = rawImage.rectTransform.rect.width;
        float height = rawImage.rectTransform.rect.height;
        int x = Mathf.Clamp((int) ((localCursor.x + width / 2) / width * texture.width), 0, texture.width - 1);
        int y = Mathf.Clamp((int) ((localCursor.y + height / 2) / height * texture.height), 0, texture.height - 1);

        Vector2 currentPosition = new Vector2(x, y);

        if (lastPosition != Vector2.zero)
        {
            DrawLine(lastPosition, currentPosition);
        }

        lastPosition = currentPosition;
    }

    void DrawLine(Vector2 start, Vector2 end)
    {
        int x0 = (int) start.x;
        int y0 = (int) start.y;
        int x1 = (int) end.x;
        int y1 = (int) end.y;

        int dx = Mathf.Abs(x1 - x0);
        int dy = -Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx + dy, e2; // error value e_xy

        while (true)
        {
            // Set the pixel and initialize its fade timer
            if (x0 >= 0 && x0 < texture.width && y0 >= 0 && y0 < texture.height)
            {
                texture.SetPixel(x0, y0, Color.black);
                timer[x0, y0] = fadeDuration; // Reset the timer for fading
            }

            if (x0 == x1 && y0 == y1) break;
            e2 = 2 * err;
            if (e2 >= dy)
            {
                err += dy;
                x0 += sx;
            }

            if (e2 <= dx)
            {
                err += dx;
                y0 += sy;
            }
        }

        texture.Apply();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPainting = false;
        lastPosition = Vector2.zero;
    }

    private void ResetTexture()
    {
        for (int i = 0; i < texture.width; i++)
        for (int j = 0; j < texture.height; j++)
            texture.SetPixel(i, j, Color.white);

        texture.Apply();
    }

    void FadePixelsBack()
    {
        bool updated = false;
        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            {
                if (timer[i, j] > 0)
                {
                    float alpha = timer[i, j] / fadeDuration;
                    texture.SetPixel(i, j, Color.Lerp(Color.white, Color.black, alpha));
                    timer[i, j] -= Time.deltaTime;
                    updated = true;
                }
            }
        }
        if (updated) {
            texture.Apply();
        }
    }
}