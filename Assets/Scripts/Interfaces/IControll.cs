using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControll
{
    bool IsBoost { get; set; }
    bool IsShoot { get; set; }
    bool OnLeftArrow { get; set; }
    bool OnRightArrow { get; set; }
    bool OnUpArrow { get; set; }
    bool OnDownArrow { get; set; }
}
