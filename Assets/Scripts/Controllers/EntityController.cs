using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using static BaseEntity;

public class EntityController : MonoBehaviour
{
    public GameObject allyPrefab1;
    public GameObject allyPrefab2;

    public GameObject enemyPrefab1;

    //list of entities
    public List<BaseEntity> allyEntities;
    public List<BaseEntity> enemyEntities;
    public List<BaseEntity> entitiesToAdd;

    //endanimation
    [SerializeField]
    private EndAnimation endAnimation;

    //List of enemy base stats
    [SerializeField]
    private List<int> _enemyBaseHealth;

    //level controller
    [SerializeField] 
    LevelController _levelController;

    [SerializeField]
    PlayerDataSO _playerDataSO;

    // Cached list of all entities
    private List<BaseEntity> _allEntities = new();
    private bool _entitiesDirty = true;

    //Singleton
    private EntityController instance;
    public static EntityController Instance { get; private set; }

    private int _nextLevelUnlock;

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
        DebugUtility.Assert(!enemyEntities.IsEmpty(), "There is no enemy base");
        DebugUtility.Assert(!allyEntities.IsEmpty(), "There is no ally base");

        enemyEntities[0].Init(allyEntities[0].transform);
        enemyEntities[0].GetComponent<SpriteRenderer>().sortingOrder = 15;
        allyEntities[0].Init(enemyEntities[0].transform);
        allyEntities[0].GetComponent<SpriteRenderer>().sortingOrder = 15;

        entitiesToAdd = new List<BaseEntity>();
        SetLevel(_levelController.GetLevel().levelID);
        _nextLevelUnlock = _levelController.GetLevel().nextLevelID;

