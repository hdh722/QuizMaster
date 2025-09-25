using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Program : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Hello, World");
        Publisher publisher = new Publisher();
        publisher.msg += ResultProcess;
        publisher.SendMessage("추가 문제 주세요");

        Debug.Log("종료");
    }

    void ResultProcess(string msg)
    {
        Debug.Log($"받은 메시지: {msg}");
    }

    void OtherProcess(string text)
    {
        Debug.Log($"다른 처리: {text}");
    }

    public class Publisher
    {
        public delegate void OnMessage(string msg);
        public event OnMessage msg;
        public void SendMessage(string text)
        {
            Debug.Log($"API 통신중... {text}");

            msg?.Invoke(text);
        }
    }
}
