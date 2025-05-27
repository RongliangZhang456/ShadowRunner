using UnityEngine;

public class PanelBase : MonoBehaviour
{
    public virtual string PanelID => gameObject.name;

    public virtual void Show() => gameObject.SetActive(true);
    public virtual void Hide() => gameObject.SetActive(false);
}