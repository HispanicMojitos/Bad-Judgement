using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sky : MonoBehaviour
{
    [SerializeField] private Material skybox;

    private static float skyboxRotationCycle = .25F;

    private void Update()
    {
        float actualSkyboxRotation = skybox.GetFloat("_Rotation");
        float newSkyBoxRotation = actualSkyboxRotation + (skyboxRotationCycle * Time.deltaTime);

        skybox.SetFloat("_Rotation", newSkyBoxRotation);
    }

    private void OnApplicationQuit()
    {
        skybox.SetFloat("_Rotation", 0f);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level != SceneManager.GetActiveScene().buildIndex) skybox.SetFloat("_Rotation", 0f);
    }
}
