package com.example.unityapplication;

import com.unity3d.player.UnityPlayerActivity ;
import com.unity3d.player.UnityPlayer;

import android.annotation.SuppressLint;
import android.content.ContentResolver;
import android.content.Context;
import android.content.Intent;
import android.database.Cursor;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.os.Looper;
import android.provider.OpenableColumns;
import android.util.Log;
import android.widget.Toast;

import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.nio.file.Path;
import java.util.Timer;
import java.util.TimerTask;
import java.util.zip.ZipEntry;
import java.util.zip.ZipInputStream;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.io.StringWriter;
import java.io.PrintWriter;
import java.io.FileWriter;
import java.io.RandomAccessFile;


public class OverrideExample extends UnityPlayerActivity  {
    File root;
    String tag = "OverrideExample";
    Context context;
    Timer timer;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        // Calls UnityPlayerActivity.onCreate()
        super.onCreate(savedInstanceState);
        // Prints debug message to Logcat

         context = getApplicationContext();
        timer = new Timer();//实例化Timer类

        Log.d(tag, "onCreate called!");
        delayReceive(this.getIntent());

        //记录崩溃信息
        final Thread.UncaughtExceptionHandler defaultHandler = Thread.getDefaultUncaughtExceptionHandler();
        Thread.setDefaultUncaughtExceptionHandler(new Thread.UncaughtExceptionHandler() {
            @Override
            public void uncaughtException(Thread thread, Throwable throwable) {
                //获取崩溃时的UNIX时间戳
                long timeMillis = System.currentTimeMillis();
                //将时间戳转换成人类能看懂的格式，建立一个String拼接器
                StringBuilder stringBuilder = new StringBuilder(new SimpleDateFormat("yyyy/MM/dd HH:mm:ss").format(new Date(timeMillis)));

                String errorLogFileName = "永恒岛崩溃日志_"+stringBuilder.toString();
                stringBuilder.append(":\n");
                //获取错误信息
                stringBuilder.append(throwable.getMessage());
                stringBuilder.append("\n");
                //获取堆栈信息
                StringWriter sw = new StringWriter();
                PrintWriter pw = new PrintWriter(sw);
                throwable.printStackTrace(pw);
                stringBuilder.append(sw.toString());

                //这就是完整的错误信息了，你可以拿来上传服务器，或者做成本地文件保存等等等等
                String errorLog = stringBuilder.toString();

                writeTxtToFile(errorLog,errorLogFileName);
                //最后如何处理这个崩溃，这里使用默认的处理方式让APP停止运行
                defaultHandler.uncaughtException(thread, throwable);
            }
        });
    }

    /**
     * 将字符串写入到文本文件中
     * @param str
     * @param fileName
     */
    public static void writeTxtToFile(String str,String fileName) {
        // 每次写入时，都换行写
        String strContent = str + "\r\n";
        //生成文件夹之后，再生成文件，不然会出错
        File file = null;
        try {
            file = new File(getDownDirs(), fileName);
            if (!file.exists()) {
                file.createNewFile();
            }else{
                //清空文本内容
                FileWriter fileWriter =new FileWriter(file);
                fileWriter.write("");
                fileWriter.flush();
                fileWriter.close();
            }
            RandomAccessFile raf = new RandomAccessFile(file, "rwd");
            raf.seek(file.length());
            raf.write(strContent.getBytes());
            raf.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    public static File getDownDirs(){
        File dir =Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DOWNLOADS);
        if (!dir.exists()) {
            dir.mkdirs();
        }
        return dir;
    }
    @Override
    protected void onNewIntent(Intent intent) {
        super.onNewIntent(intent);
        delayReceive(intent);
    }

    public void delayReceive(Intent intent)
    {
         logMessage(  "开始复制文件!");

        timer.schedule(new TimerTask(){
            public void run()
            {
                receiveActionSend(intent);
            }},3500);//五百毫秒
    }
    public void receiveActionSend(Intent intent)
    {
        //UnityPlayer.UnitySendMessage("Init", "OnMessage", "receiveActionSend"); // 参数：GameObject名 + 方法名 + 参数
        String action = intent.getAction();
        Log.d(tag,"action:"+action);
        Log.d(tag,"getDataString:"+intent.getDataString());
        if (Intent.ACTION_VIEW.equals(action)) {
             File tmp = null;

            String str = intent.getDataString();

            if (null != str) {
                tmp = uriToFileApiQ(Uri.parse(str));
                if (tmp!=null)
                {
                    Log.d(tag, tmp.getAbsolutePath());
                     logMessage(  "完成复制文件!");

                }
                String s3 = ".zip";
                if (!tmp.getName().endsWith(s3))
                {
                     logMessage(  "只能打开zip文件!");
                    return;
                }

                //setText("主程序仅支持打开" +s3+"的文件");return;
                File target = new File(root.getAbsolutePath() + File.separator + "已安装" + tmp.getName());
                if (target.exists() && target.isFile())
                {
                    //setText(target.getName() +”已完成安装!!!可以开始游戏了!!!"");return;
                    logMessage( "已完成安装!!!可以开始游戏了!");
                    //UnityPlayer.UnitySendMessage("Init", "OnMessage", "已完成安装!!!可以开始游戏了!!!");
                    return;
                }

                logMessage( "开始解压!");

                File finalTmp = tmp;
                timer.schedule(new TimerTask(){
                    public void run()
                    {
                        unzip(finalTmp);
                        logMessage("解压完成!请点击开始游戏按钮，开始游戏");
                    }},3000);//五百毫秒
            }

        }
    }

    public void logMessage(String m)
    {
        if (Looper.myLooper()==null)
            Looper.prepare();
        Toast.makeText(context,m, Toast.LENGTH_LONG).show();
    }
    public void unzip(File tmp)
    {
        byte[] buffer = new byte[2048];
        try (FileInputStream fis = new FileInputStream(tmp);
             BufferedInputStream bis = new BufferedInputStream(fis);
             ZipInputStream stream = new ZipInputStream(bis)) {
            Path outDir =  tmp.getParentFile().toPath();
            ZipEntry entry;
            while ((entry = stream.getNextEntry()) != null) {

                Path filePath = outDir.resolve(entry.getName());

                try (FileOutputStream fos = new FileOutputStream(filePath.toFile());
                     BufferedOutputStream bos = new BufferedOutputStream(fos, buffer.length)) {

                    int len;
                    while ((len = stream.read(buffer)) > 0) {
                        bos.write(buffer, 0, len);
                    }
                }
            }
        }
        catch (FileNotFoundException e) {
            throw new RuntimeException(e);
        } catch (IOException e) {
            throw new RuntimeException(e);
        }
    }
    @SuppressLint("Range")
    public File uriToFileApiQ(Uri uri)
    {
        File file = null;
        Log.d(tag,"getScheme:"+uri.getScheme());

        if (uri.getScheme().equals(ContentResolver.SCHEME_FILE))
        {
            file= new File(uri.getPath());
        }
        else if (uri.getScheme().equals(ContentResolver.SCHEME_CONTENT))
        {
            ContentResolver contentResolver=getContentResolver();
            String displayName = "tmp.zip";
            Cursor cursor=contentResolver.query(uri,null,null,null,null);
            if (cursor.moveToNext())
                displayName = cursor.getString((cursor.getColumnIndex(OpenableColumns.DISPLAY_NAME)));
            Log.d(tag,"displayName:"+uri.getScheme());
            root = new File(Environment.getExternalStorageDirectory().getAbsoluteFile() + "/ForeverStory1");
            Log.d(tag,"root path:"+root.getAbsolutePath());
            //Log.d(tag,"getFilesDir path:"+getFilesDir().getAbsolutePath());

            File cache = new File(root,displayName);
            Log.d(tag,"cache path:"+cache.getAbsolutePath());

            //UnityPlayer.UnitySendMessage("Init", "OnMessage", "uriToFileApiQ"); // 参数：GameObject名 + 方法名 + 参数
            try(InputStream is = contentResolver.openInputStream(uri))
            {
                copyInputStreamToFile(is,cache);
                file = cache;
            }
            catch (IOException e)
            {
                Log.d(tag,"e"+e.getLocalizedMessage());
                return null;
            }

        }
        return file;
    }

    public void copyInputStreamToFile(InputStream i,File f)
    {
        int read = 0;
        byte[] buffer = new byte[8 * 1024];
        try {
            OutputStream outputStream = new FileOutputStream(f);
            while ((read = i.read(buffer)) != -1) {
                outputStream.write(buffer, 0, read);
            }
            outputStream.flush();
        }
        catch (FileNotFoundException e) {
            throw new RuntimeException(e);
        } catch (IOException e) {
            throw new RuntimeException(e);
        }
    }
}