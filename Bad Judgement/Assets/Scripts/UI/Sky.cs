using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
