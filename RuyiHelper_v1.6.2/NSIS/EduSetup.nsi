; 该脚本使用 HM VNISEdit 脚本编辑器向导产生

!include "LogicLib.nsh"

!define /date PRODUCT_TIME %Y%m%d%H%M%S
!define /date DATE_Y %y
!define /date DATE_MD %m%d

;  OutFile  "Test_${PRODUCT_VERSION}_${PRODUCT_DATE}.exe"

; 安装程序初始定义常量
!define PRODUCT_NAME "如e小助手"
!define PRODUCT_VERSION "1.0.${DATE_Y}.${DATE_MD}"
!define PRODUCT_PUBLISHER "北京天地群网科技发展有限公司"
!define PRODUCT_WEB_SITE "http://www.groupnetwork.cn"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\Update.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

SetCompressor lzma

; ------ MUI 现代界面定义 (1.67 版本以上兼容) ------
!include "MUI.nsh"

; ------ 取当前日期 ------
!include "FileFunc.nsh"

; MUI 预定义常量
!define MUI_ABORTWARNING
!define MUI_ICON "ico32.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"
!define ExeName "如e小助手.exe"

;!define MUI_WELCOMEFINISHPAGE_BITMAP "left.bmp"

; 欢迎页面
!insertmacro MUI_PAGE_WELCOME
; 安装目录选择页面
!insertmacro MUI_PAGE_DIRECTORY
; 安装过程页面
!insertmacro MUI_PAGE_INSTFILES
; 安装完成页面
!define MUI_FINISHPAGE_RUN "$INSTDIR\${ExeName}"
!insertmacro MUI_PAGE_FINISH

; 安装卸载过程页面
!insertmacro MUI_UNPAGE_INSTFILES

; 安装界面包含的语言设置
!insertmacro MUI_LANGUAGE "SimpChinese"

;文件版本声明
  VIProductVersion "1.0.2.1"
  VIAddVersionKey /LANG=2052 "ProductName" "如e小助手"
  VIAddVersionKey /LANG=2052 "Comments" "免费使用，不限分发。"
  VIAddVersionKey /LANG=2052 "CompanyName" "www.groupnetwork.cn"
  VIAddVersionKey /LANG=2052 "LegalTrademarks" "如e小助手"
  VIAddVersionKey /LANG=2052 "LegalCopyright" "北京天地群网科技发展有限公司"
  VIAddVersionKey /LANG=2052 "FileDescription" "如e小助手"
  VIAddVersionKey /LANG=2052 "FileVersion" "${Ver}"
  VIAddVersionKey /LANG=2052 "ProductVersion" "${Ver}" ;产品版本
;VIAddVersionKey InternalName "${Name}" ;内部名称
;VIAddVersionKey LegalTrademarks "${PRODUCT_PUBLISHER}" ;合法商标 ;
;VIAddVersionKey OriginalFilename "XX.exe" ;源文件名
;VIAddVersionKey PrivateBuild "XX" ;个人内部版本说明
;VIAddVersionKey SpecialBuild "XX" ;特殊内部本本说明

; 安装预释放文件
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS
; ------ MUI 现代界面定义结束 ------

ReserveFile "${NSISDIR}\Plugins\splash.dll"
ReserveFile "splash.png"

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "D:\Release\如e小助手\如e小助手_${PRODUCT_VERSION}_Beta.exe"
InstallDir "D:\Program Files\如e小助手"
InstallDirRegKey HKLM "${PRODUCT_UNINST_KEY}" "UninstallString"
ShowInstDetails show
ShowUnInstDetails show
BrandingText "如意教育www.groupnetwork.cn"

