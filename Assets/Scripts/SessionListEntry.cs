using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SessionListEntry : MonoBehaviour
{
    public TextMeshProUGUI roomName, playerCount;
    public Button joinButton;

    public int GetSceneIndex(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);

            string name = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            if (name == sceneName)
                return i;
        }

        return -1;
    }
    public void JoinRoom()
    {
        // Check if there is an active runner instance
        if (NetworkManager.runnerInstance == null)
        {
            NetworkManager.runnerInstance = FindObjectOfType<NetworkRunner>();
            if (NetworkManager.runnerInstance == null)
            {
                NetworkManager.runnerInstance = new GameObject("NetworkRunner").AddComponent<NetworkRunner>();
            }
        }

        // Ensure the runner is not already running a session
        if (NetworkManager.runnerInstance.IsRunning)
        {
            Debug.LogWarning("NetworkRunner is already running. Shutting down before joining a new session.");
            NetworkManager.runnerInstance.Shutdown();
        }

        // Start the game with the session name from the roomName field
        NetworkManager.runnerInstance.StartGame(new StartGameArgs()
        {
            Scene = SceneRef.FromIndex(GetSceneIndex(NetworkManager.gamePlaySceneName)),
            SessionName = roomName.text, // Use the room name provided in the UI
            GameMode = GameMode.Shared, // Clients join an existing session
        });

        Debug.Log("Joining room: " + roomName.text);
    }

}
