package com.kc.kombicim.Activity

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.Toast
import com.kc.kombicim.Service.IKombiCimService
import com.kc.kombicim.Model.Request.LoginRequest
import com.kc.kombicim.Model.Response.LoginResponse
import com.kc.kombicim.R
import com.kc.kombicim.Service.BasicAuthInterceptor
import com.kc.kombicim.Utility.Settings
import io.paperdb.Paper
import kotlinx.android.synthetic.main.activity_login.*
import okhttp3.OkHttpClient
import retrofit2.*
import retrofit2.converter.gson.GsonConverterFactory


class LoginActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_login)

        buttonLogin.setOnClickListener {
            val emailAddress = editTextEmail.text
            val password = editTextPassword.text
            val content = "$emailAddress:$password"
            val base64 = android.util.Base64.encode(content.toByteArray(), android.util.Base64.DEFAULT).toString(Charsets.UTF_8)
            val loginRequest = LoginRequest(base64)

            val clientBuilder = OkHttpClient.Builder().addInterceptor(BasicAuthInterceptor(Settings.API_USERNAME, Settings.API_PASSWORD))
            clientBuilder.addInterceptor { chain ->
                val request = chain.request().newBuilder().build()
//                val request = chain.request().newBuilder().addHeader(Settings.API_HEADER_API_TOKEN, Settings.GetToken()).build()
                chain.proceed(request)
            }

            val retrofit = Retrofit.Builder().addConverterFactory(GsonConverterFactory.create()).baseUrl(Settings.API_BASE_URL).client(clientBuilder.build()).build()
            val kombicimService = retrofit.create(IKombiCimService::class.java)

            val loginCall = kombicimService.login(loginRequest)
            loginCall.enqueue(object : Callback<LoginResponse> {
                override fun onFailure(call: Call<LoginResponse>, t: Throwable) {

                }

                override fun onResponse(call: Call<LoginResponse>, response: Response<LoginResponse>) {
                    if (response.isSuccessful && response.body() != null) {
                        if (response.body()!!.Token != null) {
                            Paper.book().write(Settings.KEY_TOKEN, response.body()!!.Token)
                            finish()
                            startActivity(Intent(applicationContext, MainActivity::class.java))
                        } else {
                            Toast.makeText(applicationContext, "Kullanıcı adı ya da şifre yanlış.", Toast.LENGTH_SHORT).show()
                        }
                    }
                }

            });
        }
    }
}