Section "主程序" SEC01
  SetOutPath "$INSTDIR"
  SetOverwrite on
  
  Delete "$INSTDIR\RueHelper.exe"
  Delete "$INSTDIR\RueHelper.exe.config"
  Delete "$INSTDIR\Update.exe"
  Delete "$INSTDIR\Update.exe.config"
  Delete "$INSTDIR\RueSqlite.db"

  File "..\如e小助手_Release\update.ini"
  File "..\如e小助手_Release\RueUpdate.exe"
  File "..\如e小助手_Release\RueMng.exe"

  File "..\如e小助手_Release\${ExeName}"
  File "..\如e小助手_Release\Newtonsoft.Json.dll"
  File "..\如e小助手_Release\Interop.WMPLib.dll"
  File "..\如e小助手_Release\ICSharpCode.SharpZipLib.dll"
  File "..\如e小助手_Release\AxInterop.WMPLib.dll"
  File "..\如e小助手_Release\SQLite.Interop.dll"
  File "..\如e小助手_Release\System.Data.SQLite.dll"
  File "..\如e小助手_Release\log4net.dll"
  File "..\如e小助手_Release\log4net.config"

  CreateDirectory "$SMPROGRAMS\如e小助手"
  CreateShortCut "$SMPROGRAMS\如e小助手\如e小助手.lnk" "$INSTDIR\${ExeName}"
  CreateShortCut "$DESKTOP\如e小助手.lnk" "$INSTDIR\${ExeName}"
  
  File "..\如e小助手_Release\${ExeName}.config"
  SetOverwrite off
  File "..\如e小助手_Release\App.ini"
SectionEnd


Section -AdditionalIcons
  WriteIniStr "$INSTDIR\${PRODUCT_NAME}.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"
  CreateShortCut "$SMPROGRAMS\如e小助手\Website.lnk" "$INSTDIR\${PRODUCT_NAME}.url"
  CreateShortCut "$SMPROGRAMS\如e小助手\Uninstall.lnk" "$INSTDIR\uninst.exe"
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\RueUpdate.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\Update.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd

#-- 根据 NSIS 脚本编辑规则，所有 Function 区段必须放置在 Section 区段之后编写，以避免安装程序出现未可预知的问题。--#

Function .onInit0
    Push "${ExeName}"
    ProcessWork::existsprocess
    Pop $R0
  StrCmp $R0 "1" AA BB
  AA:
  MessageBox   MB_ICONSTOP   "如e小助手 正在运行,请先关闭程序!"
  Quit
  BB:
  !insertmacro MUI_LANGDLL_DISPLAY
FunctionEnd


Function .onInit
;关闭进程
  Push $R0
  CheckProc:
    Push "${ExeName}"
    ProcessWork::existsprocess
    Pop $R0
    IntCmp $R0 0 Done
    MessageBox MB_OKCANCEL|MB_ICONSTOP "安装程序检测到 ${PRODUCT_NAME} 正在运行。$\r$\n$\r$\n点击 “确定” 强制关闭${PRODUCT_NAME}，继续安装。$\r$\n点击 “取消” 退出安装程序。" IDCANCEL Exit
    Push "${ExeName}"
    Processwork::KillProcess
    Sleep 1000
    Goto CheckProc
    Exit:
    Abort
    Done:
    Pop $R0

FunctionEnd

/******************************
 *  以下是检测NetFramework4   *
 ******************************/
Section -.NET
Call GetNetFrameworkVersion
Pop $R1
 ${If} $R1 < '4.0.30319'
 SetDetailsPrint textonly
 DetailPrint "正在安装 .NET Framework 4"
 SetDetailsPrint listonly
 SetOutPath "$TEMP"
 SetOverwrite on
 ;File "dotNetFx40_Full_x86_x64.exe"
 ;ExecWait '$TEMP\dotNetFx40_Full_x86_x64.exe /q /norestart /ChainingPackage FullX64Bootstrapper' $R1
 ;Delete "$TEMP\dotNetFx40_Full_x86_x64.exe"
 ${EndIf}
SectionEnd

