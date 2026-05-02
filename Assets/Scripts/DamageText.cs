using System;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class DamageText : MonoBehaviour
{
    public Vector3 minVelocity;
    public Vector3 maxVelocity;
    
    public Vector3 spawnOffset;

    
    public Vector3 minRotation;
    public Vector3 maxRotation;
    
    public Vector2 lifetimeMinMax;
    
    private float spawnTime;
    private float lifetime;
    private Vector3 startingScale;
    
    private void Update()
    {
        transform.LookAt(Camera.main.transform);

        float currentLifetime = Time.time - spawnTime;

        float lifetimeRatio = currentLifetime / lifetime;

        transform.localScale = startingScale * (1-lifetimeRatio);
        
        if (currentLifetime > lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void Awake()
    {
        transform.position += getRandomVector(-spawnOffset, spawnOffset); 
        
        spawnTime = Time.time;
        
        if (TryGetComponent(out Rigidbody _rb))
        {
            _rb.linearVelocity = getRandomVector(minVelocity, maxVelocity);
            _rb.angularVelocity = getRandomVector(minRotation, maxRotation);
        }

        Random random = new Random();
        lifetime = (float)(lifetimeMinMax.x + random.NextDouble() * (lifetimeMinMax.y - lifetimeMinMax.x));
        
        startingScale = transform.localScale;
    }

    public void setText(String newText)
    {
        if (TryGetComponent(out TextMeshProUGUI text))
        {
            text.text = newText;
        }
    }
    
    Vector3 getRandomVector(Vector3 min, Vector3 max)
    {
        Random random = new Random();
        return new Vector3(
                    (float)(min.x + random.NextDouble() * (max.x - min.x)),
                    (float)(min.y + random.NextDouble() * (max.y - min.y)),
                    (float)(min.z + random.NextDouble() * (max.z - min.z))
        );
    }
}
