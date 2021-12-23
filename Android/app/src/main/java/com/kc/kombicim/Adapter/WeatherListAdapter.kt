package com.kc.kombicim.Adapter

import android.graphics.Color
import com.chad.library.adapter.base.BaseQuickAdapter
import com.chad.library.adapter.base.BaseViewHolder
import com.kc.kombicim.Model.Dto.WeatherDto
import com.kc.kombicim.R
import java.text.DateFormatSymbols
import java.text.SimpleDateFormat
import java.time.LocalDate
import java.time.ZoneId
import java.time.format.DateTimeFormatter
import java.time.temporal.TemporalAccessor
import java.util.*

class WeatherListAdapter(resource: Int, weatherList: List<WeatherDto>) : BaseQuickAdapter<WeatherDto, BaseViewHolder>(resource, weatherList) {


    override fun convert(helper: BaseViewHolder, item: WeatherDto) {

        val localDate = java.time.ZonedDateTime.parse(item.date, DateTimeFormatter.ISO_LOCAL_DATE_TIME.withZone(ZoneId.of("UTC"))).toInstant().atZone(ZoneId.systemDefault())
        val localDateStr =  DateTimeFormatter.ofPattern("HH:mm").format(localDate)

        helper.setText(R.id.item_tvSicaklik, "${item.temperature} °C").setText(R.id.item_tvSaat, localDateStr).setText(R.id.item_tvNem, "% ${item.humidity}")

        if (helper.layoutPosition % 2 == 0) helper!!.setBackgroundColor(R.id.layout_itemWeather, Color.parseColor("#dcdcdc"))
        else helper!!.setBackgroundColor(R.id.layout_itemWeather, Color.parseColor("#eeeeee"))

    }
}