using UnityEngine;

public class TileBehavior : MonoBehaviour
{
    Renderer _renderer;
    public Material squareFilledMat;
    Material originalMaterial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject towerPrefab;
    GameObject tileTower;
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        originalMaterial = _renderer.material;
    }

    void OnMouseOver()
    {
        if (squareFilledMat)
            _renderer.sharedMaterial = squareFilledMat;
       
    }

    void OnMouseExit() {
        if(!tileTower) {
            _renderer.sharedMaterial = originalMaterial;
        }
    }

    void OnMouseDown() { 
        if (!tileTower) {
            if (towerPrefab) {
                HighlightTile();
                 tileTower = Instantiate(towerPrefab, 
                transform.parent.position, transform.parent.rotation);
            }
        }
        
       
    }
    
    void HighlightTile() {
        if (squareFilledMat)
            _renderer.sharedMaterial = squareFilledMat;
    }
}
