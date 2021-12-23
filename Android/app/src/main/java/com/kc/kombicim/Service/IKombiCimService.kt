package com.kc.kombicim.Service

import com.kc.kombicim.Model.Request.*
import com.kc.kombicim.Model.Response.*
import retrofit2.Call
import retrofit2.http.*

interface IKombiCimService {
    @POST(value = "Login")
    fun login(@Body loginRequest: LoginRequest): Call<LoginResponse>

    @GET(value = "Weather")
    fun getWeather(@Query("lastHours") lastHours: Int): Call<WeatherResponse>

    @GET(value = "CombiLog")
    fun getCombiLog(@Query("lastHours") lastHours: Int): Call<CombiLogResponse>

    @GET(value = "Profile")
    fun getProfiles(): Call<ProfileResponse>

    @GET(value = "Location/GetThermometers")
    fun getThermometers(): Call<LocationsResponse>

    @POST(value = "Profile/SetActive")
    fun setActiveProfile(@Body activeProfileRequest: ActiveProfileRequest): Call<BaseResponse>

    @POST(value = "Settings/SetProfileMinTemperature")
    fun setProfileMinTemperature(@Body profileMinTemperatureRequest: ProfileMinTemperatureRequest): Call<BaseResponse>

    @POST(value = "Settings/SetProfileThermometer")
    fun setProfileThermometer(@Body profileMinTemperatureRequest: SetProfileThermometerRequest): Call<BaseResponse>

    @POST(value = "Settings/SetState")
    fun setState(@Body setStateRequest: StateRequest): Call<BaseResponse>
}