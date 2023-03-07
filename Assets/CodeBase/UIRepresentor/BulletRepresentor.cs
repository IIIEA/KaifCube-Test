using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Player;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UIRepresentor
{
  public class BulletRepresentor : MonoBehaviour
  {
    [SerializeField] private Image _ballUI;
    [SerializeField] private Image _reloadImage;
    [SerializeField] private Gun _gun;

    private Coroutine _reloadAnimationRoutine;
    private List<Image> _activeImages = new();

    private void Awake()
    {
      for (int i = 0; i < _gun.ProjectileCount; i++)
      {
        var image = Instantiate(_ballUI, transform);
        _activeImages.Add(image);
      }

      _reloadImage.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
      _gun.Shooted += OnShooted;
      _gun.BulletRestored += OnBulletRestored;
      _gun.RestoreStarted += OnRestoreStarted;
    }

    private void OnDisable()
    {
      _gun.Shooted -= OnShooted;
      _gun.BulletRestored -= OnBulletRestored;
      _gun.RestoreStarted -= OnRestoreStarted;
    }

    private void OnRestoreStarted()
    {
      if (_reloadAnimationRoutine != null)
        StopCoroutine(_reloadAnimationRoutine);

      _reloadAnimationRoutine = StartCoroutine(ReloadAnimation());
    }

    private void OnBulletRestored()
    {
      var imageToEnable = _activeImages.FirstOrDefault(image => image.gameObject.activeSelf == false);

      if (imageToEnable != null)
        imageToEnable.gameObject.SetActive(true);
    }

    private void OnShooted()
    {
      var imageToDisable = _activeImages.FirstOrDefault(image => image.gameObject.activeSelf);

      if (imageToDisable != null)
        imageToDisable.gameObject.SetActive(false);
    }

    private IEnumerator ReloadAnimation()
    {
      _reloadImage.fillAmount = 0;
      _reloadImage.gameObject.SetActive(true);

      while (_reloadImage.fillAmount <= _gun.TimeToRestoreBullet)
      {
        _reloadImage.fillAmount += Time.deltaTime;

        if ((_gun.TimeToRestoreBullet - _reloadImage.fillAmount) <= Mathf.Epsilon)
        {
          _reloadImage.gameObject.SetActive(false);
          yield break;
        }

        yield return null;
      }
    }
  }
}