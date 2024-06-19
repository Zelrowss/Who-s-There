using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDManager : MonoBehaviour
{
    [Header("Reference")]
    private PlayerController _playerController;

    [Header("SprintBar")]
    public Image sprintBarFillerImage;
    public Image sprintBarBackgroundImage;
    public Image sprintLogoImage;
    public Color fullColor = Color.white;
    public Color midColor = Color.yellow;
    public Color lowColor = Color.red;
    private bool canChangeSprintBarColor = false;

    private void SetSprintBarAlpha(float alpha)
    {
        Color fillerColor = sprintBarFillerImage.color;
        fillerColor.a = alpha;
        sprintBarFillerImage.color = fillerColor;

        Color backgroundColor = new Color(.3f, .3f, .3f, alpha);
        sprintBarBackgroundImage.color = backgroundColor;

        Color logoColor = new Color(1f, 1f, 1f, alpha);
        sprintLogoImage.color = logoColor;
    }

    public IEnumerator ShowSprintBar() {
        canChangeSprintBarColor = true;
        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);

            SetSprintBarAlpha(alpha);
            yield return null;
        }

        SetSprintBarAlpha(1f);
    }

    public IEnumerator HideSprintBar() {
        canChangeSprintBarColor = false;
        yield return new WaitForSeconds(2.5f);
        
        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - (elapsedTime / duration));

            SetSprintBarAlpha(alpha);
            yield return null;
        }

        SetSprintBarAlpha(0f);
    }

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        sprintBarFillerImage.fillAmount = 1;
        canChangeSprintBarColor = false;
    }

    private void Update()
    {
        sprintBarFillerImage.fillAmount = _playerController.sprintAmount / _playerController.sprintMax;

        if (canChangeSprintBarColor) {
            float fillAmount = sprintBarFillerImage.fillAmount;

            if (fillAmount > 0.6f)
            {
                sprintBarFillerImage.color = Color.Lerp(midColor, fullColor, (fillAmount - 0.6f) / 0.4f);
            }
            else if (fillAmount > 0.05f)
            {
                sprintBarFillerImage.color = Color.Lerp(lowColor, midColor, (fillAmount - 0.05f) / 0.55f);
            }
            else
            {
                sprintBarFillerImage.color = lowColor;
            }
        }
    }

}
