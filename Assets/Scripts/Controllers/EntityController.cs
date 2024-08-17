using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    public GameObject allyPrefab1;
    public GameObject allyPrefab2;

    public GameObject enemyPrefab1;

    //list of entities
    public List<BaseEntity> allyEntities;
    public List<BaseEntity> enemyEntities;

    //Singleton
    private EntityController instance;
    public static EntityController Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void Init()
    {

    }

    public void HandleUpdate()
    {
        //DEBUGGING ONLY
        //spawn ally
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //spawn ally
            SpawnEntity(allyPrefab1, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //spawn ally
            SpawnEntity(allyPrefab2, false);
        }
        //spawn enemy
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            //spawn ally
            SpawnEntity(enemyPrefab1, true);
        }

        //loop through all entities and update
        List<BaseEntity> deadAllyEntity = new List<BaseEntity>();

        //ally entities
        foreach (var entity in allyEntities)
        {
            //check if dead
            if (entity.isDead)
            {
                //add to dead list
                deadAllyEntity.Add(entity);
                continue;
            }
            //check to init
            if (!entity.hasInit)
                entity.Init(enemyEntities[0].transform);
            //Range check for ally entities to attack
            foreach (var entity2 in enemyEntities)
            {
                if (entity.DetectedEnemy(Vector3.Distance(entity.transform.position, entity2.transform.position) <= entity.GetStats().detectRange))
                {
                    break;
                }
            }
            entity.HandleUpdate();
        }
        //remove dead entities
        foreach (BaseEntity entityToDestroy in deadAllyEntity)
        {
            //check if is base entity
            if (entityToDestroy == allyEntities[0])
            {
                //Destroy all entities
                EndGame(true) ;
                break;
            }

            //remove from list
            allyEntities.Remove(entityToDestroy);
            //destroy entity
            Destroy(entityToDestroy.gameObject);
        }


        //enemy entities
        List<BaseEntity> deadEnemyEntity = new List<BaseEntity>();
        foreach (var entity in enemyEntities)
        {
            //check if dead
            if (entity.isDead)
            {
                //add to dead list
                deadEnemyEntity.Add(entity);
                continue;
            }
            //check to init
            if (!entity.hasInit)
                entity.Init(allyEntities[0].transform);
            //Range check for ally entities to attack
            foreach (var entity2 in allyEntities)
            {
                if (entity.DetectedEnemy(Vector3.Distance(entity.transform.position, entity2.transform.position) <= entity.GetStats().detectRange))
                {
                    break;
                }
            }
            entity.HandleUpdate();
        }

        //remove dead entities
        foreach (BaseEntity entityToDestroy in deadEnemyEntity)
        {
            //check if is base entity
            if (entityToDestroy == enemyEntities[0])
            {
                //Destroy all entities
                EndGame(false);
                break;
            }



            //remove from list
            enemyEntities.Remove(entityToDestroy);
            //destroy entity
            Destroy(entityToDestroy.gameObject);
        }
    }

    public void HandleEntityAttack(BaseEntity _attackingEntity)
    {
        Vector3 _entityPos = _attackingEntity.transform.position;
        EntityStats _entityStats = _attackingEntity.GetStats();
        //check attack dist between troops
        if (_attackingEntity.isEnemy)
        {
            foreach(BaseEntity e in allyEntities)
            {
                //distance check
                float dist = Vector3.Distance(_entityPos, e.transform.position);
                if (_entityStats.minAttackRange <= dist && dist <= _entityStats.maxAttackRange)
                {
                    //attack
                    e.Damage(_entityStats.attackDamage);
                    //check if not area of effect
                    if (! _entityStats.isAreaOfEffect)
                    {
                        break; //only hit one enemy
                    }
                }
            }
        }
        //check attack dist between enemy
        else
        {
            foreach (BaseEntity e in enemyEntities)
            {
                //distance check
                float dist = Vector3.Distance(_entityPos, e.transform.position);
                if (_entityStats.minAttackRange <= dist && dist <= _entityStats.maxAttackRange)
                {
                    //attack
                    e.Damage(_entityStats.attackDamage);
                    //check if not area of effect
                    if (!_entityStats.isAreaOfEffect)
                    {
                        break; //only hit one enemy
                    }
                }
            }
        }
    }

    public void SpawnEntity(GameObject _entityToSpawnPrefab, bool isEnemyEntity)
    {
        //instantiate new entity
        GameObject newEntity = Instantiate(_entityToSpawnPrefab);
        //spawn at own base
        newEntity.transform.position = isEnemyEntity ? enemyEntities[0].transform.position : allyEntities[0].transform.position;
        newEntity.transform.parent = isEnemyEntity ? enemyEntities[0].transform.parent : allyEntities[0].transform.parent;
        newEntity.transform.rotation = newEntity.transform.parent.rotation;
        newEntity.GetComponent<BaseEntity>().isEnemy = isEnemyEntity;
        //add to own list
        if (isEnemyEntity )
        {
            enemyEntities.Add(newEntity.GetComponent<BaseEntity>());
        }
        else
        {
           allyEntities.Add(newEntity.GetComponent<BaseEntity>());
        }
    }

    public void SpawnPlayerEntity(GameObject _entityToSpawnPrefab, Vector3 position)
    {
        //instantiate new player entity
        GameObject newEntity = Instantiate(_entityToSpawnPrefab);
        //spawn at set position
        newEntity.transform.position = new Vector3(position.x, 0, 0);
        newEntity.transform.parent = allyEntities[0].transform.parent;
        newEntity.transform.localPosition = new Vector3(newEntity.transform.localPosition.x, 0, 0);
        newEntity.transform.rotation = newEntity.transform.parent.rotation;
        newEntity.GetComponent<BaseEntity>().isEnemy = false;

        allyEntities.Add(newEntity.GetComponent<BaseEntity>());
    }

    public void EndGame(bool enemyWin)
    {
        if (enemyWin)
        {
            int i = -1;
            //destroy all ally entity
            foreach (var e in allyEntities)
            {
                i++;
                if (i == 0) continue;

                //BaseEntity entityToDestroy = allyEntities[i];
                //remove from list
                allyEntities.Remove(e);
                //destroy entity
                Destroy(e.gameObject);
            }
            Debug.Log("Enemy Wins");
        }
        else
        {
            int i = -1;
            //destroy all enemy entity
            foreach (var e in enemyEntities)
            {
                i++;
                if (i == 0) continue;
                //BaseEntity entityToDestroy = enemyEntities[i];
                //remove from list
                enemyEntities.Remove(e);
                //destroy entity
                Destroy(e.gameObject);
            }

            Debug.Log("Player Wins");
        }
    }

    public List<BaseEntity> GetAllEntities()
    {
        List<BaseEntity> allEntities = new();

        for (int i = 1; i < allyEntities.Count; i++)
        {
            allEntities.Add(allyEntities[i]);
        }

        for (int i = 1; i < enemyEntities.Count; i++)
        {
            allEntities.Add(enemyEntities[i]);
        }

        return allEntities;
    }
}