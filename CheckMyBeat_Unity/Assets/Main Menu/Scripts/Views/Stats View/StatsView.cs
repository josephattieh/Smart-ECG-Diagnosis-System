using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsView : MonoBehaviour
{
    public static StatsView instance;
    public RawImage stats1;
    public RawImage stats2;
    public RawImage stats3;
    public RawImage stats4;
    public RawImage stats5;
    public RawImage stats6;

    private void Awake()
    {
        instance = this;
    }

   public void Increase1()
    {
        stats1.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        stats1.gameObject.transform.SetAsLastSibling();
    }
    public void Decrease1()
    {
        stats1.gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public void Increase2()
    {
        stats2.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        stats2.gameObject.transform.SetAsLastSibling();
    }
    public void Decrease2()
    {
        stats2.gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public void Increase3()
    {
        stats3.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        stats3.gameObject.transform.SetAsLastSibling();
    }
    public void Decrease3()
    {
        stats3.gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public void Increase4()
    {
        stats4.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        stats4.gameObject.transform.SetAsLastSibling();
    }
    public void Decrease4()
    {
        stats4.gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public void Increase5()
    {
        stats5.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        stats5.gameObject.transform.SetAsLastSibling();
    }
    public void Decrease5()
    {
        stats5.gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public void Increase6()
    {
        stats6.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        stats6.gameObject.transform.SetAsLastSibling();
    }
    public void Decrease6()
    {
        stats6.gameObject.transform.localScale = new Vector3(1, 1, 1);
    }
}
