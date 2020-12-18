using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BabaTileDoodleFix : MonoBehaviour
{
  Material m_Material;
  public bool activated;
    // Start is called before the first frame update
    void Start()
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

    }

    public void setActivated()
    {
      if(activated)
      {
        //m_Material.SetColor( "_Color",  new Color(255f,255f,255f,1f));
      }
      else
      {
        m_Material.color = Color.grey;
      }
    }


}
