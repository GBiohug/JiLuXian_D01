using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


public class SceneMetaData : MonoBehaviour
{

    public GameObject mainLight = null;
    public Material skybox = null;
    public Cubemap reflection = null;
   
    public Transform WorldUpTransform = null;
    //public float FieldOfView = ;
    public Transform SpawnTransform;
    public PlayableDirector FlythroughDirector;
    public PlayableDirector SequenceDirector;
    public GameObject Root;
    public Scene Scene;
    public bool FogEnabled;
    public bool PostProcessingEnabled;
    public bool StartActive;
    
}
