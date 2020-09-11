package com.kc.kombicim.Fragment

import android.os.Bundle
import android.view.*
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProviders
import androidx.recyclerview.widget.LinearLayoutManager
import com.kc.kombicim.Adapter.CombiLogAdapter
import com.kc.kombicim.Model.Response.CombiLogResponse

import com.kc.kombicim.R
import com.kc.kombicim.Service.BasicAuthInterceptor
import com.kc.kombicim.Service.IKombiCimService
import com.kc.kombicim.Utility.Settings
import kotlinx.android.synthetic.main.fragment_combi_log.*
import okhttp3.OkHttpClient
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

class CombiLogFragment : Fragment() {

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?
    ): View? {
        val root = inflater.inflate(R.layout.fragment_combi_log, container, false)

        return root
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setHasOptionsMenu(true)
    }

    override fun onCreateOptionsMenu(menu: Menu?, inflater: MenuInflater?) {
        super.onCreateOptionsMenu(menu, inflater)
        inflater!!.inflate(R.menu.refresh_button_fragment, menu)
    }

    override fun onOptionsItemSelected(item: MenuItem?): Boolean {
        if (item!!.itemId == R.id.action_refresh) {
            getCombiLogs()
        }
        return super.onOptionsItemSelected(item)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        fragmentCombiLog_swipeRefreshLayout.isRefreshing = true
        fragmentCombiLog_swipeRefreshLayout.setOnRefreshListener {
            getCombiLogs()
        }

        recyclerView_combi_log.layoutManager = LinearLayoutManager(activity)

        getCombiLogs()
    }

    private fun getCombiLogs() {
        val clientBuilder = OkHttpClient.Builder().addInterceptor(BasicAuthInterceptor(Settings.API_USERNAME, Settings.API_PASSWORD))
        clientBuilder.addInterceptor { chain ->
            val request = chain.request().newBuilder().addHeader(Settings.API_HEADER_API_TOKEN, Settings.GetToken()).build()
            chain.proceed(request)
        }

        val retrofit = Retrofit.Builder().addConverterFactory(GsonConverterFactory.create()).baseUrl(Settings.API_BASE_URL).client(clientBuilder.build()).build()
        val kombicimService = retrofit.create(IKombiCimService::class.java)

        val combiLogCall = kombicimService.getCombiLog(12)
        combiLogCall.enqueue(object : Callback<CombiLogResponse> {
            override fun onFailure(call: Call<CombiLogResponse>, t: Throwable) {
                fragmentCombiLog_swipeRefreshLayout.isRefreshing = false
            }

            override fun onResponse(call: Call<CombiLogResponse>, response: Response<CombiLogResponse>) {
                if (response.isSuccessful && response.body() != null && recyclerView_combi_log != null && !isDetached) {
                    recyclerView_combi_log.adapter = CombiLogAdapter(R.layout.item_combi_log, response.body()!!.combiLogs.toMutableList())
                }

                fragmentCombiLog_swipeRefreshLayout.isRefreshing = false
            }
        })
    }
}