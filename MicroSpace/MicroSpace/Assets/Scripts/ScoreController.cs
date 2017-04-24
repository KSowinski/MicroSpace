using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public GameObject Text;

    private float _delay;
    private int _score;

	void Start ()
	{
	    ResetScore();
	}

    void Update ()
    {
	    if (_delay > 0f)
	    {
	        _delay -= Time.deltaTime;
            return;
	    }

	    AddScore(10);
        _delay = 1f;
    }

    public void AddScore(int value)
    {
        _score += value;
        Text.GetComponent<Text>().text = _score.ToString("D8");
    }

    private void ResetScore()
    {
        _score = 0;
        _delay = 1f;
    }
}
