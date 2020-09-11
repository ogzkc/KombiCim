package com.kc.kombicim.Model.Response

import com.google.gson.annotations.SerializedName
import com.kc.kombicim.Model.Dto.CombiLogDto
import com.kc.kombicim.Model.Dto.WeatherData

data class CombiLogResponse (

    @SerializedName("CombiLogs") val combiLogs : List<CombiLogDto>,
    @SerializedName("Success") val success : Boolean
)