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
        if (TowerBuilder.Instance.HasSelectedTower())
            return;
        if (squareFilledMat)
            _renderer.sharedMaterial = squareFilledMat;

    }

    void OnMouseExit()
    {
        // if(!TowerBuilder.Instance.HasSelectedTower())
        //     return;
        if (!tileTower)
        {
            _renderer.sharedMaterial = originalMaterial;
        }
    }

    void OnMouseDown()
    {
        if (!tileTower)
        {

            if (TowerBuilder.Instance.HasSelectedTower())
            {
                int cost = TowerBuilder.Instance.GetSelectedTowerCost();
                if (!MoneyManager.Instance.BuyTower(cost))
                {
                    Debug.LogWarning("Cannot afford selected tower");
                    return;
                }
                GameObject towerPrefab = TowerBuilder.Instance.GetSelectedTowerPrefab();

                var tower = Instantiate(towerPrefab, transform.parent.position, transform.parent.rotation);

                tileTower = tower;

                TowerBuilder.Instance.ClearSelection();
            }
        }


    }

    void HighlightTile()
    {
        if (squareFilledMat)
            _renderer.sharedMaterial = squareFilledMat;
    }
}
