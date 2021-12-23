package com.kc.kombicim.Model.Dto

import com.google.gson.annotations.SerializedName

data class ProfileDto (

    @SerializedName("Id") val id : Int,
    @SerializedName("ProfileName") val profileName : String,
    @SerializedName("TypeName") val typeName : String,
    @SerializedName("MinTempValue") val minTempValue : Double?,
    @SerializedName("State") val state : Boolean?,
    @SerializedName("Active") val active : Boolean,
    @SerializedName("SelectedThermometer") val selectedThermometer: LocationDto?
)