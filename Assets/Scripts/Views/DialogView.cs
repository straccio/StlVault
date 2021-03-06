using DG.Tweening;
using StlVault.Util.Commands;
using StlVault.ViewModels;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649

namespace StlVault.Views
{
    [RequireComponent(typeof(CanvasGroup))]
    internal abstract class DialogView<T> : ViewBase<T> where T : class, IDialogModel
    {
        protected const float FadeDuration = 0.25f;
        [SerializeField] private Button _okButton;
        [SerializeField] private Button _cancelButton;

        private Tween _fade;
        private CanvasGroup _canvasGroup;
        private Transform _panel;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _panel = transform.GetChild(0);
        }

        protected virtual void OnShownChanged(bool shown)
        {
            _fade?.Kill();

            if (shown)
            {
                _canvasGroup.blocksRaycasts = true;
                gameObject.SetActive(true);

                _fade = DOTween.Sequence()
                    .Join(_canvasGroup.DOFade(1, FadeDuration))
                    .Join(_panel.DOScale(Vector3.one, FadeDuration));
            }
            else
            {
                _canvasGroup.blocksRaycasts = false;
                _fade = DOTween.Sequence()
                    .Join(_canvasGroup.DOFade(0, FadeDuration))
                    .Join(_panel.DOScale(Vector3.zero, FadeDuration))
                    .OnComplete(() => gameObject.SetActive(false));
            }
        }

        protected override void OnViewModelBound()
        {
            if(_okButton != null) _okButton.BindTo(ViewModel.AcceptCommand);
            if (_cancelButton != null) _cancelButton.BindTo(ViewModel.CancelCommand);
            
            _panel.localScale = Vector3.zero;

            gameObject.SetActive(false);
            ViewModel.Shown.ValueChanged += OnShownChanged;
        }
        
        private void Update()
        {
            if (!ViewModel.Shown) return;
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                if (!HandleAcceptKey()) ViewModel.AcceptCommand.Execute();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!HandleCancelKey()) ViewModel.CancelCommand.Execute();
            }
        }

        protected virtual bool HandleAcceptKey() => false;
        protected virtual bool HandleCancelKey() => false;
    }
}