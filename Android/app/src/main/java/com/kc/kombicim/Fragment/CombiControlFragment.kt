package com.kc.kombicim.Fragment

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.AdapterView
import android.widget.AdapterView.OnItemSelectedListener
import android.widget.ArrayAdapter
import android.widget.Toast
import androidx.fragment.app.Fragment
import com.kc.kombicim.Model.Dto.ProfileDto
import com.kc.kombicim.Model.Request.ActiveProfileRequest
import com.kc.kombicim.Model.Request.ProfileMinTemperatureRequest
import com.kc.kombicim.Model.Request.StateRequest
import com.kc.kombicim.Model.Response.BaseResponse
import com.kc.kombicim.Model.Response.ProfileResponse

import com.kc.kombicim.R
import com.kc.kombicim.Service.BasicAuthInterceptor
import com.kc.kombicim.Service.IKombiCimService
import com.kc.kombicim.Utility.Settings
import kotlinx.android.synthetic.main.fragment_combi_control.*
import okhttp3.Dispatcher
import okhttp3.OkHttpClient
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

class CombiControlFragment : Fragment() {

    val MODE_MANUAL = "manual_1"
    val MODE_AUTO_PROFILE = "auto_profile_1"

    private lateinit var profiles: List<ProfileDto>
    private var activeProfile: ProfileDto? = null

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        val root = inflater.inflate(R.layout.fragment_combi_control, container, false)

        return root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        spinner_profiles.onItemSelectedListener = object : OnItemSelectedListener {
            override fun onNothingSelected(parent: AdapterView<*>?) {
            }

            override fun onItemSelected(parent: AdapterView<*>?, view: View?, position: Int, id: Long) {

                activeProfile = profiles[position]

                if (activeProfile!!.typeName == MODE_AUTO_PROFILE) {
                    radioButton_on.isEnabled = false
                    radioButton_off.isEnabled = false

                    edittext_temperature.isEnabled = true

                    edittext_temperature.setText(activeProfile!!.minTempValue.toString())

                } else if (activeProfile!!.typeName == MODE_MANUAL) {
                    radioButton_on.isEnabled = true
                    radioButton_off.isEnabled = true

                    edittext_temperature.isEnabled = false

                    if (activeProfile!!.state!!) {
                        radioButton_on.isChecked = true
                        radioButton_off.isChecked = false
                    } else {
                        radioButton_on.isChecked = false
                        radioButton_off.isChecked = true
                    }

                }

            }

        }

        val dispatcher = Dispatcher()
        dispatcher.maxRequests = 1

        val clientBuilder = OkHttpClient.Builder().addInterceptor(BasicAuthInterceptor(Settings.API_USERNAME, Settings.API_PASSWORD)).dispatcher(dispatcher)
        clientBuilder.addInterceptor { chain ->
            val request = chain.request().newBuilder().addHeader(Settings.API_HEADER_API_TOKEN, Settings.GetToken()).build()
            chain.proceed(request)
        }

        val retrofit = Retrofit.Builder().addConverterFactory(GsonConverterFactory.create()).baseUrl(Settings.API_BASE_URL).client(clientBuilder.build()).build()
        val kombicimService = retrofit.create(IKombiCimService::class.java)

        val weatherCall = kombicimService.getProfiles()
        weatherCall.enqueue(object : Callback<ProfileResponse> {
            override fun onFailure(call: Call<ProfileResponse>, t: Throwable) {

            }

            override fun onResponse(call: Call<ProfileResponse>, response: Response<ProfileResponse>) {
                if (response.isSuccessful && response.body() != null) {
                    val profileNames = mutableListOf<String>()
                    profiles = response.body()!!.profileDtos
                    profiles.forEach { item ->
                        if (item.active) activeProfile = item

                        profileNames.add(item.profileName)
                    }

                    val adapter = ArrayAdapter<String>(context!!, android.R.layout.simple_spinner_item, profileNames)
                    adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
                    spinner_profiles.adapter = adapter

                    profiles.forEachIndexed { index, profileDto ->
                        if (profileDto.active) spinner_profiles.setSelection(index)
                    }
                }
            }
        })



        button_profile_save.setOnClickListener {

            var temp: Double? = null
            val state = radioButton_on.isChecked

            if (activeProfile!!.typeName == MODE_AUTO_PROFILE) {
                temp = edittext_temperature.text.toString().toDoubleOrNull()
                if (temp == null) {
                    Toast.makeText(context, "Sıcaklığı lütfen ##.# formatında girin.", Toast.LENGTH_SHORT).show()
                    false
                }
            }

            var setActiveProfileCall = kombicimService.setActiveProfile(ActiveProfileRequest(activeProfile!!.id))
            setActiveProfileCall.enqueue(object : Callback<BaseResponse> {
                override fun onFailure(call: Call<BaseResponse>, t: Throwable) {
                    Toast.makeText(activity!!, "Profil aktifleştirilirken hata oluştu!", Toast.LENGTH_SHORT).show()
                }

                override fun onResponse(call: Call<BaseResponse>, response: Response<BaseResponse>) {
                    if (response.isSuccessful && response.body()!!.Success)
                    else Toast.makeText(activity!!, "Profil aktifleştirilirken hata oluştu!", Toast.LENGTH_LONG).show()
                }
            })

            if (activeProfile!!.typeName == MODE_AUTO_PROFILE) {
                val setMinTempCall = kombicimService.setProfileMinTemperature(ProfileMinTemperatureRequest(activeProfile!!.id, temp!!))
                setMinTempCall.enqueue(object : Callback<BaseResponse> {
                    override fun onFailure(call: Call<BaseResponse>, t: Throwable) {
                        Toast.makeText(activity!!, "Min temperature gönderilirken hata oluştu!", Toast.LENGTH_SHORT).show()
                    }

                    override fun onResponse(call: Call<BaseResponse>, response: Response<BaseResponse>) {
                        if (response.isSuccessful && response.body()!!.Success) Toast.makeText(activity!!, "Başarıyla kaydedildi!", Toast.LENGTH_SHORT).show()
                        else Toast.makeText(activity!!, "Min temperature gönderilirken hata oluştu!", Toast.LENGTH_LONG).show()
                    }
                })
            } else if (activeProfile!!.typeName == MODE_MANUAL) {
                val setStateCall = kombicimService.setState(StateRequest((state)))

                setStateCall.enqueue(object : Callback<BaseResponse> {
                    override fun onFailure(call: Call<BaseResponse>, t: Throwable) {
                        Toast.makeText(context, "Kombi durumu gönderilirken hata oluştu!", Toast.LENGTH_SHORT).show()
                    }

                    override fun onResponse(call: Call<BaseResponse>, response: Response<BaseResponse>) {
                        if (response.isSuccessful && response.body()!!.Success) Toast.makeText(activity!!, "Başarıyla kaydedildi!", Toast.LENGTH_SHORT).show()
                        else Toast.makeText(activity!!, "Kombi durumu gönderilirken hata oluştu!", Toast.LENGTH_LONG).show()
                    }
                })
            }


        }


    }

}