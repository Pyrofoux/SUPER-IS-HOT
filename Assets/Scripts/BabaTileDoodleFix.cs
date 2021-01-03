using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BabaTileDoodleFix : MonoBehaviour
{
  Material m_Material;
  public bool activated = false;
    // Start is called before the first frame update
    void Awake()
    {
      GatherMaterial();
      setActivated();
      //m_Material = <Image>.material;
    }


    void GatherMaterial()
    {
      if(m_Material == null)
      {
        GetComponent<Image>().material = m_Material = Instantiate<Material>(GetComponent<Image>().material);
      }
    }

    // Update is called once per frame
    void Update()
    {
      m_Material.SetFloat( "_BabaTime", Time.unscaledTime );
      setActivated();

    }

    public void setActivated()
    {
      if(activated)
      {
        m_Material.color = Color.white;
      }
      else
      {
        m_Material.color = Color.grey;
      }
    }


}
