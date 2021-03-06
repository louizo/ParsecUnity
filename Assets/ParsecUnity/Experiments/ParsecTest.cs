﻿using UnityEngine;
using ParsecGaming;
using System;
using System.Collections;
using UnityEngine.UI;

public class ParsecTest : MonoBehaviour
{
    [SerializeField]
    private ParsecGuestView parsecGuestView;

    public GameObject menu;

    public GameObject scene;

    public Button hostButton;
    public Button connectButton;

    public InputField sessionId;
    public InputField peerId;

    public Toggle tryCreateTextureToggle;

    public Text statusText;

    public enum Role { None, Host, Guest }

    private Parsec parsec;
    private Role role;

    private Texture2D viewportTexture;

    private ParsecFrameDecoder decoder;

    [Header("Debug")]
    public bool pollClientFrame;

    private void Awake()
    {
        hostButton.onClick.AddListener(Host);
        connectButton.onClick.AddListener(Connect);

        parsecGuestView.gameObject.SetActive(false);
        scene.SetActive(false);
        tryCreateTextureToggle.gameObject.SetActive(false);

        decoder = new ParsecFrameDecoder();
    }

    private void OnValidate()
    {
        if (pollClientFrame && role == Role.Guest)
        {
            var status = parsec.ClientPollFrame(0, ClientReceiveFrame, 60);

            pollClientFrame = false;
        }
    }

    private void InitParsec()
    {
        parsec = new Parsec();
        Parsec.ParsecStatus status = parsec.Init();
        Debug.Log($"Parsec init: {status}");

        sampleRate = (uint)AudioSettings.outputSampleRate;
        instance = this;
        //framePCMData = new List<float[]>();

        StartCoroutine(CheckStatus());
    }

    private void OnApplicationQuit()
    {
        if (parsec != null)
            parsec.ParsecDestroy();
    }

    private void Host()
    {
        InitParsec();

        role = Role.Host;

        var status = parsec.HostStart(Parsec.ParsecHostMode.HOST_GAME, sessionId.text);
        Debug.Log($"Parsec host: {status}");
        StartCoroutine(SubmitFrame());

        menu.SetActive(false);
        scene.SetActive(true);

        parsec.KeyboardInput += Parsec_KeyboardInput;
        parsec.MouseMotion += Parsec_MouseMotion;
        parsec.MouseButton += Parsec_MouseButton;
    }

    private void Parsec_MouseMotion(object sender, Parsec.ParsecGuest guest, Parsec.ParsecMouseMotionMessage mouseMotion)
    {
        // Debug.Log($"Guest {guest.id} moved mouse {mouseMotion.x}, {mouseMotion.y}");
    }

    private void Parsec_MouseButton(object sender, Parsec.ParsecGuest guest, Parsec.ParsecMouseButtonMessage mouseButton)
    {
        Debug.Log($"Guest {guest.id} {(mouseButton.pressed ? "clicked" : "released")} key {mouseButton.button}");
    }

    private void Parsec_KeyboardInput(object sender, Parsec.ParsecGuest guest, Parsec.ParsecKeyboardMessage keyboard)
    {
        Debug.Log($"Guest {guest.id} {(keyboard.pressed ? "pressed" : "released")} key {keyboard.code}");
    }

    private void Connect()
    {
        InitParsec();

        role = Role.Guest;

        Parsec.ParsecClientConfig defaultConfig = new Parsec.ParsecClientConfig()
        {
            mediaContainer = 0,
            protocol = 1,
            secret = "",
            pngCursor = false
        };

        var status = parsec.ClientConnect(defaultConfig, sessionId.text, peerId.text);
        Debug.Log($"Parsec client: {status}");

        parsecGuestView.gameObject.SetActive(true);
        menu.SetActive(false);
        tryCreateTextureToggle.gameObject.SetActive(true);

        gameObject.AddComponent<ParsecGuestInput>().Initialize(parsec);
    }

