package com.kc.kombicim.Fragment

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.recyclerview.widget.LinearLayoutManager
import com.kc.kombicim.Adapter.WeatherListAdapter
import com.kc.kombicim.Model.Dto.WeatherData

import com.kc.kombicim.R
import kotlinx.android.synthetic.main.fragment_weather_list.*

class WeatherListFragment : Fragment() {

    private lateinit var weatherData: WeatherData
    private lateinit var rootView: View

    companion object {
        fun GetInstance(weatherData: WeatherData): WeatherListFragment {
            val weatherListFragment = WeatherListFragment()
            weatherListFragment.weatherData = weatherData
            return weatherListFragment
        }
    }

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        rootView = inflater.inflate(R.layout.fragment_weather_list, container, false)

        return rootView
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        recyclerView_weather.layoutManager = LinearLayoutManager(activity)
        recyclerView_weather.adapter = WeatherListAdapter(R.layout.item_weather,weatherData.weatherList)

    }
}