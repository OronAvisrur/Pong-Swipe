using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCharacter : MonoBehaviour
{
    protected float x;
    protected float y;
    protected float movementSpeed;

    public float MovementSpeed {get { return movementSpeed; } set{ movementSpeed = value; } }

    public virtual void Update()
    {
        x = this.gameObject.transform.localPosition.x;
        y = this.gameObject.transform.localPosition.y;

    }

    public float[] GetLocation()
    {
        return new float[2] { x, y};
    }

    public void SetLocation(float x, float y)
    {
        this.gameObject.transform.localPosition = new Vector3(x, y, -1);
    }
}
