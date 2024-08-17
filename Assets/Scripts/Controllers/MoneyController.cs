using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyController : MonoBehaviour
{
    private float money;
    private float increaseRate;

    //Singleton
    private MoneyController instance;
    public static MoneyController Instance { get; private set; }

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

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        HandleUpdate();   
    }

    public void Init()
    {
        money = 0;
        increaseRate = 10;
    }

    public void HandleUpdate()
    {
        money += increaseRate * Time.deltaTime;
        Debug.Log((int)money);
    }

    public bool SpendMoney(int _amt)
    {
        bool canSpend = money > _amt;
        if (canSpend)
        {
            money -= _amt;
        }
        return canSpend;
    }

    public void AddMoney(int _amt)
    {
        money += _amt;
    }

    public void IncreaseRate(int _amt)
    {
        increaseRate += _amt;
    }
}
