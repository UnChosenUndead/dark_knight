using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerAttack : MonoBehaviour
{
    public Image fillWaithImage1;
    public Image fillWaithImage2;
    public Image fillWaithImage3;
    public Image fillWaithImage4;
    public Image fillWaithImage5;
    public Image fillWaithImage6;

    private int[] _fadeImages = {0, 0, 0, 0, 0, 0};

    private Animator _animator;

    private bool _canAttack = true;

    private PlayerMovement _playerMovement;
    

    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckAttack();
        CheckInput();
        CheckToFade();
    }

    private void CheckAttack()
    {
        if (!_animator.IsInTransition(0)
            && _animator.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
        {
            _canAttack = true;
        }
        else
        {
            _canAttack = false;
        }
    }

    private void CheckInput()
    {
        if (_animator.GetInteger("Atk") == 0)
        {
            _playerMovement.FinishedMovement = false;

            if (!_animator.IsInTransition(0) && _animator.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
            {
                _playerMovement.FinishedMovement = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _playerMovement.TargetPose = transform.position;

            if (_playerMovement.FinishedMovement && _fadeImages[0] != 1 && _canAttack )
            {
                _fadeImages[0] = 1;
                _animator.SetInteger("Atk", 1);
                Debug.Log("Ok set 1");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _playerMovement.TargetPose = transform.position;

            if (_playerMovement.FinishedMovement && _fadeImages[1] != 1 && _canAttack)
            {
                _fadeImages[1] = 1;
                _animator.SetInteger("Atk", 2);
                Debug.Log("Ok set 2");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _playerMovement.TargetPose = transform.position;

            if (_playerMovement.FinishedMovement && _fadeImages[2] != 1 && _canAttack)
            {
                _fadeImages[2] = 1;
                _animator.SetInteger("Atk", 3);
                Debug.Log("Ok set 3");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _playerMovement.TargetPose = transform.position;

            if (_playerMovement.FinishedMovement && _fadeImages[3] != 1 && _canAttack)
            {
                _fadeImages[3] = 1;
                _animator.SetInteger("Atk", 4);
                Debug.Log("Ok set 4");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _playerMovement.TargetPose = transform.position;

            if (_playerMovement.FinishedMovement && _fadeImages[4] != 1 && _canAttack)
            {
                _fadeImages[4] = 1;
                _animator.SetInteger("Atk", 5);
                Debug.Log("Ok set 5");
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            _playerMovement.TargetPose = transform.position;

            if (_playerMovement.FinishedMovement && _fadeImages[5] != 1 && _canAttack)
            {
                _fadeImages[5] = 1;
                _animator.SetInteger("Atk", 6);
                Debug.Log("Ok set 6");
            }
        }
        else
        {
            _animator.SetInteger("Atk", 0);
            Debug.Log("Ok set 0");
        }

        if (Input.GetKey(KeyCode.Space))
        {
            _playerMovement.FinishedMovement = false;
            Vector3 targetPos = Vector3.zero;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                targetPos = new Vector3(raycastHit.point.x, transform.position.y, raycastHit.point.z);
            }

            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(targetPos - transform.position), 15f * Time.deltaTime);
        }
    }

    void CheckToFade()
    {
        if (_fadeImages[0] == 1)
        {
            if (FadeAndWait(fillWaithImage1, 1.0f))
            {
                _fadeImages[0] = 0;
            }
        }
        
        if (_fadeImages[1] == 1)
        {
            if (FadeAndWait(fillWaithImage2, 0.7f))
            {
                _fadeImages[1] = 0;
            }
        }
        
        if (_fadeImages[2] == 1)
        {
            if (FadeAndWait(fillWaithImage3, 0.1f))
            {
                _fadeImages[2] = 0;
            }
        }
        
        if (_fadeImages[3] == 1)
        {
            if (FadeAndWait(fillWaithImage4, 0.2f))
            {
                _fadeImages[3] = 0;
            }
        }
        
        if (_fadeImages[4] == 1)
        {
            if (FadeAndWait(fillWaithImage5, 0.3f))
            {
                _fadeImages[4] = 0;
            }
        }
        
        if (_fadeImages[5] == 1)
        {
            if (FadeAndWait(fillWaithImage6, 0.08f))
            {
                _fadeImages[5] = 0;
            } 
        }
    }

    bool FadeAndWait(Image fadeImg, float fadeTime)
    {
        bool faded = false;
        
        if(fadeImg == null)
        {
            return faded;
        }

        if (!fadeImg.gameObject.activeInHierarchy)
        {
            fadeImg.gameObject.SetActive(true);
            fadeImg.fillAmount = 1f;
        }

        fadeImg.fillAmount -= fadeTime * Time.deltaTime;

        if (fadeImg.fillAmount <= 0.0)
        {
            fadeImg.gameObject.SetActive(false);
            faded = true;
        }
        return faded;
    }
}
