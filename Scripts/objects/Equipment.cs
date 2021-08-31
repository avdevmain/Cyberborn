using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New equipment", menuName = "Equipment")]
public class Equipment : Item
{

      public AnimatorOverrideController AOC; // НА ПОТОМ ДЛЯ РАЗНЫХ АНИМАЦИЙ АКТИВАЦИИ, например

      public enum TypeItem
      {
           Hull,
           Engine,
           Reactor,
           Navigation,
           Hangar,
           Shield,

      }


    public TypeItem TI;

    public float power;
    public float specialValue;
    


    /*
    
    public enum Trait
    {
    Standard,
    Damaged
    }

    public Trait equipmentTrait;
    
     */
}
