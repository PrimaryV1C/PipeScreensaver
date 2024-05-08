using UnityEngine;
using System.Collections;

public class PipeSetter : MonoBehaviour
{
    public GameObject pipePrefab;
    public int totalPipeSegments;
    public Color[] pipeColors;
    private Color currentColor;
    Vector3 startPosition = Vector3.zero;
    Vector3 direction = Vector3.up;
    Quaternion rotation = Quaternion.identity;


    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
