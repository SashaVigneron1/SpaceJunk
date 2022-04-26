using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [System.Serializable]
    struct RendererAndIndex
    {
        public MeshRenderer renderer;
        public int index;
    }

    [SerializeField] private List<RendererAndIndex> renderersAndIndexes;

    public void SetColor(Color color)
    {
        foreach (var rendererAndIndex in renderersAndIndexes)
        {
            rendererAndIndex.renderer.materials[rendererAndIndex.index].color = color;
        }
    }
}
