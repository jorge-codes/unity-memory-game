using UnityEngine;
using System.Collections;

public class CardController : MonoBehaviour
{
    public Renderer cardIconRenderer;
    private Sprite _sprite;
    private int _id;
    private GameManager _manager;
    private Collider _collider;
    private bool _isFlipped;
    
    
    public void Setup(Sprite sprite, int cardId, GameManager gameManager)
    {
        _sprite = sprite;
        _id = cardId;
        _manager = gameManager;
        cardIconRenderer.material.mainTexture = _sprite.texture;
        transform.rotation = Quaternion.identity;
    }
    
    public void Flip(bool val)
    {
        _isFlipped = val;
        if (_isFlipped)
        {
            _manager.FlipCard(_id); 
        }

        StartCoroutine(FlipAnimation(_manager.flipTime, _isFlipped));
    }

    private IEnumerator FlipAnimation(float time, bool isFlipped)
    {
        Quaternion origin = transform.rotation;
        Vector3 rotationVector = Vector3.zero;
        if (_isFlipped)
        {
            rotationVector = new Vector3(0f, 180f, 0f);            
        }
        Quaternion target = Quaternion.Euler(rotationVector);
        float timer = 0f;
        while (timer < time)
        {
            transform.rotation = Quaternion.Slerp(origin, target, timer/time);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void Disable()
    {
        _collider.enabled = false;
    }

    public void ResetCard()
    {
        Flip(false);
        _collider.enabled = true;
    }

    
    private void Start()
    {
        _isFlipped = false;
        _collider = GetComponent<Collider>();
    }
     
    private void OnMouseDown()
    {
        _isFlipped = !_isFlipped;
        Flip(_isFlipped);
    }
}
