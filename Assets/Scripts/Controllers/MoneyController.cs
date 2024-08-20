using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyController : MonoBehaviour
{
    public float money;
    [SerializeField] private float increaseRate;

    [SerializeField] private TextMeshProUGUI _costText;

    [SerializeField]
    private MoneyAnimation _moneyAnimation;

    [SerializeField]
    private GameObject _moneyAnimationParent;

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
        _costText.text = ((int)(money)).ToString();
    }

    public void Init()
    {
        money = 0;
    }

    public void HandleUpdate()
    {
        money += increaseRate * Time.deltaTime;
        //Debug.Log((int)money);
    }

    public bool SpendMoney(int _amt)
    {
        bool canSpend = money > _amt;
        if (canSpend)
        {
            money -= _amt;
            MoneyAnimation anim = Instantiate(_moneyAnimation, _moneyAnimationParent.transform).GetComponent<MoneyAnimation>();
            anim.SetText("- " + _amt);

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