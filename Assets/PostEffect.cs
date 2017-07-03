﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffect : MonoBehaviour {

    Camera attachedCamera;
    public Shader Post_Outline;
    public Shader DrawSimple;
    Camera tempCam;
    Material Post_Mat;
    // public RenderTexture tempRT;

    // ------ Begin Unity Methods ------

    void Start()
    {
        attachedCamera = GetComponent<Camera>();
        tempCam = new GameObject().AddComponent<Camera>();
        tempCam.enabled = false;
        Post_Mat = new Material(Post_Outline);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //set up a temporary camera
        tempCam.CopyFrom(attachedCamera);
        tempCam.clearFlags = CameraClearFlags.Color;
        tempCam.backgroundColor = Color.black;

        //cull any layer that isn't the outline
        tempCam.cullingMask = 1 << LayerMask.NameToLayer("Outline");

        //make the temporary rendertexture
        RenderTexture TempRT = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.R8);

        //put it to video memory
        TempRT.Create();

        //set the camera's target texture when rendering
        tempCam.targetTexture = TempRT;

        //render all objects this camera can render, but with our custom shader.
        tempCam.RenderWithShader(DrawSimple, "");

        Post_Mat.SetTexture("_SceneTex", source);

        //copy the temporary RT to the final image
        Graphics.Blit(TempRT, destination, Post_Mat);

        //release the temporary RT
        TempRT.Release();
    }

    // ------ End Unity Methods ------

    // ------ Begin Non-Unity Methods ------

    // ------ End Non-Unity Methods ------
}
