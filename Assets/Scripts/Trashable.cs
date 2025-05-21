using System;
using DG.Tweening;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Trashable : MonoBehaviour
{
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private Ease animationEase = Ease.InOutSine;

    private bool isTrash = false;
    public bool IsTrash => isTrash;

    private bool wasGrabbed = false;
    public bool WasGrabbed => wasGrabbed;

    private InteractableUnityEventWrapper _eventWrapper;
    private HandGrabInteractable _interactable;

    private void Start()
    {
        _interactable = GetComponentInChildren<HandGrabInteractable>();
        if (_interactable == null) return;

        _eventWrapper = _interactable.GetComponent<InteractableUnityEventWrapper>();

        if (_eventWrapper == null) return;
        _eventWrapper.WhenSelect.AddListener(Grabbed);
        _eventWrapper.WhenUnselect.AddListener(Released);
    }

    private void OnDestroy()
    {
        if (_eventWrapper == null) return;
        _eventWrapper.WhenSelect.RemoveListener(Grabbed);
        _eventWrapper.WhenUnselect.RemoveListener(Released);
    }

    private void Released()
    {
        wasGrabbed = false;
    }

    private void Grabbed()
    {
        wasGrabbed = true;
    }

    public void Trash(Transform trashCan)
    {        
        if (_interactable != null)
        {
            _interactable.enabled = false; // disable being grabbable
        }

        Sequence trashSeq = DOTween.Sequence();
        trashSeq.Join(transform.DOMove(trashCan.transform.position, animationDuration).SetEase(animationEase));
        trashSeq.Join(transform.DOScale(Vector3.one * 0.001f, animationDuration).SetEase(animationEase));
        trashSeq.OnComplete(() => Destroy(gameObject));

        isTrash = true;
    }
}
