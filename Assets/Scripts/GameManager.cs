using System;                                       //不能删，在#elif UNITY_ANDROID里面能用到
using UnityEngine;
using UnityEngine.UI;
 
public class GameManager : MonoBehaviour
{
    public Button button_XAddY,button_MAXXY,button_XAdd1,button_Send;
    public Text text_log;
 
    AndroidJavaClass jc;
    AndroidJavaObject jo;
 
    public InputField inputFieldX;                  //用来输入数字的UI组件
    public InputField inputFieldY;
 
    private void Start() {
        text_log.text+="\r\n"+"初始化Start";
        button_XAddY.onClick.AddListener(AddXY);
        button_MAXXY.onClick.AddListener(MaxXY);
        button_XAdd1.onClick.AddListener(AddX1);
        button_Send.onClick.AddListener(SendCall);
#if UNITY_EDITOR                                    //预处理器指令，简单来说，在什么平台，调用里面的什么方法，注意的是，不在这个平台的话，他不会编译
#elif UNITY_ANDROID                                 //比如，这个安卓，在Unity_Editor模式下，他不会进行编译
        jc=new AndroidJavaClass("com.unity3d.player.UnityPlayer");  //实例化AndroidJavaClass，里面的com.unity3d.player.UnityPlayer是固定的
        jo=jc.GetStatic<AndroidJavaObject>("currentActivity");      //固定的写法
#elif UNITY_IOS
#endif
        text_log.text="\r\n"+"初始化结束";
    }
 
    private void AddXY(){
#if UNITY_EDITOR
        text_log.text = "add";
#elif UNITY_ANDROID
        text_log.text += "\r\n"+"X+Y开始：";
        //int a =Int32.Parse(inputFieldX.text);         //这里就很神奇，刚开始,使用的是Convert.ToInt32(),Unity总是报错，后来检查错误时候发现了
        //int b =Int32.Parse(inputFieldY.text);         //改成了Int32.Parse(),就可以通过Build了，然后我把他又改过来了，神奇的通过了
        int a = Convert.ToInt32(inputFieldX.text);        //就...奇奇怪怪
        int b = Convert.ToInt32(inputFieldY.text);
        text_log.text =
 "\r\n"+jo.Call<int>("Sum",a,b).ToString();   //jo.Call<a>("b",c,d) a里面代表返回值类型，没有的话<>就不写，b代表SDK里面的方法名，Unity就是通过这种形式调用的，c,d代表参数，一个参数写一个，多个参数写多个，可以创建一个数组，来表达多个参数，没有可以不写
#elif UNITY_IOS
#endif
    }
    private void MaxXY(){
#if UNITY_EDITOR
#elif UNITY_ANDROID
        text_log.text+="\r\n"+"X,Y最大值：";
        // int a =Int32.Parse(inputFieldX.text);
        // int b =Int32.Parse(inputFieldY.text);
        int a=Convert.ToInt32(inputFieldX.text);
        int b=Convert.ToInt32(inputFieldY.text);
        text_log.text="\r\n"+jo.Call<int>("Max",a,b).ToString();
#elif UNITY_IOS
#endif
    }
 
    private void AddX1(){
#if UNITY_EDITOR
#elif UNITY_ANDROID
        text_log.text+="\r\n"+"X+1的值：";
        // int a =Int32.Parse(inputFieldX.text);
        int a=Convert.ToInt32(inputFieldX.text);
        int b=Convert.ToInt32(inputFieldY.text);
        text_log.text = "\r\n"+"AddOne:" + jo.Call<int>("AddOne",a);
#elif UNITY_IOS
#endif
    }
 
    private void SendCall(){
#if UNITY_EDITOR
#elif UNITY_ANDROID
        text_log.text+="\r\n"+"调用安卓方法：";
         //调用Java中的一个方法，该方法会回调Unity中的指定的一个方法，这里会回调Receive( )
        jo.Call("CallUnityFunc","Unity Call Android.\n");   //没有返回值的写法
#elif UNITY_IOS
#endif
 
    }
 
    //不能删 在SDK里调用勒 用来回调
    public void SendAndroidFunc(string str){
        text_log.text="\r\n"+"回调后的值：";
        text_log.text="\r\n"+str;
    }
 
}