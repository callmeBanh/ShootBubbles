using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class sound : MonoBehaviour
{
    public Image icons;
   public Sprite soundOnIcon;
   public Sprite soundOffIcon;

    void Start()
    {
        UpdateIcon();
    }

   public void toggleSound()
   {
        if(musicBackground.instance != null)
        {
            musicBackground.instance.toggleMusic();
            UpdateIcon();
        }
   }

   private void UpdateIcon()
   {
        if(musicBackground.instance != null && icons != null)
        {
            icons.sprite = musicBackground.instance.isMute() ? soundOffIcon : soundOnIcon;
        }
   }

}
