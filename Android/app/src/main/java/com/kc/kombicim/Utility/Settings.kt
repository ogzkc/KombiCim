package com.kc.kombicim.Utility

import io.paperdb.Paper

class Settings {
    companion object {
        const val KEY_TOKEN = "key_token"
        const val KEY_EMAIL = "key_email"

        const val DATETIME_FORMAT = "yyyy-MM-dd'T'HH:mm:ss"

        const val API_BASE_URL = "http://xxxxxxxxxxxx/"
        const val API_HEADER_API_TOKEN = "ApiToken"

        const val API_USERNAME = "xxxxxxxxxx"
        const val API_PASSWORD = "yyyyyyyyyy"

        fun GetToken(): String {
            if (Paper.book().contains(KEY_TOKEN)) return Paper.book().read(KEY_TOKEN)
            else return ""
        }
    }
}