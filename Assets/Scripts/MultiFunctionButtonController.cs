using UnityEngine;
using UnityEngine.UI;

public class MultiFunctionButtonController : MonoBehaviour
{
    public GameObject targetObject;
    public Button mainButton;
    public Button yesButton;
    public Button noButton;

    public GameObject AObject;
    public ReceipeCheck receipeCheckScript;
    public OrderManager orderManagerScript;

    private bool isOrderActive = false;
    private bool isCheckingRecipe = false;

    void Start()
    {
        if (targetObject != null)
        {
            Vector3 pos = targetObject.transform.position;
            pos.y = 10f;
            targetObject.transform.position = pos;
        }

        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        mainButton.gameObject.SetActive(true);

        mainButton.onClick.AddListener(OnMainButtonClicked);
        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
    }

    void Update()
    {
        if (targetObject == null || AObject == null) return;

        float y = targetObject.transform.position.y;

        if (Mathf.Approximately(y, 10f))
        {
            if (!isOrderActive && !isCheckingRecipe)
            {
                mainButton.gameObject.SetActive(true);
                yesButton.gameObject.SetActive(false);
                noButton.gameObject.SetActive(false);
            }
        }
        else if (Mathf.Approximately(y, 0f))
        {
            if (!isOrderActive && !isCheckingRecipe)
            {
                if (IsIngredientOnTopOfAObject())
                {
                    mainButton.gameObject.SetActive(true);
                    yesButton.gameObject.SetActive(false);
                    noButton.gameObject.SetActive(false);
                }
                else
                {
                    mainButton.gameObject.SetActive(false);
                }
            }
        }
    }

    void OnMainButtonClicked()
    {
        if (targetObject == null)
            return;

        float y = targetObject.transform.position.y;

        if (Mathf.Approximately(y, 10f))
        {
            if (orderManagerScript == null)
            {
                Debug.LogError("OrderManager 스크립트가 할당되지 않았습니다.");
                return;
            }

            string newOrder = orderManagerScript.GenerateRandomOrder();

            if (!string.IsNullOrEmpty(newOrder))
            {
                mainButton.gameObject.SetActive(false);
                yesButton.gameObject.SetActive(true);
                noButton.gameObject.SetActive(true);

                isOrderActive = true;
                isCheckingRecipe = false;
            }
            else
            {
                mainButton.gameObject.SetActive(true);
                yesButton.gameObject.SetActive(false);
                noButton.gameObject.SetActive(false);

                isOrderActive = false;
            }
        }
        else if (Mathf.Approximately(y, 0f))
        {
            if (receipeCheckScript != null)
            {
                isCheckingRecipe = true;

                receipeCheckScript.CheckRecipe();
                ZPositionController.ResetZPositionTo15();

                yesButton.gameObject.SetActive(false);
                noButton.gameObject.SetActive(false);

                Vector3 pos = targetObject.transform.position;
                pos.y = 10f;
                targetObject.transform.position = pos;

                DestroyIngredientsOnTopOfAObject();

                isOrderActive = false;
            }
            else
            {
                Debug.LogError("ReceipeCheck 스크립트가 할당되지 않았습니다.");
            }
        }
    }

    void OnYesButtonClicked()
    {
        if (targetObject != null)
        {
            Vector3 pos = targetObject.transform.position;
            pos.y = 0f;
            targetObject.transform.position = pos;
        }

        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);

        isOrderActive = false;
        isCheckingRecipe = false;

        mainButton.gameObject.SetActive(true);

        if (orderManagerScript != null)
        {
            orderManagerScript.CompleteOrder();
        }
    }

    void OnNoButtonClicked()
    {
        if (targetObject != null && Mathf.Approximately(targetObject.transform.position.y, 0f))
        {
            yesButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);
        }

        mainButton.gameObject.SetActive(true);
        isOrderActive = false;
        isCheckingRecipe = false;

        if (orderManagerScript != null)
        {
            orderManagerScript.CompleteOrder();
        }
    }

    bool IsIngredientOnTopOfAObject()
    {
        float aZ = AObject.transform.position.z;
        GameObject[] ingredients = GameObject.FindGameObjectsWithTag("Ingredient");
        foreach (var obj in ingredients)
        {
            if (obj.transform.position.z < aZ)
                return true;
        }
        return false;
    }

    void DestroyIngredientsOnTopOfAObject()
    {
        float aZ = AObject.transform.position.z;
        GameObject[] ingredients = GameObject.FindGameObjectsWithTag("Ingredient");
        foreach (var obj in ingredients)
        {
            if (obj.transform.position.z < aZ)
            {
                Destroy(obj);
            }
        }
    }
}
