using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterSlot : MonoBehaviour 
{
    [SerializeField] LetterSlot leftSlot;
    [SerializeField] LetterSlot rightSlot;

    [SerializeField] Sprite spriteEmpty;
    [SerializeField] Sprite spriteAssigned;
    [SerializeField] Sprite spriteDuplicated;

    public LetterShooter shooter;
    public Transform assignedLetter = null;
    public bool comboStatus = false;

    [HideInInspector] public SpriteRenderer _spriteRenderer;

    void Start() {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if ((rightSlot == null || rightSlot.assignedLetter == null) && assignedLetter != null) CalculateCombo(null, new List<Letter>());
    }

    public void AssignLetter(Transform newLetter) {
        assignedLetter = newLetter;
        shooter = assignedLetter.GetComponentInChildren<LetterShooter>();
        _spriteRenderer.sprite = spriteAssigned;

        GetFarRightSlot().CalculateCombo(null, new List<Letter>());
    }

    public void DeassignLetter() {
        assignedLetter = null;
        shooter = null;
        _spriteRenderer.sprite = spriteEmpty;
        LetterSlot mRight = GetFarRightSlot();
        if (mRight != this) mRight.CalculateCombo(null, new List<Letter>());

        if (leftSlot != null && leftSlot.assignedLetter != null) leftSlot.CalculateCombo(null, new List<Letter>());
    }

    public void CalculateCombo(BulletData data, List<Letter> comboList) {
        Debug.Log("Calculating Combo : " + transform.name);

        if(data == null) {
            if (leftSlot != null && leftSlot.assignedLetter != null) {
                data = new BulletData();
                Debug.Log("Calculating Combo : Spawned New Bullet");
            } else {
                comboStatus = false;
                shooter.UpdateData(shooter.data);
                return;
            }
        }
        
        string id = assignedLetter.GetComponent<Letter>().letterID;

        int index = CheckDuplication(assignedLetter.GetComponent<Letter>(), comboList);
        if (index == -1) {
            Debug.Log("Calculating Combo Letter : " + id);
            switch (id) {
                case "A":
                case "I":
                    Debug.Log("fast");
                    data._shotDelay /= 1.5f;
                break;

                case "Z":
                case "F":
                    data._poisonTime += 1;
                break;

                case "B":
                case "Y":
                    data._slowDownTime += 1;
                break;

                case "Question":
                case "Exclamation":
                    data._stunTime += 1;
                break;

                case "O":
                case "U":
                    data._splashRadius += 1;
                break;

                case "S":
                    data._speed *= 2;
                    data._damage *= 1.15f;
                    data._zoneSize *= 5;
                break;

                case "Dash":
                    data._piercing = true;
                break;

                case "8":
                    data._boomerang = true;
                break;

                case "G":
                    data._damage *= 1.8f;
                break;

                case "P":
                    data._damage *= 1.3f;
                break;

                default:
                    Debug.LogError("Character ID Not Defined!");
                break;
            }
            _spriteRenderer.sprite = spriteAssigned;
        } else {
            _spriteRenderer.sprite = spriteDuplicated;

            comboList[index].assignedSlot.GetComponent<LetterSlot>()._spriteRenderer.sprite = spriteDuplicated;
        }

        comboList.Add(assignedLetter.GetComponent<Letter>());
        comboStatus = true;
        if (leftSlot != null && leftSlot.assignedLetter != null) {
            //We are at the middle
            leftSlot.CalculateCombo(data, comboList);
        } else {
            //We are at the most left and calculation ended
            Debug.Log(transform.name);

            comboList[comboList.Count / 2].assignedSlot.GetComponent<LetterSlot>().shooter.UpdateData(data);
            comboList[comboList.Count / 2].assignedSlot.GetComponent<LetterSlot>().comboStatus = false;
        }
    }

    public LetterSlot GetFarRightSlot() {
        if (rightSlot != null && rightSlot.assignedLetter != null) return rightSlot.GetComponent<LetterSlot>().GetFarRightSlot();

        return this;
    }

    private int CheckDuplication(Letter let, List<Letter> list) {
        for(int i = 0; i < list.Count; i++) {
            if (let.letterID == list[i].letterID) return i;
        }

        return -1;
    }
}
