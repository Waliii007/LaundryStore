using LaundaryMan;
using UnityEngine;
using UnityEngine.UI;

public class GenderSelectionPanel : MonoBehaviour
{
    public Image selectedAvatarIcon;
    public Sprite girlSprite;
    public Sprite MaleSprite;

    public GameObject girlHighlight;
    public GameObject MaleHighlight;

    public Button girlButton;
    public Button MaleButton;

     
    private void OnEnable()
    {
        Sex savedAvatar = PlayerPrefsManager.GenderSelection;
        if (savedAvatar == Sex.Female)
        {
            SelectAvatar(Sex.Female);
        }
        else if (savedAvatar == Sex.Male)
        {
            SelectAvatar(Sex.Male);
        }
    }

    public void OnAvatarButtonClicked(int avatarIndex)
    {
        // 0 = Girl, 1 = Male
        Sex gender = avatarIndex == 0 ? Sex.Male : Sex.Female;
        SelectAvatar(gender);
        if (SoundManager.instance)
        {
            SoundManager.instance.Play(SoundName.Click);
        }
    }

    private void SelectAvatar(Sex gender)
    {
        PlayerPrefsManager.GenderSelection = gender;

        if (gender == Sex.Female)
        {
            selectedAvatarIcon.sprite = girlSprite;
            girlHighlight.SetActive(true);
            MaleHighlight.SetActive(false);
        }
        else
        {
            selectedAvatarIcon.sprite = MaleSprite;
            girlHighlight.SetActive(false);
            MaleHighlight.SetActive(true);
        }
        print(gender);
    }

    public void OnNextClicked()
    {
        Singolton.Instance.canvasManager.CurrentStateChanger(UserInfoStates.PlayerProfile); // or next screen
        if (SoundManager.instance)
        {
            SoundManager.instance.Play(SoundName.Click);
        }
    }
}
 