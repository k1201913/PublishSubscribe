package com.example.eikkujar.subscriber_client;

import android.content.Context;
import android.os.StrictMode;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.Toast;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.SocketException;
import java.net.UnknownHostException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;


public class MainActivity extends ActionBarActivity {

    public static final int SERVERPORT = 10001;
    DatagramSocket client;
    ArrayAdapter adapter;
    private volatile Thread eventsReceiver = null;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
        StrictMode.setThreadPolicy(policy);

        try{
            client = new DatagramSocket();
        }
        catch (SocketException e)
        {
            Toast.makeText (this, "Socket creation failed", Toast.LENGTH_LONG).show();
        }

        final ArrayList<String> eventList = new ArrayList<String>();
        eventList.add("date, topic, eventdata");
        adapter = new ArrayAdapter(this,
                android.R.layout.simple_list_item_1, eventList);
        final ListView listview = (ListView) findViewById(R.id.eventList);
        listview.setAdapter(adapter);

        listview.setOnItemClickListener(new AdapterView.OnItemClickListener() {

            @Override
            public void onItemClick(AdapterView<?> parent, final View view,
                                    int position, long id) {
                final String item = (String) parent.getItemAtPosition(position);
                view.animate().setDuration(2000).alpha(0)
                        .withEndAction(new Runnable() {
                            @Override
                            public void run() {
                                eventList.remove(item);
                                adapter.notifyDataSetChanged();
                                view.setAlpha(1);
                            }
                        });
            }
        });
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        eventsReceiver = null;
    }


        @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.

        EditText ip = (EditText) findViewById(R.id.ip);
        EditText topic = (EditText) findViewById(R.id.topic);

        if(ip.getText().length()==0 || topic.getText().length()==0){
            String txt = getResources().getString(R.string.ipOrTopicMissing);
            Toast.makeText (this, txt, Toast.LENGTH_LONG).show();
            return true;
        }

        try {
            InetAddress serverAddr = InetAddress.getByName(ip.getText().toString());
            String msg;
            byte[] buf;
            DatagramPacket packet;
            switch (item.getItemId()) {
                case R.id.subscribe_action:
                    msg = "Subscribe" + "," + topic.getText(); // + "," + "10";
                    adapter.add(msg) ;
                    buf = msg.getBytes();
                    packet = new DatagramPacket(
                            buf, buf.length, serverAddr, SERVERPORT);
                    client.send(packet);
                    eventsReceiver = new Thread(new Receiver());
                    eventsReceiver.start();
                    return true;
                case R.id.unSubscribe_action:
                    msg = "UnSubscribe" + "," + topic.getText();
                    adapter.add(msg) ;
                    buf = msg.getBytes();
                    packet = new DatagramPacket(
                            buf, buf.length, serverAddr, SERVERPORT);
                    client.send(packet);
                    eventsReceiver =null;
                    return true;
                case R.id.clearList_action:
                    adapter.clear();
                    return true;
                case R.id.clearHistory_action:
                    msg = "Clear" + "," + topic.getText();
                    adapter.add(msg) ;
                    buf = msg.getBytes();
                    packet = new DatagramPacket(
                            buf, buf.length, serverAddr, SERVERPORT);
                    client.send(packet);
                    return true;

            }


        }
        catch (UnknownHostException e)
        {
            Toast.makeText (this, "Unknown host", Toast.LENGTH_LONG).show();
        }
        catch (IOException e)
        {
            Toast.makeText (this, "IO exception", Toast.LENGTH_LONG).show();
        }

        return false;
    }

    public class Receiver implements Runnable {
        @Override
        public void run() {
            Thread thisThread = Thread.currentThread();
            while (eventsReceiver == thisThread) {
                {
                    try {
                        byte[] buf = new byte[200];
                        DatagramPacket packet = new DatagramPacket(buf, buf.length);
                        client.receive(packet);
                        runOnUiThread(new Message(new String(packet.getData())));
                    } catch (Exception e) {
                        try {
                            throw e;
                        } catch (IOException e1) {
                            e1.printStackTrace();
                        }
                    }
                }
            }
        }
    }

    public class Message implements Runnable{

        String msg;

        Message(String msg)
        {
            this.msg= msg;
        }

        @Override
        public void run() {
            adapter.add(msg);
        }
    }

}
