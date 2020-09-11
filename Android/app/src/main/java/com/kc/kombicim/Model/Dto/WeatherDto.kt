package com.kc.kombicim.Model.Dto

import com.google.gson.annotations.SerializedName

data class WeatherDto (

    @SerializedName("Temperature") val temperature : Double,
    @SerializedName("Humidity") val humidity : Double,
    @SerializedName("Date") val date : String
)