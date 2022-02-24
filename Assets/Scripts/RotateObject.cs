using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.Instance.howToPlayPressed == true)
        {
            Rotate();
        }
        else
        {
            this.transform.DORestart(true);
        }
       // DOTween.SetTweensCapacity(2000, 100);
    }

    void Rotate()
    {
        this.transform.DORotate(new Vector3(0, 0, 360), 5, RotateMode.LocalAxisAdd).SetLoops(-1).SetEase(Ease.Linear);
    }
}
