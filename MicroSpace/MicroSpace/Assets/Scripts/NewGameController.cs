using System.Collections;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameController : MonoBehaviour
{
    public bool RandomGen = false;  

	public void Start ()
    {
        if(!RandomGen) return;
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(3);
        GameObject.Find("Title").transform.gameObject.SetActive(false);  
        GameObject.FindGameObjectWithTag("SmallWorldCreator").transform.GetComponent<SmallWorldCreator>().Create();
        Inpt.IsEnabled = true;
    }

    public void Update()
    {
        if (Inpt.GetKeyDown(KeyCode.F12))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
