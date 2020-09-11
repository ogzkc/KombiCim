package com.kc.kombicim.Model.Response

import com.google.gson.annotations.SerializedName
import com.kc.kombicim.Model.Dto.CombiLogDto
import com.kc.kombicim.Model.Dto.ProfileDto
import com.kc.kombicim.Model.Dto.WeatherData

data class ProfileResponse (

    @SerializedName("ProfileDtos") val profileDtos : List<ProfileDto>,
    @SerializedName("Success") val success : Boolean
)