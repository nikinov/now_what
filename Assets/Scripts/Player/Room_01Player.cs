using UnityEngine;

public class Room_01Player : MonoBehaviour
{
    [SerializeField] private GameObject myRoom01;
    
    private bool test;
    // Start is called before the first frame update
    void Awake()
    {
        myRoom01.GetComponent<Manager>().OnOpen += OnOpen;
        myRoom01.GetComponent<Manager>().OnClose += OnClose;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnOpen()
    {
        test = true;
    }

    void OnClose()
    {
        transform.position = new Vector3(0, 0, 4.485f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "door")
        {
            if (test)
            {
                test = false;           
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                myRoom01.GetComponent<Manager>().OpenState("MainMenu");
            }
                
            
        }
    }
}
