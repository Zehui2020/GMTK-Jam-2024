using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopSelectionController : MonoBehaviour
{
    private Entity _selectedEntity;
    [SerializeField] private SpriteRenderer placeholderSprite;

    [SerializeField] private Transform minSpawnPoint;
    [SerializeField] private Transform maxSpawnPoint;

    public void SelectTroop(Entity entityToSpawn)
    {
        _selectedEntity = entityToSpawn;
        placeholderSprite.sprite = _selectedEntity.placeholderSprite;
        placeholderSprite.gameObject.SetActive(true);
    }

    public void SpawnTroop(Vector3 position)
    {
        if (_selectedEntity == null)
        {
            return;
        }

        EntityController.Instance.SpawnPlayerEntity(_selectedEntity._entityPrefab, position);
        _selectedEntity = null;
    }

    private void Update()
    {
        if (_selectedEntity == null)
        {
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

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SpawnTroop(cursorWorldPosition);
            placeholderSprite.gameObject.SetActive(false);
        }
    }
}