using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class HubLocation : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int m_sceneIndex = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);

        SceneManager.LoadScene(m_sceneIndex);
    }
}
