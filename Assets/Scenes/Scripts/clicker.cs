using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clicker : MonoBehaviour
{
    // Start is called before the first frame update
    MinionTarget hired;
    public Texture2D cursorDefault, cursorMinion, cursorEnemy;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    void Start()
    {
        Cursor.SetCursor(cursorDefault, hotSpot, cursorMode);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);


        // check if collider is not a null
        if(hit.collider != null)
        {
            // if hovering over a minion
            if(hit.collider.gameObject.tag == "Minion")
            {
                Cursor.SetCursor(cursorMinion, hotSpot, cursorMode);
                if(Input.GetMouseButtonDown(0))
                {
                        // get component of the minion clicked and make them hired
                        MinionTarget minionController = hit.collider.gameObject.GetComponent<MinionTarget>();
                        minionController.hired = true;
                        if(minionController.status == "hostage")
                        {
                            minionController.status = "roam";
                            minionController.changeTargets();
                        }
                        Debug.Log("Minion: " + hit.collider.gameObject.name + " is now hired");
                }
            }
            else if(hit.collider.gameObject.tag == "Enemy")
            {
                Cursor.SetCursor(cursorEnemy, hotSpot, cursorMode);
                if(Input.GetMouseButtonDown(0))
                {
                    MainControllerScript controller = GameObject.Find("MainCharacter").GetComponent<MainControllerScript>();
                    if(controller != null)
                    {
                        controller.launchProjectile();
                    }
                }
                
            } else {
                // default cursor
                Cursor.SetCursor(cursorDefault, hotSpot, cursorMode);
            }
        } else {
                // default cursor
                Cursor.SetCursor(cursorDefault, hotSpot, cursorMode);
        }
    }
}
