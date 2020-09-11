package com.kc.kombicim.Model.Dto

import com.google.gson.annotations.SerializedName

data class CombiLogDto (

    @SerializedName("State") val state : Boolean,
    @SerializedName("Date") val date : String
)