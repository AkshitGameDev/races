using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;

    private float timer;

    private void OnEnable()
    {
        timer = lifeTime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
            gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
    }
}