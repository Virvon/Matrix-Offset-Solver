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

        public void ExportToJson(List<Matrix4x4> matrices, string path)
        {
            string jsonPath = Path.Combine(Application.streamingAssetsPath, path);
            
            if (File.Exists(jsonPath))
                File.Delete(jsonPath);
            
            MatrixListWrapper wrapper = new()
            {
                matrices = new List<MatrixJson>(matrices.Count),
            };
            
            foreach (Matrix4x4 matrix in matrices)
            {
                wrapper.matrices.Add(new MatrixJson
                {
                    m00 = matrix.m00, m10 = matrix.m10, m20 = matrix.m20, m30 = matrix.m30,
                    m01 = matrix.m01, m11 = matrix.m11, m21 = matrix.m21, m31 = matrix.m31,
                    m02 = matrix.m02, m12 = matrix.m12, m22 = matrix.m22, m32 = matrix.m32,
                    m03 = matrix.m03, m13 = matrix.m13, m23 = matrix.m23, m33 = matrix.m33
                });
            }
            
            string json = JsonUtility.ToJson(wrapper, true);
            
            json = json.Replace("{\"matrices\":", "");
            
            File.WriteAllText(jsonPath, json);
        }

        private List<Matrix4x4> ParseJsonToMatrices(string jsonText)
        {
            if (jsonText.Trim().StartsWith("{") == false)
                jsonText = "{\"matrices\":" + jsonText + "}";

            MatrixListWrapper wrapper = JsonUtility.FromJson<MatrixListWrapper>(jsonText);

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
        private class MatrixListWrapper
        {
            public List<MatrixJson> matrices;
        }
    }
}