﻿using StlVault.ViewModels;
using UnityEngine;
using UnityEngine.UI;
using static StlVault.ViewModels.SelectionMode;

#pragma warning disable 0649

namespace StlVault.Views
{
    internal class DetailMenu : ViewBase<DetailMenuModel>
    {
        [SerializeField] private Button _currentButton;
        [SerializeField] private Button _selectionButton;

        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _inactiveColor;

        [SerializeField] private StatsPanel _statsPanel;
        [SerializeField] private TagInputView _tagsPanel;
        [SerializeField] private RotatePanel _rotatePanel;
        [SerializeField] private ViewPanel _viewPanel;
        
        private Image _currentImage;
        private Image _selectedImage;

        private void Start()
        {
            _currentImage = _currentButton.GetComponent<Image>();
            _selectedImage = _selectionButton.GetComponent<Image>();
        }

        protected override void OnViewModelBound()
        {
            _currentButton.BindTo(ViewModel.SwitchToCurrentModeCommand);
            _selectionButton.BindTo(ViewModel.SwitchToSelectionModeCommand);
            
            ViewModel.Mode.ValueChanged += ModeOnValueChanged;
            ModeOnValueChanged(ViewModel.Mode);

            _statsPanel.BindTo(ViewModel.StatsModel);
            _tagsPanel.BindTo(ViewModel.TagEditorModel);
            _rotatePanel.BindTo(ViewModel.RotateModel);
            _viewPanel.BindTo(ViewModel.ViewPanelModel);
        }

        private void ModeOnValueChanged(SelectionMode mode)
        {
            _currentImage.color = mode == Current ? _activeColor : _inactiveColor;
            _selectedImage.color = mode == Selection ? _activeColor : _inactiveColor;
        }
    }
}