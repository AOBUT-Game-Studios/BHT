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
    public int[] storm1 = {120, 150};
    public int[] storm2 = {210, 240};
    bool stormOn = false;
    float flickerStart, burnStart;
    float intensityValue = 0.5f;


    // objects
    public GameObject timerText;

 



    // Start is called before the first frame update
    void Start()
    {
        gameTimeSeconds = gameTimeMinutes * 60;
        if(Random.Range(0, 1) == 1)
        {

        }
        Invoke("stormStart", Random.Range(0.0f, 20.0f));
    }

    // Update is called once per frame
    void Update()
    {
        timeInt = Mathf.FloorToInt(Time.time);
        // convert time.time to seconds and minutes
        currentGameTimeSeconds = (timeInt % 60);
        currentGameTimeMinutes = (timeInt % 3600) / 60;
        
        // if seconds are below 10 print them as "09" instead of "9"
        UISeconds = (currentGameTimeSeconds < 10) ? '0' + currentGameTimeSeconds.ToString() : currentGameTimeSeconds.ToString();
        UIMinutes = (17 + currentGameTimeMinutes).ToString();

        // update time        
        timerText.GetComponent<TMP_Text>().text = UIMinutes + ":" + UISeconds;


        if(Time.time >= gameTimeSeconds)
        {
            // game over
        }


        // storm events
        if((Time.time == storm1[0] || Time.time == storm2[0]) && !stormOn)
        {
            stormOn = true;
        }
    }
    public void stormStart()
    {
        GameObject.Find("Global Light 2D").GetComponent<Light2D>().intensity = 0.0f;
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
