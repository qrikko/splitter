using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilePreview : MonoBehaviour
{
    private GridLayoutGroup _layout;
    
    public void set_cell_size(float size) {
        _layout.cellSize = new Vector2(size, size);
    }

    void Start() {
        _layout = GetComponent<GridLayoutGroup>();
    }
}
