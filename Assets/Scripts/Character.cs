using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class Character : MonoBehaviour
{
    [Header("Character Data")]
    public string characterName = "Unnamed";
    public int age = 20;
    public bool isActive = true;
    public Color characterColor = Color.white;
    public Vector2 position = Vector2.zero;

    [Header("Movement Settings")]
    public float speed = 1f;
    public float amplitude = 1f;
    private SpriteRenderer spriteRenderer;
    private Vector2 basePosition;
    private float perlinOffset;
    private string previousName;
    private Vector3 previousPosition;
    private bool previousActive;
    private Color previousColor;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        basePosition = transform.position;

        perlinOffset = Random.Range(0, 1000);
    }

    private void Update()
    {
        Validate();

        if (!Application.isPlaying) return;

        if (!isActive) return;

        float time = Time.time * speed + perlinOffset;
        float offsetX = Mathf.PerlinNoise(time, 0f) - 0.5f;
        float offsetY = Mathf.PerlinNoise(0f, time) - 0.5f;

        Vector2 offset = new Vector2(offsetX, offsetY) * amplitude;
        transform.position = basePosition + offset;
    }

    void OnValidate()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        Validate();
    }

    void OnDisable()
    {
        Validate();
    }

    public void Validate()
    {

        //Name
        if (gameObject.name != previousName)
        {
            previousName = gameObject.name;
            characterName = gameObject.name;
        }
        if (characterName != previousName)
        {
            previousName = characterName;
            gameObject.name = characterName;
        }

        //IsActive

        if (gameObject.activeSelf != previousActive)
        {
            previousActive = gameObject.activeSelf;
            isActive = gameObject.activeSelf;
        }

        if (isActive != previousActive)
        {
            previousActive = isActive;
            gameObject.SetActive(isActive);
        }
        
        //Color

        if (spriteRenderer.color != previousColor)
        {
            previousColor = spriteRenderer.color;
            characterColor = spriteRenderer.color;
        }

        if (characterColor != previousColor)
        {
            previousColor = characterColor;
            spriteRenderer.color = characterColor;
        }

        //Position

        if (transform.position != previousPosition)
        {
            previousPosition = transform.position;
            position = transform.position;
        }

        if (position != (Vector2)previousPosition)
        {
            previousPosition = position;
            transform.position = position;
        }
    }
}
