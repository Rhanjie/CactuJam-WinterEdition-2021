using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleGenerator : MonoBehaviour {
    public List<Transform> obstacles;
    
    private int currentObstacleNumber = 2;
    
    // Start is called before the first frame update
    void Start() {
        GenerateNextObstacles();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void GenerateNextObstacles() {
        for (int i = 0; i < 20; i++) {
            int randomInxed = Random.Range(0, obstacles.Count);
            Transform transform = Instantiate(obstacles[randomInxed]).GetComponent<Transform>();

            float randomX = Random.Range(-18, 40) / 10.0f;
            
            transform.position = new Vector3(randomX, -0.9f, currentObstacleNumber++ * -4.0f);
        }
    }
}
