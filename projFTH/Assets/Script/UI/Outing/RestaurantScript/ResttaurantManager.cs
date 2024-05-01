﻿using Script.UI.Outing;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class RestaurantManager : MonoBehaviour
{
    public GameObject foodListPrefab;
    public GameObject foodList;
    public Transform foodListLayout;

    private PointerEventData eventData;

    private RestaurantDao restaurantDao;
    private RestaurantUIController RestaurantUIController;
    private FoodListVO foodlistVO;
    private List<FoodListVO> FoodList;


    private void Start()
    {
        restaurantDao = GetComponent<RestaurantDao>(); // RestaurantDao 컴포넌트를 가져와서 초기화합니다.
        RestaurantUIController = FindObjectOfType<RestaurantUIController>(); // RestaurantManager를 찾아서 초기화합니다.
        FoodList = restaurantDao.GetFoodListFromDB();


    }
    public void OnclickFoodList()
    {
        foreach (var dic in FoodList)
        {
            GameObject foodListInstance = Instantiate(foodListPrefab, foodListLayout);
            foodListInstance.name = "foodlist" + dic.FoodNo;
            Text textComponent = foodListInstance.GetComponentInChildren<Text>();

            if (textComponent != null)
            {
                textComponent.text = dic.FoodNm + "\r\n" +
                               " " + dic.FoodPr;
            }
        }
        foodList.SetActive(false);
    }
    public void GetclickListValue()
    {
        GameObject clickList = EventSystem.current.currentSelectedGameObject;
        string objectName = clickList.name;
        string indexString = objectName.Replace("foodlist", "");
        int index = int.Parse(indexString);
        FoodListVO fv = FoodList[index-1];
        ProcessPayment(fv.FoodPr);
    }
    public void ProcessPayment(int foodPr)
    {
        int userCash = restaurantDao.GetUserInfoFromDB();
        int NowCash = userCash - foodPr;
        Debug.Log("DB 유저 현금 " + userCash);
        Debug.Log("계산 후 금액 " + NowCash);
        if (NowCash > 0)
        {
            restaurantDao.UpdateUserCash(NowCash);
            RestaurantUIController.OnClickBuyComple();
        }
        else
        {
            Debug.Log("Not enough cash!");
            RestaurantUIController.OnClickBuyFail();

        }
    }
}
