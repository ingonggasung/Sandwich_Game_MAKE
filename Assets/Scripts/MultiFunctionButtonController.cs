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
    private bool isCheckingRecipe = false; // 레시피 검사 중 상태 저장

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
            // y=10 상태에서는 레시피 체크가 끝난 후 대기 상태
            if (!isOrderActive && !isCheckingRecipe)
            {
                mainButton.gameObject.SetActive(true);
                yesButton.gameObject.SetActive(false);
                noButton.gameObject.SetActive(false);
            }
        }
        else if (Mathf.Approximately(y, 0f))
        {
            // y=0 상태에서는 레시피 검사 중이 아니면 버튼 상태 토글
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

            orderManagerScript.GenerateRandomOrder();

            mainButton.gameObject.SetActive(false);
            yesButton.gameObject.SetActive(true);
            noButton.gameObject.SetActive(true);

            isOrderActive = true;
            isCheckingRecipe = false;
        }
        else if (Mathf.Approximately(y, 0f))
        {
            if (receipeCheckScript != null)
            {
                // 레시피 체크 시작
                isCheckingRecipe = true;

                receipeCheckScript.CheckRecipe();

                // 레시피 체크 중에는 yes/no 버튼 비활성화 유지
                yesButton.gameObject.SetActive(false);
                noButton.gameObject.SetActive(false);

                // 타겟 오브젝트 y 좌표를 10으로 올림
                Vector3 pos = targetObject.transform.position;
                pos.y = 10f;
                targetObject.transform.position = pos;

                // Ingredient 오브젝트 파괴
                DestroyIngredientsOnTopOfAObject();

                // 주문 상태 갱신
                isOrderActive = false;

                // mainButton은 y가 10인 상태에서는 Update에서 다시 활성화될 것임
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
            orderManagerScript.CompleteOrder();
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
