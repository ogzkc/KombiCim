package com.kc.kombicim.Model.Dto

import com.google.gson.annotations.SerializedName

data class Location (
    @SerializedName("Id") val id : Int,
    @SerializedName("DeviceId") val deviceId : String,
    @SerializedName("DeviceTypeName") val deviceTypeName : String,
    @SerializedName("Name") val name : String
)