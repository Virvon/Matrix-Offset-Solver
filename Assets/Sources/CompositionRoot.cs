using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Sources
{
    public class CompositionRoot : MonoBehaviour
    {
        [SerializeField] private Test _test;
        [SerializeField] private Vizualize _vizualize;
        
        private void Start()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            
            List<Matrix4x4> modelMatrices = _test.GetMatrix("model.json");
            List<Matrix4x4> spaceMatrices = _test.GetMatrix("space.json");
            
            Debug.Log(modelMatrices.Count);
            Debug.Log(spaceMatrices.Count);
            
            List<Matrix4x4> validOffsets = FindValidOffsets(modelMatrices, spaceMatrices);
            
            stopwatch.Stop();
            
            Debug.Log($"Found {validOffsets.Count} valid offsets");
            foreach (var offset in validOffsets)
            {
                Debug.Log(MatrixToString(offset));
            }
            
            Debug.Log("Count " + validOffsets.Count);
            Debug.Log("Time " + stopwatch.Elapsed);
            
            _vizualize.VisualizeModel(validOffsets, Color.red);
            _vizualize.VisualizeModel(modelMatrices, Color.white);
            _vizualize.VisualizeModel(spaceMatrices, Color.cyan);
        }
        
        string MatrixToString(Matrix4x4 matrix)
        {
            return $"{matrix.m00:F6} {matrix.m01:F6} {matrix.m02:F6} {matrix.m03:F6}\n" +
                   $"{matrix.m10:F6} {matrix.m11:F6} {matrix.m12:F6} {matrix.m13:F6}\n" +
                   $"{matrix.m20:F6} {matrix.m21:F6} {matrix.m22:F6} {matrix.m23:F6}\n" +
                   $"{matrix.m30:F6} {matrix.m31:F6} {matrix.m32:F6} {matrix.m33:F6}";
        }

        
        
        List<Matrix4x4> FindValidOffsets(List<Matrix4x4> modelMatrices, List<Matrix4x4> spaceMatrices)
        {
            List<Matrix4x4> validOffsets = new List<Matrix4x4>();

            if (modelMatrices.Count == 0 || spaceMatrices.Count == 0)
            {
                Debug.Log("return " + modelMatrices.Count + " " + spaceMatrices.Count);
                return validOffsets;
            }
            
            Matrix4x4 firstModel = modelMatrices[0];
            
            if (!MatrixInvert(firstModel, out Matrix4x4 invFirstModel))
            {
                Debug.LogError("First model matrix is not invertible");
                return validOffsets;
            }

            foreach (Matrix4x4 spaceMatrix in spaceMatrices)
            {
                Matrix4x4 candidate = spaceMatrix * invFirstModel;
        
                bool isValid = true;
                for (int i = 1; i < modelMatrices.Count; i++)
                {
                    Matrix4x4 transformed = candidate * modelMatrices[i];
                    if (!ContainsMatrix(spaceMatrices, transformed))
                    {
                        isValid = false;
                        break;
                    }
                }

                if (isValid)
                    validOffsets.Add(candidate);
            }

            return validOffsets;
        }

        bool MatrixInvert(Matrix4x4 matrix, out Matrix4x4 inverse)
        {
            try
            {
                inverse = matrix.inverse;
                return true;
            }
            catch
            {
                inverse = Matrix4x4.identity;
                return false;
            }
        }

        bool ContainsMatrix(List<Matrix4x4> matrices, Matrix4x4 target, float epsilon = 1e-5f)
        {
            foreach (Matrix4x4 matrix in matrices)
            {
                bool match = true;
                for (int i = 0; i < 16; i++)
                {
                    if (Mathf.Abs(matrix[i] - target[i]) > epsilon)
                    {
                        match = false;
                        break;
                    }
                }
                if (match) return true;
            }
            return false;
        }
    }
}