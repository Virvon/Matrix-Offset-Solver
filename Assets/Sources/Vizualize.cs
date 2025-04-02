using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources
{
    public class Vizualize : MonoBehaviour
    {
        public void VisualizeModel(List<Matrix4x4> model, Color color) {
            foreach(var matrix in model) {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                
                cube.transform.position = matrix.GetPosition();
                cube.transform.rotation = matrix.rotation;
                cube.transform.localScale = matrix.lossyScale;
    
                cube.GetComponent<Renderer>().material.color = color;
            }
        }
    }
}