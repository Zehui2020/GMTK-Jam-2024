using UnityEngine;
using UnityEngine.Events;
public class TroopSelectionController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer placeholderSprite;

    [SerializeField] private Transform minSpawnPoint;
    [SerializeField] private Transform maxSpawnPoint;

    [Header("TroopSelectionController Events")]
    [SerializeField] private UnityEvent<Entity> _onTroopSpawned;

    private Entity _selectedEntity;

    public bool canSpawn = true;

    public void SetCanSpawn()
    {
        canSpawn = true;
    }

    public void SetCannotSpawn()
    {
        canSpawn = false;
    }

    public void SelectTroop(Entity entityToSpawn)
    {
        _selectedEntity = entityToSpawn;
        placeholderSprite.sprite = _selectedEntity.placeholderSprite;
        placeholderSprite.gameObject.SetActive(true);
    }

    public Entity GetSelectedEntity()
    {
        return _selectedEntity;
    }

    public void SpawnTroop(Vector3 position)
    {
        if (_selectedEntity == null)
        {
            return;
        }

        EntityController.Instance.SpawnPlayerEntity(_selectedEntity._entityPrefab, position);
        _onTroopSpawned.Invoke(_selectedEntity);
        _selectedEntity = null;
    }

    private void Update()
    {
        if (_selectedEntity == null)
        {
            return;
        }

        if (MoneyController.Instance.money < _selectedEntity._stats.cost)
        {
            placeholderSprite.gameObject.SetActive(false);
            _selectedEntity = null;
            return;
        }

        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = 0;
        Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        cursorWorldPosition = new Vector3(
            Mathf.Clamp(cursorWorldPosition.x, minSpawnPoint.position.x, maxSpawnPoint.position.x),
            Mathf.Clamp(cursorWorldPosition.y, minSpawnPoint.position.y, maxSpawnPoint.position.y),
            Mathf.Clamp(cursorWorldPosition.z, minSpawnPoint.position.z, maxSpawnPoint.position.z));

        placeholderSprite.transform.localPosition = new Vector3(
            cursorWorldPosition.x,
            placeholderSprite.transform.localPosition.y, 
            placeholderSprite.transform.localPosition.z);

        if (Input.GetMouseButtonDown(0) && canSpawn)
        {
            SpawnTroop(cursorWorldPosition);
            placeholderSprite.gameObject.SetActive(false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            placeholderSprite.gameObject.SetActive(false);
            _selectedEntity = null;
        }
    }
}