package com.kc.kombicim.Activity

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle

import android.content.Intent

import android.os.Handler
import com.kc.kombicim.R
import com.kc.kombicim.Utility.Settings
import io.paperdb.Paper


class SplashActivity : AppCompatActivity() {

    val DELAY: Long = 600

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_splash)

        Handler().postDelayed({
            if (isLoggedIn())
                startActivity(Intent(this, MainActivity::class.java))
            else
                startActivity(Intent(this, LoginActivity::class.java))

            finish()
        }, DELAY)
    }

    private fun isLoggedIn(): Boolean = Paper.book().contains(Settings.KEY_TOKEN)

}
