using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouseClick : MonoBehaviour
{
     private void Update()
     {
         if (Input.GetMouseButtonDown(0))
         {
              Debug.Log("Pressed primary button.");

         }
     }
}
