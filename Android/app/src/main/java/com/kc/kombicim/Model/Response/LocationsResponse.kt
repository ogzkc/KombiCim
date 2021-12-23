package com.kc.kombicim.Model.Response

import com.google.gson.annotations.SerializedName
import com.kc.kombicim.Model.Dto.LocationDto
import com.kc.kombicim.Model.Dto.WeatherData

data class LocationsResponse  (
    @SerializedName("LocationDtos") val locationDtos : List<LocationDto>,
    @SerializedName("Success") val success : Boolean
)