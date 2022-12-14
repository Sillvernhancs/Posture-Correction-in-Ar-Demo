using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;

public class update_body: MonoBehaviour
{
    string url = "https://tvo47.people.uic.edu/479%20final%20project%20website/data.json";
    public GameObject upper_spine;
    public GameObject lower_spine;
    public GameObject neck;
    public GameObject monitor;
    public dude p;

    private Quaternion neckTarget;
    private Quaternion spineTarget;
    
    public Material good;
    public Material bad;

    public AudioSource badSound;
    public AudioSource goodSound;

    private bool rotating = true;
    private Vector3 destination;
    
    void Update()
    {
        if (p!= null  && (p.accelerometer_x > 25 || p.accelerometer_x < -25)) {
            monitor.GetComponent<Renderer>().material = bad;

        } else if (p.accelerometer_y> 25 || p.accelerometer_y < -25) {
            monitor.GetComponent<Renderer>().material = bad;

        } else if (p.flex > 25 || p.flex < -30) {
            monitor.GetComponent<Renderer>().material = bad;

        } else {
            monitor.GetComponent<Renderer>().material = good;

        }
        // smoothly rotates parts
        if (lower_spine.transform.localEulerAngles == destination) {
            rotating = false;
        } else {
            rotating = true;
        }

        // if(rotating) {
        //     lower_spine.transform.Rotate(destination * Time.deltaTime);
        // }
        // if (Vector3.Distance(lower_spine.transform.eulerAngles, destination) > 0.01f) {
        //     lower_spine.transform.eulerAngles = Vector3.Lerp(lower_spine.transform.rotation.eulerAngles, destination, Time.deltaTime);
        // } else {
        //     transform.eulerAngles = destination;
        //     rotating = false;
        // }
    }

    void Start()
    {
    // wait a couple seconds to start and then refresh every 900 seconds
       InvokeRepeating("GetDataFromWeb", 1f, 0.05f);
   }


   void process(string text) {
        string[] data = text.Split(',');
        p.flex = float.Parse(data[0]);
        p.accelerometer_x = float.Parse(data[1]);
        p.accelerometer_y = float.Parse(data[2]);
        p.accelerometer_z = float.Parse(data[3]);
        // p.gyro_x = float.Parse(data[4]);
        // p.gyro_y = float.Parse(data[5]);
        // p.gyro_z = float.Parse(data[6]);
        // DEBUGS
        // print(p.accelerometer_x + ":" + p.accelerometer_y + ":" + p.accelerometer_z);

        //spine3

        lower_spine.transform.localEulerAngles = new Vector3(p.accelerometer_x*-1, 0, p.accelerometer_y);
        // destination = new Vector3(p.accelerometer_x * -1, 0, p.accelerometer_y);
        // lower_spine.transform.rotation = Quaternion.Slerp(lower_spine.transform.rotation, Quaternion.LookRotation(destination), 4f * Time.deltaTime);
        // lower_spine.transform.Rotate(destination * Time.deltaTime);

        // upper_spine.transform.localEulerAngles = new Vector3(p.accelerometer_x/2, p.accelerometer_y/2, p.accelerometer_z/2);
        // lower_spine.transform.eulerAngles.x = p.flex;
        
        
        
        // neck
        // neck.transform.Rotate(p.flex, 0.0f, 0.0f, Space.Self);
        neck.transform.localEulerAngles = new Vector3(p.flex, 0.0f, 0.0f);
   }

   void GetDataFromWeb()
   {

       StartCoroutine(GetRequest(url));
   }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    process(webRequest.downloadHandler.text);
                    break;
            }
        }
    }
}
[System.Serializable]
public class dude{
    public float flex = 0;
    public float accelerometer_x = 0;
    public float accelerometer_y = 0;
    public float accelerometer_z = 0;
    public float gyro_x = 0;
    public float gyro_y = 0;
    public float gyro_z = 0;
}