    private void Update()
    {
        if (role == Role.Guest && tryCreateTextureToggle.isOn)
        {
            parsec.ClientPollFrame(0, ClientReceiveFrame, 60);
        }

        if (role == Role.Host)
        {
            parsec.HostPollInput();
        }
    }

    private void ClientReceiveFrame(Parsec.ParsecFrame frame, IntPtr image, IntPtr opaque)
    {
        // Debug.Log($"Frame received at {Time.time}. Format:{frame.format} Size:{frame.width}x{frame.height} ActualSize:{frame.fullWidth}x{frame.fullHeight} ByteSize:{frame.size}");

        decoder.Decode(frame, image);

        Vector2 padding = new Vector2((float)frame.width / frame.fullWidth, (float)frame.height / frame.fullHeight);
        parsecGuestView.Populate(decoder.Y, decoder.U, decoder.V, padding, decoder.AspectRatio);
    }

    private IEnumerator CheckStatus()
    {
        while (true)
        {
            yield return null;

            string text;

            if (role == Role.Host)
            {
                parsec.HostGetStatus(out Parsec.ParsecHostStatus status);
                text = $"Host status\nGuests: {status.numGuests}\nRunning: {status.running}\nInvalid id:{status.invalidSessionID}";
            }
            else
            {
                var status = parsec.ClientGetStatus(out Parsec.ParsecClientStatus clientStatus);
                text = $"Client status: {status}\nSelf guest status: {clientStatus.self.state}\nNet failure: {clientStatus.networkFailure}";
            }

            statusText.text = text;
        }
    }

    private IEnumerator SubmitFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();

            //Debug.Log($"Submitting frame received at {Time.time}");

            if (viewportTexture != null)
                Destroy(viewportTexture);

            viewportTexture = ScreenCapture.CaptureScreenshotAsTexture(1);
            UnityNative.UnitySubmitFrame(parsec.GetPointer(), viewportTexture.GetNativeTexturePtr());
            GL.IssuePluginEvent(UnityNative.UnityGetRenderEventFunction(), 1);
        }
    }

    #region Audio Stuff
    private static ParsecTest instance;
    private uint sampleRate;

    //private System.Collections.Generic.List<float[]> framePCMData;

    // private int audioCount = 2;
    // private int dataLength;

    public static void SubmitAudio(float[] data)
    {
        instance.SubmitAudioInternal(data);
    }

    private void SubmitAudioInternal(float[] data)
    {
        parsec.HostSubmitAudio(sampleRate, data, (uint)(data.Length / 2));

        //if (role == Role.Host)
        //{
        //    dataLength = Mathf.Max(dataLength, data.Length);

        //    framePCMData.Add(data);

        //    if (framePCMData.Count >= audioCount)
        //    {
        //        float[] combinedData = new float[dataLength];

        //        for (int i = 0; i < combinedData.Length; i++)
        //        {
        //            float pcm = 0;
        //            int count = 0;

        //            for (int j = 0; j < framePCMData.Count; j++)
        //            {
        //                if (i < framePCMData[j].Length)
        //                {
        //                    pcm += framePCMData[j][i];
        //                    count++;
        //                }
        //            }

        //            combinedData[i] = pcm / count;
        //        }

        //        parsec.HostSubmitAudio(sampleRate, combinedData, (uint)(combinedData.Length / 2));
        //        dataLength = 0;
        //        framePCMData.Clear();
        //    }
        //}
    }

    private void ClientReceiveAudio(short[] pcm, uint frames, IntPtr opaque)
    {
        Debug.Log("audio received");
    }

    //private void OnAudioFilterRead(float[] data, int channels)
    //{
    //    if (role == Role.Guest)
    //    {
    //        Debug.Log("poll!");

    //        parsec.ClientPollAudio(ClientReceiveAudio, 60);
    //    }
    //}
    #endregion
}