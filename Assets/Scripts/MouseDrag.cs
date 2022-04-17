using UnityEngine;
using DG.Tweening;
public class MouseDrag : MonoBehaviour {

    [SerializeField] LayerMask letterLayer;
    [SerializeField] LayerMask slotLayer;
    [SerializeField] LayerMask deckLayer;

    Vector3 _dragOffset;
    Vector3 _targetStart;
    Transform _startSlot = null;
    Camera _cam;
    
    Transform _targetObject;
    Letter _targetLetter;
    bool _dragging = false;

    void Awake() {
        _cam = Camera.main;
    }

    void Update() {
        if (Input.GetMouseButton(0)) {
            if (!_dragging) {
                RaycastHit2D hit = Physics2D.Raycast(GetMousePos(), Vector2.zero, Mathf.Infinity, letterLayer);

                if (hit.collider != null && hit.collider.GetComponent<Letter>().draggable) {
                    _targetObject = hit.collider.transform;
                    _targetLetter = _targetObject.GetComponent<Letter>();
                    _targetStart = _targetObject.position;

                    if (_targetLetter.assignedSlot) {
                        _startSlot = _targetLetter.assignedSlot;
                        _targetLetter.assignedSlot.GetComponent<LetterSlot>().DeassignLetter();
                    } else {
                        _startSlot = null;
                    }
                        
                    _targetLetter.assignedSlot = null;

                    _dragOffset = hit.collider.transform.position - GetMousePos();
                    _targetLetter.UpdateAnimationState(false);
                } else {
                    _targetObject = null;
                }

            } else if(_dragging && _targetObject != null) {
                _targetObject.position = GetMousePos() + _dragOffset;
            }
            //TODO(eren) : ?ntihar
            _dragging = true;
        } else {
            if (_dragging && _targetObject != null) {
                RaycastHit2D hitSlot = Physics2D.Raycast(_targetObject.position - new Vector3(0,0,5), Vector2.zero, Mathf.Infinity, slotLayer);

                if (hitSlot.collider != null) {
                    if (hitSlot.collider.GetComponent<LetterSlot>().assignedLetter == null) {
                        _targetLetter.MoveTo(hitSlot.collider.transform.position, 0.2f);

                        if (_targetLetter.assignedSlot)
                            _targetLetter.assignedSlot.GetComponent<LetterSlot>().DeassignLetter();
                        _targetLetter.assignedSlot = hitSlot.collider.transform;

                        hitSlot.collider.GetComponent<LetterSlot>().AssignLetter(_targetObject.transform);

                        _targetLetter.UpdateAnimationState(true);
                    } else {
                        _targetLetter.MoveTo(_targetStart);
                        if(_startSlot != null) {
                            _startSlot.GetComponent<LetterSlot>().AssignLetter(_targetObject);
                            _targetLetter.assignedSlot = _startSlot;
                        }

                    }
                     
                } else {
                    RaycastHit2D hitDeck = Physics2D.Raycast(_targetObject.position - new Vector3(0, 0, 5), Vector2.zero, Mathf.Infinity, deckLayer);

                    if (hitDeck.collider == null) {
                        _targetLetter.MoveTo(_targetStart);

                        if (_startSlot != null) {
                            _startSlot.GetComponent<LetterSlot>().AssignLetter(_targetObject);
                            _targetLetter.assignedSlot = _startSlot;
                        }

                    } else {
                        if (_targetLetter.assignedSlot)
                            _targetLetter.assignedSlot.GetComponent<LetterSlot>().DeassignLetter();
                        _targetLetter.assignedSlot = null;

                        _targetLetter.UpdateAnimationState(true);
                    }
                        
                }
                _targetObject = null;
                _startSlot = null;
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