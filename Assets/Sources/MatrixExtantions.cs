using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources
{
    public static class MatrixExtantions
    {
        public static List<Matrix4x4> GetOffsets(this List<Matrix4x4> spaceMatrices, List<Matrix4x4> modelMatrices)
        {
            List<Matrix4x4> validOffsets = new ();

            if (modelMatrices.Count == 0 || spaceMatrices.Count == 0)
                return validOffsets;
            
            Matrix4x4 firstModel = modelMatrices[0];

            if (firstModel.TryInvert(out Matrix4x4 inverseModel) == false)
                throw new Exception("First model matrix is not invertible");

            foreach (Matrix4x4 spaceMatrix in spaceMatrices)
            {
                Matrix4x4 candidate = spaceMatrix * inverseModel;
        
                bool isValid = true;

                foreach (Matrix4x4 modelMatrix in modelMatrices)
                {
                    Matrix4x4 transformedMatrix = candidate * modelMatrix;
                }
                
                for (int i = 1; i < modelMatrices.Count; i++)
                {
                    Matrix4x4 transformedMatrix = candidate * modelMatrices[i];
                    
                    if (spaceMatrices.ContainsMatrix(transformedMatrix) == false)
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
        
        public static bool TryInvert(this Matrix4x4 matrix, out Matrix4x4 inverseMatrix)
        {
            try
            {
                inverseMatrix = matrix.inverse;
                return true;
            }
            catch
            {
                inverseMatrix = Matrix4x4.identity;
                return false;
            }
        }
        
        public static bool ContainsMatrix(this List<Matrix4x4> matrices, Matrix4x4 target, float epsilon = 1e-5f)
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
                
                if (match)
                    return true;
            }
            return false;
        }
    }
}