using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clicker : MonoBehaviour
{
    // Start is called before the first frame update
    MinionTarget hired;
    public Texture2D cursorTexture, cursorMinion;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if(hit.collider.gameObject.tag == "Minion")
        {
            Cursor.SetCursor(cursorMinion, hotSpot, cursorMode);
        } else {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
        if(Input.GetMouseButtonDown(0))
        {
            if(hit.collider.gameObject.tag == "Minion")
            {
                Debug.Log("Minion Clicked" + hit.collider.gameObject.name);
                hired = hit.collider.gameObject.GetComponent<MinionTarget>();
                hired.hired = true;
            }
        }
    }
}
