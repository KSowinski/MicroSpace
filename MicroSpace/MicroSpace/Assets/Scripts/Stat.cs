using UnityEngine;
using UnityEngine.UI;


public class Stat
{
    public int Current { get; set; }
    public int Max { get; set; }

    private readonly string _sliderTag;
    private readonly string _textTag;
    private readonly bool _showMax;
    private readonly bool _addPerc;

    public Stat(int max, int current, string sliderTag, string textTag, bool showMax, bool addPerc)
    {
        Max = max;
        Current = current;

        _sliderTag = sliderTag;
        _textTag = textTag;
        _showMax = showMax;
        _addPerc = addPerc;
    }

    public void MaxOut()
    {
        Current = Max;
        UpdateVisuals(null);
    }

    public void Update(int? current, int? max)
    {
        //Update vars
        if (max.HasValue)
        {
            Max += max.Value;
            if (Max < 0) Current = 0;
        }

        if (current.HasValue)
        {
            Current += current.Value;
            if (Current < 0) Current = 0;
            if (Current > Max) Current = Max;
        } 
        
        UpdateVisuals(current);
    }

    private void UpdateVisuals(int? current)
    {
        //Update visuals
        if (!string.IsNullOrEmpty(_sliderTag))
        {
            var go = GameObject.FindGameObjectWithTag(_sliderTag);
            go.GetComponent<Slider>().maxValue = Max;
            go.GetComponent<Slider>().value = Current;
        }

        if (!string.IsNullOrEmpty(_textTag))
        {
            var gos = GameObject.FindGameObjectsWithTag(_textTag);
            var newText = _showMax
                ? string.Format("{0}{2} / {1}{2}", Current, Max, _addPerc ? "%" : string.Empty)
                : string.Format("{0}{1}", Current, _addPerc ? "%" : string.Empty);
            foreach (var go in gos)
            {
                go.GetComponent<Text>().text = newText;

                //Particle Effect
                if (current.HasValue && current.Value > 0 && go.transform.GetComponent<UIParticleEffect>() != null)
                {
                    go.transform.GetComponent<UIParticleEffect>().Show();
                }
            }
        }
    }
}