        if (_levelController.GetLevel().levelID >= 4)
            AudioManager.Instance.PlayBossBGM();
        else
            AudioManager.Instance.PlayNormalBGM();
    }

    public void HandleUpdate(float _scaleAngle)
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

        //add entities
        foreach (BaseEntity e in  entitiesToAdd)
        {
            //add in respective list
            (e.isEnemy ? enemyEntities : allyEntities).Add(e);
        }
        //clear list
        entitiesToAdd.Clear();

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

            //handle active traits
            entity.HandleActiveTrait(_scaleAngle);

            entity.HandleUpdate();

            if (entity.GetStatusEffect() == EntityStatusEffect.None)
            {
                //Range check for ally entities to attack
                foreach (var entity2 in enemyEntities)
                {
                    //check if entity is dead
                    if (entity2.GetState() == BaseEntity.EntityState.Death || !entity2.isTargetable)
                        continue;

                    if (entity.DetectedEnemy(Vector3.Distance(entity.GetPos(), entity2.GetPos()) <= entity.GetStats().detectRange))
                    {
                        break;
                    }
                }
            }
        }

        if (!deadAllyEntity.IsEmpty())
        {
            _entitiesDirty = true;
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

            //handle active traits
            entity.HandleActiveTrait(_scaleAngle);
            
            if (entity.GetStatusEffect() != EntityStatusEffect.None)
            {
                entity.HandleUpdate();
                continue;
            }

            //Range check for ally entities to attack
            foreach (var entity2 in allyEntities)
            {
                //check if entity is dead
                if (entity2.GetState() == BaseEntity.EntityState.Death || !entity2.isTargetable)
                    continue;


                if (entity.DetectedEnemy(Vector3.Distance(entity.GetPos(), entity2.GetPos()) <= entity.GetStats().detectRange))
                {
                    break;
                }
            }
            entity.HandleUpdate();
        }

        //remove dead entities
        if (!deadEnemyEntity.IsEmpty())
        {
            _entitiesDirty = true;
        }

        foreach (BaseEntity entityToDestroy in deadEnemyEntity)
        {
            //check if is base entity
            if (entityToDestroy == enemyEntities[0])
            {
                //Destroy all entities
                EndGame(false);
                break;
            }

            //check to get money from killing enemy
            //check if enemy was killed
            if (entityToDestroy.GetStats().health <= 0)
            {
                //get money
                MoneyController.Instance.AddMoney(entityToDestroy.GetStats().moneyEarnedOnDeath);
            }

            //remove from list
            enemyEntities.Remove(entityToDestroy);

            //destroy entity
            Destroy(entityToDestroy.gameObject);
        }
    }

    public void HandleEntityAttack(BaseEntity _attackingEntity)
    {
        Vector3 _entityPos = _attackingEntity.GetPos();
        EntityStats _entityStats = _attackingEntity.GetStats();
        //check attack dist between troops
        if (_attackingEntity.isEnemy)
        {
            foreach(BaseEntity e in allyEntities)
            {
                //check if entity is dead
                if (e.GetState() == BaseEntity.EntityState.Death || !e.isTargetable)
                    continue;

                //distance check
                float dist = Vector3.Distance(_entityPos, e.GetPos());
                if (_entityStats.minAttackRange <= dist && dist <= _entityStats.maxAttackRange)
                {
                    //attack
                    e.Damage(_attackingEntity.GetSumDamage());
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
                //check if entity is dead
                if (e.GetState() == BaseEntity.EntityState.Death || !e.isTargetable)
                    continue;

                //distance check
                float dist = Vector3.Distance(_entityPos, e.GetPos());
                if (_entityStats.minAttackRange <= dist && dist <= _entityStats.maxAttackRange)
                {
                    //attack
                    e.Damage(_attackingEntity.GetSumDamage());
                    //check if not area of effect
                    if (!_entityStats.isAreaOfEffect)
                    {
                        break; //only hit one enemy
                    }
                }
            }
        }
    }

    public BaseEntity SpawnEntity(GameObject _entityToSpawnPrefab, bool isEnemyEntity)
    {
        //instantiate new entity
        GameObject newEntity = Instantiate(_entityToSpawnPrefab);
        //spawn at own base
        newEntity.transform.position = isEnemyEntity ? enemyEntities[0].transform.position : allyEntities[0].transform.position;
        newEntity.transform.parent = isEnemyEntity ? enemyEntities[0].transform.parent : allyEntities[0].transform.parent;
        newEntity.transform.rotation = newEntity.transform.parent.rotation;

        BaseEntity baseEntity = newEntity.GetComponent<BaseEntity>();
        baseEntity.isEnemy = isEnemyEntity;

        //add to add list
        entitiesToAdd.Add(baseEntity);
        baseEntity.Init(
            (isEnemyEntity ? allyEntities : enemyEntities).First().transform);
        _entitiesDirty = true;

        return baseEntity;
    }

    public void SpawnPlayerEntity(GameObject _entityToSpawnPrefab, Vector3 position)
    {
        //instantiate new player entity
        GameObject newEntity = Instantiate(_entityToSpawnPrefab);
        //spawn at set position
        newEntity.transform.parent = allyEntities[0].transform.parent;
        newEntity.transform.position = new Vector3(position.x, 0, 0);
        newEntity.transform.localPosition = new Vector3(newEntity.transform.localPosition.x, 0, 0);
        newEntity.transform.rotation = newEntity.transform.parent.rotation;

        BaseEntity baseEntity = newEntity.GetComponent<BaseEntity>();
        baseEntity.isEnemy = false;

        //allyEntities.Add(baseEntity);
        //add to add list
        entitiesToAdd.Add(baseEntity);
        DebugUtility.Assert(!enemyEntities.IsEmpty(), "There is no enemy base");
        baseEntity.Init(enemyEntities[0].transform);

        MoneyController.Instance.SpendMoney(baseEntity.GetStats().cost);

        _entitiesDirty = true;
    }

    public void AddEntity(GameObject _entityToAdd, bool isEnemyEntity)
    {
        _entityToAdd.transform.parent = isEnemyEntity ? enemyEntities[0].transform.parent : allyEntities[0].transform.parent;
        _entityToAdd.transform.rotation = _entityToAdd.transform.parent.rotation;
        BaseEntity _baseEntity = _entityToAdd.GetComponent<BaseEntity>();
        _baseEntity.isEnemy = isEnemyEntity;
        //add to add list
        entitiesToAdd.Add(_baseEntity);
        _entitiesDirty = true;
    }

    public void EndGame(bool enemyWin)
    {
        var entities = enemyWin ? allyEntities : enemyEntities;

        for (int i = 1; i < entities.Count; ++i)
        {
            Destroy(entities[i].gameObject);
            entities.RemoveAt(i);
            i--;
        }

        _entitiesDirty = true;

        //Debug.Log(enemyWin ? "Enemy wins" : "Player wins");

        if (enemyWin)
        {
            endAnimation.EndGame(false);
        }
        else
        {
            if (_playerDataSO)
            {
                if (_playerDataSO.MaxUnlockedLevel < _nextLevelUnlock)
                {
                    _playerDataSO.MaxUnlockedLevel = _nextLevelUnlock;
                }
            }
            endAnimation.EndGame(true);
            
        }
    }

    public List<BaseEntity> GetAllEntities()
    {
        if (_entitiesDirty)
        {
            _allEntities = allyEntities
                .GetRange(1, allyEntities.Count - 1)
                .Concat(enemyEntities.GetRange(1, enemyEntities.Count - 1))
                .ToList();
        }

        return _allEntities;
    }

    public void ApplyStatusEffect(BaseEntity _entity, EntityStatusEffect _effect, int totalEntitiesAffected, bool applyWithoutDist = false)
    {
        Vector3 _entityPos = _entity.GetPos();
        EntityStats _entityStats = _entity.GetStats();
        //check attack dist between troops
        if (_entity.isEnemy)
        {
            foreach (BaseEntity e in allyEntities)
            {
                //check if entity is dead
                if (e.GetState() == BaseEntity.EntityState.Death || !e.isTargetable)
                    continue;

                bool pass = true;
                //check if effect is stun
                if (_effect == EntityStatusEffect.Stun)
                {
                    //chance
                    pass = Random.Range(0.0f, 1.0f) <= _entity.GetStats().attackTraitPercentage;
                }

                //distance check
                float dist = Vector3.Distance(_entityPos, e.GetPos());
                if ((_entityStats.minAttackRange <= dist && dist <= _entityStats.maxAttackRange && pass) || applyWithoutDist)
                {
                    //apply
                    e.ApplyStatusEffect(_effect);
                    totalEntitiesAffected--;
                    //check if not area of effect
                    if (totalEntitiesAffected == 0)
                    {
                        break; //hit max enemy
                    }
                }
            }
        }
        //check attack dist between enemy
        else
        {
            foreach (BaseEntity e in enemyEntities)
            {
                //check if entity is dead
                if (e.GetState() == BaseEntity.EntityState.Death || !e.isTargetable)
                    continue;

                bool pass = true;
                //check if effect is stun
                if (_effect == EntityStatusEffect.Stun)
                {
                    //chance
                    pass = Random.Range(0.0f, 1.0f) <= _entity.GetStats().attackTraitPercentage;
                }

                //distance check
                float dist = Vector3.Distance(_entityPos, e.GetPos());
                if ((_entityStats.minAttackRange <= dist && dist <= _entityStats.maxAttackRange && pass) || applyWithoutDist)
                {
                    //apply
                    e.ApplyStatusEffect(_effect);
                    totalEntitiesAffected--;
                    //check if not area of effect
                    if (totalEntitiesAffected == 0)
                    {
                        break; //hit max enemy
                    }
                }
            }
        }
    }
    public void SetLevel(int levelID)
    {
        if (levelID == -1)
        {
            //endless
            enemyEntities[0].Damage(-99999999);
        }
        else
        {
            enemyEntities[0].Damage(_enemyBaseHealth[0]
                - _enemyBaseHealth[levelID]);
        }
    }

}