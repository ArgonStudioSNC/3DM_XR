using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public void ResetTransform()
    {
        transform.position = new Vector3(0, 13, 0);
    }
}
