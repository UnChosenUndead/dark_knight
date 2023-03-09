using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseScript : MonoBehaviour
{
    public Texture2D cursorTexture;

    private readonly CursorMode _mode = CursorMode.ForceSoftware;

    private readonly Vector2 _hotspot = Vector2.zero;

    public GameObject mousePoint;

    private GameObject instatiatedMouse;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.SetCursor(cursorTexture, _hotspot, _mode);
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider is TerrainCollider)
                {
                    Vector3 temp = hit.point;
                    temp.y = 5.06f;

                    if (instatiatedMouse == null)
                    {
                        instatiatedMouse = Instantiate(mousePoint, temp, Quaternion.identity);
                    }
                    else
                    {
                        Destroy(instatiatedMouse);
                        instatiatedMouse = Instantiate(mousePoint, temp, Quaternion.identity);
                    }
                }
            }
        }
    }
}
