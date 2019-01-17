using UnityEngine;

public class DealerController : MonoBehaviour
{
    [SerializeField] private int _rows = 3;
    [SerializeField] private int _columns = 4;
    [SerializeField] private Vector2 _margin;
    [SerializeField] private Transform _deckParent;
    
    public void PlaceCards(CardController[] cardArray)
    {
        if (_deckParent == null)
        {
            _deckParent = transform;
        }

        GameObject temp = cardArray[0].gameObject;
        Bounds cardBounds = temp.GetComponent<Collider>().bounds;
        
        Vector3 totalSize = Vector2.zero;
        totalSize.x = cardBounds.center.x * 4f + _margin.x * 4f;
        totalSize.y = cardBounds.center.y * 3f + _margin.y * 3f;
        Vector3 intialPosition = _deckParent.position - totalSize;
        int i, j, x = 0;
        for (i = 0; i < _rows; i++)
        {
            for (j = 0; j < _columns; j++)
            {
                Vector3 position = intialPosition;
                position.y += cardBounds.size.y * i + _margin.y * i;
                position.x += cardBounds.size.x * j + _margin.x * j;
                cardArray[x].gameObject.transform.position = position;
                x++;
            }
        }
    }
}
