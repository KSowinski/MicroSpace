using Assets.Scripts;
using UnityEngine;

public class BeamController : MonoBehaviour
{
    public GameObject BeamCircle;
    public GameObject BeamCentre;
    private bool _isActive;
    private float _progress = 0f;
    private float _speed = 1f;
    private GameObject _activeAsteroid;

    // Update is called once per frame
	void Update ()
    {
		if(!_isActive) return;

	    _progress += Time.deltaTime*_speed;
	    var newScale = Mathf.Lerp(1f, 0f, _progress);
        BeamCircle.transform.localScale = new Vector3(newScale, newScale, 1f);
	    if (newScale <= 0f) Reset();

	    if (Inpt.GetMouseButton(0))
	    {
	        if (newScale <= 0.25f)
	        {
                FindObjectOfType<ScoreController>().AddScore(50);

                var resources = _activeAsteroid.transform.GetComponent<AsteroidController>().ExtractResource();

	            switch (resources.Type)
	            {
	                case ResourceType.Titanium:
                        ShipController.Resources.Update(resources.Amount, null);
                        break;

	                case ResourceType.Fuel:
                        ShipController.Fuel.Update(resources.Amount, null);
                        break;
	            }
	                
	            Reset();
	            if (resources.SourceDestroyed) _isActive = false;
	        }
	        else
	        {
	            Reset();
	        }
	    }
    }

    public void Enable()
    {
        BeamCircle.SetActive(true);
        BeamCentre.SetActive(true);
        Reset();
        _isActive = false;
    }

    public void Disable()
    {
        Reset();
        _isActive = false;
        BeamCircle.SetActive(false);
        BeamCentre.SetActive(false);
    }

    public void Activate(GameObject asteroid)
    {
        _activeAsteroid = asteroid;
        _isActive = true;
    }

    private void Reset()
    {
        _progress = 0f;
        BeamCircle.transform.localScale = Vector3.one;
    }
}
