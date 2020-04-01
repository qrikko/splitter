using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContextMenu : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _context_menu;
    
    void OnEnable() {
        _context_menu.SetActive(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_context_menu.activeSelf) {
            _context_menu.SetActive(false);
        }
        else if (eventData.button == PointerEventData.InputButton.Right) {
            _context_menu.SetActive(true);
        }
    }
}
