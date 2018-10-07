﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    public AudioSource audioSc1, audioSc2;
    public static Cutscene instance;
    public static int cutsceneIndex, currentSprite = 0;
    public static bool playCutscene, sceneEnd;
    public static string nextSceneName;
    private float timeLol;
    Color d11, d22, d33;
    GameObject cutsceneGameobject;
    Animator d1, d2, d3;
    Image panel;
    Animator fadePanel;
    int maxScene, sceneAudioIndexOffset = 0;
    bool timebool,played;
    float timealert = 0, inputtime = 0;
    bool locked = true;
    public static bool dying = false;
    //float a1, a2, a3;
    private Sounds audioList;
    public List<Sprite> cutscene1 = new List<Sprite>();
    public List<Sprite> cutscene2 = new List<Sprite>();
    public List<Sprite> cutscene3 = new List<Sprite>();
    public List<Sprite> cutscene4 = new List<Sprite>();
    public List<Sprite> cutscene5 = new List<Sprite>();
    public List<Sprite> cutscene6 = new List<Sprite>();
    public List<Sprite> cutscene7 = new List<Sprite>();
    public List<Sprite> cutscene8 = new List<Sprite>();

    void Start()
    {
        audioList = FindObjectOfType<Sounds>();
        audioSc1 = GameObject.FindGameObjectWithTag("Primary Audio").GetComponent<AudioSource>();
        audioSc2 = GameObject.FindGameObjectWithTag("Secondary Audio").GetComponent<AudioSource>();
        timebool = true;
        played = false;
        cutsceneIndex = 0;
        maxScene = 0;
        fadePanel = GameObject.FindGameObjectWithTag("FadePanel").GetComponent<Animator>();
        panel = GameObject.FindGameObjectWithTag("Cutscene").GetComponent<Image>();
        cutsceneGameobject = panel.gameObject;
        sceneEnd = false;
        if (SceneManager.GetActiveScene().name == "House" && LevelTransition.playerPos == null)
        {
            playCutscene = true;
            cutsceneIndex = 1;
        }
        else
            playCutscene = false;

        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this.gameObject);
        
        for (int temp = 0; temp < 3; temp++)
        {
            cutscene1.Add(Resources.Load<Sprite>("Ch1/Ch1-" + (temp + 1)));
        }
        for (int temp = 0; temp < 5; temp++)
        {
            cutscene2.Add(Resources.Load<Sprite>("Ch1/Ch1-" + (temp + 4)));
        }
        for (int temp = 0; temp < 6; temp++)
        {
            cutscene3.Add(Resources.Load<Sprite>("Ch1/Ch1-" + (temp + 9)));
        }
        for (int temp = 0; temp < 9; temp++)
        {
            cutscene4.Add(Resources.Load<Sprite>("Ch1/Ch1-" + (temp + 15)));
        }
        for (int temp = 0; temp < 2; temp++)
        {
            cutscene5.Add(Resources.Load<Sprite>("Ch1/Ch1-" + (temp + 24)));
        }
        for (int temp = 0; temp < 2; temp++)
        {
            cutscene6.Add(Resources.Load<Sprite>("Ch1/Ch1-" + (temp + 90)));
        }
        for (int temp = 0; temp < 11; temp++)
        {
            cutscene7.Add(Resources.Load<Sprite>("Ch1/Ch1-" + (temp + 26)));
        }
        for (int temp = 0; temp < 8; temp++)
        {
            cutscene8.Add(Resources.Load<Sprite>("Ch1/Ch1-" + (temp + 37)));
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "ToCafe")
        {
            d1 = GameObject.FindGameObjectWithTag("Death1").GetComponent<Animator>();
            d2 = GameObject.FindGameObjectWithTag("Death2").GetComponent<Animator>();
            d3 = GameObject.FindGameObjectWithTag("Death3").GetComponent<Animator>();
            d11 = d1.gameObject.GetComponent<Image>().color;
            d22 = d2.gameObject.GetComponent<Image>().color;
            d33 = d3.gameObject.GetComponent<Image>().color;
            /*a1 = d11.a;
            a2 = d22.a;
            a3 = d33.a;*/

            if (dying)
                DeathIsComing();
        }
        else dying = false;

        if (Time.timeSinceLevelLoad <= Time.fixedDeltaTime && SceneManager.GetActiveScene().name != "Bedroom" && SceneManager.GetActiveScene().name != "SchoolBedroom" && SceneManager.GetActiveScene().name != "MainMenu")
        {
            fadePanel = GameObject.FindGameObjectWithTag("FadePanel").GetComponent<Animator>();
            if(SceneManager.GetActiveScene().name != "FallingBuildingScene" && SceneManager.GetActiveScene().name != "BuildingEnding")
            {
                panel = GameObject.FindGameObjectWithTag("Cutscene").GetComponent<Image>();
                cutsceneGameobject = panel.gameObject;
            }
        }
        if (Time.timeSinceLevelLoad <= Time.fixedDeltaTime && SceneManager.GetActiveScene().name == "CorriInTheHouse")
        {
            audioSc1.clip = audioList.audioDict["BGM"][1];
            audioSc1.Play();

            audioSc2.clip = audioList.audioDict["Corridor"][3];
            audioSc2.Play();
        }

        if (Time.timeSinceLevelLoad <= Time.fixedDeltaTime && SceneManager.GetActiveScene().name == "ToCafe")
        {
            audioSc1.clip = audioList.audioDict["BGM"][2];
            audioSc1.Play();
        }

        if (playCutscene)
        {
            if(!played)
            {
                played = true;
                audioSc2.PlayOneShot(audioList.audioDict["Cutscenes"][GetSceneAudioIndex(cutsceneIndex) + sceneAudioIndexOffset]);
            }
            audioSc1.Pause();
            if(timebool)
            {
                timeLol = Time.time;
                timebool = false;
            }

            panel.enabled = true;

            PlayerController.canmove = false;

            if (Input.GetKeyUp(KeyCode.A))
            {
                sceneAudioIndexOffset--;
                currentSprite--;
                currentSprite = Mathf.Clamp(currentSprite, 0, GetCutscene(cutsceneIndex).Count);
                audioSc2.Play();
                audioSc2.PlayOneShot(audioList.audioDict["Cutscenes"][GetSceneAudioIndex(cutsceneIndex) + sceneAudioIndexOffset]);
            }
            if (Input.GetKeyUp(KeyCode.D) && currentSprite < maxScene)
            {
                timeLol = Time.time;
                timebool = true;
                currentSprite++;                
                currentSprite = Mathf.Clamp(currentSprite, 0, GetCutscene(cutsceneIndex).Count);
                if (currentSprite == GetCutscene(cutsceneIndex).Count)
                {
                    currentSprite--;
                    playCutscene = false;
                    StartCoroutine(TriggerCut());
                    if (sceneEnd)
                    {
                        SceneManager.LoadScene(nextSceneName);
                        sceneEnd = false;
                    }
                    if(cutsceneIndex == 3)
                    {
                        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(26, GameObject.FindGameObjectWithTag("Player").transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
                        Camera.main.transform.position = new Vector3(32.5f, Camera.main.transform.position.y, Camera.main.transform.position.z);
                        PlayerController.lockRun = false;
                        CameraFollow.camfol.camXPosMin = 32.5f;
                        CameraFollow.camfol.camXPosMax = 93.5f;

                    }
                    //if (cutsceneIndex != 4)
                    PlayerController.canmove = true;
                }
            }
            else if(Input.GetKeyUp(KeyCode.D) && currentSprite >= maxScene && ((Time.time - timeLol) > 1.2f))
            {                
                timeLol = Time.time;
                timebool = true;
                currentSprite++;
                maxScene = currentSprite;
                sceneAudioIndexOffset++;
                currentSprite = Mathf.Clamp(currentSprite, 0, GetCutscene(cutsceneIndex).Count);
                audioSc2.Play();
                audioSc2.PlayOneShot(audioList.audioDict["Cutscenes"][GetSceneAudioIndex(cutsceneIndex) + sceneAudioIndexOffset]);
                if (currentSprite == GetCutscene(cutsceneIndex).Count)
                {
                    currentSprite--;
                    playCutscene = false;
                    StartCoroutine(TriggerCut());
                    if (sceneEnd)
                    {
                        SceneManager.LoadScene(nextSceneName);
                        sceneEnd = false;
                    }
                    if (cutsceneIndex == 3)
                    {
                        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(26, GameObject.FindGameObjectWithTag("Player").transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
                        Camera.main.transform.position = new Vector3(32.5f, Camera.main.transform.position.y, Camera.main.transform.position.z);
                        PlayerController.lockRun = false;
                        CameraFollow.camfol.camXPosMin = 32.5f;
                        CameraFollow.camfol.camXPosMax = 93.5f;
                    }

                    if (cutsceneIndex == 1)
                    {
                        AudioClip clip = audioList.audioDict["BGM"][0];
                        audioSc1.clip = clip;
                        audioSc1.Play();
                    }
                    if (cutsceneIndex == 2)
                    {
                        audioSc1.clip = audioList.audioDict["BGM"][2];
                        audioSc1.Play();
                    }
                    if (cutsceneIndex == 3)
                    {
                        audioSc1.clip = audioList.audioDict["BGM"][3];
                        audioSc1.Play();
                    }
                    //if (cutsceneIndex != 4)
                    PlayerController.canmove = true;
                }
            }

            panel.sprite = GetCutscene(cutsceneIndex)[currentSprite];
            Color tCol;
            tCol = panel.color;
            tCol.a = 1;
            panel.color = tCol;

        }
        else
        {
            audioSc1.UnPause();
            sceneAudioIndexOffset = 0;
            played = false;
            currentSprite = 0;
            maxScene = 0;
            if (cutsceneGameobject == true)
                panel.enabled = false;
        }
    }

    void DeathIsComing()
    {
        d1.SetBool("out", true);
        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) && (Input.GetKey(KeyCode.LeftShift)))
        {
            inputtime = Time.time;
            timealert = 0;
            locked = true;
            if (d33.a == 1)
                d3.SetBool("out", false);
            if(d33.a == 0)
            {
                d2.SetBool("out", false);
            }
        }
        else if (inputtime + 1 < Time.time)
        {
            inputtime = 0;
            if (d11.a == 0)
                d1.SetBool("out", true);
            if (d11.a == 1)
            {
                d2.SetBool("out", true);
                if (d22.a == 1)
                {
                    d3.SetBool("out", true);
                    if (d33.a == 1)
                    {
                        if (locked)
                        {
                            timealert = Time.time;
                            locked = false;
                        }
                        if (timealert + 1.5f < Time.time)
                        {
                            GameObject.FindGameObjectWithTag("FadePanel").GetComponent<Animator>().SetBool("out", true);
                            StartCoroutine("ChangeScene");
                        }
                    }
                }
            }
        }
        /*d1.SetBool("out", true);
        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) && (Input.GetKey(KeyCode.LeftShift)))
        {
            timealert = 0;
            locked = true;
            if (a3 >= 1)
            {
                a3 = 1;
                a3 = d33.a - 0.1f;
            }
            if (a3 <= 0)
            {
                a3 = 0;
                a2 = d33.a - 0.1f;
            }
            d33.a = a3;
            d22.a = a2;
        }
        else
        {
            if (a1 >= 1)
            {
                Debug.Log(d11.a + " " + d22.a + " " + d33.a);
                a1 = 1;
                a2 = d22.a + 0.1f;
                GameObject.FindGameObjectWithTag("Death1").GetComponent<Image>().color = new Color(0, 0, 0, a1);
                GameObject.FindGameObjectWithTag("Death2").GetComponent<Image>().color = new Color (0, 0, 0, a2);
                if (a2 >= 1)
                {
                    a2 = 1;
                    a3 = d33.a + 0.1f;
                    GameObject.FindGameObjectWithTag("Death2").GetComponent<Image>().color = new Color(0, 0, 0, a2);
                    GameObject.FindGameObjectWithTag("Death3").GetComponent<Image>().color = new Color(0, 0, 0, a3);
                    if (a3 >= 1)
                    {
                        a3 = 1;
                        d33.a = a3;
                        GameObject.FindGameObjectWithTag("Death3").GetComponent<Image>().color = new Color(0, 0, 0, a3);
                        if (locked)
                        {
                            timealert = Time.time;
                            locked = false;
                        }
                        if (timealert + 1.5f < Time.time)
                        {
                            GameObject.FindGameObjectWithTag("FadePanel").GetComponent<Animator>().SetBool("out", true);
                            StartCoroutine("ChangeScene");
                        }
                    }
                }
            }
        }*/
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator TriggerCut()
    {
        fadePanel.SetBool("out", false);
        Color tCol2;
        tCol2 = panel.color;
        tCol2.a = 0;
        panel.color = tCol2;
        if (sceneEnd)
        {
            SceneManager.LoadScene(nextSceneName);
            sceneEnd = false;
        }
        if (cutsceneIndex == 7)
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (cutsceneIndex == 8)
        {
            PlayerController.jumpingAvailable = false;
            SceneManager.LoadScene("BuildingEnding");
        }
        yield return new WaitForSeconds(0.5f);
    }

    List<Sprite> GetCutscene(int x)
    {
        switch (x)
        {
            case 1:
                return cutscene1;
            case 2:
                return cutscene2;
            case 3:
                return cutscene3;
            case 4:
                return cutscene4;
            case 5:
                return cutscene5;
            case 6:
                return cutscene6;
            case 7:
                return cutscene7;
            case 8:
                return cutscene8;
            default:
                return null;
        }
    }

    int GetSceneAudioIndex(int x)
    {
        switch (x)
        {
            case 1:
                return 0;
            case 2:
                return 3;
            case 3:
                return 8;
            case 4:
                return 14;
            case 5:
                return 23;
            case 6:
                return -1;
            case 7:
                return -1;
            case 8:
                return -1;
            default:
                return -1;
        }
    }
}
