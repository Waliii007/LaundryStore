using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class SettingsPanel : MonoBehaviour
    {
       [Header("Buttons")]
    [SerializeField] private Button soundButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button hapticsButton;

    [Header("Sprites")]
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    private bool isSoundOn;
    private bool isMusicOn;
    private bool isHapticsOn;

    private void Start()
    {
        // Load saved states
        isSoundOn = PlayerPrefs.GetInt("SFXMuted", 0) == 0;
        isMusicOn = PlayerPrefs.GetInt("BGMuted", 0) == 0;
        isHapticsOn = PlayerPrefs.GetInt("HapticsEnabled", 1) == 1;

        // Setup visuals
        UpdateButtonVisual(soundButton, isSoundOn);
        UpdateButtonVisual(musicButton, isMusicOn);
        UpdateButtonVisual(hapticsButton, isHapticsOn);

        // Add listeners
        soundButton.onClick.AddListener(() => ToggleSound());
        musicButton.onClick.AddListener(() => ToggleMusic());
        hapticsButton.onClick.AddListener(() => ToggleHaptics());
    }

    private void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        PlayerPrefs.SetInt("SFXMuted", isSoundOn ? 0 : 1);
        UpdateButtonVisual(soundButton, isSoundOn);
        SoundManager.instance.ToggleMute(SoundType.SFX);
    }

    private void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        PlayerPrefs.SetInt("BGMuted", isMusicOn ? 0 : 1);
        UpdateButtonVisual(musicButton, isMusicOn);
        SoundManager.instance.ToggleMute(SoundType.Background);
    }

    private void ToggleHaptics()
    {
        isHapticsOn = !isHapticsOn;
        PlayerPrefs.SetInt("HapticsEnabled", isHapticsOn ? 1 : 0);
        UpdateButtonVisual(hapticsButton, isHapticsOn);
        HapticManager.SetHapticsEnabled(isHapticsOn);
    }

    private void UpdateButtonVisual(Button button, bool isOn)
    {
        Image image = button.GetComponent<Image>();
        if (image != null)
        {
            image.sprite = isOn ? onSprite : offSprite;
        }
    }
    }
}
public static class HapticManager
{
    private static bool enabled = true;

    public static void SetHapticsEnabled(bool isEnabled)
    {
        enabled = isEnabled;
        // Add platform-specific haptic control here
    }

    public static void TriggerHaptic()
    {
        if (!enabled) return;
        // Trigger haptic feedback here
    }

    public static bool IsEnabled()
    {
        return enabled;
    }
}