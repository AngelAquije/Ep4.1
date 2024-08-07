using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float flipYRotationTime = 0.5f;
    private Coroutine turnCoroutine;
    private PlayerMain main;
    private bool isFacingRight;

    private void Awake()
    {
        main = playerTransform.gameObject.GetComponent<PlayerMain>();
        isFacingRight = main.facingRight;
    }
    private void Update()
    {
        transform.position = playerTransform.position;
    }

    public void CallTurn() 
    {
        turnCoroutine = StartCoroutine(FlipYLerp());
    }

    private IEnumerator FlipYLerp() 
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < flipYRotationTime) 
        {
            elapsedTime += Time.deltaTime;
            yRotation = Mathf.Lerp(startRotation, endRotationAmount,(elapsedTime / flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            yield return null;
        }
    }

    private float DetermineEndRotation() 
    {
        isFacingRight = !isFacingRight;

        if (isFacingRight)
        {
            return 180f;
        }

        else 
        {
            return 0f;
        }
    }
}
