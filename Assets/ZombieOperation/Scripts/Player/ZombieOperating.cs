using UnityEngine;
using System.Collections;

public enum ZombieOperatingType
{
    Operating,
    Syringe,
}

public class ZombieOperating : MonoBehaviour
{
    public ZombieOperatingType zombieOperatingType
    {
        set {}
        get { return operatingType; }
    }

    public ZombieOperatingType operatingType = ZombieOperatingType.Operating;
    public GameObject operatingObject;
    public GameObject syringeObject;

    [SerializeField]
    private Operating operatingComponent;
    [SerializeField]
    private Syringe syringeComponent;

    void Start ()
    {
        switch (operatingType)
        {
            case ZombieOperatingType.Operating:
                syringeObject.SetActive(false);
                //syringeComponent.enabled = false;
                break;

            case ZombieOperatingType.Syringe:
                operatingObject.SetActive(false);
                //operatingComponent.enabled = false;
                break;
        }
    }
	
	void Update ()
    {

	}

    public void ChangeMightiness(ZombieOperatingType eOperatingType)
    {
        operatingType = eOperatingType;

        switch (operatingType)
        {
            case ZombieOperatingType.Operating:
                operatingObject.SetActive(true);
                //operatingComponent.enabled = true;
                syringeObject.SetActive(false);
                //syringeComponent.enabled = false;

                break;

            case ZombieOperatingType.Syringe:
                syringeObject.SetActive(true);
                //syringeComponent.enabled = true;

                operatingObject.SetActive(false);
                //operatingComponent.enabled = false;
                break;
        }
    }

    public void OperatingEvent()
    {
        operatingComponent.Decision();
    }

    public void SyringeEvent(bool OnEvent)
    {
        if(OnEvent)
        {
            syringeComponent.OnInjection();
        }
        else
        {
            syringeComponent.OffInjection();
        }
    }
}
