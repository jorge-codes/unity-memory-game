using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BasicCardController : MonoBehaviour
{
    public Image back;
    public Image icon;
    public Sprite backDefault;
    public Sprite backFlipped;
    public int number;
    public GameManager manager;
    

    public void Setup(Sprite sprite, int number, GameManager manager = null)
    {
        this.manager = manager;
        this.number = number;
        back.sprite = backDefault;
        icon.sprite = sprite;
        icon.enabled = false;
    }

    public void Flip(bool isUp)
    {
        icon.enabled = isUp;
        back.sprite = backDefault;
        
        if (isUp)
        {
            back.sprite = backFlipped;
            if (manager != null)
            {
                manager.FlipCard(number);
            }
        }
    }
    
    
}
