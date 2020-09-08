using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class SceneController : MonoBehaviour
{
    public float brightnessOffset = 0.0f;
    public int maxEnemies = 10;
    public int maxMinions = 8;
    public int maxEnforcers = 2;
    public int gameTimeMinutes = 5;
    int gameTimeSeconds;
    int currentGameTimeSeconds = 0;
    int currentGameTimeMinutes = 0;
    string UISeconds;
    string UIMinutes;
    int timeInt;
    public float storm1 = 120;
    public float storm2 = 210;
    public bool stormOn = true;
    float flickerStart, burnStart;
    float intensityValue = 0.5f;

    //Enemy Spawning
    public GameObject enemyPrefab;
    public Transform enemyParent;
    public Transform spawnLocations;


    // objects
    public GameObject timerText;

 



    // Start is called before the first frame update
    void Start()
    {
        gameTimeSeconds = gameTimeMinutes * 60;
    }

    // Update is called once per frame
    void Update()
    {
        updateTime();
        globalIntensity();

        if(Time.time >= gameTimeSeconds)
        {
            // game over
        }


        // storm events
        if(((Time.time >= storm1 && Time.time <= storm1 + 1) || (Time.time >= storm2 && Time.time <= storm1 + 1)) && !stormOn)
        {
            stormStart();
        }
    }
    void spawnEnemy() 
    {
        // Instantiate(enemyPrefab, spawnLocations, Quaternion.identity, enemyParent);
    }
    void updateTime()
    {
        timeInt = Mathf.FloorToInt(Time.time);
        // convert time.time to seconds and minutes
        currentGameTimeSeconds = (timeInt % 60);
        currentGameTimeMinutes = (timeInt % 3600) / 60;
        
        // if seconds are below 10 print them as "09" instead of "9"
        UISeconds = (currentGameTimeSeconds < 10) ? '0' + currentGameTimeSeconds.ToString() : currentGameTimeSeconds.ToString();
        UIMinutes = (19 + currentGameTimeMinutes).ToString();

        // update time        
        timerText.GetComponent<TMP_Text>().text = UIMinutes + ":" + UISeconds;
    }
    void globalIntensity()
    {
        if(!stormOn)
        {
            GameObject.Find("Global Light 2D").GetComponent<Light2D>().intensity = ((gameTimeSeconds - currentGameTimeSeconds) / (gameTimeSeconds / 0.4f)) + 0.1f + brightnessOffset;
        }
    }
    public void stormStart()
    {
        stormOn = true;
        GameObject.Find("Global Light 2D").GetComponent<Light2D>().intensity = 0.02f;
        // flicker for 3 seconds
        flickerStart = Time.time;
        flickerLights();

        // burn lights
        burnStart = Time.time + 3.0f;
        Invoke("burnLights", 3.0f);
        
        // end storm
        Invoke("stormEnd", 30);

        // start
    }
    public void stormEnd()
    {
        GameObject.Find("Global Light 2D").GetComponent<Light2D>().intensity = 0.25f;
        for(int i = 0; i < GameObject.Find("LightPosts").transform.childCount; i++)
        {
            if(GameObject.Find("PointLight" + i) != null)
            {
                GameObject.Find("PointLight" + i).GetComponent<Light2D>().intensity = 1;
            }
        }
        stormOn = false;
    }
    void flickerLights()
    {
        float duration = 3.0f;
        for(int i = 0; i < GameObject.Find("LightPosts").transform.childCount; i++)
        {
            if(GameObject.Find("PointLight" + i) != null)
            {
                GameObject.Find("PointLight" + i).GetComponent<Light2D>().intensity = Random.Range(0.0f, 1.0f);
            }
        }
        // if it is still not over, repeat
        if(!(Time.time > flickerStart + duration))
        {
            Invoke("flickerLights", 0.001f);
        }
    }
    void burnLights()
    {
        float duration = 2.0f;
        for(int i = 0; i < GameObject.Find("LightPosts").transform.childCount; i++)
        {
            if(GameObject.Find("PointLight" + i) != null)
            {
                GameObject.Find("PointLight" + i).GetComponent<Light2D>().intensity += intensityValue;
            }
        }
        
        if(!(Time.time > burnStart + duration))
        {
            Invoke("burnLights", 0.1f);
        } else
        {
            for(int i = 0; i < GameObject.Find("LightPosts").transform.childCount; i++)
            {
                if(GameObject.Find("PointLight" + i) != null)
                {
                    GameObject.Find("PointLight" + i).GetComponent<Light2D>().intensity = 0;
                }
            }
        }
    }
}
