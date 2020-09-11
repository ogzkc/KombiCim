package com.kc.kombicim.Model.Dto

import com.google.gson.annotations.SerializedName

data class WeatherData (

    @SerializedName("WeatherList") val weatherList : List<WeatherDto>,
    @SerializedName("Location") val location : Location
)