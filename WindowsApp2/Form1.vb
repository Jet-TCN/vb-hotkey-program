Option Explicit On
Imports System.Runtime.InteropServices

Public Class Form1

    Dim play0, play1, hasnotplayed1, hasnotplayed0 As Boolean

    Dim num() As Integer = {0, 0}
    Dim delay, delay2, x As Integer
    Dim operand As Integer = 5
    Dim strOutput() As String = {"I like to eat pizza"}
    Dim strExtra, mute, executeFunc As Boolean
    Dim nullMsg As String = "NULL!"
    Dim strInput() As String = {nullMsg, nullMsg, nullMsg, nullMsg, nullMsg, nullMsg, nullMsg, nullMsg, nullMsg, nullMsg, nullMsg, nullMsg}

    Private Const VER_PLATFORM_WIN32_NT = 2
    Private Const VER_PLATFORM_WIN32_WINDOWS = 1
    Private Const VK_SCROLL = &H91
    Private Const KEYEVENTF_EXTENDEDKEY = &H1
    Private Const KEYEVENTF_KEYUP = &H2

    Public Declare Function GetAsyncKeyState Lib "user32" _
          Alias "GetAsyncKeyState" (ByVal vKey As Integer) _
          As Integer

    Private Declare Function SetKeyboardState Lib "user32" _
   (lppbKeyState As Byte) As Long


    Private Declare Sub keybd_event Lib "user32" (
        ByVal bVk As Byte,
        ByVal bScan As Byte,
        ByVal dwFlags As Integer,
        ByVal dwExtraInfo As Integer
    )

    Public Sub Scroll_Off()
        keybd_event(VK_SCROLL, &H45, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(VK_SCROLL, &H45, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load



        play0 = True
        play1 = True
        hasnotplayed1 = True
        hasnotplayed0 = True

        delay = 100
        x = 1
        delay2 = 1

        'MessageBox.Show(Me.Size.ToString)

        My.Forms.Form1.TopMost = True
        My.Forms.Form1.Left = 0
        My.Forms.Form1.Top = 0
        My.Forms.Form1.Size = New Size(292, 65)

        Timer1.Interval = delay
        Timer1.Start()

        Timer2.Interval = delay2
        Timer2.Start()

        FlavourLabel.Text = FlavourLabel.Text + My.User.Name
        'FlavourLabel.ForeColor = Color.Black
        'FlavourLabel.BackColor = Color.White

        Scroll_Off()

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        If (GetAsyncKeyState(Keys.F12) And My.Computer.Keyboard.CtrlKeyDown) Then
            If (My.Forms.Form1.Opacity) = 1 Then
                My.Forms.Form1.Opacity = 0

            Else
                My.Forms.Form1.Opacity = 1
            End If
        End If

        If (GetAsyncKeyState(Keys.F11) And My.Computer.Keyboard.CtrlKeyDown) Then
            My.Forms.Form1.Size = New Size(292, 227)
        Else
            My.Forms.Form1.Size = New Size(292, 65)
        End If

        Label3.Text = Math.Round(My.Computer.Info.TotalPhysicalMemory / 1024 / 1024 / 1024) - Math.Round(My.Computer.Info.AvailablePhysicalMemory / 1000000) / 1000 & "GB   /   " &
         Math.Round(My.Computer.Info.TotalPhysicalMemory / 1024 / 1024 / 1024) & "GB"

        strExtra = My.Computer.Keyboard.ScrollLock
        strInput(0) = GetAsyncKeyState(Keys.M)
        strInput(1) = GetAsyncKeyState(Keys.Escape)
        strInput(2) = GetAsyncKeyState(Keys.NumPad7)
        strInput(3) = GetAsyncKeyState(Keys.NumPad8)
        strInput(4) = GetAsyncKeyState(Keys.NumPad9)
        strInput(5) = GetAsyncKeyState(Keys.NumPad4)
        strInput(6) = GetAsyncKeyState(Keys.NumPad5)
        strInput(7) = GetAsyncKeyState(Keys.NumPad6)

        For i = 48 To 57
            If (GetAsyncKeyState(i) And strExtra) Then
                ' MessageBox.Show(i-48)
                RichTextBox1.Text = RichTextBox1.Text + (i - 48).ToString
                My.Computer.Keyboard.SendKeys("{BACKSPACE}")
            End If
        Next i

        If (GetAsyncKeyState(Keys.Add) Or GetAsyncKeyState(Keys.Divide) Or GetAsyncKeyState(Keys.Subtract) Or GetAsyncKeyState(Keys.Multiply) And strExtra) Then
            If (RichTextBox1.Text IsNot "") Then
                num(0) = RichTextBox1.Text
                RichTextBox1.Text = ""
                My.Computer.Keyboard.SendKeys("{BACKSPACE}")
                If GetAsyncKeyState(Keys.Add) Then
                    operand = 0
                ElseIf GetAsyncKeyState(Keys.Subtract) Then
                    operand = 1
                ElseIf GetAsyncKeyState(Keys.Multiply) Then
                    operand = 2
                ElseIf GetAsyncKeyState(Keys.Divide) Then
                    operand = 3
                End If
            End If

        End If

            If (GetAsyncKeyState(Keys.Enter) And strExtra And RichTextBox1.Text IsNot "") Then
            num(1) = RichTextBox1.Text
            RichTextBox1.Text = ""
            'MsgBox((num(0) + num(1)).ToString)
            My.Computer.Keyboard.SendKeys("{BACKSPACE}")
            Scroll_Off()
            executeFunc = True
            'MessageBox.Show()

        End If

        If executeFunc = True Then
            If operand = 0 Then
                My.Computer.Keyboard.SendKeys((num(0) + num(1)).ToString)
            ElseIf operand = 1 Then
                My.Computer.Keyboard.SendKeys((num(0) - num(1)).ToString)
            ElseIf operand = 2 Then
                My.Computer.Keyboard.SendKeys((num(0) * num(1)).ToString)
            ElseIf operand = 3 Then
                My.Computer.Keyboard.SendKeys((num(0) / num(1)).ToString)
            End If

            executeFunc = False
        End If



        If (strExtra) Then
            Label2.Text = "HotKey: ON"
            Label2.ForeColor = Color.Green
        ElseIf (strExtra = False) Then
            Label2.Text = "HotKey: OFF"
            Label2.ForeColor = Color.Red
        End If

        If Label2.ForeColor = Color.Green And hasnotplayed1 = True Then
            play1 = True
        ElseIf Label2.ForeColor = Color.Red And hasnotplayed0 = True Then
            play0 = True
        End If
        If mute = False Then
            If play1 = True Then
                My.Computer.Audio.Play(My.Resources.hotkey_on, AudioPlayMode.Background)
                play1 = False
                hasnotplayed1 = False
                hasnotplayed0 = True
            ElseIf play0 = True Then
                My.Computer.Audio.Play(My.Resources.hotkey_off, AudioPlayMode.Background)
                play0 = False
                hasnotplayed0 = False
                hasnotplayed1 = True
            End If
        End If


        'RichTextBox1.AppendText(strExtra & strInput(0) & vbNewLine)

        If (strExtra And strInput(3)) Then
            My.Computer.Keyboard.SendKeys("{BACKSPACE}")
            My.Computer.Keyboard.SendKeys("π")
        ElseIf (strExtra And strInput(1)) Then
            Close()
        ElseIf (strExtra And strInput(0)) Then
            If mute = False Then
                mute = True
            Else
                mute = False
            End If
        ElseIf (strExtra And strInput(2)) Then
            My.Computer.Keyboard.SendKeys("{BACKSPACE}")
            My.Computer.Keyboard.SendKeys("Δ")
            Me.Button1.PerformClick()
        ElseIf (strExtra And strInput(4)) Then
            My.Computer.Keyboard.SendKeys("{BACKSPACE}")
            My.Computer.Keyboard.SendKeys("λ")
        ElseIf (strExtra And strInput(5)) Then
            My.Computer.Keyboard.SendKeys("{BACKSPACE}")
            My.Computer.Keyboard.SendKeys("¯\_(ツ)_/¯")
        ElseIf (strExtra And strInput(6)) Then
            My.Computer.Keyboard.SendKeys("{BACKSPACE}")
            My.Computer.Keyboard.SendKeys("™")
        ElseIf (strExtra And strInput(7)) Then
            My.Computer.Keyboard.SendKeys("{BACKSPACE}")
            My.Computer.Keyboard.SendKeys("²")
        End If

    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Label1.Text = "Mouse X: " + MousePosition.X.ToString + "  Mouse Y: " + MousePosition.Y.ToString
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)


    End Sub
End Class
