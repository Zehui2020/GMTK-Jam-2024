using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScaleController : MonoBehaviour
{
    [SerializeField] private Transform _pivotPosition;
    [SerializeField] private Transform _bar;

    [SerializeField] private float _rotationSensitivity;
    [SerializeField] private float _clampRotation;
    [SerializeField] private float _calculateInterval;
    [SerializeField] private float _baseLerpSpeed;
    [SerializeField] private float _decelerationFactor;

    private Coroutine _calculateRoutine;
    private Coroutine _rotateRoutine;

    [SerializeField] private TextMeshProUGUI _angleText;

    public void StartCalculation()
    {
        _calculateRoutine = StartCoroutine(CalculateRoutine());
    }

    public void PauseCalculation()
    {
        if (_calculateRoutine == null)
            return;

        StopCoroutine(_calculateRoutine);
    }

    private float CalculateResultant()
    {
        float totalLeftResultant = 0;
        float totalRightResultant = 0;

        List<BaseEntity> entities = EntityController.Instance.GetAllEntities();
        foreach (BaseEntity entity in entities)
        {
            if (entity == null)
            {
                continue;
            }

            bool isRight = false;

            if (entity.transform.position.x > _pivotPosition.transform.position.x)
            {
                isRight = true;
            }

            // M = F x d
            float moment = entity.GetStats().weight * Mathf.Abs(entity.transform.position.x - _pivotPosition.position.x);

            if (isRight)
            {
                totalRightResultant += moment;
            }
            else
            {
                totalLeftResultant += moment;
            }
        }

        return totalLeftResultant - totalRightResultant;
    }

    private void CalculateRotation()
    {
        float resultant = CalculateResultant();
        float angle = resultant * _rotationSensitivity;
        angle = Mathf.Clamp(angle, -_clampRotation, _clampRotation);

        Quaternion targetAngle = Quaternion.Euler(0, 0, angle);
        float rotationSpeed = Mathf.Abs(resultant) * _baseLerpSpeed;

        if (_rotateRoutine != null)
            StopCoroutine(_rotateRoutine);
        _rotateRoutine = StartCoroutine(LerpRotation(targetAngle, rotationSpeed, _decelerationFactor));

        _angleText.text = "Angle: " + angle;
    }

    private IEnumerator LerpRotation(Quaternion targetRotation, float initialRotationSpeed, float decelerationFactor)
    {
        Quaternion initialRotation = _bar.rotation;
        float rotationSpeed = initialRotationSpeed;

        while (Quaternion.Angle(_bar.rotation, targetRotation) > 0.01f)
        {
            float angleDifference = Quaternion.Angle(_bar.rotation, targetRotation);

            rotationSpeed = Mathf.Lerp(0, initialRotationSpeed, angleDifference / decelerationFactor);

            float angle = rotationSpeed * Time.deltaTime;
            _bar.rotation = Quaternion.RotateTowards(_bar.rotation, targetRotation, angle);
            yield return null;
        }

        _bar.rotation = targetRotation;
    }


    private IEnumerator CalculateRoutine()
    {
        while (true)
        {
            CalculateRotation();
            yield return new WaitForSeconds(_calculateInterval);
        }
    }
}
