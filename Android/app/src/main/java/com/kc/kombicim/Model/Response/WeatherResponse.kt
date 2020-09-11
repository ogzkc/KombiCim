package com.kc.kombicim.Model.Response

import com.google.gson.annotations.SerializedName
import com.kc.kombicim.Model.Dto.WeatherData

data class WeatherResponse (

    @SerializedName("WeatherDataList") val weatherDataList : List<WeatherData>,
    @SerializedName("Success") val success : Boolean
)