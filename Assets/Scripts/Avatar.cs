using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Avatar : MonoBehaviour
{
    public Level level;
    public UI ui;

    public static float MOVING_SPEED = 1.25f;
    public bool isMovingCamera = false;
    public Vector3 aimingPosition = new Vector3 (0, 0, -30f);
    public Tile aimedTile = null;

    // Start is called before the first frame update
    void Start()
    {
        this.level = GameObject.FindObjectOfType<Level>();
        this.ui = GameObject.FindObjectOfType<UI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.isMovingCamera && this.aimedTile != null)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, this.aimedTile.getAimingPosition(), Time.deltaTime * 0.99f);
        }
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, this.aimingPosition, Time.deltaTime * 0.99f);
        }
    }

    public void setMoveCamera(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started && context.phase != InputActionPhase.Canceled)
        {
            this.isMovingCamera = true;
        }
        else
        {
            this.isMovingCamera = false;
            Tile tile = this.level.levelGen.getTileAt(this.transform.position);
            if (tile != null)
            {
                this.aimedTile = tile;
            }
        }

    }

    public void moveCamera(InputAction.CallbackContext context)
    {
        if (this.isMovingCamera)
        {
            Vector2 delta = context.ReadValue<Vector2>();
            this.aimingPosition += new Vector3(delta.x, delta.y, 0f) * Avatar.MOVING_SPEED * Time.deltaTime;
        }
    }
}
