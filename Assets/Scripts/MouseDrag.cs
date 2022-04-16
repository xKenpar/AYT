using UnityEngine;

public class MouseDrag : MonoBehaviour {

    [SerializeField] LayerMask letterLayer;
    [SerializeField] LayerMask slotLayer;
    [SerializeField] LayerMask deckLayer;

    Vector3 _dragOffset;
    Vector3 _targetStart;
    Camera _cam;
    
    Transform _targetObject;
    bool _dragging = false;

    void Awake() {
        _cam = Camera.main;
    }

    void Update() {
        if (Input.GetMouseButton(0)) {
            if (!_dragging) {
                RaycastHit2D hit = Physics2D.Raycast(GetMousePos(), Vector2.zero, Mathf.Infinity, letterLayer);

                if (hit.collider != null) {
                    _targetObject = hit.collider.transform;
                    _targetStart = _targetObject.position;
                    _dragOffset = hit.collider.transform.position - GetMousePos();
                } else {
                    _targetObject = null;
                }

            } else if(_dragging && _targetObject != null) {
                _targetObject.position = GetMousePos() + _dragOffset;
            }

            _dragging = true;
        } else {
            if (_dragging && _targetObject != null) {
                RaycastHit2D hitSlot = Physics2D.Raycast(_targetObject.position - new Vector3(0,0,5), Vector2.zero, Mathf.Infinity, slotLayer);

                if (hitSlot.collider != null) {
                    if (hitSlot.collider.GetComponent<LetterSlot>().assignedLetter == null) {
                        _targetObject.position = hitSlot.collider.transform.position;
                        hitSlot.collider.GetComponent<LetterSlot>().assignedLetter = transform;

                        if(_targetObject.GetComponent<Letter>().assignedSlot)
                            _targetObject.GetComponent<Letter>().assignedSlot.GetComponent<LetterSlot>().assignedLetter = null;
                        _targetObject.GetComponent<Letter>().assignedSlot = hitSlot.collider.transform;
                    } else {
                        _targetObject.position = _targetStart;
                    }
                    
                } else {
                    RaycastHit2D hitDeck = Physics2D.Raycast(_targetObject.position - new Vector3(0, 0, 5), Vector2.zero, Mathf.Infinity, deckLayer);

                    if (hitDeck.collider == null)
                        _targetObject.position = _targetStart;
                    else
                        if(_targetObject.GetComponent<Letter>().assignedSlot)
                            _targetObject.GetComponent<Letter>().assignedSlot.GetComponent<LetterSlot>().assignedLetter = null;
                }
            }

            _dragging = false;
        }
    }

    Vector3 GetMousePos() {
        var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
}