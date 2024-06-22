using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float steerSpeed = 180f;
    public float BodySpeed = 5f;
    public int gap = 10;
    public GameObject bodyPrefab;
    List<GameObject> bodyParts = new List<GameObject>();
    List<Vector3> positionHistory = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        GrowSnake();
        GrowSnake();
        GrowSnake();
    }
    // Update is called once per frame
    void Update()
    {
        //Move forward
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        //Steer
        float steerDirection = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerDirection * steerSpeed * Time.deltaTime);

        //Store body parts
        positionHistory.Insert(0, transform.position);

        //Move body parts
        int index = 0;
        foreach (var body in bodyParts)
        {
            Vector3 point = positionHistory[Math.Clamp(index * gap, 0, positionHistory.Count - 1)];

            //Move body towards the point along the snakes path
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * BodySpeed * Time.deltaTime;

            //Rotate body towards the point along the snakes path
            body.transform.LookAt(point);

            index++;
        }

    }
    void GrowSnake()
    {
        //Instantiate body instance and 
        //add it to the list
        GameObject body = Instantiate(bodyPrefab);
        bodyParts.Add(body);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("apple"))
        {
            Destroy(other.gameObject);
            GrowSnake();
        }
    }
}
