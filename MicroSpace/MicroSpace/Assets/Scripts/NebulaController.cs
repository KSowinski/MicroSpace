using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class NebulaController : MonoBehaviour
{
    public Material[] NebulaMaterials;
    private float _speed;
    private Vector2 _angle;

    public void Start()
    {
        transform.GetComponent<Renderer>().sortingLayerID = 1;
        transform.GetComponent<Renderer>().sortingOrder = 100;

        //Random texture
        transform.GetComponent<MeshRenderer>().materials = new Material[]
        {
            NebulaMaterials[Random.Range(0, NebulaMaterials.Length)]
        };

        _speed = Random.Range(2f, 4f);
        _angle = Random.insideUnitCircle;
    }

    private void Update()
    {
        TextureUvMovement();
    }

    private void TextureUvMovement()
    {
        var mr = this.GetComponent<MeshRenderer>();
        var mat = mr.material;
        var offset = mat.mainTextureOffset;
        offset.x += Time.deltaTime * _angle.x * (_speed / 100f) * (-1);
        offset.y += Time.deltaTime * _angle.y * (_speed / 100f) * (-1);
        mat.mainTextureOffset = offset;
        
    }
}

