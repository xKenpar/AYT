using UnityEngine;
using DG.Tweening;
public class MouseDrag : MonoBehaviour {

    [SerializeField] LayerMask letterLayer;
    [SerializeField] LayerMask slotLayer;
    [SerializeField] LayerMask deckLayer;
    [SerializeField] LayerMask recycleLayer;

    [SerializeField] GameObject LetterRecycle;

    Vector3 _dragOffset;
    Vector3 _targetStart;
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
                    _dragOffset = hit.collider.transform.position - GetMousePos();
                    _targetLetter.UpdateAnimationState(false);

                    AudioManager.Play(AudioType.Pickup);
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
                        _targetLetter.MoveTo(hitSlot.collider.transform.position, 0.2f);
                        hitSlot.collider.GetComponent<LetterSlot>().AssignLetter(_targetObject.transform);

                        if(_targetLetter.assignedSlot)
                            _targetLetter.assignedSlot.GetComponent<LetterSlot>().DeassignLetter();
                        _targetLetter.assignedSlot = hitSlot.collider.transform;

                        _targetLetter.UpdateAnimationState(true);
                        
                        AudioManager.Play(AudioType.Pickup);
                    } else {
                        _targetLetter.MoveTo(_targetStart);
                    }
                     
                } else {
                    RaycastHit2D hitDeck = Physics2D.Raycast(_targetObject.position - new Vector3(0, 0, 5), Vector2.zero, Mathf.Infinity, deckLayer);

                    if (hitDeck.collider == null) {
                        RaycastHit2D hitRecycle = Physics2D.Raycast(_targetObject.position - new Vector3(0, 0, 5), Vector2.zero, Mathf.Infinity, recycleLayer);
                        if(hitRecycle.collider == null){
                            _targetLetter.MoveTo(_targetStart);
                        } else {
                             if (_targetLetter.assignedSlot)
                                _targetLetter.assignedSlot.GetComponent<LetterSlot>().DeassignLetter();
                            _targetLetter.assignedSlot = null;

                            Instantiate(LetterRecycle, _targetLetter.transform.position, Quaternion.identity);
                            Destroy(_targetLetter.gameObject);

                            LetterManager.RecycleLetter();
                        }
                    } else {
                        if (_targetLetter.assignedSlot)
                            _targetLetter.assignedSlot.GetComponent<LetterSlot>().DeassignLetter();
                        _targetLetter.assignedSlot = null;

                        _targetLetter.UpdateAnimationState(true);
                    }
                        
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