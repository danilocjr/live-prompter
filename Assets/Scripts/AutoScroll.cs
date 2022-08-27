using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AutoScroll : MonoBehaviour
{
    public float speed = 10f;
    Vector3 pos = Vector3.zero;
    public RectTransform frame;
    [SerializeField] Slider speedSlider;

    bool scroll = false;

    void Start()
    {
        pos = frame.position;
        speedSlider.value = 5;
        speed = 5;
    }

    public void OnClickToggleRun()
    {
        scroll = !scroll;
    }

    public void Reset()
    {
        frame.position = pos;
    }

    public void ChangeScrollSpeed(float _speed)
    {
        speed = _speed * 3;
    }
    private void Update()
    {
        if (scroll)
        {
            frame.position = new Vector3(pos.x, frame.position.y + (Time.deltaTime * speed), pos.z);
        }
       
    }

}