Function GetNetFrameworkVersion
;获取.Net Framework版本支持
Push $1
Push $0

ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Install"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Version"
StrCmp $0 1 KnowNetFrameworkVersion +1

ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5" "Install"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5" "Version"
StrCmp $0 1 KnowNetFrameworkVersion +1

ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.0\Setup" "InstallSuccess"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.0\Setup" "Version"
StrCmp $0 1 KnowNetFrameworkVersion +1

ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727" "Install"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727" "Version"
StrCmp $1 "" +1 +2
StrCpy $1 "2.0.50727.832"
StrCmp $0 1 KnowNetFrameworkVersion +1
ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v1.1.4322" "Install"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v1.1.4322" "Version"
StrCmp $1 "" +1 +2
StrCpy $1 "1.1.4322.573"
StrCmp $0 1 KnowNetFrameworkVersion +1
ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\.NETFramework\policy\v1.0" "Install"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\.NETFramework\policy\v1.0" "Version"
StrCmp $1 "" +1 +2
StrCpy $1 "1.0.3705.0"
StrCmp $0 1 KnowNetFrameworkVersion +1
StrCpy $1 "not .NetFramework"
KnowNetFrameworkVersion:
Pop $0
Exch $1
FunctionEnd

/******************************
 *  以下是安装程序的卸载部分  *
 ******************************/
Section Uninstall
  Delete "$INSTDIR\${PRODUCT_NAME}.url"
  Delete "$INSTDIR\uninst.exe"
  Delete "$INSTDIR\AxInterop.WMPLib.dll"
  Delete "$INSTDIR\ICSharpCode.SharpZipLib.dll"
  Delete "$INSTDIR\Interop.WMPLib.dll"
  Delete "$INSTDIR\log4net.config"
  Delete "$INSTDIR\log4net.dll"
  Delete "$INSTDIR\Newtonsoft.Json.dll"
  Delete "$INSTDIR\SQLite.Interop.dll"
  Delete "$INSTDIR\System.Data.SQLite.dll"
  Delete "$INSTDIR\${ExeName}"
  Delete "$INSTDIR\RueUpdate.exe"
  Delete "$INSTDIR\RueMng.exe"
  Delete "$INSTDIR\update.ini"
  
  Delete "$INSTDIR\update.log"
  Delete "$INSTDIR\msg.log"
  Delete "$INSTDIR\RueSqlite.db"

  Delete "$SMPROGRAMS\如e小助手\Uninstall.lnk"
  Delete "$SMPROGRAMS\如e小助手\Website.lnk"
  Delete "$DESKTOP\如e小助手.lnk"
  Delete "$SMPROGRAMS\如e小助手\如e小助手.lnk"

  RMDir "$SMPROGRAMS\如e小助手\conf"
; 清除安装程序创建的且在卸载时可能为空的子目录，对于递归添加的文件目录，请由最内层的子目录开始清除(注意，不要带 /r 参数，否则会失去 DelFileByLog 的意义)
;  RMDir "$SMPROGRAMS\如e小助手"
;  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd

#-- 根据 NSIS 脚本编辑规则，所有 Function 区段必须放置在 Section 区段之后编写，以避免安装程序出现未可预知的问题。--#

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "您确实要完全移除 $(^Name) ，及其所有的组件？" IDYES +2
  Abort
  ;检测程序是否运行

  Push $R0
  CheckProc:
    Push "${ExeName}"
    ProcessWork::existsprocess
    Pop $R0
    IntCmp $R0 0 Done
    MessageBox MB_OKCANCEL|MB_ICONSTOP "卸载程序检测到 ${PRODUCT_NAME} 正在运行。$\r$\n$\r$\n点击 “确定” 强制关闭${PRODUCT_NAME}，继续卸载。$\r$\n点击 “取消” 退出卸载程序。" IDCANCEL Exit
    Push "${ExeName}"
    Processwork::KillProcess
    Sleep 1000
    Goto CheckProc
    Exit:
    Abort
    Done:
    Pop $R0
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) 已成功地从您的计算机移除。"
FunctionEnd

