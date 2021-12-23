package com.kc.kombicim.Adapter

import android.graphics.Color
import com.chad.library.adapter.base.BaseQuickAdapter
import com.chad.library.adapter.base.BaseViewHolder
import com.kc.kombicim.Model.Dto.CombiLogDto
import com.kc.kombicim.R
import com.kc.kombicim.Utility.Settings
import java.text.SimpleDateFormat
import java.time.ZoneId
import java.time.format.DateTimeFormatter

class CombiLogAdapter(resource: Int, combiLogList: MutableList<CombiLogDto>) : BaseQuickAdapter<CombiLogDto, BaseViewHolder>(resource, combiLogList) {
    override fun convert(helper: BaseViewHolder, item: CombiLogDto) {

        var state = "Açık"
        if (!item.state)
            state = "Kapalı"

        val localDate = java.time.ZonedDateTime.parse(item.date, DateTimeFormatter.ISO_LOCAL_DATE_TIME.withZone(ZoneId.of("UTC"))).toInstant().atZone(ZoneId.systemDefault())
        val localDateStr =  DateTimeFormatter.ofPattern("HH:mm").format(localDate)

        helper.setText(R.id.item_combi_log_tvState, state).setText(R.id.item_combi_log_tvHour, localDateStr)

        helper.setTextColor(R.id.item_combi_log_tvState, Color.BLACK)

        if (item.state)
            helper!!.setBackgroundColor(R.id.layout_item_combi_log, Color.parseColor("#C5E1A5"))
        else
            helper!!.setBackgroundColor(R.id.layout_item_combi_log, Color.parseColor("#EF9A9A"))

    }
}