package com.kc.kombicim.Model.Dto

import com.google.gson.annotations.SerializedName

data class LocationDto (

    @SerializedName("Id") val id : Int,
    @SerializedName("Name") val name: String,
    @SerializedName("DeviceId") val deviceId : String,
    @SerializedName("DeviceTypeName") val deviceTypeName : String,
    @SerializedName("MinTempValue") val minTempValue : Double?
)