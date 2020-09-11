package com.kc.kombicim.Adapter

import android.graphics.Color
import com.chad.library.adapter.base.BaseQuickAdapter
import com.chad.library.adapter.base.BaseViewHolder
import com.kc.kombicim.Model.Dto.CombiLogDto
import com.kc.kombicim.R
import java.text.SimpleDateFormat

class CombiLogAdapter(resource: Int, combiLogList: MutableList<CombiLogDto>) : BaseQuickAdapter<CombiLogDto, BaseViewHolder>(resource, combiLogList) {
    override fun convert(helper: BaseViewHolder, item: CombiLogDto) {

        val date = SimpleDateFormat("YYYY-MM-dd'T'HH:mm:ss").parse(item.date)
        var state = "Açık"
        if (!item.state)
            state = "Kapalı"

        helper.setText(R.id.item_combi_log_tvState, state)
            .setText(R.id.item_combi_log_tvHour, SimpleDateFormat("HH:mm").format(date))

        if (helper.layoutPosition % 2 == 0)
            helper!!.setBackgroundColor(R.id.layout_item_combi_log, Color.parseColor("#dcdcdc"))
        else
            helper!!.setBackgroundColor(R.id.layout_item_combi_log, Color.parseColor("#eeeeee"))

    }
}