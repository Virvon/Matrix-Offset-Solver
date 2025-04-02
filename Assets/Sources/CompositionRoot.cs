using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Sources
{
    public class CompositionRoot : MonoBehaviour
    {
        private const string ModelPath = "model.json";
        private const string SpacePath = "space.json";
        private const string OffsetsPath = "offsets.json";
        private const float OffsetEpsilon = 1e-5f;

        [SerializeField] private Transform _container;
        
        private MatrixLoader _matrixLoader;
        private MatrixVizualizer _visualizer;

        private void Awake()
        {
            _matrixLoader = new();
            _visualizer = new(_container);
        }

        private void Start()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            
            List<Matrix4x4> modelMatrices = _matrixLoader.Load(ModelPath);
            List<Matrix4x4> spaceMatrices = _matrixLoader.Load(SpacePath);

            List<Matrix4x4> validOffsets = spaceMatrices.GetOffsets(modelMatrices, OffsetEpsilon);
            
            stopwatch.Stop();
            
            _matrixLoader.ExportToJson(validOffsets, OffsetsPath);
            
            Debug.Log($"Found {validOffsets.Count} valid offsets per time: {stopwatch.Elapsed}");

            stopwatch.Reset();
            stopwatch.Start();

            _visualizer.Visualize(validOffsets, Color.red);
            _visualizer.Visualize(modelMatrices, Color.white);
            _visualizer.Visualize(spaceMatrices, Color.cyan);
            
            stopwatch.Stop();
            
            Debug.Log("Visualize time: " + stopwatch.Elapsed);
        }
    }
}