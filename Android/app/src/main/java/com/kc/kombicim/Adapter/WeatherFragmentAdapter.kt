package com.kc.kombicim.Adapter

import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentManager
import androidx.fragment.app.FragmentPagerAdapter
import androidx.fragment.app.FragmentStatePagerAdapter
import com.kc.kombicim.Fragment.WeatherListFragment
import com.kc.kombicim.Model.Adapter.WeatherViewPagerDataHolder

class WeatherFragmentAdapter(fm: FragmentManager, val dataHolder: WeatherViewPagerDataHolder) : FragmentStatePagerAdapter(fm) {

    override fun getItem(position: Int): Fragment {
        return WeatherListFragment.GetInstance(dataHolder.weatherDatas!![position])
    }

    override fun getCount(): Int {
        return dataHolder.weatherDatas!!.size
    }

    override fun getPageTitle(position: Int): CharSequence {
        return dataHolder.weatherDatas!![position].location.name!!
    }

    override fun getItemPosition(`object`: Any): Int {
        return POSITION_NONE
    }
}