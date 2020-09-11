package com.kc.kombicim.Adapter

import android.graphics.Color
import com.chad.library.adapter.base.BaseQuickAdapter
import com.chad.library.adapter.base.BaseViewHolder
import com.kc.kombicim.Model.Dto.WeatherDto
import com.kc.kombicim.R
import java.text.SimpleDateFormat
import java.time.LocalDate
import java.time.format.DateTimeFormatter

class WeatherListAdapter(resource: Int, weatherList: List<WeatherDto>) : BaseQuickAdapter<WeatherDto, BaseViewHolder>(resource, weatherList) {


    override fun convert(helper: BaseViewHolder, item: WeatherDto) {

        val date = SimpleDateFormat("YYYY-MM-dd'T'HH:mm:ss").parse(item.date)

        helper.setText(R.id.item_tvSicaklik, "${item.temperature} °C")
            .setText(R.id.item_tvSaat, SimpleDateFormat("HH:mm").format(date))
            .setText(R.id.item_tvNem, "% ${item.humidity}")

        if (helper.layoutPosition % 2 == 0)
            helper!!.setBackgroundColor(R.id.layout_itemWeather, Color.parseColor("#dcdcdc"))
        else
            helper!!.setBackgroundColor(R.id.layout_itemWeather, Color.parseColor("#eeeeee"))

    }
}