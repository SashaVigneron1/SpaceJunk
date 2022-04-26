using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBoundariesScript : MonoBehaviour
{

    [SerializeField] GameObject topWall;
    [SerializeField] GameObject bottomWall;
    [SerializeField] GameObject leftWall;
    [SerializeField] GameObject rightWall;
    [SerializeField] GameObject asteroid;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBoundaries(float width, float height)
    {
        Vector3 topPos = new Vector3(0,0, (height / 2) + 5);
        Vector3 bottomPos = new Vector3(0, 0, -((height / 2) + 5));
        Vector3 leftPos = new Vector3(-((width / 2) + 5), 0, 0);
        Vector3 rightPos = new Vector3(((width / 2) + 5), 0, 0);

        topWall.transform.position = topPos;
        topWall.transform.localScale = new Vector3(width + 10, 10, 0.1f);

        bottomWall.transform.position = bottomPos;
        bottomWall.transform.localScale = new Vector3(width + 10, 10, 0.1f);

        leftWall.transform.position = leftPos;
        leftWall.transform.localScale = new Vector3(0.1f, 10, height + 10);

        rightWall.transform.position = rightPos;
        rightWall.transform.localScale = new Vector3(0.1f, 10, height + 10);

        AddAsteroidBeld(width, height);

    }

    void AddAsteroidBeld(float width, float height)
    {
        int startXOffset = 15;

        for(int i = 0; i < 3; i++)
        {
            int randomOffset = Random.Range(15, 20);
            int randomOffsetExtra = Random.Range(15, 20);

            float spaceBetweenX = width /  (randomOffset  + (randomOffsetExtra * i));
            float startX = -((width / 2)+10);

            Quaternion rotation = new Quaternion();

            int amount = 0;
            while (startX + (spaceBetweenX * amount) <= (width / 2) +20 + startXOffset)
            {
                randomOffset = Random.Range(20, 25);
                randomOffsetExtra = Random.Range(20, 25);

                int extraRandomOffset = Random.Range(5, 10);

                spaceBetweenX = width / (randomOffset + (randomOffsetExtra * i));

                Vector3 pos = new Vector3((startX -startXOffset )+ (spaceBetweenX * amount), 0, -((height / 2) +8 + (extraRandomOffset*i)));
                var a = Instantiate(asteroid, pos, rotation);
                a.transform.parent = this.gameObject.transform;
                var script = a.GetComponent<Asteroid>();
                script.SetStationary();
                ///////////////////////////////////
                randomOffset = Random.Range(15, 20);
                randomOffsetExtra = Random.Range(15, 20);
                extraRandomOffset = Random.Range(5, 10);

                Vector3 pos2 = new Vector3((startX - startXOffset) + (spaceBetweenX * amount), 0, (height / 2) + 8 +(extraRandomOffset*i));
                var a2 = Instantiate(asteroid, pos2, rotation);
                a2.transform.parent = this.gameObject.transform;
                var script2 = a2.GetComponent<Asteroid>();
                script2.SetStationary();

                amount++;
            }

            amount = 0;
            float spaceBetweenZ = height/ (randomOffset + (randomOffsetExtra * i));
            float startZ = -((height + 10) / 2);


            while (startZ + (spaceBetweenZ * amount) <= (height / 2) + 10)
            {

                randomOffset = Random.Range(15, 20);
                randomOffsetExtra = Random.Range(15, 20);

                int extraRandomOffset = Random.Range(5, 10);

                spaceBetweenZ = height / (randomOffset + (randomOffsetExtra * i));

                Vector3 pos = new Vector3((startX +1) - (extraRandomOffset * i), 0, (startZ + (spaceBetweenZ * amount)));
                var a = Instantiate(asteroid, pos, rotation);
                a.transform.parent = this.gameObject.transform;
                var script = a.GetComponent<Asteroid>();
                script.SetStationary();

                randomOffset = Random.Range(15, 20);
                randomOffsetExtra = Random.Range(15, 20);

                Vector3 pos2 = new Vector3((Mathf.Abs(startX) -1) + (extraRandomOffset * i), 0, (startZ + (spaceBetweenZ * amount)));
                var a2 = Instantiate(asteroid, pos2, rotation);
                a2.transform.parent = this.gameObject.transform;
                var script2 = a2.GetComponent<Asteroid>();
                script2.SetStationary();

                amount++;
            }
        }
        

        



    }
}
