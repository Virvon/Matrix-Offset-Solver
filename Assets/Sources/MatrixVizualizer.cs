using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources
{
    public class MatrixVizualizer
    {
        private readonly Transform _container;

        public MatrixVizualizer(Transform container) =>
            _container = container;

        public void Visualize(List<Matrix4x4> matrixList, Color color)
        {
            foreach(var matrix in matrixList)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.parent = _container;
                
                cube.transform.position = matrix.GetPosition();
                cube.transform.rotation = matrix.rotation;
                cube.transform.localScale = matrix.lossyScale;
    
                cube.GetComponent<Renderer>().material.color = color;
            }
        }
    }
}