using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Sources
{
    public class MatrixLoader
    {
        public List<Matrix4x4> Load(string path)
        {
            string jsonPath = Path.Combine(Application.streamingAssetsPath, path);
            
            if (File.Exists(jsonPath) == false)
                throw new Exception("JSON file not found at: " + jsonPath);
            
            string jsonText = File.ReadAllText(jsonPath);
            List<Matrix4x4> matrices = ParseJsonToMatrices(jsonText);

            return matrices;
        }

        private List<Matrix4x4> ParseJsonToMatrices(string jsonText)
        {
            if (jsonText.Trim().StartsWith("{") == false)
                jsonText = "{\"matrices\":" + jsonText + "}";

            MatrixArrayWrapper wrapper = JsonUtility.FromJson<MatrixArrayWrapper>(jsonText);

            List<Matrix4x4> matrices = new List<Matrix4x4>();
            
            foreach (MatrixJson jsonMatrix in wrapper.matrices)
            {
                Matrix4x4 matrix = new Matrix4x4();
            
                matrix.m00 = jsonMatrix.m00; matrix.m01 = jsonMatrix.m01; matrix.m02 = jsonMatrix.m02; matrix.m03 = jsonMatrix.m03;
                matrix.m10 = jsonMatrix.m10; matrix.m11 = jsonMatrix.m11; matrix.m12 = jsonMatrix.m12; matrix.m13 = jsonMatrix.m13;
                matrix.m20 = jsonMatrix.m20; matrix.m21 = jsonMatrix.m21; matrix.m22 = jsonMatrix.m22; matrix.m23 = jsonMatrix.m23;
                matrix.m30 = jsonMatrix.m30; matrix.m31 = jsonMatrix.m31; matrix.m32 = jsonMatrix.m32; matrix.m33 = jsonMatrix.m33;
            
                matrices.Add(matrix);
            }

            return matrices;
        }

        [Serializable]
        private class MatrixJson
        {
            public float m00; public float m10; public float m20; public float m30;
            public float m01; public float m11; public float m21; public float m31;
            public float m02; public float m12; public float m22; public float m32;
            public float m03; public float m13; public float m23; public float m33;
        }

        [Serializable]
        private class MatrixArrayWrapper
        {
            public List<MatrixJson> matrices;
        }
    }
}