using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToPlayer : MonoBehaviour
{
    public Transform Player;

    void Update()
    {
        if (Player != null)
        {
            Vector3 direction = Player.position - transform.position;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);

            Quaternion rotation_y = Quaternion.Euler(0, 180, 0);
            transform.rotation = rotation*rotation_y;
        }
    }
}
