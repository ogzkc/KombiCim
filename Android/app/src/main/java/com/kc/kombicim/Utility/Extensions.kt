package com.kc.kombicim.Utility

fun Double.format(digits: Int) = "%.${digits}f".format(this)