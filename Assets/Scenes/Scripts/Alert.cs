using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : MonoBehaviour
{

    Transform enemyPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyPos != null)
        {
            Vector3 direction = enemyPos.position - this.transform.position;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.back);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 20.0f * Time.deltaTime);
        } else {
            Destroy(gameObject);
        }

    }

    public void CreateAlert(Transform enemy)
    {
        enemyPos = enemy;
    }
}
