using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class MouseGameController : MonoBehaviour
{
    public GameObject MouseBody;
    public GameObject MouseLeftWhite;
    public GameObject MouseLeftRed;
    public GameObject MouseRightWhite;
    public GameObject MouseRightRed;
    public GameObject Slider;

    private bool _isActive = false;
    private bool _currentButton;
    private float _startDelay = 1f;

    public void Activate()
    {
        MouseBody.SetActive(true);
        MouseLeftWhite.SetActive(true);
        MouseLeftRed.SetActive(false);
        MouseRightWhite.SetActive(true);
        MouseRightRed.SetActive(false);
        Slider.SetActive(true);
        _isActive = true;
        _startDelay = 1f;
        Slider.transform.GetComponent<Slider>().value = 0;
    }

    private void SetRandomButton()
    {
        _currentButton = Random.Range(1, 101) <= 50; //50% left - 50% right
        if (_currentButton)
        {
            MouseLeftWhite.SetActive(false);
            MouseLeftRed.SetActive(true);
            MouseRightWhite.SetActive(true);
            MouseRightRed.SetActive(false);
        }
        else
        {
            MouseLeftWhite.SetActive(true);
            MouseLeftRed.SetActive(false);
            MouseRightWhite.SetActive(false);
            MouseRightRed.SetActive(true);
        }
    }

    public void Hide()
    {
        MouseBody.SetActive(false);
        MouseLeftWhite.SetActive(false);
        MouseLeftRed.SetActive(false);
        MouseRightWhite.SetActive(false);
        MouseRightRed.SetActive(false);
        Slider.SetActive(false);
        _isActive = false;
    }
    
    void Update ()
    {
		if(!_isActive) return;

        if (_startDelay > 0f)
        {
            _startDelay -= Time.deltaTime;
            if (_startDelay <= 0f)
            {
                SetRandomButton();
            }
            return;
        }
        
        //Decrease progress over time
        UpdateProgress(-1f * Time.deltaTime * 10f);

        if (ShipController.SelectedShipArea == SelectedShipArea.Shield || ShipController.SelectedShipArea == SelectedShipArea.Repair)
        {
            if (Inpt.GetMouseButtonDown(0) && _currentButton || Inpt.GetMouseButtonDown(1) && !_currentButton)
            {
                UpdateProgress(20);
                if (Slider.transform.GetComponent<Slider>().value >= Slider.transform.GetComponent<Slider>().maxValue)
                {
                    if (ShipController.SelectedShipArea == SelectedShipArea.Shield)
                    {
                        ShipController.Shield.Update(20, null);
                    }
                    else if (ShipController.SelectedShipArea == SelectedShipArea.Repair)
                    {
                        ShipController.Hull.Update(20, null);
                    }
                    Activate();
                }
                else
                {
                    SetRandomButton();
                }
            }
            else if (Inpt.GetMouseButtonDown(0) && !_currentButton || Inpt.GetMouseButtonDown(1) && _currentButton)
            {
                Activate();
            }
        }
    }

    private void UpdateProgress(float value)
    {
        Slider.transform.GetComponent<Slider>().value += value;
    }
}
