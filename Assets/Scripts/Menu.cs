using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Menu : MonoBehaviour {

    public NetworkManager NetworkManager;
    public Canvas MenuCanvas;
    public Canvas GameCanvas;
    public GameObject HostQuitButton;
    public GameObject ClientQuitButton;

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void StartHost()
    {
        NetworkManager.StartHost();
        MenuCanvas.enabled = false;
        GameCanvas.enabled = true;
        HostQuitButton.SetActive(true);
        ClientQuitButton.SetActive(false);
    }

    public void StartClient()
    {
        NetworkManager.StartClient();
        MenuCanvas.enabled = false;
        GameCanvas.enabled = true;
        HostQuitButton.SetActive(false);
        ClientQuitButton.SetActive(true);
    }

    public void StopHost()
    {
        NetworkManager.StopHost();
        MenuCanvas.enabled = true;
        GameCanvas.enabled = false;
        HostQuitButton.SetActive(false);
        ClientQuitButton.SetActive(false);
        GameObject.FindObjectOfType<NetworkManager>().GetComponent<ScoreManager>().ResetScoreboard();
    }

    public void StopClient()
    {
        NetworkManager.StopClient();
        MenuCanvas.enabled = true;
        GameCanvas.enabled = false;
        HostQuitButton.SetActive(false);
        ClientQuitButton.SetActive(false);
        GameObject.FindObjectOfType<NetworkManager>().GetComponent<ScoreManager>().ResetScoreboard();
    }
}
