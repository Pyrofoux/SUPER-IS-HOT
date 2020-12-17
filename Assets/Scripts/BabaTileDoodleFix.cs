using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BabaTileDoodleFix : MonoBehaviour
{
  Material m_Material;

    // Start is called before the first frame update
    void Start()
    {
      m_Material = GetComponent<Image>().material;
      //m_Material = <Image>.material;
    }

    // Update is called once per frame
    void Update()
    {
        m_Material.SetFloat( "_BabaTime", Time.unscaledTime );
    }
}
