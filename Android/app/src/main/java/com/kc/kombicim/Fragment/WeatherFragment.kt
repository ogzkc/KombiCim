package com.kc.kombicim.Fragment

import android.content.Intent
import android.os.Bundle
import android.view.*
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProviders
import com.kc.kombicim.Activity.LoginActivity
import com.kc.kombicim.Adapter.WeatherFragmentAdapter
import com.kc.kombicim.Model.Adapter.WeatherViewPagerDataHolder
import com.kc.kombicim.Model.Response.WeatherResponse

import com.kc.kombicim.R
import com.kc.kombicim.Service.BasicAuthInterceptor
import com.kc.kombicim.Service.IKombiCimService
import com.kc.kombicim.Utility.Settings
import io.paperdb.Paper
import kotlinx.android.synthetic.main.fragment_weather.*
import okhttp3.OkHttpClient
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

class WeatherFragment : Fragment() {

    var dataHolder: WeatherViewPagerDataHolder? = null
    var pagerAdapter: WeatherFragmentAdapter? = null

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        val root = inflater.inflate(R.layout.fragment_weather, container, false)

        getWeatherData()

        return root
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setHasOptionsMenu(true)
    }

    override fun onCreateOptionsMenu(menu: Menu?, inflater: MenuInflater?) {
        super.onCreateOptionsMenu(menu, inflater)
        inflater!!.inflate(R.menu.refresh_button_fragment, menu)
    }

    override fun onOptionsItemSelected(item: MenuItem?): Boolean {
        if (item!!.itemId == R.id.action_refresh) {
            getWeatherData()
        }
        return super.onOptionsItemSelected(item)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

    }

    private fun getWeatherData() {
        val clientBuilder = OkHttpClient.Builder().addInterceptor(BasicAuthInterceptor(Settings.API_USERNAME, Settings.API_PASSWORD))
        clientBuilder.addInterceptor { chain ->
            val request = chain.request().newBuilder().addHeader(Settings.API_HEADER_API_TOKEN, Settings.GetToken()).build()
            chain.proceed(request)
        }

        val retrofit = Retrofit.Builder().addConverterFactory(GsonConverterFactory.create()).baseUrl(Settings.API_BASE_URL).client(clientBuilder.build()).build()
        val kombicimService = retrofit.create(IKombiCimService::class.java)

        val weatherCall = kombicimService.getWeather(12)
        weatherCall.enqueue(object : Callback<WeatherResponse> {
            override fun onFailure(call: Call<WeatherResponse>, t: Throwable) {
            }

            override fun onResponse(call: Call<WeatherResponse>, response: Response<WeatherResponse>) {
                if (!isDetached && response.isSuccessful && response.body() != null && fragmentWeather_viewPager != null) {
                    dataHolder = WeatherViewPagerDataHolder(response.body()!!.weatherDataList)
                    initViewPager()
                }
            }
        })
    }


    private fun initViewPager() {
        pagerAdapter = WeatherFragmentAdapter(childFragmentManager, dataHolder!!)
        fragmentWeather_viewPager.adapter = pagerAdapter
        activityMain_indicator.setViewPager(fragmentWeather_viewPager)
        activityMain_indicator.textColor = R.color.md_blue_700
        activityMain_indicator.footerColor = R.color.md_blue_grey_200
        activityMain_indicator.footerLineHeight = 2f
        activityMain_indicator.selectedColor = R.color.md_orange_300
    }

}