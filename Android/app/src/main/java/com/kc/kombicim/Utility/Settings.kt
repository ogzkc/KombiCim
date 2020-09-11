package com.kc.kombicim.Utility

import io.paperdb.Paper

class Settings {
    companion object {
        const val KEY_TOKEN = "key_token"

        const val API_BASE_URL = "http://xxxxxxx/KombiCim/Mobile/api/"
        const val API_HEADER_API_TOKEN = "ApiToken"

        const val API_USERNAME = "xxxxxxx"
        const val API_PASSWORD = "yyyyyyy"

        fun GetToken(): String {
            if (Paper.book().contains(KEY_TOKEN)) return Paper.book().read(KEY_TOKEN)
            else return ""
        }
    }
}