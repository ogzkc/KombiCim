package com.kc.kombicim

import android.app.Application
import io.paperdb.Paper

class KombiCimApplication : Application() {
    override fun onCreate() {
        super.onCreate()

        Paper.init(this)
    }
}