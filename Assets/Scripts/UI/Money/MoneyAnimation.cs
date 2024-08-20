using TMPro;
using UnityEngine;

public class MoneyAnimation : MonoBehaviour
{
    [SerializeField]
    private TMP_Text moneyText;

    private float _currTime;

    public void SetText(string newText)
    {
        moneyText.text = newText;
    }

    private void Start()
    {
        _currTime = 0;
    }

    private void Update()
    {
        _currTime += Time.deltaTime * 2;
        if (_currTime <= 1)
        {
            moneyText.color = Color.Lerp(new(1, 0, 0, 0), new(1, 0, 0, 1), _currTime);
            moneyText.transform.localPosition = Vector3.Lerp(new(0, 0, 0), new(0, -60, 0), _currTime);
        }
        else if (_currTime <= 2)
        {
            moneyText.color = new(1, 0, 0, 1);
            moneyText.transform.localPosition = new Vector3(0, -60, 0);
        }
        else if (_currTime <= 3)
        {
            moneyText.color = Color.Lerp(new(1, 0, 0, 1), new(1, 0, 0, 0), _currTime - 2);
            moneyText.transform.localPosition = Vector3.Lerp(new(0, -60, 0), new(-50, -60, 0), _currTime - 2);
        }
    }
}
